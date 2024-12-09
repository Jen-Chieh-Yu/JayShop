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
                return Ok();
            }
            return Ok(currentCart.ToJson());
        }
        [HttpPut("AddToCart")]
        public IActionResult AddToCart(int product_id)
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.AddToCart(product_id);
            }
            return NoContent();
        }
        [HttpPut("RemoveFromCart")]
        public IActionResult RemoveFromCart([FromBody] CartItem cartItem)
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.RemoveFromCart(cartItem.ID);
            }
            return NoContent();
        }
        [HttpPut("IncreaseItem")]
        public IActionResult IncreaseItem([FromBody] CartItem cartItem)
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.IncreaseItem(cartItem.ID);
                return Ok();
            }
            return NoContent();
        }
        [HttpPut("DecreaseItem")]
        public IActionResult DecreaseItem([FromBody] CartItem cartItem)
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.DecreaseItem(cartItem.ID);
            }
            return NoContent();
        }
        public IActionResult CleanCart()
        {
            var currentCart = _cartService.GetCart();
            if (currentCart != null)
            {
                _cartService.CleanCart();
            }
            return NoContent();
        }
    }
}
