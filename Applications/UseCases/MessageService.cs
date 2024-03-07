using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Resources;
using Tourist_Project.Properties;

namespace Tourist_Project.Applications.UseCases
{
    public class MessageService
    {
        public bool ShowMessageBox(string message, string title)
        {
            ResourceManager resourceManager = new ResourceManager(typeof(Resources));

            var localizedMessage = resourceManager.GetString(message, TranslationSource.Instance.CurrentCulture);
            var localizedCaption = resourceManager.GetString(title, TranslationSource.Instance.CurrentCulture);

            MessageBoxResult result = MessageBox.Show(localizedMessage, localizedCaption, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes;
        }
    }
}
