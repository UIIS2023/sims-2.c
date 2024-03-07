using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views;
using Tourist_Project.WPF.Views.Guide;

namespace Tourist_Project.WPF.ViewModels.Guide
{
    public class GuideProfileViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Years { get; set; } = new();
        private readonly TourService tourService = new ();
        private readonly ImageService imageService = new();
        private readonly UserService userService = new ();
        private readonly TourReviewService reviewService = new ();
        private readonly MessageService messageService = new ();
        private readonly Window window;
        private App app = (App)System.Windows.Application.Current;
        public Tour Tour { get; set; }
        public User LoggedInUser { get; set; }

        private string role;

        public string Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged("Role");
            }
        }

        private string tourImageLink;

        public string TourImageLink
        {
            get { return tourImageLink; }
            set
            {
                tourImageLink = value;
                OnPropertyChanged("TourImageLink");
            }
        }

        private string selectedYear;
        public string SelectedYear
        {
            get => selectedYear;
            set
            {
                selectedYear = value;
                OnPropertyChanged("SelectedYear");
                BestTourInfo();
            }
        }

        private ObservableCollection<string> superLanguages;

        public ObservableCollection<string> SuperLanguages
        {
            get { return superLanguages; }
            set
            {
                superLanguages = value;
                OnPropertyChanged("SuperLanguages");
            }
        }

        #region IPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands
        public ICommand HomeViewCommand { get; set; }
        public ICommand StatisticsViewCommand { get; set; }
        public ICommand ToSerbianCommand { get; set; }
        public ICommand ToEnglishCommand { get; set; }
        public ICommand QuitJobCommand { get; set; }
        public ICommand ToDarkThemeCommand { get; set; }
        public ICommand ToLightThemeCommand { get; set; }
        #endregion
        public GuideProfileViewModel(Window window, User loggedInUser)
        {
            this.window = window;
            LoggedInUser = loggedInUser;

            HomeViewCommand = new RelayCommand(HomeView);
            StatisticsViewCommand = new RelayCommand(StatisticsView);
            ToSerbianCommand = new RelayCommand(ToSerbian, CanToSerbian);
            ToEnglishCommand = new RelayCommand(ToEnglish, CanToEnglish);
            QuitJobCommand = new RelayCommand(QuitJob);
            ToDarkThemeCommand = new RelayCommand(ToDarkTheme, CanToDarkTheme);
            ToLightThemeCommand = new RelayCommand(ToLightTheme, CanToLightTheme);

            Years = new ObservableCollection<string>(tourService.GetAllYears(loggedInUser.Id));
            SelectedYear = "2023";
            TourImageLink = imageService.Get(Tour.ImageId).Url;
            SuperLanguages = new ObservableCollection<string>(reviewService.GetSuperLanguages(LoggedInUser.Id));
            Role = userService.SetRole(loggedInUser, SuperLanguages.Count);
        }

        private void ToDarkTheme()
        {
            var app = (App)Application.Current;
            app.CurrentTheme = "Dark";
            app.SwitchTheme(app.CurrentTheme);
        }

        private bool CanToDarkTheme()
        {
            return app.CurrentTheme == "Light";
        }

        private void ToLightTheme()
        {
            var app = (App)Application.Current;
            app.CurrentTheme = "Light";
            app.SwitchTheme(app.CurrentTheme);
        }

        private bool CanToLightTheme()
        {
            return app.CurrentTheme == "Dark";
        }

        private void QuitJob()
        {
            if (!messageService.ShowMessageBox("QuitJobText", "QuitJob")) return;

            userService.QuitJob(LoggedInUser);
            tourService.CancelAllToursByGuide(LoggedInUser.Id);
        }

        private void ToSerbian()
        {
            var app = (App)Application.Current;
            app.CurrentLanguage = "sr-LATN";
            app.ChangeLanguage(app.CurrentLanguage);
        }

        private bool CanToSerbian()
        {
            return app.CurrentLanguage.Equals("en-US");
        }

        private void ToEnglish()
        {
            var app = (App)Application.Current;
            app.CurrentLanguage = "en-US";
            app.ChangeLanguage(app.CurrentLanguage);
        }

        private bool CanToEnglish()
        {
            return app.CurrentLanguage.Equals("sr-LATN");
        }

        private void HomeView()
        {
            window.Close();
        }

        private void StatisticsView()
        {
            var statisticsWindow = new StatisticsOfTourView(Tour, LoggedInUser);
            statisticsWindow.Show();
            window.Close();
        }

        private void BestTourInfo()
        {
            Tour = !SelectedYear.Equals("Overall") ? tourService.GetMostVisited(int.Parse(SelectedYear), LoggedInUser) : tourService.GetOverallBest(LoggedInUser);
            OnPropertyChanged("Tour");
        }
    }
}
