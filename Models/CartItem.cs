namespace JayShop.Models
{
    [Serializable]
    public class CartItem
    {
        public int ID { get; set; }
        public string? ImgUrl { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int Subtotal
        {
            get
            {
                return this.Price * this.Quantity;
            }
        }
    }
}
