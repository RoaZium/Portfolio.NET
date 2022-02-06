using System;

namespace PrismWPF.Common.Validation
{
    public static class ValidationUtils
    {
        public static bool IsEmail(this string text)
        {
            if (text == null)
            {
                return false;
            }

            return ValidationRegex.CreateEmailRegex().IsMatch(text);
        }

        public static bool IsUrl(this string text)
        {
            if (text == null)
            {
                return false;
            }

            return ValidationRegex.CreateUrlRegex().IsMatch(text);
        }

        public static bool IsInteger(this string text)
        {
            if (text == null)
            {
                return false;
            }

            return int.TryParse(text, out int temp);
        }

        public static bool IsDouble(this string text)
        {
            if (text == null)
            {
                return false;
            }

            return double.TryParse(text, out double temp);
        }

        public static bool IsBoolean(this string text)
        {
            if (text == null)
            {
                return false;
            }

            return bool.TryParse(text, out bool temp);
        }

        public static bool IsDateTime(this string text)
        {
            if (text == null)
            {
                return false;
            }

            return DateTime.TryParse(text, out DateTime temp);
        }
    }
}
