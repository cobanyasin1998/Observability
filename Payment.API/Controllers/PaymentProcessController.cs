using Common.Shared.DTOs;
using Common.Shared.DTOs.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentProcessController : ControllerBase
    {
        [HttpPost]
        public PaymentCreateProcessResponseDto Payment(PaymentCreateProcessRequestDto request)
        {

            const decimal balance = 1000;
            if (request.TotalPrice > balance)
            {
                return new PaymentCreateProcessResponseDto() { Description = "Yetersiz Bakiye" };
            }

            return new PaymentCreateProcessResponseDto() { Description = "Başarılı" };
        }
    }
}
