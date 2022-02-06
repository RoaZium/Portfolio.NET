﻿using System.ComponentModel.DataAnnotations;

namespace PrismWPF.Common.Validation
{
    public class IntegerValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string valueAsString = value as string;
            return valueAsString.IsInteger();
        }

        public override string FormatErrorMessage(string name)
        {
            return "Value is invalid.";
        }
    }
}
