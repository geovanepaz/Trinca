using System;
using System.Collections.Generic;
using System.Linq;

namespace Cross.Util.Extensions
{
    public static class LinqExtension
    {
        public static T MinBy<T, TProp>(this IEnumerable<T> source, Func<T, TProp> propSelector) => source.OrderBy(propSelector).FirstOrDefault();

        public static T MaxBy<T, TProp>(this IEnumerable<T> source, Func<T, TProp> propSelector) => source.OrderBy(propSelector).LastOrDefault();
    }
}