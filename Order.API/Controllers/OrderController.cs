using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Order API");
        }
        [HttpGet("GetException")]
        public IActionResult GetException()
        {
            var a = 10;
            var b  = 0;
            var c = a / b;
            return Ok("Order API");
        }
    }
}
