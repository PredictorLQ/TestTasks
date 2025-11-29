using Microsoft.OpenApi.Models;

namespace Umbrella.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Umbrella API",
                Version = "v1",
                Description = "API для системы отчетности на базе Stimulsoft"
            });

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            var domainXmlFile = "Umbrella.Domain.xml";
            var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);
            if (File.Exists(domainXmlPath))
                c.IncludeXmlComments(domainXmlPath);
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Umbrella API v1");
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}