namespace Observability.Shared
{
    public class OpenTelemetryOptions
    {
        public string ServiceName { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string ActivitySourceName { get; set; } = null!;
    }
}
