using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Tourist_Project.WPF.Converters
{
    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try 
            { 
                int monthNumber = int.Parse((string)value);

                string monthName = GetMonthName(monthNumber);
                
                return monthName;
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
                string monthName = value.ToString();
                    
                int monthNumber = GetMonthNumber(monthName);

                return monthNumber;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        private string GetMonthName(int monthNumber)
        {
            switch (monthNumber)
            {
                case 1: 
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April"; 
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7: 
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    throw new ArgumentOutOfRangeException("Invalid month number. Month number must be between 1 and 12.");
            }
        }

        private int GetMonthNumber(string monthName)
        {
            switch (monthName.ToLower())
            {
                case "january":
                    return 1;
                case "february":
                    return 2;
                case "march":
                    return 3;
                case "april":
                    return 4;
                case "may":
                    return 5;
                case "june":
                    return 6;
                case "july":
                    return 7;
                case "august":
                    return 8;
                case "september":
                    return 9;
                case "october":
                    return 10;
                case "november":
                    return 11;
                case "december":
                    return 12;
                default:
                    throw new ArgumentOutOfRangeException("Invalid month name.");
            }
        }
    }

}
