using Mapster;
using Microsoft.Extensions.Logging;
using Umbrella.Application.Contracts.Repositories;
using Umbrella.Application.Contracts.Services;
using Umbrella.Domain.Dtos;
using Umbrella.Domain.Models;

namespace Umbrella.Infrastructure.Services;

public sealed class DataSourceService(
    IDataSourceRepository dataSourceRepository,
    ILogger<DataSourceService> logger) : IDataSourceService
{
    private readonly string _serviceName = nameof(DataSourceService);

    public async Task<DataSourceDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetByIdAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение источника данных. DataSourceId: {dataSourceId}",
            _serviceName, methodName, id);

        var dataSource = await dataSourceRepository.GetByIdAsync(id, cancellationToken);
        if (dataSource == null)
        {
            logger.LogWarning("{serviceName}.{methodName}. Источник данных не найден. DataSourceId: {dataSourceId}",
                _serviceName, methodName, id);
            return null;
        }

        return dataSource.Adapt<DataSourceDto>();
    }

    public async Task<List<DataSourceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var methodName = nameof(GetAllAsync);
        logger.LogDebug("{serviceName}.{methodName}. Получение всех источников данных", _serviceName, methodName);

        var dataSources = await dataSourceRepository.GetAllAsync(cancellationToken);
        return dataSources.Adapt<List<DataSourceDto>>();
    }

    public async Task<DataSourceDto> CreateAsync(DataSourceModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(CreateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Создание источника данных. Name: {name}",
            _serviceName, methodName, model.Name);

        var dataSource = model.Adapt<Domain.Entities.DataSource>();

        var createdDataSource = await dataSourceRepository.CreateAsync(dataSource, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Источник данных создан. DataSourceId: {dataSourceId}",
            _serviceName, methodName, createdDataSource.Id);

        return createdDataSource.Adapt<DataSourceDto>();
    }

    public async Task<DataSourceDto> UpdateAsync(Guid id, DataSourceModel model, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(UpdateAsync);
        logger.LogInformation("{serviceName}.{methodName}. Обновление источника данных. DataSourceId: {dataSourceId}",
            _serviceName, methodName, id);

        var dataSource = await dataSourceRepository.GetByIdAsync(id, cancellationToken);
        if (dataSource == null)
        {
            logger.LogError("{serviceName}.{methodName}. Источник данных не найден. DataSourceId: {dataSourceId}",
                _serviceName, methodName, id);
            throw new InvalidOperationException($"Источник данных с Id {id} не найден");
        }

        dataSource.Name = model.Name;
        dataSource.Type = model.Type;
        dataSource.ConnectionString = model.ConnectionString;
        dataSource.Description = model.Description;
        dataSource.IsActive = model.IsActive;

        var updatedDataSource = await dataSourceRepository.UpdateAsync(dataSource, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Источник данных обновлен. DataSourceId: {dataSourceId}",
            _serviceName, methodName, updatedDataSource.Id);

        return updatedDataSource.Adapt<DataSourceDto>();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var methodName = nameof(DeleteAsync);
        logger.LogInformation("{serviceName}.{methodName}. Удаление источника данных. DataSourceId: {dataSourceId}",
            _serviceName, methodName, id);

        await dataSourceRepository.DeleteAsync(id, cancellationToken);

        logger.LogInformation("{serviceName}.{methodName}. Источник данных удален. DataSourceId: {dataSourceId}",
            _serviceName, methodName, id);
    }
}