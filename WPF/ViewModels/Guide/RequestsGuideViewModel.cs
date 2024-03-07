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
using Tourist_Project.WPF.Views.Guide;

namespace Tourist_Project.WPF.ViewModels.Guide
{
    public class RequestsGuideViewModel : INotifyPropertyChanged
    {
        private readonly TourRequestService tourRequestService = new();
        private readonly LocationService locationService = new();
        private Window window;
        private App app = (App)System.Windows.Application.Current;

        private string pickedFilter;

        public string PickedFilter
        {
            get { return pickedFilter; }
            set
            {
                pickedFilter = value;
                OnPropertyChanged("PickedFilter");
            }
        }

        private string searchBox;

        public string SearchBox
        {
            get { return searchBox; }
            set
            {
                searchBox = value;
                OnPropertyChanged("SearchBox");
            }
        }

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

        private ObservableCollection<TourRequest> tourRequests;

        public ObservableCollection<TourRequest> TourRequests
        {
            get { return tourRequests; }
            set
            {
                tourRequests = value;
                OnPropertyChanged("TourRequests");
            }
        }

        private TourRequest selectedRequest;

        public TourRequest SelectedRequest
        {
            get { return selectedRequest; }
            set
            {
                selectedRequest = value;
                OnPropertyChanged("SelectedRequest");
            }
        }

        public User LoggedInUser { get; set; }

        public List<string> Filters { get; set; } = new List<string>()
        {
            "Location",
            "Date",
            "Language"
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Commands
        public ICommand ProfileViewCommand { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand AcceptCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand StatisticsCommand { get; set; }
        public ICommand ToSerbianCommand { get; set; }
        public ICommand ToEnglishCommand { get; set; }
        public ICommand AcceptPartCommand { get; set; }
        public ICommand ToDarkThemeCommand { get; set; }
        public ICommand ToLightThemeCommand { get; set; }
        #endregion

        public RequestsGuideViewModel(Window window, User user)
        {
            this.window = window;
            LoggedInUser = user;

            LoadRequests();

            startClock();

            ProfileViewCommand = new RelayCommand(ProfileWindow);
            HomeCommand = new RelayCommand(HomeWindow);
            AcceptCommand = new RelayCommand(AcceptWindow, CanAccept);
            SearchCommand = new RelayCommand(Search);
            StatisticsCommand = new RelayCommand(StatisticsWindow);
            ToSerbianCommand = new RelayCommand(ToSerbian, CanToSerbian);
            ToEnglishCommand = new RelayCommand(ToEnglish, CanToEnglish);
            AcceptPartCommand = new RelayCommand(AcceptPart, CanAcceptPart);
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

        private void AcceptPart()
        {
            var acceptPart = new AcceptComplexTourPartView(LoggedInUser, SelectedRequest);
            acceptPart.Show();
        }

        private bool CanAcceptPart()
        {
            if(SelectedRequest != null)
                return SelectedRequest.ComplexTourId != -1;
            return false;
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

        private void StatisticsWindow()
        {
            var statisticsWindow = new RequestsStatisticsView(LoggedInUser);
            statisticsWindow.Show();
        }

        private void Search()
        {
            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.SearchRequests(PickedFilter, SearchBox));
            LoadRequests();
        }

        private bool CanAccept()
        {
            if(SelectedRequest != null)
                return SelectedRequest.ComplexTourId == -1;
            return false;
        }

        private void AcceptWindow()
        {
            var acceptWindow = new AcceptClassicRequestView(LoggedInUser, SelectedRequest);
            acceptWindow.Show();
        }

        private void HomeWindow()
        {
            var homeWindow = new TodayToursView(LoggedInUser);
            homeWindow.Show();
            window.Close();
        }

        private void ProfileWindow()
        {
            var profileWindow = new GuideProfileView(LoggedInUser);
            profileWindow.Show();
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

        public void LoadRequests()
        {
            TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllPending());
            foreach (var request in TourRequests)
            {
                request.Location = locationService.Get(request.LocationId);
            }
        }


    }
}
