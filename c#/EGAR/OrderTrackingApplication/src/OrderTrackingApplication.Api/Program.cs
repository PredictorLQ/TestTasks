using OrderTrackingApplication.Api.Extensions;
using OrderTrackingApplication.Api.WebSockets;
using OrderTrackingApplication.Application.Extensions;
using OrderTrackingApplication.Infrastructure.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables()
        .Build())
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

    builder.Services.AddSwaggerConfiguration();
    builder.AddOpenTelemetryTelemetry();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication();

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
            policy.WithOrigins(
                    "http://localhost:5173",
                    "http://localhost:3000",
                    "http://127.0.0.1:5173")
                .AllowAnyHeader()
                .AllowAnyMethod());
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerConfiguration();
    }

    app.UseSerilogRequestLogging();
    app.UseCors();

    if (app.Configuration.GetValue("UseHttpsRedirection", true))
    {
        app.UseHttpsRedirection();
    }

    app.UseOrderStatusWebSockets();
    app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
    app.MapControllers();

    if (!app.Environment.IsEnvironment("Testing"))
    {
        await app.Services.MigrateDatabaseAsync();
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Приложение завершилось с ошибкой");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program;
