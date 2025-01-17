using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public static class CustomExtensions
    {
        //These are customer extensions made to reduce errors when entering data
        public static string ToNullSafeString(this object obj)
        {
            //This ensures any null string that are accidentally created is turned in an empty string
            return (obj ?? string.Empty).ToString();
        }

        public static bool isNumber(this string value)
        {
            //This verifies that an actual number is entered into filed where only numbers are accepted
            double result;
            return double.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result);
        }
    }
}