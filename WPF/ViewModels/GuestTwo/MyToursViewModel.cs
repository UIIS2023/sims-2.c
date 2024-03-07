using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.DTO;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;
using Tourist_Project.WPF.Views;

namespace Tourist_Project.WPF.ViewModels
{
    public class MyToursViewModel : ViewModelBase
    {
        private readonly TourService tourService = new();
        private readonly TourAttendanceService attendanceService = new();
        private readonly NavigationStore navigationStore;

        public User LoggedInUser { get; set; }
        public ObservableCollection<TourDTO> FutureTours { get; set; }
        public ObservableCollection<TourDTO> TodaysTours { get; set; }
        public TourDTO SelectedFutureTour { get; set; }
        public TourDTO SelectedTodayTour { get; set; }
        public TourAttendance TourAttendance => attendanceService.GetByTourIdAndUserId(SelectedTodayTour.Id, LoggedInUser.Id);

        private Message message;
        public Message Message
        {
            get { return message; }
            set
            {
                if (message != value)
                {
                    message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }
        public ICommand PreviewCommand { get; set; }
        public ICommand JoinCommand { get; set; }
        public ICommand WatchLiveCommand { get; set; }
        public ICommand HelpCommand { get; set; }

        public MyToursViewModel(User user, NavigationStore navigationStore) 
        {
            LoggedInUser = user;
            this.navigationStore = navigationStore;

            Message = new Message();
            FutureTours = new ObservableCollection<TourDTO>(tourService.GetUsersFutureTours(LoggedInUser.Id));
            TodaysTours = new ObservableCollection<TourDTO>(tourService.GetUsersTodayTours(LoggedInUser.Id));

            PreviewCommand = new NavigateCommand<MyTourPreviewViewModel>(this.navigationStore, () => new MyTourPreviewViewModel(SelectedFutureTour, this.navigationStore, this), CanPreview);
            JoinCommand = new RelayCommand(OnJoinClick, CanJoin);
            WatchLiveCommand = new NavigateCommand<TourLiveGuestViewModel>(this.navigationStore, () => new TourLiveGuestViewModel(SelectedTodayTour, navigationStore, this), CanWatchLive);
            HelpCommand = new NavigateCommand<MyToursHelpViewModel>(navigationStore, () => new MyToursHelpViewModel(navigationStore, this));
        }

        private bool CanPreview()
        {
            return SelectedFutureTour != null;
        }

        private bool CanJoin()
        {
            return SelectedTodayTour != null && SelectedTodayTour.Status == Status.Begin && TourAttendance.Presence == Presence.No;
        }

        private bool CanWatchLive()
        {
            return SelectedTodayTour != null && TourAttendance.Presence == Presence.Yes;
        }

        private async Task ShowMessageAndHide(Message message)
        {
            Message = message;
            await Task.Delay(5000);
            Message = new Message();
        }

        private void OnJoinClick()
        {
            TourAttendance tourAttendance = attendanceService.GetByTourIdAndUserId(SelectedTodayTour.Id, LoggedInUser.Id);
            tourAttendance.Presence = Presence.Joined;
            attendanceService.Update(tourAttendance);
            _ = ShowMessageAndHide(new Message(true, "You have successfully joined the tour,\nnow you have to wait for the guide to call you out"));
        }
    }
}
