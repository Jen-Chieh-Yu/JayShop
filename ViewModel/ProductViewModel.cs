using JayShop.Models;

namespace JayShop.ViewModel
{
    public class ProductViewModel
    {
        public Product? TargetProduct { get; set; }
        public List<Product>? RelevantProducts { get; set; }
    }
}
