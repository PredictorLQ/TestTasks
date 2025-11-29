using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;

namespace Umbrella.Application.Queries.DataSources.GetDataSourceById;

public sealed partial class GetDataSourceByIdQuery
{
    public sealed class GetDataSourceByIdQueryHandle(
        IDataSourceService dataSourceService,
        ILogger<GetDataSourceByIdQueryHandle> logger) : IRequestHandler<GetDataSourceByIdQuery, DataSourceDto?>
    {
        public async Task<DataSourceDto?> Handle(GetDataSourceByIdQuery request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogDebug("GetDataSourceByIdQuery.{methodName}. Получение источника данных. DataSourceId: {dataSourceId}",
                methodName, request.Id);

            var result = await dataSourceService.GetByIdAsync(request.Id, cancellationToken);

            logger.LogDebug("GetDataSourceByIdQuery.{methodName}. Источник данных {found}. DataSourceId: {dataSourceId}",
                methodName, result != null ? "найден" : "не найден", request.Id);

            return result;
        }
    }
}