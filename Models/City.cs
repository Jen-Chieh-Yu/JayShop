using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace JayShop.Models
{
    public class City
    {
        [Key]
        public int CityCode {  get; set; }
        public string CityName { get; set; } = string.Empty;
    }
}
