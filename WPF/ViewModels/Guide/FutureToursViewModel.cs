using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class FutureToursViewModel : INotifyPropertyChanged
    {
        private Window window;
        private Tour tour;
        private App app = (App)System.Windows.Application.Current;
        private readonly TourService tourService = new();
        private readonly TourVoucherService voucherService = new();
        private readonly MessageService messageService = new();

        public User LoggedInUser { get; set; }

        public static ObservableCollection<Tour> FutureTours { get; set; }
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

        public Tour SelectedTour
        {
            get => tour;
            set
            {
                tour = value;
                OnPropertyChanged("SelectedTour");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        #region Command
        public ICommand CancelTourCommand { get; set; }
        public ICommand HomePageCommand { get; set; }
        public ICommand ProfileViewCommand { get; set; }
        public ICommand ToSerbianCommand { get; set; }
        public ICommand ToEnglishCommand { get; set; }
        public ICommand RequestsViewCommand { get; set; }
        public ICommand ToDarkThemeCommand { get; set; }
        public ICommand ToLightThemeCommand { get; set; }
        #endregion

        public FutureToursViewModel(Window window, User loggedInUser)
        {
            FutureTours = new ObservableCollection<Tour>(tourService.GetFutureTours());

            LoggedInUser = loggedInUser;
            this.window = window;
            startClock();

            HomePageCommand = new RelayCommand(HomePage);
            CancelTourCommand = new RelayCommand(CancelTour, CanCancelTour);
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

        private void RequestsView()
        {
            var requestsWindow = new RequestsGuideView(LoggedInUser);
            requestsWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            requestsWindow.Show();
            window.Close();
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

        private void HomePage()
        {
            var todayToursView = new TodayToursView(LoggedInUser);
            todayToursView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            todayToursView.Show();
            window.Close();
        }

        private bool CanCancelTour()
        {
            if (SelectedTour is null)
            {
                return false;
            }
            else
            {
                return (SelectedTour.StartTime - DateTime.Now).TotalHours >= 48;
            }
        }

        private void CancelTour()
        {
            if (!messageService.ShowMessageBox("CancelTourText", "CancelTour")) return;

            SelectedTour.Status = Status.Cancel;
            voucherService.VouchersDistribution(SelectedTour.Id);

            UpdateData();
        }

        private void ProfileView()
        {
            var profileView = new GuideProfileView(LoggedInUser);
            profileView.Owner = window;
            profileView.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            profileView.Show();
        }

        private void UpdateData()
        {
            tourService.Update(SelectedTour);
            FutureTours.Clear();
            foreach (var tour in tourService.GetFutureTours())
            {
                FutureTours.Add(tour);
            }
        }
    }
}
