using MediatR;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Enums;

namespace Umbrella.Application.Commands.DataSources.UpdateDataSource;

/// <summary>
/// Команда для обновления источника данных
/// </summary>
public sealed partial class UpdateDataSourceCommand(
    Guid id,
    string name,
    DataSourceType type,
    string connectionString,
    string? description,
    bool isActive)
    : IRequest<DataSourceDto>
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
    public DataSourceType Type { get; } = type;
    public string ConnectionString { get; } = connectionString;
    public string? Description { get; } = description;
    public bool IsActive { get; } = isActive;
}