using System;
using System.Globalization;
using System.Windows.Controls;

namespace Tourist_Project.WPF.Validation
{
    public class MinValueValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value?.ToString(), out int num))
            {
                if(num > 0) return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Value has to be 1 or more.");
/*            if (value != null) return new ValidationResult(false, "Unknown error occurred.");
            var min = (int)value;
            return min <= 0 ? new ValidationResult(false, "Value has to be 1 or more.") : new ValidationResult(true, null);*/
        }
    } 
}

