namespace OrderTrackingApplication.Infrastructure.Telemetry;

public static class OrderTrackingTelemetry
{
    public const string ActivitySourceName = "OrderTrackingApplication";
    public static readonly System.Diagnostics.ActivitySource ActivitySource = new(ActivitySourceName);
}
