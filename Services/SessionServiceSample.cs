using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace JayShop.Services
{
    public static class SessionServiceSample
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
        public static void RemoveSession(this ISession session, string key)
        {
            session.Remove(key);
        }
    }
}
