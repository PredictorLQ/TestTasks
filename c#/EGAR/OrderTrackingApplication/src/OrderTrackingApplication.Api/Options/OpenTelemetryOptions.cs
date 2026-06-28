namespace OrderTrackingApplication.Api.Options;

public sealed class OpenTelemetryOptions
{
    public const string SectionName = "OpenTelemetry";

    public bool Enabled { get; set; } = true;
    public string ServiceName { get; set; } = "order-tracking-api";
    public string ServiceVersion { get; set; } = "1.0.0";
    public string OtlpEndpoint { get; set; } = "http://localhost:4317";
    public bool ExportToConsole { get; set; }
}
