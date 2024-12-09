using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace JayShop.Services
{
    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private ISession? _session => _httpContextAccessor.HttpContext?.Session;
        public void SetObjectAsJson(string key, object value)
        {
            _session?.SetString(key, JsonConvert.SerializeObject(value));
        }
        public T? GetObjectFromJson<T>(string key)
        {
            if (_session == null)
            {
                return default;
            }
            else
            {
                var value = _session.GetString(key);
                return value == null ? default : JsonConvert.DeserializeObject<T>(value);
            }
        }
        public void RemoveSession(string key)
        {
            _session?.Remove(key);
        }
    }
}
