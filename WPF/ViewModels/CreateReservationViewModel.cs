using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tourist_Project.Domain.Models;
using Tourist_Project.Applications.UseCases;
using System.Windows.Input;
using System.Windows.Controls;
using Tourist_Project.WPF.Views;
using System.ComponentModel;

namespace Tourist_Project.WPF.ViewModels
{
    public class CreateReservationViewModel : INotifyPropertyChanged
    {

        private Window _window;
        private ReservationService _reservationService { get; set; }
        public List<Reservation> ReservationsForSelectedAccommodation { get; set; }

        public String Name { get; set;}
        public int StayingDays { get; set;}
        public int MinStayingDays { get; set; }

        public int GuestNum { get; set; }

        private int _searchedGuestNum;
        public int SearchedGuestNum
        {
            get { return _searchedGuestNum; }
            set
            {
                _searchedGuestNum = value;
                OnPropertyChanged(nameof(SearchedGuestNum));
            }
        }
        public String Type { get; set; }

        public DateTime From { get; set; }
        public DateTime  To { get; set; }
        public List<DateTime> AvailableDates { get; set; }

        public ICommand SeeAvailableReservations_Command { get; set; }

        public Accommodation SelectedAccommodation { get; set; }
        
        public CreateReservationViewModel(Window _window, Accommodation selectedAccommodation)
        {
            this._window = _window;
            _reservationService = new ReservationService();
            Name = selectedAccommodation.Name;
            Type = selectedAccommodation.Type.ToString();
            MinStayingDays = selectedAccommodation.MinStayingDays;
            GuestNum = selectedAccommodation.MaxGuestNum;
            SeeAvailableReservations_Command = new RelayCommand(ShowAvailableReservations, CanCreate);
            //ReservationsForSelectedAccommodation = new List<Reservation>();
            //ReservationsForSelectedAccommodation = _reservationService.FindReservationsForAccommodation(SelectedAccommodation);
            SetFromAndToDefault();
            SelectedAccommodation = new Accommodation();
            SelectedAccommodation = selectedAccommodation;

        }

        public void SetFromAndToDefault() 
        {
            From = DateTime.Now;
            To = DateTime.Now;
        }
        public List<DateTime> GenerateFreeDates()
        {
            DateTime dt = DateTime.Now;
            dt.AddMonths(2);
            AvailableDates.Add(dt);
            return AvailableDates;
        }
        

        public bool CanCreate()
        {
            if (CreateConditions()) //ako su uslovi ispunjeni
            {
                return true;
            }
            else 
                return true;
        }

        public bool CreateConditions()
        {
            if (From != DateTime.MinValue && To != DateTime.MinValue && From < To && StayingDays >= MinStayingDays && SearchedGuestNum <= GuestNum && SearchedGuestNum != 0)
                return true;
            else
                return false;
        }
        
        public void ShowAvailableReservations()
        {
            var availableReservationsWindow = new AvailableReservationsWindow(SelectedAccommodation, From, To, StayingDays, SearchedGuestNum);
            availableReservationsWindow.Show();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //ideja je da on ima 2 meseca, odabere od kad do kad hoce smestaj i onda da mu otvorim novi prozor koji mu prikazuje sve raspone

    }
}
