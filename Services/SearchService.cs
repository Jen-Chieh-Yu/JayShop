using JayShop.DBConnection;
using JayShop.Models;

namespace JayShop.Services
{
    public class SearchService
    {
        private readonly DataBaseConnection _db;
        public SearchService(DataBaseConnection db)
        {
            _db = db;
        }
        public List<Product> Search(string keyword)
        {
            var spaceKey = " ";
            var keywords = keyword.Trim().Split(spaceKey, StringSplitOptions.RemoveEmptyEntries);
            var searchResult = _db.Products.AsEnumerable()
                                                        .Where(product => keywords.Any(kw => product.Name.Contains(kw)))
                                                        .ToList();
            return searchResult;
        }
    }
}
