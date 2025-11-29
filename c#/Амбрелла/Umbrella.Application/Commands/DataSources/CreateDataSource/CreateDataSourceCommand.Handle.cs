using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Application.Commands.DataSources.CreateDataSource;

public sealed partial class CreateDataSourceCommand
{
    public sealed class CreateDataSourceCommandHandle(
        IDataSourceService dataSourceService,
        ILogger<CreateDataSourceCommandHandle> logger) : IRequestHandler<CreateDataSourceCommand, DataSourceDto>
    {
        public async Task<DataSourceDto> Handle(CreateDataSourceCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("CreateDataSourceCommand.{methodName}. Начало создания источника данных. Name: {name}",
                methodName, request.Name);

            var model = request.Adapt<DataSourceModel>();

            var result = await dataSourceService.CreateAsync(model, cancellationToken);

            logger.LogInformation("CreateDataSourceCommand.{methodName}. Источник данных создан. DataSourceId: {dataSourceId}",
                methodName, result.Id);

            return result;
        }
    }
}