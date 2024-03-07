using System;
using System.Windows;
using System.Windows.Input;

namespace Tourist_Project.WPF.ViewModels
{
    public class RescheduleReservationViewModel
    {
        private Window _window; 

        public DateTime NewBegginigDate { get; set; }
        public DateTime NewEndDate { get; set; }

        public int NewStayingDays { get; set; }

        public ICommand ConfirmRescheduling_Command { get; set; }

        public RescheduleReservationViewModel(Window window)
        {
            this._window = window;
           // ConfirmRescheduling_Command = new RelayCommand(, CanReschedule);

        }

        private bool CanReschedule()
        {
            if (NewBegginigDate != DateTime.MinValue && NewEndDate != DateTime.MinValue)
                return true;
            else
                return false;
        }

        //private List<>

    }
}
