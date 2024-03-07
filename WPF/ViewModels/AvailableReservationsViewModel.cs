using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.WPF.Views;
using Tourist_Project.Domain.Models;
using System.Windows;
using Tourist_Project.Applications.UseCases;
using System.Windows.Input;

namespace Tourist_Project.WPF.ViewModels
{
    public class AvailableReservationsViewModel
    {

        public struct DateSpan
        {
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }

        public List<DateSpan> AvailableDates { get; set; } = new();


        private Window _window;
        private ReservationService _reservationService;
        public List<Reservation> ReservationsForAccommodation { get; set; }
        
        public List<DateTime> FreeDates { get; set; }

        public DateTime SearchedStartingDate { get; set; }
        public DateTime SearchedEndingDate { get; set; }  

        public Accommodation SelectedAccommodation { get; set; }

        public int StayingDays { get; set; }    

        public ICommand ConfirmReservation_Command { get; set; }

        public String Name { get; set; }
        public Reservation SelectedReservation { get; set; }    

        public List<Reservation> ReservationsForDisplay { get; set; }

        
        public int GuestsNum { get; set; }  
        public List<String> StartingDates { get; set; }
        public List<String> EndingDates { get; set; }   
        public AvailableReservationsViewModel(Accommodation selectedAccommodation, DateTime from, DateTime to, int stayingDays, int guestsNum)
        {
            _reservationService = new ReservationService();
            ReservationsForAccommodation = new List<Reservation>();
            ReservationsForDisplay = new List<Reservation>();
            FreeDates = new List<DateTime>();
            ReservationsForAccommodation = _reservationService.FindReservationsForAccommodation(selectedAccommodation);
            SearchedStartingDate = from;
            SearchedEndingDate = to;
            StayingDays = stayingDays;
            //pravim listu rezervacija gde mi je reservation.CheckIn = startingDate, a reservation.Checkout = endingDate i to cuvam u fajl
            SelectedReservation = new Reservation();
            Name = selectedAccommodation.Name;
            SelectedAccommodation = new Accommodation();
            SelectedAccommodation = selectedAccommodation;
            GuestsNum = guestsNum;
            CheckFreeDays();
            GenerateRelevantReservations();
            ConfirmReservation_Command = new RelayCommand(SaveReservation, CanReserve);
            
        }


        private void CheckFreeDays()
        {
            double timeSpan = 0;
            while(AvailableDates.Count == 0)
            {
                timeSpan = SearchedEndingDate.Subtract(SearchedStartingDate).Days;
                UpdateAvailableDates(SearchedStartingDate.AddDays(timeSpan), SearchedEndingDate.AddDays(timeSpan));
            }
        }


        private void GenerateRelevantReservations()
        {
            
            foreach (DateSpan ds in AvailableDates)
            {
                Reservation reservation = new Reservation(ds.Start, ds.End, GuestsNum, StayingDays, SelectedAccommodation, ReservationStatus.Regular);
                ReservationsForDisplay.Add(reservation);
            }
            
        }

        private void UpdateAvailableDates(DateTime searchedStartingDate, DateTime searchedEndingDate)
        {
            DateTime possibleStartingDate = searchedStartingDate;
            DateTime possibleEndingDate = searchedStartingDate.AddDays(StayingDays);

            while (searchedEndingDate.Date >= possibleEndingDate.Date) // .Date so it doesnt include time
            {

                bool isDateConflicted = ConflictionExists(possibleStartingDate, possibleEndingDate);

                AddFreeDatesToList(possibleStartingDate, possibleEndingDate, isDateConflicted);

                possibleStartingDate = possibleEndingDate.AddDays(1);
                possibleEndingDate = possibleStartingDate.AddDays(StayingDays);

            }

        }

        private void AddAvailableDates(bool areDatesInConflicts, DateTime possibleStartingDate, DateTime possibleEndingDate)
        {
            if (areDatesInConflicts)
            {
                Reservation reservation = new Reservation(possibleStartingDate, possibleEndingDate, StayingDays, GuestsNum, SelectedAccommodation, ReservationStatus.Regular);
                DateTime dt = possibleStartingDate;
                while(dt <= possibleEndingDate)
                {
                    dt.AddDays(1);
                    FreeDates.Add(dt);
                    if (FreeDates.Count > 30)
                        break;
                }
                
                ReservationsForDisplay.Add(reservation);
            }
        }
        private bool ConflictionExists(DateTime possibleStartingDate, DateTime possibleEndingDate)
        {
           

            foreach (var reservation in ReservationsForAccommodation)
            {
                bool areDatesConflicted = reservation.CheckIn.Date <= possibleEndingDate.Date && possibleStartingDate.Date <= reservation.CheckOut.Date;

                if (areDatesConflicted)
                    return true;

            }

            return false;
        }

        private void AddFreeDatesToList(DateTime possibleStartingDate, DateTime possibleEndingDate, bool isDateConflicted)
        {
            if (isDateConflicted) return;

            var tempDateSpan = new DateSpan();
            tempDateSpan.Start = possibleStartingDate;
            tempDateSpan.End = possibleEndingDate;
            AvailableDates.Add(tempDateSpan);

        }

        private bool CanReserve()
        {
            if (SelectedReservation != null)
                return true;
            else
                return false;
        }

        private void SaveReservation()
        {
            _reservationService.Create(SelectedReservation);
        }

    }
}
