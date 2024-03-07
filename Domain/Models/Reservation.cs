using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum ReservationStatus { Regular, Cancelled, Rescheduled }
    public class Reservation : ISerializable,INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if(value == id) return;
                id = value;
                OnPropertyChanged("Id");
            }
        }
        private int guestId;
        public int GuestId
        {
            get => guestId;
            set
            {
                if (value == guestId) return;
                guestId = value;
                OnPropertyChanged("GuestId");
            }
        }

        private DateTime checkIn;
        public DateTime CheckIn
        {
            get => checkIn;
            set
            {
                if(value == checkIn) return;
                checkIn = value;
                OnPropertyChanged("CheckIn");
            }
        }

        private DateTime checkOut;
        public DateTime CheckOut
        {
            get => checkOut;
            set
            {
                if(value == checkOut) return;
                checkOut = value;
                OnPropertyChanged("CheckOut");
            }
        }
        private int guestsNum;
        public int GuestsNum
        {
            get => guestsNum;
            set
            {
                if(value == guestsNum) return;
                guestsNum = value;
                OnPropertyChanged("GuestsNum");
            }
        }
        private int stayingDays;
        public int StayingDays
        {
            get => stayingDays;
            set
            {
                if (value == stayingDays) return;
                stayingDays = value;
                OnPropertyChanged("StayingDays");
            }
        }
        private int accommodationId;
        public int AccommodationId
        {
            get => accommodationId;
            set
            {
                if (value == accommodationId) return;
                accommodationId = value;
                OnPropertyChanged("AccommodationId");
            }
        }

        private List<Date> availableDates;
        public List<Date> AvailableDates
        {
            get => availableDates;
            set
            {
                if(value == availableDates) return;
                availableDates = value;
                OnPropertyChanged("AvailableDates");
            }
        }
        public Accommodation Accommodation { get; set; }

        private ReservationStatus status;
        public ReservationStatus Status
        {
            get => status;
            set
            {
                if(value == status) return;
                status = value;
                OnPropertyChanged();
            }
        }

        public Reservation() { }
        public Reservation(DateTime checkIn, DateTime checkOut, int guestsNum, int stayingDays, Accommodation accommodation, ReservationStatus status)
        {
            CheckIn = checkIn;
            CheckOut = checkOut;
            GuestsNum = guestsNum;
            StayingDays = stayingDays;
            AccommodationId = accommodation.Id;
            AvailableDates = new List<Date>();
            Accommodation = accommodation;
            Status = status;
            //AvailableDates = new List<DateTime>();
        }
        public string[] ToCSV()
        {
            string[] csvValues = { 
                Id.ToString(), 
                GuestId.ToString(), 
                CheckIn.ToString(), 
                CheckOut.ToString(),  
                GuestsNum.ToString(), 
                StayingDays.ToString(), 
                AccommodationId.ToString(),
                Status.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            CheckIn = DateTime.Parse(values[2]);
            CheckOut = DateTime.Parse(values[3]);
            GuestsNum = Convert.ToInt32(values[4]);
            StayingDays = Convert.ToInt32(values[5]);
            AccommodationId = Convert.ToInt32(values[6]);
            Status = Enum.Parse<ReservationStatus>(values[7]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}