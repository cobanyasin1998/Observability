using Observability.Shared;
using System.Diagnostics;

namespace Order.API.OrderServices
{
    public class OrderService
    {
        public Task CreateAsync(OrderCreateRequestDto orderCreateRequestDto)
        {
            Activity.Current?.SetTag("Asp.NetCore Tag1","Tag1 value");
           using var activity = ActivitySourceProvider.Source.StartActivity();

           activity?.AddEvent(new("OrderService.CreateAsync started"));
           activity?.SetTag("order User Id: ", orderCreateRequestDto.UserId);

           activity?.AddEvent(new("OrderService.CreateAsync finished"));


            return Task.CompletedTask;
        }
    }
}
