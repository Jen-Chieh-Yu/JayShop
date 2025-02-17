using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JayShop.Models;
using JayShop.DBConnection;
using JayShop.Services;

namespace JayShop.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchApiController : ControllerBase
    {
        private readonly SearchService _searchService;
        public SearchApiController(SearchService searchService)
        {
            _searchService = searchService;
        }
        [HttpGet("Search")]
        public IActionResult Search(string keyword)
        {
            var searchResult = _searchService.Search(keyword);

            if (searchResult.Count == 0 || searchResult == null)
            {
                return NoContent();
            }
            return Ok(new { success = true, searchResult = searchResult });
        }
    }
}
