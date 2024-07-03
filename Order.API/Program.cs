using Common.Shared;
using Microsoft.EntityFrameworkCore;
using Observability.Shared;
using Order.API.Models;
using Order.API.OrderServices;
using Order.API.StockServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<StockService>(); 
builder.Services.AddHttpClient<StockService>(opt =>
{
    opt.BaseAddress = new Uri((builder.Configuration.GetSection("ApiServices")["StockService"])!);

});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddOpenTelemetryExtension(builder.Configuration);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<RequestAndResponseActivityMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
