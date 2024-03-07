using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class LocationStatisticsViewModel : INotifyPropertyChanged
    {
        private int reservationNo;
        public int ReservationNo
        {
            get => reservationNo;
            set
            {
                if (reservationNo == value) return;
                reservationNo = value;
                OnPropertyChanged();
            }
        }

        private int occupancy;
        public int Occupancy
        {
            get => occupancy;
            set
            {
                if (occupancy == value) return;
                occupancy = value;
                OnPropertyChanged();
            }
        }

        private int accommodationNo;

        public int AccommodationNo
        {
            get => accommodationNo;
            set
            {
                if (accommodationNo == value) return;
                accommodationNo = value;
                OnPropertyChanged();
            }
        }
        public Location Location { get; set; }
        private readonly AccommodationService accommodationService = new();
        private readonly ReservationService reservationService = new();
        private readonly LocationService locationService = new();
        public LocationStatisticsViewModel(int locationId)
        {
            Location = locationService.Get(locationId);
            AccommodationNo = accommodationService.GetAccommodationIds(Location.Id).Count;
            ReservationNo = reservationService.GetReservationsOnLocation(Location.Id).Count;
            if (AccommodationNo == 0)
                Occupancy = 0;
            else
                Occupancy = reservationService.GetOccupancyOnLocation(locationId) / accommodationNo;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}