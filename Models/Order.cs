using Microsoft.CodeAnalysis.CSharp.Syntax;
using JayShop.DBConnection;
using System.ComponentModel.DataAnnotations;

namespace JayShop.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int Total { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public int DeliveryMethod { get; set; }
        public int CityCode { get; set; }
        public int DistrictCode { get; set; }
        public string StreetAddress { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string? Memo { get; set; }
        public string? DateTime { get; set; }

        public List<OrderDetails> OrderDetails { get; } = new List<OrderDetails>();
    }
}
