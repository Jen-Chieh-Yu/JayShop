using Microsoft.AspNetCore.Mvc;
using JayShop.DBConnection;
using JayShop.Services;
using JayShop.Models;
using JayShop.ViewModel;

namespace JayShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly SessionService _sessionService;
        private string _sessionKey = "order";
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService, 
                                                    CartService cartService, 
                                                    SessionService sessionService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _sessionService = sessionService;

        }
        public IActionResult Order()
        {
            var currentCart = _cartService.GetCart();

            if (currentCart != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Homepage", "Home");
            }
        }

        [HttpPost]
        public IActionResult SendOder()
        {
            //var currentOrder = _sessionService.GetObjectFromJson<Order>(SESSION_KEY);
            var currentOrder = _orderService.GetOrder();
            if (currentOrder != null)
            {
                _orderService.SendOrder(currentOrder);
                _cartService.CleanCart();
                _orderService.CleanOrder();
                return RedirectToAction("FinishOrder");
            }
            return View();
        }

        public IActionResult FinishOrder()
        {
            return View();
        }

        public IActionResult SearchOrder()
        {
            var currentUser = _sessionService.GetObjectFromJson<User>("user");
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var order = _orderService.SearchOrder(currentUser.UserID);
                return View();
            }
        }
    }
}
