using Microsoft.AspNetCore.Mvc;
using Order.API.OrderServices;
using Order.API.RedisServices;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly RedisService _redisService;
        public OrderController(OrderService orderService, RedisService redisService)
        {
            _orderService = orderService;
            _redisService = redisService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Order API");
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequestDto requestDto)
        {
            var result = await _orderService.CreateAsync(requestDto);
            return new ObjectResult(result) { StatusCode = result.StatusCode };
        }



        [HttpGet("GetException")]
        public async Task<IActionResult> GetException()
        {
            var result = await _redisService.SetAsync("Normal"," ex.Message");

            try
            {
                var a = 10;
                var b = 0;
                var c = a / b;
            }
            catch (Exception ex)
            {
                var result2 = await _redisService.SetAsync("Exception", ex.Message);
                return new ObjectResult(result) { StatusCode = 500 };
            }

            return Ok("Order API");
        }
    }
}
