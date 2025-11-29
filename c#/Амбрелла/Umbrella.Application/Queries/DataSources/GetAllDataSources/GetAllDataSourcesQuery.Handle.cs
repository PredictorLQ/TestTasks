using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.DataSources.GetAllDataSources;

public sealed partial class GetAllDataSourcesQuery
{
    public sealed class GetAllDataSourcesQueryHandle(
        IDataSourceService dataSourceService,
        ILogger<GetAllDataSourcesQueryHandle> logger) : IRequestHandler<GetAllDataSourcesQuery, List<DataSourceDto>>
    {
        public async Task<List<DataSourceDto>> Handle(GetAllDataSourcesQuery request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogDebug("GetAllDataSourcesQuery.{methodName}. Получение всех источников данных", methodName);

            var result = await dataSourceService.GetAllAsync(cancellationToken);

            logger.LogDebug("GetAllDataSourcesQuery.{methodName}. Получено источников данных: {count}",
                methodName, result.Count);

            return result;
        }
    }
}