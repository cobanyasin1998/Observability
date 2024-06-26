using Observability.ConsoleApp;
using Observability.ConsoleApp.Consts;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var traceProvider = Sdk.CreateTracerProviderBuilder()
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
    .Build();

ServiceHelper serviceHelper = new();
serviceHelper.Work1();