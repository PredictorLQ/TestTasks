using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Айдиджит_Групп.Abstractions;
using Айдиджит_Групп.Assistant;
using Айдиджит_Групп.Models;

namespace Айдиджит_Групп.Controllers;

[Consumes("application/json")]
[Produces("application/json")]
[Route("api/roster")]
[ApiController]
public sealed class RosterController : ControllerBase
{
    private readonly IRosterService _rosterService;

    public RosterController(IRosterService rosterService)
        => _rosterService = rosterService;

    /// <summary>
    /// Get page of roster
    /// </summary>
    /// <response code="200"/>
    [ProducesResponseType(typeof(Page<RosterReference>), 200)]
    [HttpGet]
    public async Task<IActionResult> GetPageAsync(
        [FromQuery] uint offset = 0,
        [FromQuery][Range(5, 100)] uint count = 20,
        [FromQuery][MaxLength(256)] string? searchText = default)
        => Ok(await _rosterService.GetPageAsync(offset, count, searchText));
}
