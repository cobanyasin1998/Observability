using Common.Shared;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Observability.Shared;
using Order.API.Models;
using Order.API.OrderServices;
using Order.API.RedisServices;
using Order.API.StockServices;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Logging.Shared.Logging.ConfigureLogging);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<StockService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisService = sp.GetRequiredService<RedisService>();
    return redisService.GetConnectionMultiplexer;

});
builder.Services.AddHttpClient<StockService>(opt =>
{
    opt.BaseAddress = new Uri((builder.Configuration.GetSection("ApiServices")["StockService"])!);

});
builder.Services.AddSingleton(_ =>
{
    return new RedisService(builder.Configuration);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddOpenTelemetryExtension(builder.Configuration);
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });
    });

});

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
