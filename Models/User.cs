using JayShop.DBConnection;

namespace JayShop.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set;}
        public string? Password { get; set; }
        public DateTime BirthDay { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
