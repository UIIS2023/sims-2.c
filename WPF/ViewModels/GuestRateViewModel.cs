using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels
{
    public class GuestRateViewModel:INotifyPropertyChanged
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
        private Reservation reservation;
        public Reservation Reservation
        {
            get => reservation;
            set
            {
                if(value == reservation) return;
                reservation = value;
                OnPropertyChanged("Reservation");
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
                OnPropertyChanged("User");
            }
        }
        private GuestRating guestRating;
        public GuestRating GuestRating
        {
            get => guestRating;
            set
            {
                if(value == guestRating) return;
                guestRating = value;
                OnPropertyChanged("GuestRating");
            }
        }

        private Notification notification;

        public Notification Notification
        {
            get => notification;
            set
            {
                if(value == notification) return;
                notification = value;
                OnPropertyChanged("Notification");
            }
        }

        private static AccommodationService accommodationService = new();
        private static ReservationService reservationService = new();
        private static UserService userService = new();
        private static GuestRateService guestRateService = new();

        public GuestRateViewModel(Notification notification)
        {
            GuestRating = guestRateService.Get(notification.TypeId);
            Reservation = reservationService.Get(GuestRating.ReservationId);
            Accommodation = accommodationService.Get(Reservation.AccommodationId);
            User = userService.GetOne(Reservation.GuestId);
            Notification = notification;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        } 
}
