using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DinaGameEngine.Common
{
    public static class StringExtensions
    {
        public static string ToPascalCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var strSplit = value.Split([' ', '_', '-']);
            for (int index = 0; index < strSplit.Length; index++)
            {
                if (!string.IsNullOrEmpty(strSplit[index]))
                    strSplit[index] = string.Concat(char.ToUpper(strSplit[index][0]).ToString(), strSplit[index].ToLower().AsSpan(1));
            }
            return string.Concat(strSplit);
        }
    }
}
