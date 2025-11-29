using MediatR;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.DataSources.GetAllDataSources;

/// <summary>
/// Запрос для получения всех источников данных
/// </summary>
public sealed partial class GetAllDataSourcesQuery() : IRequest<List<DataSourceDto>>
{
}