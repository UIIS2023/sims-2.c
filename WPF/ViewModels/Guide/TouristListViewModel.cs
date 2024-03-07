using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.WPF.Views;
using Tourist_Project.WPF.Views.Guide;

namespace Tourist_Project.WPF.ViewModels.Guide
{
    public class TouristListViewModel : INotifyPropertyChanged
    {
        public static ObservableCollection<TourAttendance> TourAttendances { get; set; }
        public TourAttendance SelectedTourAttendance { get; set; }
        public TourPoint SelectedTourPoint { get; set; }
        public Tour ActiveTour { get; set; }

        private readonly UserService userService = new();
        private readonly TourPointService tourPointService = new();
        private readonly TourAttendanceService tourAttendanceService = new();
        private readonly NotificationGuestTwoService notificationService = new();
        private App app = (App)System.Windows.Application.Current;
        private readonly Window window;

        private DateTime currentTime;

        public DateTime CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                OnPropertyChanged("CurrentTime");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Command
        public ICommand CallOutCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand ToSerbianCommand { get; set; }
        public ICommand ToEnglishCommand { get; set; }
        public ICommand ToDarkThemeCommand { get; set; }
        public ICommand ToLightThemeCommand { get; set; }
        #endregion

        public TouristListViewModel(TourPoint selectedTourPoint, Tour tour, Window window) 
        { 
            this.SelectedTourPoint = selectedTourPoint;
            ActiveTour = tour;
            this.window = window;

            startClock();

            CallOutCommand = new RelayCommand(CallOut, CanCallOut);
            BackCommand = new RelayCommand(Back);
            HomeCommand = new RelayCommand(HomeView);
            ToSerbianCommand = new RelayCommand(ToSerbian, CanToSerbian);
            ToEnglishCommand = new RelayCommand(ToEnglish, CanToEnglish);
            ToDarkThemeCommand = new RelayCommand(ToDarkTheme, CanToDarkTheme);
            ToLightThemeCommand = new RelayCommand(ToLightTheme, CanToLightTheme);

            LoadTourAttendaces();
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
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }

        private void Back()
        {
            var tourLiveWindow = new TourLiveView(ActiveTour);
            tourLiveWindow.Show();
            window.Close();
        }

        private void HomeView()
        {
            window.Close();
        }

        private bool CanCallOut()
        {
            return SelectedTourAttendance != null;
        }

        private void CallOut()
        {
            SelectedTourAttendance.TourPoint = SelectedTourPoint;
            SelectedTourAttendance.CheckPointId = SelectedTourPoint.Id;
            if(SelectedTourAttendance.Presence == Presence.Joined)
            {
                SelectedTourAttendance.Presence = Presence.Pending;
                notificationService.Save(new NotificationGuestTwo(SelectedTourAttendance.UserId, SelectedTourAttendance.TourId, DateTime.Now.Date, NotificationType.ConfirmPresence));
            }
            tourAttendanceService.UpdateCollection(SelectedTourAttendance, SelectedTourPoint);
        }

        public void LoadTourAttendaces()
        {
            TourAttendances = new ObservableCollection<TourAttendance>(tourAttendanceService.GetAllByTourId(SelectedTourPoint.TourId));

            foreach (TourAttendance attendace in TourAttendances)
            {
                attendace.User = userService.GetOne(attendace.UserId);
                attendace.TourPoint = tourPointService.GetOne(attendace.CheckPointId);
            }
        }
    }
}
