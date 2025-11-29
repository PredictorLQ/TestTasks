using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Commands.DataSources.UpdateDataSource;

public sealed partial class UpdateDataSourceCommand
{
    public sealed class UpdateDataSourceCommandHandle(
        IDataSourceService dataSourceService,
        ILogger<UpdateDataSourceCommandHandle> logger) : IRequestHandler<UpdateDataSourceCommand, DataSourceDto>
    {
        public async Task<DataSourceDto> Handle(UpdateDataSourceCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("UpdateDataSourceCommand.{methodName}. Начало обновления источника данных. DataSourceId: {dataSourceId}",
                methodName, request.Id);

            var model = request.Adapt<DataSourceModel>();

            var result = await dataSourceService.UpdateAsync(request.Id, model, cancellationToken);

            logger.LogInformation("UpdateDataSourceCommand.{methodName}. Источник данных обновлен. DataSourceId: {dataSourceId}",
                methodName, request.Id);

            return result;
        }
    }
}