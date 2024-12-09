using JayShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace JayShop.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }
    }
}
