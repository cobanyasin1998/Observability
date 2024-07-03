using Common.Shared.DTOs.Order;

namespace Common.Shared.DTOs.Stock
{
    public record StockCheckAndPaymentProcessRequestDto
    {
        public string OrderCode { get; set; } =null!;
        public List<OrderItemDto> Items { get; set; } = null!;
    }
}
