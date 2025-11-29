using MediatR;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Enums;

namespace Umbrella.Application.Commands.DataSources.CreateDataSource;

/// <summary>
/// Команда для создания нового источника данных
/// </summary>
public sealed partial class CreateDataSourceCommand(
    string name,
    DataSourceType type,
    string connectionString,
    string? description,
    bool isActive)
    : IRequest<DataSourceDto>
{
    public string Name { get; } = name;
    public DataSourceType Type { get; } = type;
    public string ConnectionString { get; } = connectionString;
    public string? Description { get; } = description;
    public bool IsActive { get; } = isActive;
}