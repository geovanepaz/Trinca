using System;

namespace Cross.Util.Extensions
{
    public static class EnumExtension
    {
        public static T ToEnum<T>(this string value, bool ignoreCase = true) => (T) Enum.Parse(typeof(T), value, ignoreCase);
    }
}