using Common.Shared;
using Observability.Shared;
using Stock.API;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<StockService>();
builder.Services.AddOpenTelemetryExtension(builder.Configuration);
builder.Services.AddScoped<PaymentService>();

builder.Services.AddHttpClient<PaymentService>(opt =>
{
    opt.BaseAddress = new Uri((builder.Configuration.GetSection("ApiServices")["PaymentService"])!);

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<RequestAndResponseActivityMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
