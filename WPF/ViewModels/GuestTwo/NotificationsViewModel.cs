using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;

namespace Tourist_Project.WPF.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        public ViewModelBase CurrentViewModel => notificationNavigationStore.CurrentViewModel;
        private readonly NavigationStore notificationNavigationStore;
        private readonly NavigationStore mainNavigationStore;
        private readonly NotificationGuestTwoService notificationService = new();
        public ObservableCollection<NotificationGuestTwo> Notifications { get; set; }

        private NotificationGuestTwo selectedNotificationGuestTwo;
        public NotificationGuestTwo SelectedNotification
        {
            get => selectedNotificationGuestTwo;
            set
            {
                if (value != selectedNotificationGuestTwo)
                {
                    notificationNavigationStore.CurrentViewModel = value.NotificationType switch
                    {
                        NotificationType.ConfirmPresence => new ConfirmPresenceViewModel(value),
                        NotificationType.TourAccepted => new TourRequestAcceptedViewModel(value, mainNavigationStore, this),
                        NotificationType.NewTour => new NewTourNotificationViewModel(value, mainNavigationStore, this),
                        _ => notificationNavigationStore.CurrentViewModel
                    };
                    OnPropertyChanged();
                }
            }
        }

        public ICommand HelpCommand { get; }

        public NotificationsViewModel(User user, NavigationStore navigationStore)
        {
            mainNavigationStore = navigationStore;

            Notifications = new ObservableCollection<NotificationGuestTwo>(notificationService.GetAllForUser(user.Id));
            notificationNavigationStore = new NavigationStore();
            notificationNavigationStore.CurrentViewModel = new SelectNotificationViewModel();

            notificationNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            HelpCommand = new NavigateCommand<NotificationsHelpViewModel>(navigationStore, () => new NotificationsHelpViewModel(navigationStore, this));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
