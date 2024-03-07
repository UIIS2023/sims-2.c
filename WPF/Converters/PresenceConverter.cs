using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Resources;
using Tourist_Project.Domain.Models;
using Tourist_Project.Properties;

namespace Tourist_Project.WPF.Converters
{
    public class PresenceConverter : IValueConverter
    {
        ResourceManager resourceManager = new ResourceManager(typeof(Resources));
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Presence presence)
            {
                return presence == Presence.Yes;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
