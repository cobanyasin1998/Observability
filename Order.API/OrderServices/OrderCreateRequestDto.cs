namespace Order.API.OrderServices
{
    public record OrderCreateRequestDto()
    {
        public Guid UserId { get; set; } 
        public List<OrderItemDto> Items { get; set; } = null!;
    }
    public record OrderItemDto()
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
