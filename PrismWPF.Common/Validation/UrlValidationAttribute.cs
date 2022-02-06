using System.ComponentModel.DataAnnotations;

namespace PrismWPF.Common.Validation
{
    public sealed class UrlValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;
            return valueAsString.IsUrl();
        }

        public override string FormatErrorMessage(string name)
        {
            return "Url is invalid.";
        }
    }
}
