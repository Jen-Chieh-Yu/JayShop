using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JayShop.DBConnection;
using JayShop.Functions;
using JayShop.Models;

namespace JayShop.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly DataBaseConnection _db;
        private readonly UserService _userService;
        public UserApiController(DataBaseConnection db, UserService userService)
        {
            _db = db;
            _userService = userService;
        }
        [HttpPost("Login")]
        public IActionResult Login()
        {

            return Ok();
        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            _userService.Logout();
            return NoContent();
        }
        //[HttpPost("Register")]
        //public IActionResult Register([FromBody] User user)
        //{
        //    var registerResult = _userService.Register(user);
        //    if (registerResult == true) {
        //        return Ok();
        //    }
        //}
    }
}
