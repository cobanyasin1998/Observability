using Observability.ConsoleApp;
using Observability.ConsoleApp.Consts;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;


ActivitySource.AddActivityListener(new ActivityListener
{
    ShouldListenTo = (activitySource) => activitySource.Name == OpenTelemetryConstants.ActivitySourceFileName,
    ActivityStarted = (activity) =>
    {
        Console.WriteLine($"Activity started: {activity.DisplayName}");
    },
    ActivityStopped = (activity) =>
    {
        Console.WriteLine($"Activity stopped: {activity.DisplayName}");
    },
    Sample = (ref ActivityCreationOptions<ActivityContext> options) =>
    {
        return ActivitySamplingResult.AllData;
    }
});


using var traceProviderFile = Sdk.CreateTracerProviderBuilder().AddSource(OpenTelemetryConstants.ActivitySourceFileName).Build();


using var traceProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource(OpenTelemetryConstants.ActivitySourceName)
    .ConfigureResource(configure =>
    {
        configure.AddService(OpenTelemetryConstants.ServiceName, serviceNamespace: OpenTelemetryConstants.ServiceVersion)
        .AddAttributes(new List<KeyValuePair<string, object>>()
        {
            new KeyValuePair<string, object>("host.machineName", Environment.MachineName),
            new KeyValuePair<string, object>("host.environment", "Dev")
        });
    })
    .AddConsoleExporter()
    .AddOtlpExporter()
    .AddZipkinExporter(zipkinOpt =>
    {
        zipkinOpt.Endpoint = new Uri("http://localhost:9411/api/v2/spans");
    })
    .Build();

ServiceHelper serviceHelper = new();
serviceHelper.Work1();
serviceHelper.Work2();