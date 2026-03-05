using NCrontab;
using task.Services;

namespace task;

public class Worker : BackgroundService
{
    private static readonly TimeSpan MskOffset = TimeSpan.FromHours(3);
    private const string CronExpression = "0 2 * * *"; // Каждый день в 02:00 по МСК

    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly CrontabSchedule _schedule;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _schedule = CrontabSchedule.Parse(CronExpression);
    }

    private async Task RunImportAsync(CancellationToken stoppingToken)
    {
        var currentMskTime = DateTime.UtcNow.Add(MskOffset);
        _logger.LogInformation("Запуск импорта терминалов в {Time} MSK", currentMskTime);

        using var scope = _serviceProvider.CreateScope();
        var importService = scope.ServiceProvider.GetRequiredService<TerminalImportService>();

        var (loadedCount, deletedCount, savedCount, duration) =
            await importService.ImportTerminalsAsync(stoppingToken);

        _logger.LogInformation(
            "Импорт завершен. Загружено: {LoadedCount}, Удалено: {DeletedCount}, Сохранено: {SavedCount}, Время выполнения: {Duration}",
            loadedCount, deletedCount, savedCount, duration);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker запущен. Cron: {Cron}", CronExpression);

        // Текущее время в МСК и первое запланированное срабатывание по cron
        var nowUtc = DateTime.UtcNow;
        var nowMsk = nowUtc.Add(MskOffset);
        var nextRunMsk = _schedule.GetNextOccurrence(nowMsk);

        while (!stoppingToken.IsCancellationRequested)
        {
            // Пересчитываем задержку до следующего запуска
            var nextRunUtc = nextRunMsk.Subtract(MskOffset);
            var delay = nextRunUtc - DateTime.UtcNow;
            if (delay < TimeSpan.Zero)
            {
                delay = TimeSpan.Zero;
            }

            _logger.LogInformation(
                "Следующий запуск импорта запланирован на {NextRun} MSK (через {Delay})",
                nextRunMsk, delay);

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Worker остановлен");
                break;
            }

            try
            {
                await RunImportAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при выполнении импорта терминалов");
            }

            // Сдвигаем расписание на следующий запуск по cron
            var afterRunMsk = DateTime.UtcNow.Add(MskOffset);
            nextRunMsk = _schedule.GetNextOccurrence(afterRunMsk);
        }
    }
}