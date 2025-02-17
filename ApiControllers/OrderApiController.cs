using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JayShop.Models;
using JayShop.Services;

namespace JayShop.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        private readonly SessionService _sessionService;
        private readonly OrderService _orderService;
        private const string SESSION_KEY = "order";
        public OrderApiController(SessionService sessionService,
                                                            OrderService orderService)
        {
            _sessionService = sessionService;
            _orderService = orderService;
        }
        [HttpGet("GetOrder")]
        public IActionResult GetOrder()
        {
            var currentOrder = _orderService.GetOrder();
            return Ok(new { success = true, order = currentOrder });
        }
        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                Console.WriteLine("Order object is null.");
                return NoContent();
            }
            else
            {
                _orderService.CreateOrder(order);
                return Ok(new { success = true, href = "/Order/Order" });
            }
        }
        public IActionResult SendOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return NoContent();
            }
            else
            {
                var result = _orderService.SendOrder(order);
                if (result == true)
                {
                    _sessionService.SetObjectAsJson(SESSION_KEY, order);
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false });
                }
            }
        }
        public IActionResult SearchOrder(int userID)
        {
            var order = _orderService.SearchOrder(userID);

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(order);
            }
        }
        //[HttpPost("CleanOrder")]
        //public IActionResult CleanOrder()
        //{
        //    var currentOrder = _orderService.GetOrder();
        //    if (currentOrder != null)
        //    {
        //        _orderService.CleanOrder();
        //        return NoContent();
        //    }
        //    return NoContent();
        //}
    }
}
