using Microsoft.AspNetCore.Http;
using JayShop.DBConnection;
using JayShop.Services;
using JayShop.Models;

namespace JayShop.Services
{
    public class CartService
    {
        private readonly DataBaseConnection _db;
        private readonly SessionService _sessionService;
        private const string SESSION_KEY = "cart";
        private Cart? _cart;

        public CartService(DataBaseConnection db, SessionService sessionService)
        {
            _db = db ?? throw new ArgumentException(nameof(db),
                "Database connection cannot be null.");
            _sessionService = sessionService;
            InitializeCart();
        }

        private void InitializeCart()
        {
            _cart = _sessionService.GetObjectFromJson<Cart>(SESSION_KEY) ?? new Cart();

            if (_cart.Quantity == 0)
            {
                this.InitializeCartWithDummyData();
            }
            _sessionService.SetObjectAsJson(SESSION_KEY, _cart);
        }

        private void InitializeCartWithDummyData()
        {
            for (var item_ID = 1; item_ID < 4; item_ID++)
            {
                this.AddToCart(item_ID);
            }
        }

        public Cart? GetCart() => _cart;

        public void AddToCart(int product_id)
        {
            if (_cart != null)
            {
                var cartItems = _cart.cartItems;
                var tempItem = cartItems.Where(item => item.ID == product_id)
                                                                .Select(item => item)
                                                                .FirstOrDefault();
                if (tempItem == null)
                {
                    var targetProduct = _db.Products.Where(product => product.ID == product_id)
                                                                                    .Select(product => product)
                                                                                    .FirstOrDefault();
                    if (targetProduct != null)
                    {
                        this.AddToCart(targetProduct);
                    }
                }
                else
                {
                    tempItem.Quantity += 1;
                }
                _sessionService.SetObjectAsJson(SESSION_KEY, _cart);
            }
        }

        private void AddToCart(Product product)
        {
            if (_cart != null)
            {
                var cartItem = new CartItem()
                {
                    ID = product.ID,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ImgUrl = product.Url
                };
                _cart.cartItems.Add(cartItem);
            }
        }

        public void IncreaseItem(int item_ID)
        {
            if (_cart != null)
            {
                var cartItem = _cart.cartItems;
                var tempItem = cartItem.Where(item => item.ID == item_ID)
                                                                .Select(item => item)
                                                                .FirstOrDefault();
                if (tempItem != null)
                {
                    tempItem.Quantity++;
                    _sessionService.SetObjectAsJson(SESSION_KEY, _cart);
                }
            }
        }

        public void DecreaseItem(int item_ID)
        {
            if (_cart != null)
            {
                var cartItem = _cart.cartItems;
                var tempItem = cartItem.Where(item => item.ID == item_ID)
                                                                .Select(item => item)
                                                                .FirstOrDefault();
                if (tempItem != null && tempItem.Quantity > 1)
                {
                    tempItem.Quantity--;
                    _sessionService.SetObjectAsJson(SESSION_KEY, _cart);
                }
            }
        }

        public void RemoveFromCart(int item_ID)
        {
            if (_cart != null)
            {
                var cartItem = _cart.cartItems;
                var tempItem = cartItem.Where(item => item.ID == item_ID)
                                                                .Select(item => item)
                                                                .FirstOrDefault();
                if (tempItem != null)
                { 
                    cartItem.Remove(tempItem);
                    _sessionService.SetObjectAsJson(SESSION_KEY, _cart);
                }
            }
        }

        public void CleanCart()
        {
            if (_cart != null)
            {
                _sessionService.RemoveSession(SESSION_KEY);
            }
        }
    }
}
