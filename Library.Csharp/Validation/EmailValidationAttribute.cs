using System.ComponentModel.DataAnnotations;

namespace PrismWPF.Common.Validation
{
    public class EamilValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;
            return valueAsString.IsEmail();
        }

        public override string FormatErrorMessage(string name)
        {
            return "Email is invalid.";
        }
    }
}
