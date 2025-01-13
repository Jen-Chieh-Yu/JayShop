using System.ComponentModel.DataAnnotations;

namespace JayShop.ViewModel.User
{
    public class RegisterViewModel
    {
        public string LastName { get; set; }=string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Email {  get; set; }  = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword {  get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
