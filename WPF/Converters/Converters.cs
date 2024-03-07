using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.ViewModels;

namespace Tourist_Project.WPF.Converters
{

    public class RadioToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var enumValue = (AccommodationType)value;
                var enumType = AccommodationType.Apartment;
                switch (parameter)
                {
                    case "Apartment":
                        enumType = AccommodationType.Apartment;
                        break;
                    case "House":
                        enumType = AccommodationType.House;
                        break;
                    case "Cottage":
                        enumType = AccommodationType.Cottage;
                        break;
                    default: return null;
                }

                return enumValue.Equals(enumType);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var enumType = AccommodationType.Apartment;
                switch (parameter)
                {
                    case "Apartment":
                        enumType = AccommodationType.Apartment;
                        break;
                    case "House":
                        enumType = AccommodationType.House;
                        break;
                    case "Cottage":
                        enumType = AccommodationType.Cottage;
                        break;
                    default: return null;
                }

                return enumType;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }

}
