using JayShop.DBConnection;
using System.ComponentModel.DataAnnotations;

namespace JayShop.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set;} = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime BirthDay { get; set; }
        public string? Phone { get; set; }
    }
}
