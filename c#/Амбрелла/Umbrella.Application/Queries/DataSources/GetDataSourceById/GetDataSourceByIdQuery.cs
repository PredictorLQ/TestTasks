using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.DataSources.GetDataSourceById;

/// <summary>
/// Запрос для получения источника данных по идентификатору
/// </summary>
public sealed partial class GetDataSourceByIdQuery(Guid id) : IRequest<DataSourceDto?>
{
    public Guid Id { get; } = id;
}