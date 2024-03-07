using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels
{
    public class RenovationViewModel : INotifyPropertyChanged
    {
        private Accommodation accommodation;

        public Accommodation Accommodation
        {
            get => accommodation;
            set
            {
                if(value == accommodation) return;
                accommodation = value;
                OnPropertyChanged();
            }

        }

        private Renovation renovation;
        public Renovation Renovation
        {
            get => renovation;
            set
            {
                if(value == renovation) return;
                renovation = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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

        private readonly AccommodationService accommodationService = new();
        private readonly LocationService locationService = new();
        private readonly UserService userService = new();

        public RenovationViewModel(Renovation renovation)
        {
            Renovation = renovation;
            Accommodation = accommodationService.Get(renovation.AccommodationId);
            Location = locationService.Get(Accommodation.LocationId);
            User = userService.GetOne(Accommodation.UserId);
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}