using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Threading;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views;
using Tourist_Project.WPF.Views.Guide;

namespace Tourist_Project.WPF.ViewModels.Guide
{
    public class TodayToursViewModel : INotifyPropertyChanged
    {
        public static ObservableCollection<Tour> TodayTours { get; set; }
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
        public Tour SelectedTour { get; set; } 
        public static bool Live { get; set; }
        private readonly TourService tourService = new();
        private readonly Window window;
        private App app = (App)System.Windows.Application.Current;
        public User LoggedInUser { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Command
        public ICommand CreateCommand { get; set; }
        public ICommand StartTourCommand { get; set; }
        public ICommand FutureToursCommand { get; set; }
        public ICommand HistoryCommand { get; set; }
        public ICommand RequestsCommand { get; set; }
        public ICommand ProfileViewCommand { get; set; }
        public ICommand ToSerbianCommand { get; set; }
        public ICommand ToEnglishCommand { get; set; }
        public ICommand ToDarkThemeCommand { get; set; }
        public ICommand ToLightThemeCommand { get; set; }
        #endregion
        public TodayToursViewModel(Window window, User loggedInUser)
        {
            TodayTours = new ObservableCollection<Tour>(tourService.GetTodaysTours());

            SelectedTour = null;
            this.window = window;
            Live = false;
            LoggedInUser = loggedInUser;

            startClock();

            CreateCommand = new RelayCommand(CreateTour);
            StartTourCommand = new RelayCommand(StartTour, CanStartTour);
            FutureToursCommand = new RelayCommand(FutureTours);
            HistoryCommand = new RelayCommand(History);
            RequestsCommand = new RelayCommand(Requests);
            ProfileViewCommand = new RelayCommand(ProfileView);
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
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }

        public void ProfileView()
        {
            var profileWindow = new GuideProfileView(LoggedInUser);
            profileWindow.Owner = window;
            profileWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            profileWindow.Show();
        }

        private void Requests()
        {
            var requestsWindow = new RequestsGuideView(LoggedInUser);
            requestsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            requestsWindow.Show();
            window.Close();
        }

        private void History()
        {
            var historyWindow = new HistoryOfToursView(LoggedInUser);
            historyWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            historyWindow.Show();
            window.Close();
        }

        private void FutureTours()
        {
            var futureWindow = new FutureToursView(LoggedInUser);
            futureWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            futureWindow.Show();
            window.Close();
        }

        private void CreateTour()
        {
            var createTourWindow = new CreateTourView(LoggedInUser);
            createTourWindow.Owner = window;
            createTourWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            createTourWindow.Show();
        }

        private bool CanStartTour()
        {
            if (SelectedTour is null)
            {
                return false;
            }
            if (!Live)
            {
                return SelectedTour.Status == Status.NotBegin;
            }
            
            return SelectedTour.Status == Status.Begin;
            
        }
        private void StartTour()
        {
            Live = true;
            SelectedTour.Status = Status.Begin;
            tourService.Update(SelectedTour);

            var tourLiveWindow = new TourLiveView(SelectedTour);
            tourLiveWindow.Owner = window;
            tourLiveWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            tourLiveWindow.ShowDialog();
            tourLiveWindow.Close();

            tourService.Update(SelectedTour);
        }
    }
}
