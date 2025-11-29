using MediatR;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Services;

namespace Umbrella.Application.Commands.DataSources.DeleteDataSource;

public sealed partial class DeleteDataSourceCommand
{
    public sealed class DeleteDataSourceCommandHandle(
        IDataSourceService dataSourceService,
        ILogger<DeleteDataSourceCommandHandle> logger) : IRequestHandler<DeleteDataSourceCommand>
    {
        public async Task Handle(DeleteDataSourceCommand request, CancellationToken cancellationToken)
        {
            var methodName = nameof(Handle);
            logger.LogInformation("DeleteDataSourceCommand.{methodName}. Начало удаления источника данных. DataSourceId: {dataSourceId}",
                methodName, request.Id);

            await dataSourceService.DeleteAsync(request.Id, cancellationToken);

            logger.LogInformation("DeleteDataSourceCommand.{methodName}. Источник данных удален. DataSourceId: {dataSourceId}",
                methodName, request.Id);
        }
    }
}