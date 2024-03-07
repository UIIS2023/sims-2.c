using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum NotificationType
    {
        ConfirmPresence, TourAccepted, NewTour
    }
    public class NotificationGuestTwo : ISerializable, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public bool Opened { get; set; }
        public bool Responded { get; set; }
        public NotificationType NotificationType { get; set; }

        public NotificationGuestTwo()
        {
            Title = string.Empty;
        }

        public NotificationGuestTwo(int userId, int tourId, DateTime date, NotificationType notificationType)
        {
            UserId = userId;
            TourId = tourId;
            Date = date;
            Opened = false;
            Responded = false;
            NotificationType = notificationType;

            Title = notificationType switch
            {
                NotificationType.ConfirmPresence => "Confirm your presence",
                NotificationType.TourAccepted => "Tour accepted",
                NotificationType.NewTour => "A new tour according to your requirements",
                _ => string.Empty
            };
        }

        public string[] ToCSV()
        {
            string[] retVal = {Id.ToString(), UserId.ToString(), TourId.ToString(), Title, Date.ToString(), Opened.ToString(), Responded.ToString(), NotificationType.ToString()};
            return retVal;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            TourId = int.Parse(values[2]);
            Title = values[3];
            Date = DateTime.Parse(values[4]);
            Opened = bool.Parse(values[5]);
            Responded = bool.Parse(values[6]);
            NotificationType = Enum.Parse<NotificationType>(values[7]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
