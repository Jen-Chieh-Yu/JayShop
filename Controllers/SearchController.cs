using Microsoft.AspNetCore.Mvc;

namespace JayShop.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Search()
        {
            return View();
        }
    }
}
