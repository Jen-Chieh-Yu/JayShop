using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using JayShop.DBConnection;
using JayShop.Services;
using JayShop.ViewModel;

namespace JayShop.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductApiController(ProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            var type = 0;
            var products = _productService.GetProducts(type);
            return Ok(products);
        }
        [HttpGet("Products")]
        public IActionResult Products(int type)
        {
            var products = _productService.GetProducts(type);
            return Ok(products);
        }
        [HttpGet("GetProductInformation")]
        public IActionResult GetProductInformation(int product_id)
        {
            var productInformation = _productService.GetProductInformation(product_id);
            if (productInformation == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(productInformation);
            }
        }
    }
}
