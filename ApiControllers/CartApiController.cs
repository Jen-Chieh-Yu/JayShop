using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using JayShop.Services;
using JayShop.Models;
using NPOI.SS.UserModel;

namespace JayShop.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private ISession _session => HttpContext.Session;
        private const string _sessionKey = "cart";
        private readonly CartService _cartService;
        private readonly SessionService _sessionService;
        public CartApiController(CartService cartService, SessionService sessionService)
        {
            _cartService = cartService;
            _sessionService = sessionService;
        }
        [HttpGet("GetShoppingCart")]
        public IActionResult GetShoppingCart()
        {
            var currentCart = _cartService.GetCart();
            if (currentCart == null)
            {
                return NoContent();
            }
            else
            {
                var response = new { success = true, currentCart = currentCart };
                return Ok(response);
            }
        }
        [HttpPut("AddToCart")]
        public IActionResult AddToCart(int product_id, int product_quantity)
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.AddToCart(product_id, product_quantity);
                var response = new { success = true };
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("DeleteFromCart")]
        public IActionResult DeleteFromCart([FromBody] CartItem cartItem)
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.DeleteFromCart(cartItem.ID);
                var response = new { success = true };

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("IncreaseItem")]
        public IActionResult IncreaseItem([FromBody] CartItem cartItem)
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.IncreaseItem(cartItem.ID);
                var response = new { success = true };

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPut("DecreaseItem")]
        public IActionResult DecreaseItem([FromBody] CartItem cartItem)
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.DecreaseItem(cartItem.ID);
                var response = new { success = true };

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult CleanCart()
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.CleanCart();
                var response = new { success = true };

                return Ok(response);
            }
            else
            {
                var response = new { success = false };

                return BadRequest(response);
            }
        }
    }
}
