using System;
using System.Collections.Generic;
using System.Linq;

namespace Cross.Util.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) => self.Select((item, index) => (item, index));

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source == null || !source.Any();

        private static IEnumerable<string> Chunk(this string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize).Select(i => str.Substring(i * chunkSize, chunkSize)).ToList();
        }

        public static IEnumerable<string> CutString(this string str, int parts)
        {
            var @return = new List<string>();

            var maxChars = (int) Math.Ceiling((double) (str.Length / parts));

            for (var i = 0; i < parts; i++)
            {
                @return.Add(str.Substring(maxChars * i, maxChars));
            }

            return @return;
        }

        public static IEnumerable<T> Intertwine<T>(this List<T> a, List<T> b)
        {
            return a
                .Select((val, index) => new {val, index})
                .Concat(b.Select((val, index) => new {val, index}))
                .OrderBy(x => x.index)
                .Select(x => x.val)
                .ToList();
        }

        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }
    }
}