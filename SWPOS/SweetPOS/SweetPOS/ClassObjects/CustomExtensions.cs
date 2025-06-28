using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public static class CustomExtensions
    {
        public static string ToNullSafeString(this object obj)
        {
            return (obj ?? string.Empty).ToString();
        }

        public static bool isNumber(this string value)
        {
            double result;
            return double.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result);
        }
    }
}