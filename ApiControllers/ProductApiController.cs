using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using JayShop.DBConnection;
using JayShop.ViewModel;
using JayShop.Services;

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
        public IActionResult GetProducts(int type)
        {            
            var products = _productService.GetProducts(type);
            var response = default(object);

            if (products != null)
            {
                response = new { success = true, products = products };
            }
            else
            {
                response = new { success = false, products = products };
            }
            return Ok(response);
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
                var response = new { success = true, productInformation = productInformation };
                return Ok(response);
            }
        }
    }
}
