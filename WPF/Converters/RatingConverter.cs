using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Tourist_Project.WPF.Views;

namespace Tourist_Project.WPF.Converters
{
    public class RatingConverter : IValueConverter
    { 
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var accommodationRatingValue = (int)value;
                int Rating = 1;
                switch (parameter)
                {
                    case "1":
                        accommodationRatingValue = 1;
                        break;
                    case "2":
                        accommodationRatingValue = 2;
                        break;
                    case "3":
                        accommodationRatingValue = 3;
                        break;
                    case "4":
                        accommodationRatingValue = 4;
                        break;
                    case "5":
                        accommodationRatingValue = 5;
                        break;
                    default: return 0;
                }
                return Rating = accommodationRatingValue;
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
                int AccommodationGrade;
                switch (parameter)
                {
                    case "1":
                        AccommodationGrade = 1;
                        break;
                    case "2":
                        AccommodationGrade = 2;
                        break;
                    case "3":
                        AccommodationGrade = 3;
                        break;
                    case "4":
                        AccommodationGrade = 4;
                        break;
                    case "5":
                        AccommodationGrade = 5;
                        break;
                    default: return 0;
                }
                return AccommodationGrade;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }      
    }
}
