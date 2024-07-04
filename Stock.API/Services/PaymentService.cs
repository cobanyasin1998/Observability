using Common.Shared.DTOs.Stock;
using Common.Shared.DTOs;
using Common.Shared.DTOs.Payment;

namespace Stock.API.Services
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;
        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<(bool isSuccess, string? failMessage)> CreatePaymentProcess(PaymentCreateProcessRequestDto paymentCreateProcessRequestDto)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentCreateProcessRequestDto>("api/PaymentProcess/ProcessPayment", paymentCreateProcessRequestDto);
            var result = await response.Content.ReadFromJsonAsync<ResponseDto<StockCheckAndPaymentProcessResponseDto>>();
            return response.IsSuccessStatusCode ? (true, null) : (false, result!.Errors!.First());
        }
    }
}
