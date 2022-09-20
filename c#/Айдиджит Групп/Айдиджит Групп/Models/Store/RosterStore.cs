using System.Text.Json;

namespace Айдиджит_Групп.Models;

public interface IRosterStoreFactory
{
    Task<RosterStore> CreateAsync();
    Task RecalculationAsync();
    void Delete();
}

public sealed class RosterStoreFactory : IRosterStoreFactory
{
    private RosterStore? _store { get; set; }

    public async Task<RosterStore> CreateAsync()
    {
        if (_store == null)
        {
            _store = new();

            await _store!.Initialize();
        }

        return _store;
    }

    public async Task RecalculationAsync()
    {
        Delete();

        await CreateAsync();
    }

    public void Delete()
        => _store = default;

}

public sealed class RosterStore
{
    private const string pathToFile = "./roster.json";

    public IEnumerable<RosterModel>? RosterModels { get; set; }

    internal async Task Initialize()
    {
        using var fs = new FileStream(pathToFile, FileMode.OpenOrCreate);
        RosterModels = await JsonSerializer.DeserializeAsync<IEnumerable<RosterModel>>(fs) ?? new List<RosterModel>();
    }
}