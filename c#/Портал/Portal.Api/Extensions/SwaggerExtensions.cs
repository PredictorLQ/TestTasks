using Microsoft.OpenApi.Models;

namespace Portal.Api.Extensions;

/// <summary>
/// Расширения для настройки Swagger
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Добавить настройки Swagger
    /// </summary>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Portal API",
                Version = "v1",
                Description = "API для управления задачами"
            });

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

            var domainXmlFile = "Portal.Domain.xml";
            var domainXmlPath = Path.Combine(AppContext.BaseDirectory, domainXmlFile);
            if (File.Exists(domainXmlPath))
                c.IncludeXmlComments(domainXmlPath);
        });

        return services;
    }

    /// <summary>
    /// Настроить использование Swagger в приложении
    /// </summary>
    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portal API v1");
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}