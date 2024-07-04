using Common.Shared.DTOs.Stock;
using Microsoft.AspNetCore.Mvc;

namespace Stock.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockService _stockService;
        public StockController(StockService stockService)
        {
            _stockService = stockService;
        }
        [HttpPost]
        public async Task<IActionResult> CheckAndPaymentStart(StockCheckAndPaymentProcessRequestDto stockCheckAndPaymentProcessRequestDto)
        {
            var result = await _stockService.CheckAndPaymentProcess(stockCheckAndPaymentProcessRequestDto);

            return new ObjectResult(result) { StatusCode = result.StatusCode };

        }
    }
}
