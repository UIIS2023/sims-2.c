using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views;
using Tourist_Project.WPF.Views.Owner;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class OwnerMainWindowViewModel 
    {
        #region Collections
        private ObservableCollection<NotificationViewModel> guestRatingNotifications;
        public ObservableCollection<NotificationViewModel> GuestRatingNotifications
        {
            get => guestRatingNotifications;
            set
            {
                if (value == guestRatingNotifications) return;
                guestRatingNotifications = value;
                OnPropertyChanged(nameof(GuestRatingNotifications));
            }
        }
        private ObservableCollection<Notification> reviewNotifications;
        public ObservableCollection<Notification> ReviewNotifications
        {
            get => reviewNotifications;
            set
            {
                if(value == reviewNotifications) return;
                reviewNotifications = value;
                OnPropertyChanged(nameof(ReviewNotifications));
            }
        }
        private ObservableCollection<ReschedulingReservationViewModel> rescheduleRequests;
        public ObservableCollection<ReschedulingReservationViewModel> RescheduleRequests
        {
            get => rescheduleRequests;
            set
            {
                if(value == rescheduleRequests) return;
                rescheduleRequests = value;
                OnPropertyChanged(nameof(RescheduleRequests));
            }
        }
        private ObservableCollection<AccommodationRating> accommodationRatings;
        public ObservableCollection<AccommodationRating> AccommodationRatings
        {
            get => accommodationRatings;
            set
            {
                if(value == accommodationRatings) return;
                accommodationRatings = value;
                OnPropertyChanged(nameof(AccommodationRatings));
            }
        }
        private ObservableCollection<AccommodationViewModel> accommodationView;
        public ObservableCollection<AccommodationViewModel> AccommodationView
        {
            get => accommodationView;
            set
            {
                if(value == accommodationView) return;
                accommodationView = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<NotificationViewModel> forums;
        public ObservableCollection<NotificationViewModel> Forums
        {
            get => forums;
            set
            {
                if(value == forums) return;
                forums = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LocationStatisticsViewModel> recommendationsByReservation;
        public ObservableCollection<LocationStatisticsViewModel> RecommendationsByReservation
        {
            get => recommendationsByReservation;
            set
            {
                if(value == recommendationsByReservation) return;
                recommendationsByReservation = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<LocationStatisticsViewModel> recommendationsByOccupancy;
        public ObservableCollection<LocationStatisticsViewModel> RecommendationsByOccupancy
        {
            get => recommendationsByOccupancy;
            set
            {
                if(value == recommendationsByOccupancy) return;
                recommendationsByOccupancy = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Services
        private static AccommodationService accommodationService = new();
        private static NotificationService notificationService = new();
        private static ReservationService reservationService = new();
        private static AccommodationRatingService accommodationRatingService = new();
        private static UserService userService = new();
        private static RescheduleRequestService rescheduleRequestService = new();
        private static MessageService messageService = new();
        #endregion
        #region SelectedProperties
        public static AccommodationViewModel SelectedAccommodation { get; set; }
        public static NotificationViewModel SelectedRating { get; set; }
        public static ReschedulingReservationViewModel SelectedRescheduleRequest { get; set; } 
        #endregion
        public static User User { get; set; }
        private readonly IBindableBase bindableBase;
        public OwnerMainWindow OwnerMainWindow { get; set; }
        public LocationStatisticsViewModel BestLocationByReservation { get; set; }
        public LocationStatisticsViewModel BestLocationByOccupancy { get; set; }
        public LocationStatisticsViewModel WorstLocationByReservation { get; set; }
        public LocationStatisticsViewModel WorstLocationByOccupancy { get; set; }
        public string Rating { get; set; }
        #region Commands
        public ICommand CreateCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand RateCommand { get; set; }
        public ICommand ShowReviewsCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand ConfirmRescheduleCommand { get; set; }
        public ICommand CancelRescheduleCommand { get; set; }
        public ICommand RenovateCommand { get; set; }
        public ICommand ShowRenovationsCommand { get; set; }
        public ICommand ShowStatisticsCommand { get; set; }
        public ICommand ShowForumsCommand { get; set; }
        public ICommand CreateRecommendationCommand { get; set; }
        public ICommand CreatePDFReport { get; set; }
        #endregion

        public OwnerMainWindowViewModel(IBindableBase bindableBase)
        {
            this.bindableBase = bindableBase;
            #region CommandInstanting
            CreateCommand = new RelayCommand(Create);
            UpdateCommand = new RelayCommand(Update, CanUpdate);
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            RateCommand = new RelayCommand(Rate, CanRate);
            ShowReviewsCommand = new RelayCommand(ShowReview, CanShow);
            LogOutCommand = new RelayCommand(LogOut);
            ConfirmRescheduleCommand = new RelayCommand(ConfirmReschedule, CanConfirmReschedule);
            CancelRescheduleCommand = new RelayCommand(CancelReschedule, CanCancelReschedule);
            RenovateCommand = new RelayCommand(Renovate, CanRenovate);
            ShowRenovationsCommand = new RelayCommand(ShowRenovations);
            ShowStatisticsCommand = new RelayCommand(ShowStatistics, CanShowStatistics);
            ShowForumsCommand = new RelayCommand(ShowForums);
            CreateRecommendationCommand = new RelayCommand(CreateRecommendation);
            CreatePDFReport = new RelayCommand(PdfReport, CanUpdate);
            #endregion
            #region CollectionInstanting
            User = App.LoggedInUser;
            AccommodationRatings = new ObservableCollection<AccommodationRating>(accommodationRatingService.GetAll());
            RescheduleRequests = new ObservableCollection<ReschedulingReservationViewModel>(rescheduleRequestService.GetAll().Where(rescheduleRequest => rescheduleRequest.Status == RequestStatus.Pending).Select(rescheduleRequest => new ReschedulingReservationViewModel(rescheduleRequest, this)));
            GuestRatingNotifications = new ObservableCollection<NotificationViewModel>(notificationService.GetAllByType("GuestRate").Select(notification => new NotificationViewModel(notification)));
            ReviewNotifications = new ObservableCollection<Notification>(notificationService.GetAllByType("Reviews").Where(notification => notification.IsNotified == false));
            AccommodationView = new ObservableCollection<AccommodationViewModel>(accommodationService.GetAll().Select(accommodation => new AccommodationViewModel(accommodation)).OrderBy(o => !o.Accommodation.IsRecentlyRenovated));
            Forums = new ObservableCollection<NotificationViewModel>(notificationService.GetAllByType("Forum").Where(notification => notification.IsNotified == false).Select(notification => new NotificationViewModel(notification)));
            RecommendationsByReservation = new ObservableCollection<LocationStatisticsViewModel>(accommodationService.GetLocationsIds(User.Id).Select(id => new LocationStatisticsViewModel(id)).OrderByDescending(o => o.ReservationNo));
            BestLocationByReservation = RecommendationsByReservation.First();
            WorstLocationByReservation = RecommendationsByReservation.Last();
            RecommendationsByOccupancy = new ObservableCollection<LocationStatisticsViewModel>(accommodationService.GetLocationsIds(User.Id).Select(id => new LocationStatisticsViewModel(id)).OrderByDescending(o => o.Occupancy));
            BestLocationByOccupancy = RecommendationsByOccupancy.First();
            WorstLocationByOccupancy = RecommendationsByOccupancy.Last();
            #endregion
            Rating = accommodationRatingService.getRating().ToString("F3");
        }
        #region CommandsLogic
        public void Create()
        {
            var createWindow = new CreateAccommodation(User, this);
            createWindow.ShowDialog();
        }
        public void Update()
        {
            var updateWindow = new UpdateAccommodation(SelectedAccommodation, this);
            updateWindow.ShowDialog();
        }
        public static bool CanUpdate()
        {
            return SelectedAccommodation != null;
        }

        public void Delete()
        {
            if (!messageService.ShowMessageBox(
                    $"Are you sure you want to delete {SelectedAccommodation.Accommodation.Name}",
                    "Deleting an accommodation"))
                return;
            accommodationService.Delete(SelectedAccommodation.Accommodation.Id);
            accommodationView.Remove(SelectedAccommodation);
        }
        public static bool CanDelete()
        {
            return SelectedAccommodation != null;
        }

        public void Rate()
        {
            var rateWindow = new RateGuestWindow(SelectedRating.Notification, this);
            rateWindow.ShowDialog();
        }
        public static bool CanRate()
        {
            return SelectedRating != null;
        }

        public void ShowReview()
        {
            var showReviewsWindow = new OwnerReviewsView(this);
            foreach (var notification in ReviewNotifications)
            {
                notification.IsNotified = true;
                notificationService.Update(notification);
            }
            ReviewNotifications.Clear();
            ReviewNotifications = new ObservableCollection<Notification>(notificationService.GetAllByType("GuestRate"));
            showReviewsWindow.ShowDialog();
        }
        public bool CanShow()
        {
            return GuestRatingNotifications.Count == 0;
        }
        public void LogOut()
        {
            var loginWindow = new LoginWindow();
            bindableBase.CloseWindow();
            loginWindow.ShowDialog();
        }
        public void ConfirmReschedule()
        {
            UpdateReservation();
            SelectedRescheduleRequest.RescheduleRequest.Status = RequestStatus.Confirmed;
            rescheduleRequestService.Update(SelectedRescheduleRequest.RescheduleRequest);
            RescheduleRequests.Remove(SelectedRescheduleRequest);
        }

        private static void UpdateReservation()
        {
            SelectedRescheduleRequest.Reservation.CheckIn = SelectedRescheduleRequest.RescheduleRequest.NewBeginningDate;
            SelectedRescheduleRequest.Reservation.CheckOut = SelectedRescheduleRequest.RescheduleRequest.NewEndDate;
            SelectedRescheduleRequest.Reservation.Status = ReservationStatus.Rescheduled;
            reservationService.Update(SelectedRescheduleRequest.Reservation);
        }

        public bool CanConfirmReschedule()
        {
            return SelectedRescheduleRequest != null;
        }

        public void CancelReschedule()
        {
            var cancelRescheduleWindow = new CancelRescheduleRequest(this, SelectedRescheduleRequest);
            cancelRescheduleWindow.ShowDialog();
        }
        public bool CanCancelReschedule()
        {
            return SelectedRescheduleRequest != null;
        }

        public void Renovate()
        {
            var scheduleRenovation = new ScheduleRenovation(SelectedAccommodation);
            scheduleRenovation.ShowDialog();
        }

        public bool CanRenovate()
        {
            return SelectedAccommodation != null;
        }

        public void ShowRenovations()
        {
            var showRenovations = new ShowRenovations();
            showRenovations.ShowDialog();
        }

        public void ShowStatistics()
        {
            var showStatistics = new YearlyStatistics(SelectedAccommodation);
            showStatistics.ShowDialog();
        }

        public bool CanShowStatistics()
        {
            return SelectedAccommodation != null;
        }

        public void ShowForums()
        {
            var showForums = new Forums();
            foreach (var notification in Forums)
            {
                notification.Notification.IsNotified = true;
                notificationService.Update(notification.Notification);
            }
            Forums.Clear();
            Forums = new ObservableCollection<NotificationViewModel>(notificationService.GetAllByType("Forum").Select(notification => new NotificationViewModel(notification)));
            showForums.ShowDialog();
        }

        public void CreateRecommendation()
        {
            var createRecommendation = new Recommendations(RecommendationsByReservation, RecommendationsByOccupancy);
            createRecommendation.ShowDialog();
        }

        public static void PdfReport()
        {
            var createReport = new GeneratePDF(SelectedAccommodation);
            createReport.ShowDialog();
        }
        #endregion
        public void GuestRateUpdate(Notification notification)
        {
            foreach (var guestRatingNotification in GuestRatingNotifications)
            {
                if (guestRatingNotification.Notification != notification) continue;
                GuestRatingNotifications.Remove(guestRatingNotification);
                return;
            }
        }

        public void RescheduleRequestUpdate(ReschedulingReservationViewModel reschedulingReservationViewModel)
        {
            if (SelectedRescheduleRequest == null) 
                rescheduleRequests.Remove(reschedulingReservationViewModel); 
            else 
                RescheduleRequests.Remove(SelectedRescheduleRequest);

        }

        public void CreateAccommodation(Accommodation accommodation)
        {
            var accommodationViewModel = new AccommodationViewModel(accommodation);
            AccommodationView.Add(accommodationViewModel);
        }

        public void UpdateAccommodation()
        {
            AccommodationView.Remove(SelectedAccommodation);
            var accommodationViewModel = new AccommodationViewModel(accommodationService.Get(SelectedAccommodation.Accommodation.Id));
            AccommodationView.Add(accommodationViewModel);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}