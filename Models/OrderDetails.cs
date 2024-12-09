using System.ComponentModel.DataAnnotations;

namespace JayShop.Models
{
    [Serializable]    
    public class OrderDetails
    {
        [Key]
        public int OrderID {  get; set; }
        public int ProductID {  get; set; }
        public string? ImgUrl {  get; set; }        
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Subtotal {  get; set; }
    }
}
