using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbrella.Application.Contracts.Repositories;
using Umbrella.Application.Contracts.Services;
using Umbrella.Infrastructure.Data;
using Umbrella.Infrastructure.Mappings;
using Umbrella.Infrastructure.Repositories;
using Umbrella.Infrastructure.Services;

namespace Umbrella.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddDbContext<UmbrellaDbContext>(options =>
            options.UseNpgsql(connectionString));

        ConfigureMapster();
        AddRepositories(services);
        AddServices(services);

        return services;
    }

    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UmbrellaDbContext>();
        await context.Database.MigrateAsync();
    }

    private static void ConfigureMapster()
    {
        UmbrellaMappingRegister.Configure();
    }

    private static IServiceCollection AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDataSourceRepository, DataSourceRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IScheduledReportRepository, ScheduledReportRepository>();

        return services;
    }

    private static IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDataSourceService, DataSourceService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IScheduledReportService, ScheduledReportService>();

        return services;
    }
}