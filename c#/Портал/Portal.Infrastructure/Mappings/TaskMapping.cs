using Mapster;
using Portal.Domain.Dtos;
using TaskEntity = Portal.Domain.Entities.Task;

namespace Portal.Infrastructure.Mappings;

public static class TaskMapping
{
    public static void Configure()
    {
        TypeAdapterConfig<TaskEntity, TaskDto>.NewConfig()
            .Map(dest => dest.TaskTypeName, src => src.TaskType != null ? src.TaskType.Name : string.Empty);
    }
}