using System.ComponentModel.DataAnnotations;

namespace PrismWPF.Common.Validation
{
    public class DoubleValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;
            return valueAsString.IsDouble();
        }

        public override string FormatErrorMessage(string name)
        {
            return "Value is invalid.";
        }
    }
}
