using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JayShop.DBConnection;
using JayShop.Services;
using JayShop.Models;

namespace JayShop.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseConnection _db;
        private readonly CartService _cartService;
        private readonly SearchService _searchService;
        private ISession _session => HttpContext.Session;

        public HomeController(ILogger<HomeController> logger, DataBaseConnection db, CartService cartService, SearchService searchService)
        {
            _logger = logger;
            _db = db;
            _cartService = cartService;
            _searchService = searchService;
        }

        public IActionResult Homepage()
        {
                return View();
        }

        public IActionResult Products(int type)
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Search()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ShoppingCart()
        {
            return View();
        }
        public IActionResult ProductInformation(int product_id)
        {       
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}