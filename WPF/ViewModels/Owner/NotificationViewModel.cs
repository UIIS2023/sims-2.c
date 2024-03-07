using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class NotificationViewModel : INotifyPropertyChanged
    {
        private Notification notification;

        public Notification Notification
        {
            get => notification;
            set
            {
                if(value == notification) return;
                notification = value;
                OnPropertyChanged();
            }
        }

        private User user;
        public User User
        {
            get => user;
            set
            {
                if (value == user) return;
                user = value;
                OnPropertyChanged();
            }
        }

        private Location location;
        public Location Location
        {
            get => location;
            set
            {
                if (value == location) return;
                location = value;
                OnPropertyChanged();
            }
        }

        private DateTime checkedOut;
        public DateTime CheckedOut
        {
            get => checkedOut;
            set
            {
                if (value == checkedOut) return;
                checkedOut = value;
                OnPropertyChanged();
            }
        }

        private readonly ForumService forumService = new();
        private readonly LocationService locationService = new();
        private readonly GuestRateService guestRateService = new();
        private readonly ReservationService reservationService = new();
        private readonly UserService userService = new();

        public NotificationViewModel(Notification notification)
        {
            Notification = notification;
            if (notification.type == "Forum")
            {
                Location = locationService.Get(forumService.Get(notification.TypeId).LocationId);
            }
            else
            {
                var reservation = reservationService.Get(guestRateService.Get(notification.TypeId).ReservationId);
                User = userService.GetOne(reservation.GuestId);
                CheckedOut = reservation.CheckOut;
            }
        }

        public override string ToString()
        {
            if (Notification.Type == "Forum")
                return "A new forum has\n" +
                       $"opened on location:\n {Location}\n Check it out.";
            return "You have unrated\n" +
                   "guest:\n" +
                   $"{User.FullName}\n" +
                   "Checked out:\n" +
                   $"{CheckedOut:dd.MM.yyyy}";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}