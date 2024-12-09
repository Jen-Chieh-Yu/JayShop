using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections;
using System.Collections.Generic;
using JayShop.DBConnection;
using JayShop.Functions;
namespace JayShop.Models
{
    [Serializable]
    public class Cart
    {
        public List<CartItem> cartItems { get; }
        public Cart()
        {
            this.cartItems = new List<CartItem>();
        }

        public int Quantity
        {
            get
            {
                int total = 0;
                foreach (var item in this.cartItems)
                {
                    total += item.Quantity;
                }
                return total;
            }
        }

        public int Subtotal
        {
            get
            {
                int subTotal = 0;
                foreach (var item in this.cartItems)
                {
                    subTotal = subTotal + item.Subtotal;
                }
                return subTotal;
            }
        }

        public int DeliveryFee
        {
            get
            {
                int deliveryFee = (Subtotal >= 500) ? 0 : 60;
                return deliveryFee;
            }
        }
        public int Total
        {
            get
            {
                int total = 0;
                total = Subtotal + DeliveryFee;
                return total;
            }
        }

        public void AddItem(int ID)
        {
            var cartItem = this.cartItems;
            var tmpItem = cartItem.Where(item => item.ID == ID).Select(item => item).FirstOrDefault();

            if (tmpItem == default(CartItem))
            {
                using (DataBaseConnection dBConnection = new DataBaseConnection())
                {
                    var product = dBConnection.Products;
                    var targetProduct = product.Where(p => p.ID == ID).Select(p => p).FirstOrDefault();
                    if (targetProduct != default(Product))
                    {
                        this.AddItem(targetProduct);
                    }
                }
            }
            else
            {
                tmpItem.Quantity += 1;
            }
        }

        private void AddItem(Product product)
        {
            var cartItem = new CartItem()
            {
                ID = product.ID,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1,
                ImgUrl = product.Url
            };
            this.cartItems.Add(cartItem);
        }

        public void RemoveItem(int ID)
        {
            var cartItem = this.cartItems;
            var tmpItem = cartItem.Where(item => item.ID == ID).Select(item => item).FirstOrDefault();
            if (tmpItem != default(CartItem))
            {
                cartItem.Remove(tmpItem);
            }
        }

        public void PlusItem(int ID)
        {
            var cartItem = this.cartItems;
            var tmpItem = cartItem.Where(item => item.ID == ID).Select(item => item).FirstOrDefault();
            tmpItem.Quantity += 1;
        }

        public void MinusItem(int ID)
        {
            var cartItem = this.cartItems;
            var tmpItem = cartItem.Where(item => item.ID == ID).FirstOrDefault();
            if (tmpItem.Quantity > 1)
            {
                tmpItem.Quantity -= 1;
            }
        }

        public List<OrderDetails> ToOrderDetail(int orderID)
        {
            var orderDetail = new List<OrderDetails>();
            foreach (var item in this.cartItems)
            {
                OrderDetails detail = new OrderDetails()
                {
                    OrderID = orderID,
                    ProductID = item.ID,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                orderDetail.Add(detail);
            }
            return orderDetail;
        }
    }
}
