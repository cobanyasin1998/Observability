namespace Common.Shared.DTOs.Payment
{
    public class PaymentCreateProcessRequestDto
    {
        public string OrderCode { get; set; } = null!;
        public decimal TotalPrice { get; set; }
    }
}
