namespace Portal.Domain.Dtos;

/// <summary>
/// DTO для передачи данных о типе задачи
/// </summary>
public sealed record TaskTypeDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    DateTime? ChangedAt);

