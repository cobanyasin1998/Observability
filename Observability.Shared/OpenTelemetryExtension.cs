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
                    opt.AddEntityFrameworkCoreInstrumentation(efCoreOptions =>
                    {
                        efCoreOptions.SetDbStatementForText = true;
                        efCoreOptions.SetDbStatementForStoredProcedure = true;
                    });

                    opt.AddHttpClientInstrumentation(httpClientOptions =>
                    {
                        httpClientOptions.EnrichWithHttpRequestMessage = async (activity, request) =>
                        {
                            var requestContent = string.Empty;
                            if (request.Content is not null)
                            {
                                requestContent = await  request.Content.ReadAsStringAsync();
                            }
                            activity.SetTag("http.request.body", request.RequestUri?.ToString());
                        };
                        httpClientOptions.EnrichWithHttpResponseMessage = (activity, response) =>
                        {
                            if (response.Content is not null)
                                activity.SetTag("http.response.body", response.Content.ReadAsStringAsync());
                            
                        };

                    });

                    opt.AddConsoleExporter();
                    opt.AddOtlpExporter();
                });
        }
    }
}
