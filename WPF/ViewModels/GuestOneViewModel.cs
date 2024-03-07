using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.DTO;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using System.Windows;
using System.Windows.Input;
using Tourist_Project.WPF.Views;

namespace Tourist_Project.WPF.ViewModels;



public class GuestOneViewModel
{
    //name, locationFullName, MaxGuestNum, CancelationThreshold, MinStayingDays
    private LocationService _locationService = new LocationService();
    private AccommodationService _accommodationService = new AccommodationService();

    #region SearchParameters
    public ObservableCollection<String> Countries { get; set; }
    public ObservableCollection<String> Cities { get; set; }

    public List<String> AccommodationsType { get; set; }

    public String SelectedType { get; set; }

    public int MaxGuestNum { get; set; }
    public int MinStayingDays { get; set; }

    public int SearchedCancelationThreshold { get; set; }
    public String SelectedCountry { get; set; }
    public String SelectedCity { get; set; }
    public Accommodation SelectedAccommodation { get; set; }

    public String AccommodationName { get; set; }
    #endregion

    #region Presentation
    public static ObservableCollection<String> LocationsFullName = new();
    

    public ObservableCollection<Accommodation> Accommodations { get; set; }

    public List<Accommodation> SearchResults = new();

    public List<Location> Locations { get; set; }

    #endregion
    private Window _window;

    public ICommand RateAccommodation_Command { get; set; }
    public ICommand CreateReservation_Command { get; set; }

    public GuestOneViewModel(Window window)
    {
        this._window = window;
        Locations = new List<Location>();
        Locations = GetLocationsForAccommodations();
        Countries = new ObservableCollection<String>(_locationService.GetAllCountries());
        Cities = new ObservableCollection<String>(_locationService.GetAllCities());
        AccommodationsType = new List<String>();
        AccommodationsType = GetAccommodationsType();
        Accommodations = new ObservableCollection<Accommodation>(_accommodationService.GetAll());
        GenerateLocations();
        RateAccommodation_Command = new RelayCommand(RateAccommodation, CanRate);
        CreateReservation_Command = new RelayCommand(CreateReservation,CanCreateReservation);

    }


    #region LoadingObjectsForDisplay
   
    public void GenerateLocations()
    {   
        foreach(Accommodation accommodation in Accommodations)
        {
            //accommodation.Location = _locationService.Get(accommodation.LocationId);
        }
    }


    public List<String> GetAccommodationsType()
    {
        foreach (Accommodation accommodation in _accommodationService.GetAll())
        {
            if (!AccommodationsType.Contains(accommodation.Type.ToString()))
            {
                AccommodationsType.Add(accommodation.Type.ToString());
            }
        }
        AccommodationsType.Add("Any");
        return AccommodationsType;
    }

    public List<Location> GetLocationsForAccommodations()
    {
        foreach(Accommodation accommodation in _accommodationService.GetAll())
        {
            var temp = _locationService.Get(accommodation.LocationId);
            temp.ToString();
            //moram da pristupam preko id-a
        }
        return Locations;
    }

    public void LocationsToString()
    {
        foreach(Location location in Locations)
        {
            location.ToString();
        }
    }
    #endregion

    #region SearchLogic

    public void HandleEmptyStrings()
    {
        if (AccommodationName == null)
        {
            AccommodationName = string.Empty;
        }

        if (SelectedCountry == null)
        {
            SelectedCountry = string.Empty;
        }

        if (SelectedCity == null)
        {
            SelectedCity = string.Empty;
        }

        if (MaxGuestNum == 0)
        {
            MaxGuestNum = 1;
        }

        if (MinStayingDays == 0)
        {
            foreach (Accommodation accommodation in Accommodations)
            {
                MinStayingDays = accommodation.CancellationThreshold;
            }
        }

        if (SelectedType == null || SelectedType == "Any")
        {
            SelectedType = string.Empty;
        }
    }
    public void SearchLogic()
    {
        SearchResults.Clear();
        HandleEmptyStrings();
        foreach (Accommodation accommodation in Accommodations)
        {
            if (accommodation.Name.ToLower().Contains(AccommodationName.ToLower()) && accommodation.MaxGuestNum >= MaxGuestNum
                && accommodation.MinStayingDays >= MinStayingDays &&
                accommodation.Type.ToString().Contains(SelectedType) &&
                LocationsFullName.Contains(SelectedCountry + ", " + SelectedCity))
            {
                SearchResults.Add(accommodation);
            }
        }
    }
    #endregion


    private bool CanRate()
    {
        if (SelectedAccommodation != null)
            return true;
        return false;
    }

    public bool CanCreateReservation()
    {
        if (SelectedAccommodation != null)
            return true;
        return false;
    }

    private void RateAccommodation()
    {
        var rateAccommodationWindow = new RateAccommodationWindow(SelectedAccommodation);
        rateAccommodationWindow.Show();
    }

    private void CreateReservation()
    {
        var createReservation = new ReservationWindow(SelectedAccommodation);
        createReservation.Show();
    }



}
