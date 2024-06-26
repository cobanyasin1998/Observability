﻿using Microsoft.AspNetCore.Mvc;
using Order.API.OrderServices;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Order API");
        }
        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequestDto requestDto)
        {
            await _orderService.CreateAsync(requestDto);
            return Ok(new OrderCreateResponseDto() { Id = 500_000 });
        }



        [HttpGet("GetException")]
        public IActionResult GetException()
        {
            var a = 10;
            var b = 0;
            var c = a / b;
            return Ok("Order API");
        }
    }
}
