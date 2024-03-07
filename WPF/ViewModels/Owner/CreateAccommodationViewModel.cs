using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views.Owner;
using Xceed.Wpf.Toolkit;
using Image = Tourist_Project.Domain.Models.Image;
using Location = Tourist_Project.Domain.Models.Location;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class CreateAccommodationViewModel : INotifyPropertyChanged
    {
        #region ToCreate
        private Accommodation accommodationToCreate;
        public Accommodation AccommodationToCreate
        {
            get => accommodationToCreate;
            set
            {
                accommodationToCreate = value;
                OnPropertyChanged("AccommodationToCreate");
            }
        }
        private Location locationToCreate;
        public Location LocationToCreate
        {
            get => locationToCreate;
            set
            {
                locationToCreate = value;
                OnPropertyChanged("LocationToCreate");
            }
        }
        private Image imageToCreate;
        public Image ImageToCreate
        {
            get => imageToCreate;
            set
            {
                imageToCreate = value;
                OnPropertyChanged("ImageToCreate");
            }
        }
        public User User { get; set; }
        #endregion

        #region DemoFields

        private TextBox name;
        private ComboBox country;
        private ComboBox city;
        private RadioButton apartment;
        private RadioButton house;
        private RadioButton cottage;
        private IntegerUpDown maxGuests;
        private IntegerUpDown minStayingDays;
        private IntegerUpDown cancellationThreshold;
        private TextBox url;

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
                if (value == locations) return;
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
                if (value == countries) return;
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
        public static ICommand CancelCommand { get; set; }
        public static ICommand DemoCommand { get; set; }
        public CreateAccommodation Window;
        public OwnerMainWindowViewModel OwnerMainWindowViewModel;

        public CreateAccommodationViewModel(User user, CreateAccommodation window, OwnerMainWindowViewModel ownerMainWindowViewModel)
        {
            User = user;
            OwnerMainWindowViewModel = ownerMainWindowViewModel;
            Locations = new ObservableCollection<Location>(locationService.GetAll());
            LocationToCreate = new Location();
            AccommodationToCreate = new Accommodation();
            ImageToCreate = new Image
            {
                Association = ImageAssociation.Accommodation
            };
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            Cities = new ObservableCollection<string>(locationService.GetAllCities());
            ConfirmCommand = new RelayCommand(Create, CanCreate);
            CancelCommand = new RelayCommand(Cancel);
            DemoCommand = new RelayCommand(async() => await Demo());
            Window = window;
            Window.Country.GotKeyboardFocus += CountryDropDownClosed;
            Window.City.GotKeyboardFocus += CityDropDownClosed;

        }

        public void SetControls(TextBox name, ComboBox country, ComboBox city, RadioButton apartment, RadioButton house, RadioButton cottage,
            IntegerUpDown maxGuests, IntegerUpDown minStayingDays, IntegerUpDown cancellationThreshold, TextBox url)
        {
            this.name = name;
            this.country = country;
            this.city = city;
            this.apartment = apartment;
            this.house = house;
            this.cottage = cottage;
            this.maxGuests = maxGuests;
            this.minStayingDays = minStayingDays;
            this.cancellationThreshold = cancellationThreshold;
            this.url = url;
        }

        #region PropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region Commands
        public void Create()
        {
            IdesInitialization();
            accommodationService.Create(AccommodationToCreate);
            ImageUpdate();
            OwnerMainWindowViewModel.CreateAccommodation(accommodationToCreate);
            Window.Close();
        }

        private void IdesInitialization()
        {
            AccommodationToCreate.UserId = User.Id;
            AccommodationToCreate.ImageId = CoverPhotoId();
            AccommodationToCreate.ImageIdsCsv = imageService.FormIdesString(ImageToCreate.Url);
            AccommodationToCreate.LocationId = locationService.GetId(LocationToCreate.City, LocationToCreate.Country);
        }

        private void ImageUpdate()
        {
            ImageToCreate = imageService.Get(AccommodationToCreate.ImageId);
            ImageToCreate.AssociationId = AccommodationToCreate.Id;
            imageService.Update(ImageToCreate);
        }

        public bool CanCreate()
        {
            return AccommodationToCreate.IsValid && LocationToCreate.IsValid && ImageToCreate.IsValid;
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

        public int CoverPhotoId()
        {
            var ids = imageService.CreateImages(ImageToCreate.Url);
            return ids.FirstOrDefault();
        }

        public async Task Demo()
        {
            const string demoName = "Demo accommodation";
            StringBuilder nameBuilder = new();
            name.Focus();
            foreach (var t in demoName)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                name.Text = nameBuilder.Append(t).ToString();
            }

            country.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            country.Text = "BH";

            city.GotKeyboardFocus += CountryDropDownClosed;
            await Task.Delay(TimeSpan.FromSeconds(1));
            city.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            city.IsDropDownOpen = false;
            city.Text = "Trebinje";

            await Task.Delay(TimeSpan.FromSeconds(1));
            house.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            house.IsChecked = true;

            await Task.Delay(TimeSpan.FromSeconds(1));
            maxGuests.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            maxGuests.Value = 5;

            await Task.Delay(TimeSpan.FromSeconds(1));
            minStayingDays.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            minStayingDays.Value = 3;

            await Task.Delay(TimeSpan.FromSeconds(1));
            cancellationThreshold.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            cancellationThreshold.Value = 2;

            await Task.Delay(TimeSpan.FromSeconds(1));
            url.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            url.Text = "https://www.john-taylor.fr/location-appartement-montreux-ori100-L0107MX-76143485.jpg?datetime=2020-09-07";
        }
        #endregion
    }

}