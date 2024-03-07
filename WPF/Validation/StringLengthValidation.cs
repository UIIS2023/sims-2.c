using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Tourist_Project.WPF.Validation
{
    class StringLengthValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string input && input.Length >= 15)
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Input must be at least 15 characters long.");
        }
    }
}
