using AutoMapper;

namespace Айдиджит_Групп.Models;

public sealed class RosterReference
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SubTitle { get; set; } = string.Empty;

    public sealed class RosterReferenceProfile : Profile
    {
        public RosterReferenceProfile()
        {
            CreateMap<RosterModel, RosterReference>();
        }
    }
}