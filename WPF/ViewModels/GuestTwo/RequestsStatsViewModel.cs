using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;

namespace Tourist_Project.WPF.ViewModels
{
    public class RequestsStatsViewModel : ViewModelBase
    {
        private readonly TourRequestService requestService = new();
        private readonly LocationService locationService = new();
        private string selectedCountry;
        private string selectedCity;
        private SeriesCollection languagesChart;
        private SeriesCollection locationsChart;
        private double acceptedPercent;
        private double deniedPercent;
        private string selectedStatYear;
        private double avgGuests;
        private ObservableCollection<string> cities;
        private ObservableCollection<TourRequest> requests;
        private Message message;
        private Message undoMessage;
        private string description;

        public Message Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public Message UndoMessage
        {
            get => undoMessage;
            set
            {
                undoMessage = value;
                OnPropertyChanged();
            }
        }


        public SeriesCollection LanguagesChart
        {
            get => languagesChart;
            set
            {
                languagesChart = value;
                OnPropertyChanged();
            }
        }

        public SeriesCollection LocationsChart
        {
            get => locationsChart;
            set
            {
                locationsChart = value;
                OnPropertyChanged();
            }
        }
        
        public DateTime DisplayDateStart { get; set; } = DateTime.Today.AddDays(2).Date;

        public ObservableCollection<string> StatYears { get; set; } = new() { "All time", "2023", "2022", "2021", "2020", "2019", "2018" };

        public double AvgGuests
        {
            get => Math.Round(avgGuests, 1);
            set
            {
                if (value != avgGuests)
                {
                    avgGuests = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedStatYear
        {
            get => selectedStatYear;
            set
            {
                if (value != selectedStatYear)
                {
                    selectedStatYear = value;
                    Requests = new ObservableCollection<TourRequest>(requestService.GetForSelectedYear(LoggedUser.Id, value));
                    AcceptedPercent = requestService.GetAcceptedPercentage(LoggedUser.Id, value);
                    DeniedPercent = requestService.GetDeniedPercentage(LoggedUser.Id, value);
                    AvgGuests = requestService.GetAverageGuests(LoggedUser.Id, value);
                    OnPropertyChanged();
                }
            }
        }


        public double AcceptedPercent
        {
            get => Math.Round(acceptedPercent, 1);
            set
            {
                if (value != acceptedPercent)
                {
                    acceptedPercent = value;
                    OnPropertyChanged();
                }
            }
        }

        public double DeniedPercent
        {
            get => Math.Round(deniedPercent, 1);
            set
            {
                if (value != deniedPercent)
                {
                    deniedPercent = value;
                    OnPropertyChanged();
                }
            }
        }

        public User LoggedUser { get; set; }

        public ObservableCollection<string> Countries { get; set; }

        public ObservableCollection<string> Cities
        {
            get => cities;
            set
            {
                if (value != cities)
                {
                    cities = value;
                    SelectedCity = value.First();
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<TourRequest> Requests
        {
            get => requests;
            set
            {
                foreach (var tourRequest in value)
                {
                    tourRequest.Location = locationService.Get(tourRequest.LocationId);
                }
                requests = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCountry
        {
            get => selectedCountry;
            set
            {
                if (value != selectedCountry)
                {
                    selectedCountry = value;
                    Cities = locationService.GetCitiesFromCountry(SelectedCountry);
                    OnPropertyChanged(nameof(SelectedCountry));
                }
            }
        }

        public string SelectedCity
        {
            get => selectedCity;
            set
            {
                if (value != selectedCity)
                {
                    selectedCity = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }
        public string Language { get; set; }
        public int GuestsNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }
        public ICommand PostRequestCommand { get; set; }
        public ICommand UndoRequestCommand { get; set; }
        public ICommand HelpCommand { get; }
        public ObservableCollection<string> Languages { get; } = new() { "srpski", "engleski", "mađarski", "češki", "slovački", "norveški", "francuski" };


        public RequestsStatsViewModel(User user, NavigationStore navigationStore)
        {
            LoggedUser = user;

            LanguagesChart = requestService.GetLanguageSeriesCollection(user.Id);
            LocationsChart = requestService.GetLocationSeriesCollection(user.Id);

            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            SelectedCountry = Countries.First();
            Cities = new ObservableCollection<string>(locationService.GetCitiesFromCountry(SelectedCountry));
            SelectedCity = Cities.First();
            Description = string.Empty;
            Language = Languages.First();
            FromDate = DateTime.Now.AddDays(2).Date;
            UntilDate = DateTime.Now.AddDays(3).Date;

            SelectedStatYear = StatYears.First();

            PostRequestCommand = new RelayCommand(PostRequestClick, CanPostRequest);
            UndoRequestCommand = new RelayCommand(UndoRequestClick, () => UndoMessage.Type);
            HelpCommand = new NavigateCommand<RequestsStatsHelpViewModel>(navigationStore, () => new RequestsStatsHelpViewModel(navigationStore, this));
        }

        private void UndoRequestClick()
        {
            requestService.UndoLatestRequest(LoggedUser.Id);
            Requests = new ObservableCollection<TourRequest>(requestService.GetAllForUser(LoggedUser.Id));
            _ = ShowMessageAndHide(new Message(true, "Request undone!"));
            UndoMessage = new Message();
            LanguagesChart = requestService.GetLanguageSeriesCollection(LoggedUser.Id);
            LocationsChart = requestService.GetLocationSeriesCollection(LoggedUser.Id);
        }

        private bool CanPostRequest()
        {
            return Description != string.Empty && Description.Length >= 15 && Language != string.Empty && GuestsNumber > 0;
        }

        private void PostRequestClick()
        {
            var locationId = locationService.GetId(SelectedCity, SelectedCountry);
            TourRequest newRequest =
                new(locationId, Description, Language, GuestsNumber, FromDate, UntilDate, LoggedUser.Id, DateTime.Now)
                {
                    Location = locationService.Get(locationId)
                };
            requestService.Save(newRequest);
            Requests.Add(newRequest);

            _ = ShowMessageAndHide(new Message(true, "Successfully posted request!"));
            Description = string.Empty;

            LanguagesChart = requestService.GetLanguageSeriesCollection(LoggedUser.Id);
            LocationsChart = requestService.GetLocationSeriesCollection(LoggedUser.Id);
        }

        private async Task ShowMessageAndHide(Message message)
        {
            Message = message;
            if (message.Type)
            {
                UndoMessage = new Message(true, "Undo");
            }

            await Task.Delay(15000);
            Message = new Message();
            UndoMessage = new Message();
        }

    }
}
