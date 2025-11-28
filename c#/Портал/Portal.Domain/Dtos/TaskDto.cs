using Portal.Domain.Enums;

namespace Portal.Domain.Dtos;

/// <summary>
/// DTO для передачи данных о задаче
/// </summary>
public sealed record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskStatusEnum Status,
    TaskPriorityEnum Priority,
    DateTime? DueDate,
    DateTime CreatedAt,
    DateTime? ChangedAt,
    Guid TaskTypeId,
    string TaskTypeName);

