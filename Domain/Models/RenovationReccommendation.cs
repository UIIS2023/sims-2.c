using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public class RenovationRecommendation : INotifyPropertyChanged, ISerializable
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if(id == value) return;
                id = value;
                OnPropertyChanged();
            }
        }
        private string stateOfAccommodation;
        public string StateOfAccommodation
        {
            get => stateOfAccommodation;
            set
            {
                if(value == stateOfAccommodation) return;
                stateOfAccommodation = value;
                OnPropertyChanged();
            }
        }

        private int urgencyLevel;
        public int UrgencyLevel
        {
            get => urgencyLevel;
            set
            {
                if(value == urgencyLevel) return;
                urgencyLevel = value;
                OnPropertyChanged();
            }
        }

        private int reservationId;
        public int ReservationId
        {
            get => reservationId;
            set
            {
                if(reservationId == value) return;
                reservationId = value;
                OnPropertyChanged();
            }
        }

        public RenovationRecommendation() { }

        public RenovationRecommendation(string stateOfAccommodation, int urgencyLevel, int reservationId)
        {
            StateOfAccommodation = stateOfAccommodation;
            UrgencyLevel = urgencyLevel;
            ReservationId = reservationId;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                StateOfAccommodation,
                UrgencyLevel.ToString(),
                ReservationId.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StateOfAccommodation = values[1];
            UrgencyLevel = Convert.ToInt32(values[2]);
            ReservationId = Convert.ToInt32(values[3]);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}