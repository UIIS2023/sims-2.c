using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.DTO;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;

namespace Tourist_Project.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly TourService tourService = new();
        private readonly LocationService locationService = new();

        public User LoggedInUser { get; set; }
        private readonly NavigationStore navigationStore;

        private ObservableCollection<TourDTO> tours;
        public ObservableCollection<TourDTO> Tours
        {
            get => tours;
            set
            {
                if (value != tours)
                {
                    tours = value;
                    OnPropertyChanged(nameof(Tours));
                }
            }
        }
        public TourDTO SelectedTour { get; set; }

        private ObservableCollection<string> countries;
        public ObservableCollection<string> Countries
        {
            get => countries;
            set
            {
                if(value != countries)
                {
                    countries = value;
                    SelectedCountry = value.First();
                    OnPropertyChanged(nameof(Countries));
                }
            }
        }

        private string selectedCountry;
        public string SelectedCountry
        {
            get => selectedCountry;
            set
            {
                if (selectedCountry != value)
                {
                    selectedCountry = value;
                    Cities = locationService.GetCitiesFromCountry(SelectedCountry);
                    OnPropertyChanged(nameof(SelectedCountry));
                }
            }
        }
        private ObservableCollection<string> cities;
        public ObservableCollection<string> Cities
        {
            get { return cities; }
            set
            {
                if (cities != value)
                {
                    cities = value;
                    SelectedCity = value.First();
                    OnPropertyChanged(nameof(Cities));
                }
            }
        }
        private string selectedCity { get; set; }
        public string SelectedCity
        {
            get => selectedCity;
            set
            {
                if(selectedCity != value)
                {
                    selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }
        public ObservableCollection<string> Languages { get; set; }
        public string SelectedLanguage { get; set; }

        private int duration;
        public int Duration
        {
            get { return duration; }
            set
            {
                if(value >= 0)
                {
                    duration = value;
                }
            }
        }

        private int numberOfPeople;
        public int NumberOfPeople
        {
            get { return numberOfPeople; }
            set
            {
                if(value >= 0)
                {
                    numberOfPeople = value;
                }
            }
        }

        public ICommand SearchCommand { get; set; }
        public ICommand ShowAllCommand { get; set; }
        public ICommand ReserveCommand { get; set; }
        public ICommand HelpCommand { get; set; }


        public HomeViewModel(User user, NavigationStore navigationStore)
        {
            LoggedInUser = user;
            this.navigationStore = navigationStore;

            Tours = new ObservableCollection<TourDTO>(tourService.GetAllAvailableToursDto());
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            Languages = new ObservableCollection<string>(tourService.GetAllLanguages());
            SelectedLanguage = Languages.First();

            SearchCommand = new RelayCommand(OnSearchClick);
            ShowAllCommand = new RelayCommand(OnShowAllClick);
            ReserveCommand = new NavigateCommand<TourReservationViewModel>(navigationStore, () => new TourReservationViewModel(user, SelectedTour, this.navigationStore, this), CanReserve);
            HelpCommand = new NavigateCommand<HomeHelpViewModel>(navigationStore, () => new HomeHelpViewModel(navigationStore, this));

        }

        private bool CanReserve()
        {
            return SelectedTour != null;
        }

        private void OnShowAllClick()
        {
            Tours = new ObservableCollection<TourDTO>(tourService.GetAllAvailableToursDto());
        }

        private void OnSearchClick()
        {
            var filteredList = new ObservableCollection<TourDTO>();
            foreach (var tourDto in from tourDto in tourService.GetAllAvailableToursDto()
                     where SelectedCountry == null || tourDto.Location.Country == SelectedCountry
                     where SelectedCity == null || tourDto.Location.City == SelectedCity
                     where duration == 0 || tourDto.Duration == duration
                     where SelectedLanguage == null || tourDto.Language == SelectedLanguage
                     where numberOfPeople == 0 || tourDto.SpotsLeft >= numberOfPeople select tourDto)
            {
                filteredList.Add(tourDto);
            }
            Tours = filteredList;
        }

        private void HelpClick()
        {

        }
    }
}
