//using Microsoft.AspNetCore.Mvc;
//using JayShop.Models;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using JayShop.Services;

//namespace JayShop.Controllers
//{
//    public class ShoppingCart : Controller
//    {
//        public IActionResult AddToCart(int ID)
//        {
//            var currentCart = SessionService.GetObjectFromJson<Cart>(HttpContext.Session, "cart");

//            if (currentCart == null)
//            {
//                Cart cart = new Cart();
//                cart.AddItem(ID);
//                SessionService.SetObjectAsJson(HttpContext.Session, "cart", cart);
//            }
//            else
//            {
//                currentCart.AddItem(ID);
//                SessionService.SetObjectAsJson(HttpContext.Session, "cart", currentCart);
//            }
//            return NoContent();
//        }

//        public IActionResult RemoveFromCart(int ID)
//        {
//            var currentCart = SessionService.GetObjectFromJson<Cart>(HttpContext.Session, "cart");
//            currentCart.RemoveItem(ID);
//            SessionService.SetObjectAsJson(HttpContext.Session, "cart", currentCart);
//            return NoContent();
//        }

//        public IActionResult PlusItem(int ID)
//        {
//            var currentCart = SessionService.GetObjectFromJson<Cart>(HttpContext.Session, "cart");
//            currentCart.PlusItem(ID);
//            SessionService.SetObjectAsJson(HttpContext.Session, "cart", currentCart);
//            return NoContent();
//        }

//        public IActionResult MinusItem(int ID)
//        {
//            var currentCart = SessionService.GetObjectFromJson<Cart>(HttpContext.Session, "cart");
//            currentCart.MinusItem(ID);
//            SessionService.SetObjectAsJson(HttpContext.Session, "cart", currentCart);
//            return NoContent();
//        }
//    }
//}
