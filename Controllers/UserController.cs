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
        [HttpPost]
        public IActionResult Login(string account, string password)
        {
            var loginResult = _userService.Login(account, password);

            if (loginResult == true)
            {
                return View();
            }
            else
            {
                return View();
            }
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(string newPassowrd)
        {
            var currentUser = _userService.GetUser();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                var revisedResult = _userService.RevisePassword(newPassowrd);

                if (revisedResult == true)
                {
                    return View();
                }
                else
                {
                    return View();
                }
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            var registerResult = _userService.Register(user);

            if (registerResult == false)
            {
                return View("Register");
            }
            else
            {
                return View("RegisterSuccessful");
            }
        }
        [HttpPost]
        public IActionResult Logout()
        {
            _userService.Logout();

            return RedirectToAction("Homepage", "Home");
        }
    }
}
