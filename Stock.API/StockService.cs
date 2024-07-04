using Common.Shared.DTOs;
using Common.Shared.DTOs.Payment;
using Common.Shared.DTOs.Stock;
using Stock.API.Services;
using System.Net;

namespace Stock.API
{
    public class StockService
    {
        private readonly PaymentService _paymentService;

        public StockService(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public static Dictionary<int, int> GetProductStockList()
        {
            return new Dictionary<int, int>
            {
                { 1, 100 },
                { 2, 200 },
                { 3, 300 },
                { 4, 400 },
                { 5, 500 }
            };
        }

        public async Task<ResponseDto<StockCheckAndPaymentProcessResponseDto>> CheckAndPaymentProcess(StockCheckAndPaymentProcessRequestDto stockCheckAndPaymentProcessRequestDto)
        {
            var stockList = GetProductStockList();
            var stockStatus = new List<(int productId, bool hasStockExists)>();
            foreach (var item in stockCheckAndPaymentProcessRequestDto.Items)
            {
                stockStatus.Add((item.ProductId, stockList.ContainsKey(item.ProductId) && stockList[item.ProductId] >= item.Quantity));
            }
            if (stockStatus.Any(x => x.hasStockExists == false))
            {
                return ResponseDto<StockCheckAndPaymentProcessResponseDto>
                    .Fail(HttpStatusCode.BadRequest.GetHashCode(), "Stock is not enough for some products");
            }
            var paymentResponse = await _paymentService.CreatePaymentProcess(new PaymentCreateProcessRequestDto()
            {
                OrderCode = stockCheckAndPaymentProcessRequestDto.OrderCode,
                TotalPrice = stockCheckAndPaymentProcessRequestDto.Items.Sum(x => x.Quantity * x.UnitPrice)
            });
            if (paymentResponse.isSuccess)
            {
                return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Success(HttpStatusCode.OK.GetHashCode(), new StockCheckAndPaymentProcessResponseDto()
                {
                    Description = "Payment is successful"
                });
            }
           return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Fail(HttpStatusCode.BadRequest.GetHashCode(), paymentResponse.failMessage);
         



            // Payment process will be implemented here
        }

    }
}