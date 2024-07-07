using Common.Shared.DTOs;
using Common.Shared.DTOs.Stock;

namespace Order.API.StockServices
{
    public class StockService
    {
        private readonly HttpClient _httpClient;
        public StockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<(bool isSuccess, string? failMessage)> CheckAndPaymentProcess(StockCheckAndPaymentProcessRequestDto stockCheckAndPaymentProcessRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Stock/CheckAndPaymentStart", stockCheckAndPaymentProcessRequestDto);
           // var result = await response.Content.ReadFromJsonAsync<ResponseDto<StockCheckAndPaymentProcessResponseDto>>();
            return response.IsSuccessStatusCode ? (true,null) : (true,"");
        }
    }
}
