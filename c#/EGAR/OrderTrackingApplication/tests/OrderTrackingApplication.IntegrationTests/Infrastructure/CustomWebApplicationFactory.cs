using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OrderTrackingApplication.Application.Contracts.Messaging;
using OrderTrackingApplication.Infrastructure.Data;
using OrderTrackingApplication.Infrastructure.Messaging;

namespace OrderTrackingApplication.IntegrationTests.Infrastructure;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<OrderDbContext>>();
            services.RemoveAll<OrderDbContext>();

            services.AddDbContext<OrderDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            services.RemoveAll<IEventPublisher>();
            services.AddSingleton<FakeEventPublisher>();
            services.AddSingleton<IEventPublisher>(sp => sp.GetRequiredService<FakeEventPublisher>());

            var outboxProcessorDescriptors = services
                .Where(d => d.ImplementationType == typeof(OutboxProcessor))
                .ToList();

            foreach (var descriptor in outboxProcessorDescriptors)
            {
                services.Remove(descriptor);
            }
        });
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
