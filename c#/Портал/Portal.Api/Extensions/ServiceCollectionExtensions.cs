namespace Portal.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return Portal.Application.Extensions.DependencyInjectionExtensions.AddApplication(services);
    }
}