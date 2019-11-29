using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cross.Util.Extensions
{
    public static class StringExtension
    {
        public static bool IsAnyNullOrEmpty(this object obj) => (from pi in obj.GetType().GetProperties() where pi.PropertyType == typeof(string) select (string)pi.GetValue(obj)).Any(string.IsNullOrEmpty);

        public static string ReplaceAt(this string str, int index, int length, string replace) => str.Remove(index, Math.Min(length, str.Length - index)).Insert(index, replace);

        public static string ReplaceLastOccurrence(this string str, string find, string replace)
        {
            var place = str.LastIndexOf(find, StringComparison.Ordinal);

            if (place == -1)
            {
                return str;
            }

            var result = str.Remove(place, find.Length).Insert(place, replace);

            return result;
        }

        public static string RemoveDiacritics(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string PinCode() => new Random().Next(0, 999999).ToString("D6");

        public static string FirstFromSplit(this string source, string delimiter)
        {
            var i = source.IndexOf(delimiter);

            return i == -1 ? source : source.Substring(0, i);
        }

        public static int WordsCount(this string str) => str.Split(' ').ToList().Count;

        public static string OnlyAscii(this string input)
        {
            //string output = Regex.Replace(input, @"[^\x00-\x7F]+", string.Empty); // Todos os ASCII

            var output = Regex.Replace(input, @"[^\x20-\x7E]+", string.Empty); // Todos os ASCII normais

            output = Regex.Replace(output, @"\s+", " ");

            return output;
        }

        public static string FormatEmailHidden(this string value, int ini = 2, int fim = 2, string masc = "****")
        {
            var aux = value.Split('@');
            string retorno = "";
            if (aux.Length > 1)
            {
                retorno += aux[0].Hidden(masc, ini, fim) + "@" + aux[1];
                return retorno;
            }
            else
            {
                return null;
            }
        }

        public static string Hidden(this string value, string masc = "****", int ini = -1, int fim = -1)
        {
            if (ini == -1)
            {
                ini = Convert.ToInt32(value.Length / 2);
                if (value.Length > masc.Length)
                {
                    ini -= masc.Length / 2;
                }

            }
            if (fim == -1)
            {
                fim = ini + masc.Length;
            }
            else
            {
                fim = value.Length - fim;
                if (fim - ini < masc.Length)
                {
                    fim = ini + masc.Length;
                }

            }

            if (fim < value.Length)
            {
                return value.Substring(0, ini) + masc + value.Substring(fim);
            }
            else
            {
                return value.Substring(0, ini) + masc;
            }
        }
    }
}