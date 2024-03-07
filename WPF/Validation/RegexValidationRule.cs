using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Tourist_Project.WPF.Validation
{
    public class RegexValidationRule : ValidationRule
    {
        public string RegexPattern { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(RegexPattern))
                return ValidationResult.ValidResult;

            string input = value.ToString();
            Regex regex = new Regex(RegexPattern);

            if (regex.IsMatch(input))
                return ValidationResult.ValidResult;
            else
                return new ValidationResult(false, "Input does not match the required pattern.");
        }
    }
}
