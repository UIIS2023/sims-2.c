using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class AccommodationViewModel : INotifyPropertyChanged
    {
        private Accommodation accommodation;
        public Accommodation Accommodation
        {
            get => accommodation;
            set
            {
                if (value == accommodation) return;
                accommodation = value;
                OnPropertyChanged("Accommodation");
            }
        }

        private Location location;
        public Location Location
        {
            get => location;
            set
            {
                if(value == location) return;
                location = value;
                OnPropertyChanged("Location");
            }
        }
        private Image image;
        public Image Image
        {
            get => image;
            set
            {
                if(value == image) return;
                image = value;
                OnPropertyChanged("Image");
            }
        }

        private User user;

        public User User
        {
            get => user;
            set
            {
                if(value == user) return;
                user = value;
                OnPropertyChanged();
            }
        }
        private readonly LocationService locationService = new();
        private readonly ImageService imageService = new();
        private readonly UserService userService = new();
        public AccommodationViewModel() { }

        public AccommodationViewModel(Accommodation accommodation)
        {
            Accommodation = accommodation;
            Location = locationService.Get(accommodation.LocationId);
            Image = imageService.Get(accommodation.ImageId);
            User = userService.GetOne(accommodation.UserId);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}