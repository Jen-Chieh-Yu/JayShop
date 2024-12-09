namespace JayShop.Services;

using Microsoft.VisualStudio.Utilities;
using NPOI.SS.UserModel;
using System.Linq;
using JayShop.DBConnection;
using JayShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using System.Data;

public class OrderService
{
    private readonly DataBaseConnection _db;
    private readonly SessionService _sessionService;
    private readonly CartService _cartService;
    private const string SESSION_KEY = "order";
    private Order? _order;
    private readonly AddressService _addressService;

    public OrderService(DataBaseConnection db,
                                            SessionService sessionService,
                                            CartService cartService,
                                            AddressService cityService)
    {
        _db = db ?? throw new ArgumentException(nameof(db),
                "Database connection cannot be null.");
        _cartService = cartService;
        _sessionService = sessionService;
        _addressService = cityService;
        InitializeOrder();
    }

    private void InitializeOrder()
    {
        _order = _sessionService.GetObjectFromJson<Order>(SESSION_KEY) ?? new Order();
        _sessionService.SetObjectAsJson(SESSION_KEY, _order);
    }

    public Order? GetOrder() => _order;

    public void CreateOrder([FromBody] Order order)
    {
        if (_order != null)
        {
            var cart = _cartService.GetCart();
            if (cart != null)
            {
                var address = _addressService.GetAddress(order.CityCode,
                                                                                                order.DistrictCode,
                                                                                                order.StreetAddress);
                _order.OrderID = 1;
                _order.UserID = 1;
                _order.Total = cart.Total;
                _order.ContactName = order.ContactName;
                _order.ContactPhone = order.ContactPhone;
                _order.DeliveryMethod = order.DeliveryMethod;
                _order.CityCode = order.CityCode;
                _order.DistrictCode = order.DistrictCode;
                _order.StreetAddress = order.StreetAddress;
                _order.DeliveryAddress = address;
                _order.Memo = order.Memo;
                DateTime now  = DateTime.Now;
                string formattedDateTime = now.ToString("yyyy-MM-dd HH:mm");
                _order.DateTime = formattedDateTime;
                this.CreateOrderDetails(cart);
                Console.WriteLine("Order is created.");
                _sessionService.SetObjectAsJson(SESSION_KEY, _order);
            }
            else
            {
                Console.WriteLine("Order is fail to create.");
            }
        }
        else
        {
            Console.WriteLine("The session of order is null.");
        }
    }

    private void CreateOrderDetails(Cart cart)
    {
        if (cart != null) { 
        var cartItems = cart.cartItems;
            if (_order != null)
            {
                // To avoid duplicate order details being inserted, clear the existing details.
                _order.OrderDetails.Clear();
                foreach (var cartItem in cartItems)
                {
                    var orderDetails = new OrderDetails
                    {
                        OrderID = _order.OrderID,
                        ProductID = cartItem.ID,
                        ImgUrl = cartItem.ImgUrl,
                        Quantity = cartItem.Quantity,
                        Subtotal = cartItem.Subtotal
                    };
                    _order.OrderDetails.Add(orderDetails);
                }
            }
        }
    }

    public bool SendOrder(Order order)
    {
        var result = false;
        if (order != null)
        {
            _db.Order.Add(order);
            _db.SaveChanges();
            _sessionService.RemoveSession(SESSION_KEY);
            result = true;
            return result;
        }
        else
        {
            return result;
        }
    }

    private void SaveSession()
    {
        if(_order != null)
        {
            _sessionService.SetObjectAsJson(SESSION_KEY, _order);
        }
    }

    public void CleanOrder()
    {
        if (_order != null)
        {
            _sessionService.RemoveSession(SESSION_KEY);
        }
    }

    public List<Order> SearchOrder(int userID)
    {
        var order = _db.Order.Where(order => order.UserID == userID).ToList();
        return order;
    }
}
