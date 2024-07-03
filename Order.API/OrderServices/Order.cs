namespace Order.API.OrderServices
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = null!;
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public EOrderStatus Status { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public enum EOrderStatus : byte
    {

        Success = 1,
        Failed = 2
    }
}
