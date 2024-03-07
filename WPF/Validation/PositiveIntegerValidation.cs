using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Tourist_Project.WPF.Validation
{
    public class PositiveIntegerValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string input)
            {
                if (int.TryParse(input, out int number))
                {
                    if (number >= 0)
                    {
                        return ValidationResult.ValidResult;
                    }
                }
            }

            return new ValidationResult(false, "Input must be a positive integer.");
        }
    }
}
