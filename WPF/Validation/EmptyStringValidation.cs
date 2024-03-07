using System.Globalization;
using System.Windows.Controls;
using System.Resources;
using Tourist_Project.Properties;

namespace Tourist_Project.WPF.Validation
{

    public class EmptyStringValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ResourceManager resourceManager = new ResourceManager(typeof(Resources));
            var validationMessage =
                resourceManager.GetString("EmptyFieldText", TranslationSource.Instance.CurrentCulture);
            try
            {
                return string.IsNullOrWhiteSpace((string)value) ? 
                    new ValidationResult(false, validationMessage) 
                    : ValidationResult.ValidResult;
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occurred.");
            }
        }
    }

}