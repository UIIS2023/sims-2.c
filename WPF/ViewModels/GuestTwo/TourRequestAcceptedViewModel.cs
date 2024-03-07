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
    public class TourRequestAcceptedViewModel : ViewModelBase
    {
        private readonly TourService tourService = new();

        public string Text { get; set; }
        public TourDTO Tour { get; set; }
        public ICommand PreviewCommand { get; set; }

        public TourRequestAcceptedViewModel(NotificationGuestTwo notification, NavigationStore mainNavigationStore, NotificationsViewModel notificationsViewModel)
        {
            Tour = tourService.GetOneDTO(notification.TourId);

            Text = "A guide has accepted your tour request and has created a new tour " + Tour.Name + " on " + Tour.StartTime + ".\n\nYour spots are automatically reserved, and if you want to see more details about the tour click on the Preview button";

            PreviewCommand = new NavigateCommand<MyTourPreviewViewModel>(mainNavigationStore,
                () => new MyTourPreviewViewModel(Tour, mainNavigationStore, notificationsViewModel));
        }
    }
}
