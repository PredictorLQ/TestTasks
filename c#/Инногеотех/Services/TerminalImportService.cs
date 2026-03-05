using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using task.Data;
using task.Entities;
using task.Models;

namespace task.Services;

public class TerminalImportService
{
    private readonly DellinDictionaryDbContext _context;
    private readonly ILogger<TerminalImportService> _logger;
    private readonly string _jsonFilePath;

    private static readonly string _fileName = "terminals.json";
    private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    public TerminalImportService(
        DellinDictionaryDbContext context,
        ILogger<TerminalImportService> logger,
        IHostEnvironment environment)
    {
        _context = context;
        _logger = logger;
        var basePath = environment.ContentRootPath ?? AppDomain.CurrentDomain.BaseDirectory;
        _jsonFilePath = Path.Combine(basePath, "files", _fileName);

        if (!File.Exists(_jsonFilePath))
            _jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", _fileName);
    }

    public async Task<(int LoadedCount, int DeletedCount, int SavedCount, TimeSpan Duration)> ImportTerminalsAsync(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        int loadedCount = 0;
        int deletedCount = 0;
        int savedCount = 0;

        try
        {
            _logger.LogInformation("Начало импорта терминалов из файла: {FilePath}", _jsonFilePath);

            if (!File.Exists(_jsonFilePath))
                throw new FileNotFoundException($"Файл не найден: {_jsonFilePath}");

            var jsonContent = await File.ReadAllTextAsync(_jsonFilePath, cancellationToken);

            var cityDto = JsonSerializer.Deserialize<CityDto>(jsonContent, jsonSerializerOptions);

            if (cityDto?.Cities == null || !cityDto.Cities.Any())
            {
                _logger.LogWarning("Файл не содержит данных о городах");
                return (0, 0, 0, stopwatch.Elapsed);
            }

            var offices = new List<Office>();
            var phoneId = 1;

            foreach (var city in cityDto.Cities)
            {
                if (city.Terminals?.TerminalList == null)
                    continue;

                var cityCode = city.CityId ?? 0;
                if (cityCode == 0)
                {
                    _logger.LogWarning("Город {CityName} не имеет cityID, пропускаем", city.Name);
                    continue;
                }

                foreach (var terminal in city.Terminals.TerminalList)
                {
                    if (string.IsNullOrWhiteSpace(terminal.Id))
                        continue;

                    var office = new Office
                    {
                        Code = terminal.Id,
                        CityCode = cityCode,
                        Uuid = terminal.Id,
                        Type = DetermineOfficeType(terminal),
                        CountryCode = "RU", // По умолчанию Россия
                        Coordinates = new Coordinates
                        {
                            Latitude = ParseDouble(terminal.Latitude),
                            Longitude = ParseDouble(terminal.Longitude)
                        },
                        WorkTime = terminal.CalcSchedule?.Derival ?? terminal.CalcSchedule?.Arrival ?? string.Empty
                    };

                    // Парсинг адреса
                    AddressParser.ParseAddress(terminal.FullAddress ?? terminal.Address, office);

                    if (terminal.Phones != null && terminal.Phones.Any())
                    {
                        office.Phones = terminal.Phones.Where(x => !string.IsNullOrWhiteSpace(x.Number))
                            .Select(x => new Phone
                            {
                                Id = phoneId++,
                                PhoneNumber = x.Number!,
                                Additional = !string.IsNullOrWhiteSpace(x.Type)
                                    ? $"{x.Type}{(string.IsNullOrWhiteSpace(x.Comment) ? "" : $": {x.Comment}")}"
                                    : x.Comment
                            }).ToList();
                    }

                    offices.Add(office);
                }
            }

            loadedCount = offices.Count;
            _logger.LogInformation("Загружено {Count} терминалов из JSON", loadedCount);

            // Очистка существующих данных
            deletedCount = await _context.Offices.CountAsync(cancellationToken);
            await _context.Offices.ExecuteDeleteAsync(cancellationToken);
            _logger.LogInformation("Удалено {OldCount} старых записей", deletedCount);

            if (offices.Any())
            {
                const int batchSize = 100;
                savedCount = 0;

                for (var i = 0; i < offices.Count; i += batchSize)
                {
                    var batch = offices.Skip(i).Take(batchSize).ToList();

                    await _context.Offices.AddRangeAsync(batch, cancellationToken);
                    savedCount += await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation(
                        "Сохранено {BatchCount} терминалов в батче, всего сохранено: {TotalSaved}",
                        batch.Count,
                        savedCount);
                }

                _logger.LogInformation("Итогово сохранено {NewCount} новых терминалов", savedCount);
            }

            stopwatch.Stop();
            _logger.LogInformation(
                "Импорт завершен успешно. Загружено: {LoadedCount}, Удалено: {DeletedCount}, Сохранено: {SavedCount}, Время выполнения: {Duration}",
                loadedCount, deletedCount, savedCount, stopwatch.Elapsed);

            return (loadedCount, deletedCount, savedCount, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Ошибка импорта: {Exception}", ex.Message);
            throw;
        }
    }

    private static OfficeType? DetermineOfficeType(TerminalDto terminal)
    {
        if (terminal.IsPVZ == true)
            return OfficeType.PVZ;

        // TODO: добавить дополнительную логику определения типа по другим полям (в ТЗ нет сведений)
        return null;
    }

    private static double ParseDouble(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0.0;

        return double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var result)
            ? result
            : 0.0;
    }
}