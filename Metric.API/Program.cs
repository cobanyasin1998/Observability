using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
               .WithMetrics(opt =>
                {
                    opt.AddMeter("Metric.API", "v1");
                    opt.ConfigureResource(resource =>
                    {
                        resource.AddService("Metric.API", serviceVersion: "v1");
                    });
                    opt.AddPrometheusExporter();
                });


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
app.UseOpenTelemetryPrometheusScrapingEndpoint();
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
