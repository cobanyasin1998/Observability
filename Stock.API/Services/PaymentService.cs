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
            var response = await _httpClient.PostAsJsonAsync<PaymentCreateProcessRequestDto>("api/PaymentProcess", paymentCreateProcessRequestDto);
            //var result = await response.Content.ReadFromJsonAsync<PaymentCreateProcessResponseDto>();
            return response.IsSuccessStatusCode ? (true, null) : (false, "");
        }
    }
}
