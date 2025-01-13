using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Data.Common;
using JayShop.DBConnection;
using JayShop.Services;
using JayShop.Models;
using JayShop.ViewModel;
using JayShop.Functions;
using JayShop.ApiControllers;
using Humanizer;

namespace JayShop.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult MemberCenter()
        {
            var user = _userService.GetUser();

            if (user == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Logout()
        {
            _userService.Logout();

            return RedirectToAction("Homepage", "Home");
        }
    }
}
