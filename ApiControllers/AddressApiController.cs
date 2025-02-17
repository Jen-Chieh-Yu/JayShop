using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.OpenXmlFormats;
using NPOI.SS.Formula.Functions;
using JayShop.DBConnection;
using JayShop.Models;
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
            //if (cities != null)
            //{
            //    return Ok(new { success = true, cityList = cities });
            //}
            //else
            //{
            //    return BadRequest(new { success = false, cityList = new List<City>() });
            //}
            return Ok(cities);


        }
        [HttpGet("GetDistrict")]
        public IActionResult GetDistrict(int cityCode)
        {
            var districts = _addressService.GetDistricts(cityCode);
            //if (districts != null)
            //{
            //    return Ok(new { success = true, districtList = districts });
            //}
            //else
            //{
            //    return BadRequest(new { success = false, districtList = new List<District>() });
            //}
            return Ok(districts);
        }
        [HttpGet("GetAddress")]
        public IActionResult GetAddress(int cityCode, int districtCode, string stressAddress)
        {
            var address = _addressService.GetAddress(cityCode, districtCode, stressAddress);
            //if (address != null)
            //{
            //    return Ok(new { success = true, address = address });
            //}
            //else
            //{
            //    return BadRequest(new { success = false, address = string.Empty });
            //}
            return Ok(address);
        }
    }
}
