using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Tourist_Project.Applications.UseCases;

namespace Tourist_Project.WPF.Validation
{
    public class DatePickerValidationRule : ValidationRule
    {
        private readonly TourRequestService tourRequestService = new TourRequestService();
        public DateTime StartingTime { get; set; }
        public int Duration { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is DateTime bookingDateTime)
            {
                // Change the format to "MM/dd/yyyy hh:mm:ss tt"
                string formattedDateTime = bookingDateTime.ToString("MM/dd/yyyy hh:mm:ss tt");

                // Parse the formatted date and time back to a DateTime object
                if (DateTime.TryParseExact(formattedDateTime, "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDateTime))
                {
                    // Check if the parsed date and time is already reserved (your logic here)
                    bool isAlreadyBooked = IsAlreadyBooked(parsedDateTime, formattedDateTime);

                    if (isAlreadyBooked)
                    {
                        return new ValidationResult(false, "This date and hour is already booked.");
                    }
                    else
                    {
                        return ValidationResult.ValidResult;
                    }
                }
                else
                {
                    return new ValidationResult(false, "Invalid date and hour format. Use the format MM/dd/yyyy HH:mm.");
                }
            }

            return new ValidationResult(false, "Invalid input.");
        }



        private bool IsAlreadyBooked(DateTime bookingDateTime, string formattedDateTime)
        {
            // Implement your logic to check if the bookingDateTime is already booked
            // Return true if it is already booked; otherwise, return false
            // Example implementation:
            // bool isAlreadyBooked = YourBookingCheckLogic(bookingDateTime);
            // return isAlreadyBooked;

            // Placeholder implementation that assumes the booking is already booked
            DateTime.TryParseExact(formattedDateTime, "MM/dd/yyyy hh:mm:ss tt",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime uporedjenje);
            return bookingDateTime.Equals(uporedjenje);
        }
    }
}
