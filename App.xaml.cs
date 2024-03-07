using System;
using System.Windows;
using Tourist_Project.Domain.Models;

namespace Tourist_Project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static User LoggedInUser { get; set; }
        public string CurrentTheme { get; set; } = "Light";
        public string CurrentLanguage { get; set; } = "en-US";
        public void ChangeLanguage(string currLang)
        {
            if (currLang.Equals("en-US"))
            {
                TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            }
            else
            {
                TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo("sr-LATN");
            }
        }

        public void SwitchTheme(string themeName)
        {
            Resources.MergedDictionaries.Clear();

            var dictionary = new ResourceDictionary();

            if (themeName == "Light")
                dictionary.Source = new Uri("pack://application:,,,/WPF/Themes/LightTheme.xaml");
            else if (themeName == "Dark")
                dictionary.Source = new Uri("pack://application:,,,/WPF/Themes/DarkTheme.xaml");

            Resources.MergedDictionaries.Add(dictionary);
        }
    }
}
