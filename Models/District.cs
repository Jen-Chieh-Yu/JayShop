using System.ComponentModel.DataAnnotations;

namespace JayShop.Models
{
    public class District
    {
        public int CityCode { get; set; }
        [Key]
        public int DistrictCode { get; set; }
        public string DistrictName { get; set;}
    }
}
