using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats;
using NPOI.SS.Formula.Functions;
using JayShop.DBConnection;
using JayShop.Services;

namespace JayShop.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressApiController : ControllerBase
    {
        private readonly AddressService _addressService;
        public AddressApiController(AddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpGet("GetCity")]
        public IActionResult GetCity()
        {
            var cities = _addressService.GetCity();
            return Ok(cities);
        }
        [HttpGet("GetDistrict")]
        public IActionResult GetDistrict(int cityCode)
        {
            var districts = _addressService.GetDistricts(cityCode);
            return Ok(districts);
        }
        [HttpGet("GetAddress")]
        public IActionResult GetAddress(int cityCode, int districtCode, string stressAddress)
        {
            var address = _addressService.GetAddress(cityCode, districtCode, stressAddress);
            return Ok(address);
        }
    }
}
