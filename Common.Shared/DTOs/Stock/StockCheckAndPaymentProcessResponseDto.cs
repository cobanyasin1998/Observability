namespace Common.Shared.DTOs.Stock
{
    public record StockCheckAndPaymentProcessResponseDto
    {
        public string Description { get; set; } = null!;
        public bool IsSuccess { get; set; }

    }
}
