using Common.Shared.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.OrderServices;
using Order.API.RedisServices;

namespace Order.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly RedisService _redisService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderController> _logger;

        public OrderController(OrderService orderService, RedisService redisService, IPublishEndpoint publishEndpoint, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _redisService = redisService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
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

        [HttpGet]
        public async Task<IActionResult> SendOrderCreatedEvent()
        {

            _logger.LogInformation("OrderCreatedEvent is sending to RabbitMQ: @{userId}",new Guid().ToString());

            //Kuyruğa mesaj gönderme işlemi
            await _publishEndpoint.Publish(new OrderCreatedEvent
            {
                OrderCode = new Random().Next(0, 500).ToString()
            });
            return Ok("Order API");
        }

         
        [HttpGet("GetException")]
        public async Task<IActionResult> GetException()
        {
            var result = await _redisService.SetAsync("Normal", " ex.Message");

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
