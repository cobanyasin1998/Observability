using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Observability.Shared
{
    public static class OpenTelemetryExtension
    {
        public static void AddOpenTelemetryExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OpenTelemetryOptions>(configuration.GetSection("OpenTelemetry"));

            OpenTelemetryOptions openTelemetryOptions = (configuration.GetSection("OpenTelemetry").Get<OpenTelemetryOptions>())!;

            ActivitySourceProvider.Source = new ActivitySource(openTelemetryOptions.ActivitySourceName);

            services.AddOpenTelemetry()
                .WithTracing(opt =>
                {
                    opt.AddSource(openTelemetryOptions.ActivitySourceName).ConfigureResource(resource =>
                    {
                        resource.AddService(openTelemetryOptions.ServiceName, serviceVersion: openTelemetryOptions?.Version);
                    });
                    opt.AddAspNetCoreInstrumentation(aspNetCoreOptions =>
                    {
                        aspNetCoreOptions.Filter = (httpContext) =>
                            !string.IsNullOrWhiteSpace(httpContext.Request.Path.Value) &&
                            httpContext.Request.Path.Value.Contains("api/", StringComparison.InvariantCulture);

                        aspNetCoreOptions.RecordException = true;

                    });
                    opt.AddConsoleExporter();
                    opt.AddOtlpExporter();
                });
        }
    }
}
