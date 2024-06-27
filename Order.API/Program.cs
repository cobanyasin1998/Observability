using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Order.API.OpenTelemetry;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpenTelemetryOptions>(builder.Configuration.GetSection("OpenTelemetry"));

OpenTelemetryOptions openTelemetryOptions = (builder.Configuration.GetSection("OpenTelemetry").Get<OpenTelemetryOptions>())!;

builder.Services.AddOpenTelemetry()
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
ActivitySourceProvider.Source = new ActivitySource(openTelemetryOptions.ActivitySourceName);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
