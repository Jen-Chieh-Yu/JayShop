using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JayShop.DBConnection;
using JayShop.Functions;
using JayShop.Models;
using JayShop.ViewModel.User;

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
        public IActionResult Login([FromBody] LoginViewModel loginViewModel)
        {
            var loginResult = _userService.Login(loginViewModel);
            var success = loginResult.success;
            var errors = loginResult.errors;

            if (success == true)
            {
                var herf = "/Home/HomePage";
                return Ok(new { success = success, errors = errors, href= herf });
            }
            else
            {
                return BadRequest(new { success = success, errors = errors });
            }
        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            _userService.Logout();
            return NoContent();
        }
        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterViewModel registerViewModel)
        {
            var registerResult = _userService.Register(registerViewModel);
            var success = registerResult.success;
            var errors = registerResult.errors;

            if (success == true)
            {
                return Ok(new { success = success, errors });
            }
            else
            {
                return BadRequest(new { success = false, errors });
            }
        }
        [HttpPost("RevisePassword")]
        public IActionResult RevisePassword([FromBody] RevisePasswordViewModel revisePasswordViewModel)
        {
            if (revisePasswordViewModel != null)
            {
                var revisedResult = _userService.RevisePassword(revisePasswordViewModel);
                if (revisedResult == true)
                {
                    return Ok(new { success = true, message = "Password updated successfully." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Password updated unsuccessfully ." });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "Request body cannot be null." });
            }
        }
        [HttpPost("SaveRegisterForm")]
        public IActionResult SaveRegisterForm([FromBody] RegisterViewModel registerViewModel)
        {
            if (registerViewModel != null)
            {
                _userService.SaveRegisterForm(registerViewModel);
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false });
            }
        }
        [HttpGet("GetRegisterForm")]
        public IActionResult GetRegisterForm()
        {
            var registerForm = _userService.GetFormData();

            if (registerForm != null)
            {
                return Ok(new { success = true, registerForm });
            }
            else
            {
                return NoContent();
            }
        }
    }
}
