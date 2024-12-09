using JayShop.DBConnection;
using JayShop.Models;

namespace JayShop.Services
{
    public class AddressService
    {
        private readonly DataBaseConnection _db;
        public AddressService(DataBaseConnection db)
        {
            _db = db;
        }
        public List<City> GetCity()
        {
            var cities = _db.City
                                        .ToList();
            return cities;
        }
        public List<District> GetDistricts(int cityCode)
        {
            var districts = _db.District
                                            .Where(district => district.CityCode == cityCode)
                                            .Select(district => district)
                                            .ToList();
            return districts;
        }

        public string GetAddress(int cityCode, int districtCode, string streetAddress)
        {
            var address = "";

            var city = _db.City.Where(city => city.CityCode == cityCode)
                                            .Select(city => city.CityName)
                                            .FirstOrDefault();

            var district = _db.District
                                            .Where(district => district.DistrictCode == districtCode)
                                            .Select(district => district.DistrictName)
                                            .FirstOrDefault();

            if (!string.IsNullOrEmpty(city) &&
                !string.IsNullOrEmpty(district) &&
                !string.IsNullOrEmpty(streetAddress))
            {
                address = city + district + streetAddress;
            }
            return address;
        }
    }
}
