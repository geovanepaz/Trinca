using Newtonsoft.Json;

namespace Cross.Util.Extensions
{
    public static class JsonExtension
    {
        public static bool IsValid<T>(string value)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            try
            {
                JsonConvert.DeserializeObject<T>(value, settings);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}