using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.Repositories;
using Tourist_Project.WPF.Views;
using Tourist_Project.WPF.Views.Guide;

namespace Tourist_Project.WPF.ViewModels.Guide
{
    public class TourLiveViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TourPoint> tourPoints;

        public ObservableCollection<TourPoint> TourPoints
        {
            get { return tourPoints; }
            set
            {
                tourPoints = value; 
                OnPropertyChanged("TourPoint");
            }
        }

        public TourPoint SelectedTourPoint { get; set; }
        public Tour SelectedTour { get; set; }

        private Window window;
        private App app = (App)System.Windows.Application.Current;
        private readonly TourPointService tourPointService = new();
        private readonly TourService tourService = new();

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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Command
        public ICommand EarlyEndCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public ICommand OpenTouristListCommand { get; set; }
        public ICommand HomePageCommand { get; set; }
        public ICommand ToSerbianCommand { get; set; }
        public ICommand ToEnglishCommand { get; set; }
        public ICommand ToDarkThemeCommand { get; set; }
        public ICommand ToLightThemeCommand { get; set; }
        #endregion

        public TourLiveViewModel(Tour selectedTour, Window window) 
        {
            this.SelectedTour = selectedTour;
            this.window = window;

            TourPoints = new ObservableCollection<TourPoint>(tourPointService.GetAllForTour(selectedTour.Id));
            TourPoints[0].Visited = true;
            tourPointService.Update(TourPoints[0]);

            startClock();
            SelectedTourPoint = null;


            EarlyEndCommand = new RelayCommand(EarlyEnd);
            CheckCommand = new RelayCommand(Check, CanCheck);
            OpenTouristListCommand = new RelayCommand(OpenToruistList, CanOpenTouristList);
            HomePageCommand = new RelayCommand(HomeView);
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
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }

        private void tickevent(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
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

        private void EarlyEnd()
        {
            SelectedTour.Status = Status.End;
            TodayToursViewModel.Live = false;
            window.Close();
        }

        private bool CanCheck()
        {
            return SelectedTourPoint != null;
        }
        private void Check()
        {
            SelectedTourPoint.Visited = true;
            tourPointService.Update(SelectedTourPoint);
            UpdateCollection();
            if (tourPointService.EndTour(TourPoints))
            {
                SelectedTour.Status = Status.End;
                tourService.Update(SelectedTour);
                TodayToursViewModel.Live = false;
                window.Close();
            }
        }

        private bool CanOpenTouristList()
        {
            return SelectedTourPoint is not null;
        }

        private void OpenToruistList()
        {
            var touristListWindow = new TouristListView(SelectedTourPoint, SelectedTour);
            touristListWindow.Show();
            window.Close();
        }

        private void UpdateCollection()
        {
            TourPoints.Clear();
            foreach (var point in tourPointService.GetAllForTour(SelectedTour.Id))
            {
                TourPoints.Add(point);
            }
        }
    }
}
