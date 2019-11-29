using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Cross.Util.Extensions
{
    public static class SessionExtension
    {
        public static async void Set(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
            await session.CommitAsync();
        }

        public static async Task<T> Get<T>(this ISession session, string key)
        {
            await session.LoadAsync();
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}