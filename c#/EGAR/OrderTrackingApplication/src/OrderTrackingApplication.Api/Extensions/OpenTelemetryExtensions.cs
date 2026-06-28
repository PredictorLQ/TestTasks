using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderTrackingApplication.Api.Options;
using OrderTrackingApplication.Infrastructure.Telemetry;

namespace OrderTrackingApplication.Api.Extensions;

public static class OpenTelemetryExtensions
{
    public static IHostApplicationBuilder AddOpenTelemetryTelemetry(this IHostApplicationBuilder builder)
    {
        if (builder.Environment.IsEnvironment("Testing"))
        {
            return builder;
        }

        var options = builder.Configuration
            .GetSection(OpenTelemetryOptions.SectionName)
            .Get<OpenTelemetryOptions>() ?? new OpenTelemetryOptions();

        if (!options.Enabled)
        {
            return builder;
        }

        var exportToConsole = options.ExportToConsole || builder.Environment.IsDevelopment();

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: options.ServiceName,
                    serviceVersion: options.ServiceVersion)
                .AddAttributes(new Dictionary<string, object>
                {
                    ["deployment.environment"] = builder.Environment.EnvironmentName
                }))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(instrumentation =>
                    {
                        instrumentation.RecordException = true;
                        instrumentation.Filter = context =>
                            !context.Request.Path.StartsWithSegments("/health");
                    })
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation(instrumentation =>
                    {
                        instrumentation.SetDbStatementForText = true;
                    })
                    .AddNpgsql()
                    .AddSource(OrderTrackingTelemetry.ActivitySourceName)
                    .AddOtlpExporter(exporter =>
                    {
                        exporter.Endpoint = new Uri(options.OtlpEndpoint);
                        exporter.Protocol = OtlpExportProtocol.Grpc;
                    });

                if (exportToConsole)
                {
                    tracing.AddConsoleExporter();
                }
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter(exporter =>
                    {
                        exporter.Endpoint = new Uri(options.OtlpEndpoint);
                        exporter.Protocol = OtlpExportProtocol.Grpc;
                    });

                if (exportToConsole)
                {
                    metrics.AddConsoleExporter();
                }
            });

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
            logging.AddOtlpExporter(exporter =>
            {
                exporter.Endpoint = new Uri(options.OtlpEndpoint);
                exporter.Protocol = OtlpExportProtocol.Grpc;
            });
        });

        return builder;
    }
}
