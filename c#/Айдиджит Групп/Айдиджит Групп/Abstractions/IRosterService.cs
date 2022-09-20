using Айдиджит_Групп.Assistant;
using Айдиджит_Групп.Models;

namespace Айдиджит_Групп.Abstractions;

public interface IRosterService
{
    public Task<Page<RosterReference>> GetPageAsync(uint? offset = null, uint? count = null, string? searchText = default);
}