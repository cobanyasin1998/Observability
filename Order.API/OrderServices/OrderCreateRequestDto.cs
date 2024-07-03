using Common.Shared.DTOs.Order;

namespace Order.API.OrderServices
{
    public record OrderCreateRequestDto()
    {
        public List<OrderItemDto> Items { get; set; } = null!;
    }
   
}
