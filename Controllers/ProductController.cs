using Microsoft.AspNetCore.Mvc;

namespace JayShop.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Products()
        {
            return View();
        }
        public IActionResult ProductInformation()
        {
            return View();
        }
        public IActionResult Test()
        {
            return View();
        }
    }
}
