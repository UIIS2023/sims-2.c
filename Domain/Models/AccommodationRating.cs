using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public class AccommodationRating : ISerializable, INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (value == id) return;
                id = value;
                OnPropertyChanged("Id");
            }
        }
        private int userId;
        public int UserId
        {
            get => userId;
            set
            {
                if (value == userId) return;
                userId = value;
                OnPropertyChanged("UserId");
            }
        }
        private int cleanness;
        public int Cleanness
        {
            get => cleanness;
            set
            {
                if (value == cleanness) return;
                cleanness = value;
                OnPropertyChanged("Cleanness");
            }
        }
        private int accommodationGrade;
        public int AccommodationGrade
        {
            get => accommodationGrade;
            set
            {
                if (value == accommodationGrade) return;
                accommodationGrade = value;
                OnPropertyChanged("AccommodationGrade");
            }
        }
        private int ownerRating;
        public int OwnerRating
        {
            get => ownerRating;
            set
            {
                if (value == ownerRating) return;
                ownerRating = value;
                OnPropertyChanged("OwnerRating");
            }
        }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                if(value == comment) return;
                comment = value;
                OnPropertyChanged("Comment");
            }
        }
        private int imageId;
        public int ImageId
        {
            get => imageId;
            set
            {
                if (value == imageId) return;
                imageId = value;
                OnPropertyChanged("ImageId");
            }
        }
        private int reservationId;
        public int ReservationId
        {
            get => reservationId;
            set
            {
                if (value == reservationId) return;
                reservationId = value;
                OnPropertyChanged("ReservationId");
            }
        }

        public AccommodationRating() { }
        public AccommodationRating(int userId, int cleanness, string comment, int imageId, int reservationId, int ownerRating, int accommodationGrade)
        {
            UserId = userId;
            Cleanness = cleanness;
            Comment = comment;
            ImageId = imageId;
            ReservationId = reservationId;
            OwnerRating = ownerRating;
            AccommodationGrade = accommodationGrade;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                ReservationId.ToString(),
                Cleanness.ToString(),
                OwnerRating.ToString(),
                AccommodationGrade.ToString(),
                Comment,
                ImageId.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            ReservationId = Convert.ToInt32(values[2]);
            Cleanness = Convert.ToInt32(values[3]);
            OwnerRating = Convert.ToInt32(values[4]);
            AccommodationGrade = Convert.ToInt32(values[5]);
            Comment = values[6];
            ImageId = Convert.ToInt32(values[7]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
