using Microsoft.AspNetCore.Mvc;
using JayShop.DBConnection;
using JayShop.Services;

namespace JayShop.ViewComponents
{

    [ViewComponent(Name = "ProductView")]
    public class ProductViewComponent : ViewComponent
    {
        private DataBaseConnection context;
        private readonly ProductService productService;

        public ProductViewComponent(DataBaseConnection context)
        {
            this.context = context;
        }
        //private ProductViewComponent(ProductService _productService)
        //{
        //    productService = _productService; 
        //}

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var product = this.context.Products.ToList();
            return View("_ProductComponent", product);
            //var product = productService.GetProduct();
            //return View("_ProductComponent", product);
        }
    }
}
