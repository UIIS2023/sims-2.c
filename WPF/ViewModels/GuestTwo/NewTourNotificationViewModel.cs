using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.DTO;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;

namespace Tourist_Project.WPF.ViewModels
{
    public class NewTourNotificationViewModel : ViewModelBase
    {
        private readonly TourService tourService = new();

        public string Text { get; set; }
        public TourDTO Tour { get; set; }
        public ICommand PreviewCommand { get; set; }

        public NewTourNotificationViewModel(NotificationGuestTwo notification, NavigationStore mainNavigationStore, NotificationsViewModel notificationsViewModel)
        {
            Tour = tourService.GetOneDTO(notification.TourId);

            Text = "A guide has created a new tour that is similar to one of your denied requests.\n\nYou can make a reservation in the Home Page, or preview it here by clicking on the Preview button";

            PreviewCommand = new NavigateCommand<MyTourPreviewViewModel>(mainNavigationStore,
                () => new MyTourPreviewViewModel(Tour, mainNavigationStore, notificationsViewModel));
        }
    }
}
