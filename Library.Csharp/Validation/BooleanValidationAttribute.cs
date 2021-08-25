using System.ComponentModel.DataAnnotations;

namespace PrismWPF.Common.Validation
{
    public class BooleanValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;
            return valueAsString.IsBoolean();
        }

        public override string FormatErrorMessage(string name)
        {
            return "Value is invalid.";
        }
    }
}
