using NPOI.SS.Formula.Functions;
using System.IO;
using JayShop.DBConnection;
using JayShop.Models;
using JayShop.ViewModel;

namespace JayShop.Services
{
    public class ProductService
    {
        private readonly DataBaseConnection _db;
        public ProductService(DataBaseConnection db)
        {
            _db = db;
        }

        public List<Product> GetProducts(int productType)
        {
            if (productType == 0)
            {
                var products = _db.Products.ToList();
                return products;
            }
            else
            {
                var products = _db.Products.Where(product => product.Type == productType).ToList();
                return products;
            }
        }

        public ProductViewModel? GetProductInformation(int productID)
        {
            var targetProduct = _db.Products
                                                        .Where(product => product.ID == productID)
                                                        .Select(product => product)
                                                        .FirstOrDefault();

            if (targetProduct != null)
            {
                var type = targetProduct.Type;
                var relevantProudcts = _db.Products
                                                                    .Where(product => product.Type == type)
                                                                    .Select(product => product)
                                                                    .ToList();
                var productVM = new ProductViewModel
                {
                    TargetProduct = targetProduct,
                    RelevantProducts = relevantProudcts
                };
                return productVM;
            }
            return null;
        }
        //private List<Product> GetProductData()
        //{
        //    string filePath = "./wwwroot/photo/Merchandise/";
        //    string[] files = Directory.GetFiles(filePath);
        //    var products = new List<Product>();
        //    int i = 0;
        //    foreach (var file in files)
        //    {
        //        i++;
        //        string productName = Path.GetFileNameWithoutExtension(file);
        //        int price = 100;
        //        string fileName = Path.GetFileName(file);
        //        products.Add(new Product() { ID = i, Name = productName, Price = price, Url = "/photo/Merchandise/" + fileName });
        //    };
        //    return products;
        //}
        //public List<Product> GetProduct()
        //{
        //    var products = GetProductData();
        //    return products;
        //}
    }
}
