using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using JayShop.Controllers;
using JayShop.Services;
using JayShop.Models;

namespace JayShop.ViewComponents
{
    [ViewComponent(Name = "CartView")]
    public class ShoppingCartViewComponent : ViewComponent
    { 
        private readonly Cart cart;
        public ShoppingCartViewComponent(Cart cart)
        {
            this.cart = cart;
        }

        //public async Task<IViewComponentResult> InvokeAsync()
        //{
        //    var cart = SessionService.GetObjectFromJson<Cart>(HttpContext.Session, "cart");
        //    return View(cart);
        //}
                
        //public async Task<IViewComponentResult> InvokeAsync(string actionName, int ID)
        //{
        //    switch (actionName)
        //    {
        //        case "AddItem":
        //            cart.AddItem(ID);
        //            break;

        //        case "RemoveItem":

        //            break;

        //        case "PlusItem":

        //            break;

        //        case "MinusItem":

        //            break;

        //        default:
        //            break;
        //    }
            
        //    return View();
        //}
    }
}
