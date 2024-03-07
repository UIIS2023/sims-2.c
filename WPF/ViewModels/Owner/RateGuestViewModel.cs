using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class RateGuestViewModel : INotifyPropertyChanged
    {
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

        private GuestRateViewModel guestRate;
        public GuestRateViewModel GuestRate
        {
            get => guestRate;
            set
            {
                if(value == guestRate) return;
                guestRate = value;
                OnPropertyChanged("GuestRate");
            }
        }
        private readonly GuestRateService ratingService = new ();
        private readonly NotificationService notificationService = new ();
        public ICommand RateCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private readonly IBindableBase bindableBase;

        public OwnerMainWindowViewModel OwnerMainWindowViewModel;
        public RateGuestViewModel(Notification notification, IBindableBase bindableBase, OwnerMainWindowViewModel ownerMainWindowViewModel)
        {
            this.bindableBase = bindableBase;
            Notification = notification;
            OwnerMainWindowViewModel = ownerMainWindowViewModel;
            GuestRate = new GuestRateViewModel(notification);
            RateCommand = new RelayCommand(Rate, CanRate);
            CancelCommand = new RelayCommand(Cancel);
        }
        #region Commands
        public void Rate()
        {
            notificationService.Delete(Notification.Id);
            ratingService.Update(GuestRate.GuestRating);
            OwnerMainWindowViewModel.GuestRateUpdate(Notification);
            bindableBase.CloseWindow();
        }

        public bool CanRate()
        {
            return true;
        }

        public void Cancel()
        {
            bindableBase.CloseWindow();
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}