namespace Айдиджит_Групп.Models;

public sealed class RosterModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;
}