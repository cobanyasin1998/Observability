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
        public async Task<IActionResult> ProcessPayment(PaymentCreateProcessRequestDto request)
        {

            const decimal balance = 1000;
            if (request.TotalPrice > balance)
            {
                return BadRequest(ResponseDto<PaymentCreateProcessResponseDto>.Fail(400, "Yetersiz Bakiye"));
            }

            return Ok(ResponseDto<PaymentCreateProcessResponseDto>.Success(200, new PaymentCreateProcessResponseDto() { Description = "Başarılı" }));
        }
    }
}
