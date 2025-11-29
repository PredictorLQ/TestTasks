using MediatR;

namespace Umbrella.Application.Commands.DataSources.DeleteDataSource;

/// <summary>
/// Команда для удаления источника данных
/// </summary>
public sealed partial class DeleteDataSourceCommand(Guid id) : IRequest
{
    public Guid Id { get; } = id;
}