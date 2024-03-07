using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views.Owner;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class UpdateAccommodationViewModel : INotifyPropertyChanged
    {
        #region UpdateProperties
        private Accommodation accommodation;
        public Accommodation Accommodation
        {
            get => accommodation;
            set
            {
                accommodation = value;
                OnPropertyChanged("Accommodation");
            }
        }

        private Image image;
        public Image Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged("Image");
            }
        }

        private Location location;
        public Location Location
        {
            get => location;
            set
            {
                location = value;
                OnPropertyChanged("Location");
            }
        } 
        #endregion

        private readonly ImageService imageService = new();
        private readonly LocationService locationService = new();
        private readonly AccommodationService accommodationService = new();
        private ObservableCollection<Location> locations;
        public ObservableCollection<Location> Locations
        {
            get => locations;
            set
            {
                if(value == locations) return;
                locations = value;
                OnPropertyChanged("Locations");
            }
        }

        private ObservableCollection<string> countries;
        public ObservableCollection<string> Countries
        {
            get => countries;
            set
            {
                if(value == countries) return;
                countries = value;
                OnPropertyChanged("Countries");
            }
        }
        private ObservableCollection<string> cities;
        public ObservableCollection<string> Cities
        {
            get => cities;
            set
            {
                if (value == cities) return;
                cities = value;
                OnPropertyChanged("Cities");
            }
        }
        public static ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public UpdateAccommodation Window;
        public OwnerMainWindowViewModel OwnerMainWindowViewModel;

        public UpdateAccommodationViewModel(UpdateAccommodation window, AccommodationViewModel accommodationViewModel, OwnerMainWindowViewModel ownerMainWindowViewModel)
        {
            Accommodation = accommodationViewModel.Accommodation;
            OwnerMainWindowViewModel = ownerMainWindowViewModel;
            Image = accommodationViewModel.Image;
            Location = accommodationViewModel.Location;
            Locations = new ObservableCollection<Location>(locationService.GetAll());
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            Cities = new ObservableCollection<string>(locationService.GetAllCities());
            ConfirmCommand = new RelayCommand(Update, CanUpdate);
            CancelCommand = new RelayCommand(Cancel);
            Window = window;
            Window.Country.GotKeyboardFocus += CountryDropDownClosed;
            Window.City.GotKeyboardFocus += CityDropDownClosed;
        }
        #region PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Commands
        public void Update()
        {
            Accommodation.ImageIdsCsv = imageService.FormIdesString(Image.Url);
            Accommodation.LocationId = locationService.GetId(Location.City, Location.Country);
            accommodationService.Update(accommodation);
            OwnerMainWindowViewModel.UpdateAccommodation();
            Window.Close();
        }

        public bool CanUpdate()
        {
            return Accommodation.IsValid && Location.IsValid && Image.IsValid;
        }
        public void Cancel()
        {
            Window.Close();
        }

        public void CountryDropDownClosed(object sender, KeyboardFocusChangedEventArgs e)
        {
            Cities.Clear();
            foreach (var location in Locations)
            {
                if (location.Country.Equals(Window.Country.Text))
                    Cities.Add(location.City);
            }
        }
        public void CityDropDownClosed(object sender, KeyboardFocusChangedEventArgs e)
        {
            foreach (var location in Locations)
            {
                if (location.City.Equals(Window.City.Text))
                    Window.Country.Text = location.Country;
            }
        }
        #endregion
    }
}