using Common.Shared.Events;
using MassTransit;
using System.Diagnostics;
using System.Text.Json;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        public  Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderCode = context.Message.OrderCode;

            Thread.Sleep(2000);

            Activity.Current?.SetTag("OrderCreatedEventConsumer.message.body", JsonSerializer.Serialize(context.Message));

            Console.WriteLine($"OrderCreatedEventConsumer: {orderCode}");

            return Task.CompletedTask;
        }
    }
}
