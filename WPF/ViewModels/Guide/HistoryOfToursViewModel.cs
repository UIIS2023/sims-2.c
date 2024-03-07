using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views;
using Tourist_Project.WPF.Views.Guide;

namespace Tourist_Project.WPF.ViewModels.Guide
{
    public class HistoryOfToursViewModel : INotifyPropertyChanged
    {
        public static ObservableCollection<Tour> Tours { get; set; }
        
        private TourService tourService = new();
        private Window window;
        private App app = (App)System.Windows.Application.Current;

        public Tour SelectedTour { get; set; }

        private DateTime currentTime;
        public DateTime CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                OnPropertyChanged();
            }
        }

        public User LoggedInUser { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Command
        public ICommand HomeViewCommand { get; set; }
        public ICommand FutureViewCommand { get; set; }
        public ICommand StatisticsViewCommand { get; set; }
        public ICommand ReviewViewCommand { get; set; }
        public ICommand ProfileViewCommand { get; set; }
        public ICommand RequestsViewCommand { get; set; }
        public ICommand ToSerbianCommand { get; set; }
        public ICommand ToEnglishCommand { get; set; }
        public ICommand ToDarkThemeCommand { get; set; }
        public ICommand ToLightThemeCommand { get; set; }
        #endregion

        public HistoryOfToursViewModel(Window window, User loggedInUser) 
        {
            LoggedInUser = loggedInUser;
            this.window = window;
            SelectedTour = null;
            Tours = new ObservableCollection<Tour>(tourService.GetPastTours());

            startClock();

            HomeViewCommand = new RelayCommand(HomeView);
            FutureViewCommand = new RelayCommand(FutureView);
            StatisticsViewCommand = new RelayCommand(StatisticsView, CanStatisticsView);
            ReviewViewCommand = new RelayCommand(ReviewView, CanReviewView);
            ProfileViewCommand = new RelayCommand(ProfileView);
            RequestsViewCommand = new RelayCommand(RequestsView);
            ToSerbianCommand = new RelayCommand(ToSerbian, CanToSerbian);
            ToEnglishCommand = new RelayCommand(ToEnglish, CanToEnglish);
            ToDarkThemeCommand = new RelayCommand(ToDarkTheme, CanToDarkTheme);
            ToLightThemeCommand = new RelayCommand(ToLightTheme, CanToLightTheme);
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

        private void startClock()
        {
            CurrentTime = DateTime.Now;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickEvent;
            timer.Start();
        }

        private void tickEvent(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }

        public void RequestsView()
        {
            var requestsWindow = new RequestsGuideView(LoggedInUser);
            requestsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            requestsWindow.Show();
            window.Close();
        }

        public void ProfileView()
        {
            var profileWindow = new GuideProfileView(LoggedInUser);
            profileWindow.Owner = window;
            profileWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            profileWindow.Show();
        }

        public void HomeView()
        {
            var homeWindow = new TodayToursView(LoggedInUser);
            homeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            homeWindow.Show();
            window.Close();
        }

        public void FutureView()
        {
            var futureWindow = new FutureToursView(LoggedInUser);
            futureWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            futureWindow.Show();
            window.Close();
        }

        private bool CanStatisticsView()
        {
            return SelectedTour != null;
        }

        public void StatisticsView()
        {
            var statisticsWindow = new StatisticsOfTourView(SelectedTour, LoggedInUser);
            statisticsWindow.Owner = window;
            statisticsWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            statisticsWindow.Show();
        }

        private bool CanReviewView()
        {
            return SelectedTour is not null;
        }

        public void ReviewView()
        {
            var reviewWindow = new TourReviewsGuideView(SelectedTour);
            reviewWindow.Owner = window;
            reviewWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            reviewWindow.Show();
        }
    }
}
