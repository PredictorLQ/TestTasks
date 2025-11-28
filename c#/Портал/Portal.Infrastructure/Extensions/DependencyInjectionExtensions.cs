using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portal.Application.Contracts.Repositories;
using Portal.Application.Contracts.Services;
using Portal.Infrastructure.Data;
using Portal.Infrastructure.Mappings;
using Portal.Infrastructure.Repositories;
using Portal.Infrastructure.Services;

namespace Portal.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddDbContext<PortalDbContext>(options =>
            options.UseNpgsql(connectionString));

        ConfigureMapster();
        AddRepositories(services);
        AddServices(services);

        return services;
    }

    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PortalDbContext>();
        await context.Database.MigrateAsync();
        await SeedData.SeedAsync(context);
    }

    private static void ConfigureMapster()
    {
        TaskMapping.Configure();
    }

    private static IServiceCollection AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();

        return services;
    }

    private static IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<ITaskTypeService, TaskTypeService>();

        return services;
    }
}