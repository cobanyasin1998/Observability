using Common.Shared.DTOs;
using Common.Shared.DTOs.Stock;
using System.Net;

namespace Stock.API
{
    public class StockService
    {
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

        public ResponseDto<StockCheckAndPaymentProcessResponseDto> CheckAndPaymentProcess(StockCheckAndPaymentProcessRequestDto stockCheckAndPaymentProcessRequestDto)
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
            return ResponseDto<StockCheckAndPaymentProcessResponseDto>.Success(HttpStatusCode.OK.GetHashCode(), new StockCheckAndPaymentProcessResponseDto()
            {
                Description = "Stock is enough for all products",
            });
            // Payment process will be implemented here
        }

    }
}