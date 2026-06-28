using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderTrackingApplication.Application.Contracts.Messaging;
using OrderTrackingApplication.Application.Contracts.Repositories;
using OrderTrackingApplication.Application.Contracts.Services;
using OrderTrackingApplication.Infrastructure.Data;
using OrderTrackingApplication.Infrastructure.Mappings;
using OrderTrackingApplication.Infrastructure.Messaging;
using OrderTrackingApplication.Infrastructure.Repositories;
using OrderTrackingApplication.Infrastructure.Services;

namespace OrderTrackingApplication.Infrastructure.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.Configure<KafkaOptions>(configuration.GetSection(KafkaOptions.SectionName));

        ConfigureMapster();
        AddRepositories(services);
        AddMessaging(services);

        services.AddSingleton<IOrderNotificationService, OrderNotificationService>();

        return services;
    }

    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        await context.Database.MigrateAsync();
    }

    private static void ConfigureMapster()
    {
        OrderMappingRegister.Configure();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
    }

    private static void AddMessaging(IServiceCollection services)
    {
        services.AddSingleton<IEventPublisher, KafkaEventPublisher>();
        services.AddHostedService<OutboxProcessor>();
    }
}
