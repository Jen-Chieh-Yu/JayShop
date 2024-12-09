using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Text;

namespace JayShop.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public int Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
        public string? Url { get; set; }
    }
}
