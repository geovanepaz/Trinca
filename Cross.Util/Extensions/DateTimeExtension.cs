using System;

namespace Cross.Util.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime ToBrasil(DateTime data) => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(data, "E. South America Standard Time");
    }
}