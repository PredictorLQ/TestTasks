using AutoMapper.QueryableExtensions;
using Айдиджит_Групп.Abstractions;
using Айдиджит_Групп.Assistant;
using Айдиджит_Групп.Models;

namespace Айдиджит_Групп.Services;

public sealed class RosterService : IRosterService
{
    private readonly AutoMapper.IConfigurationProvider _configurationProvider;
    private readonly IRosterStoreFactory _rosterStoreFactory;

    public RosterService(AutoMapper.IConfigurationProvider configurationProvider, IRosterStoreFactory rosterStoreFactory)
        => (_configurationProvider, _rosterStoreFactory) = (configurationProvider, rosterStoreFactory);

    public async Task<Page<RosterReference>> GetPageAsync(uint? offset = null, uint? count = null, string? searchText = default)
    {
        var store = await _rosterStoreFactory.CreateAsync();

        var items = store.RosterModels!.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
            items = items.Where(u => u.Title.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) || u.SubTitle.Contains(searchText, StringComparison.CurrentCultureIgnoreCase));

        return items
            .OrderByDescending(u => u.Title)
                .ThenByDescending(u => u.SubTitle)
            .ProjectTo<RosterReference>(_configurationProvider)
            .ToPage(offset, count);
    }
}