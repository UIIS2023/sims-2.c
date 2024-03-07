using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.WPF.Views;

namespace Tourist_Project.WPF.Converters
{
    public class OwnerRatingConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int ownerRatingValue = (int)value;
                int defaultRating = 1;
                switch (parameter)
                {
                    case "1":
                        ownerRatingValue = 1;
                        break;
                    case "2":
                        ownerRatingValue = 2;
                        break;
                    case "3":
                        ownerRatingValue = 3;
                        break;
                    case "4":
                        ownerRatingValue = 4;
                        break;
                    case "5":
                        ownerRatingValue = 5;
                        break;
                    default: return 0;
                }
                return defaultRating.Equals(ownerRatingValue);
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
                int OwnerGrade = 1;
                switch (parameter)
                {
                    case "1":
                        OwnerGrade = 1;
                        break;
                    case "2":
                        OwnerGrade = 2;
                        break;
                    case "3":
                        OwnerGrade = 3;
                        break;
                    case "4":
                        OwnerGrade = 4;
                        break;
                    case "5":
                        OwnerGrade = 5;
                        break;
                    default: return 0;
                }
                return OwnerGrade;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}

