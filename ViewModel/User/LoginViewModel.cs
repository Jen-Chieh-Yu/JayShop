using System.ComponentModel.DataAnnotations;

namespace JayShop.ViewModel.User
{
    public class LoginViewModel
    {
        public string Email { get; set; }=string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
