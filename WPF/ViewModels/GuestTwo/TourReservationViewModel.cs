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
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.DTO;
using Tourist_Project.Repositories;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;

namespace Tourist_Project.WPF.ViewModels
{
    public class TourReservationViewModel : ViewModelBase
    {
        private readonly TourVoucherService voucherService = new();
        private readonly TourReservationService reservationService = new();
        private readonly TourAttendanceService attendanceService = new();
        private readonly TourPointRepository tourPointRepository = new();
        private readonly ImageRepository imageRepository = new();
        private readonly NavigationStore navigationStore;
        private bool reservationExisted;
        private TourVoucher consumedTourVoucher;
        private ObservableCollection<string> vouchers;
        private string selectedVoucherName;
        private Message message;
        private Message undoMessage;
        private int imageId;
        private readonly int imagesCount;
        private Image currentImage;

        public List<Image> TourImages { get; set; }
        public int GuestsNumber { get; set; }
        public User LoggedUser { get; set; }
        public TourDTO SelectedTour { get; set; }
        public string Checkpoints { get; set; }

        public ObservableCollection<string> Vouchers
        {
            get => vouchers;
            set
            {
                vouchers = value;
                OnPropertyChanged();
            }
        }

        public string SelectedVoucherName
        {
            get => selectedVoucherName;
            set
            {
                if(value != selectedVoucherName)
                {
                    selectedVoucherName = value;
                    OnPropertyChanged(nameof(SelectedVoucherName));
                }
            }
        }

        public Message Message
        {
            get => message;
            set
            {
                if (value == message) return;
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public Message UndoMessage
        {
            get => undoMessage;
            set
            {
                undoMessage = value;
                OnPropertyChanged();
            }
        }

        public Image CurrentImage
        {
            get => currentImage;
            set
            {
                if (value != currentImage)
                {
                    currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }
        public ICommand NextCommand { get; set; }
        public ICommand PreviousCommand { get; set; }
        public ICommand ReserveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand UndoReservationCommand { get; set; }
        public ICommand HelpCommand { get; set; }

        public TourReservationViewModel(User user, TourDTO tour, NavigationStore navigationStore, HomeViewModel previousViewModel)
        {
            LoggedUser = user;
            SelectedTour = tour;
            this.navigationStore = navigationStore;
            UndoMessage = new Message();
            reservationExisted = reservationService.GetByUserIdAndTourId(LoggedUser.Id, SelectedTour.Id) != null;

            TourImages = imageRepository.GetByAssociationAndId(ImageAssociation.Tour, tour.Id);
            imagesCount = TourImages.Count;
            if (imagesCount > 0)
            {
                CurrentImage = TourImages[0];
                imageId = 0;
            }
            else
            {
                CurrentImage = new Image("/Images/No images to show.jpg");
            }


            Checkpoints = tourPointRepository.GetAllForTourString(SelectedTour.Id);
            Vouchers = voucherService.LoadVouchers(user.Id);
            SelectedVoucherName = Vouchers.First();

            BackCommand = new NavigateCommand<HomeViewModel>(this.navigationStore, () => previousViewModel);
            HelpCommand = new NavigateCommand<TourReservationHelpViewModel>(navigationStore, () => new TourReservationHelpViewModel(navigationStore, this));
            ReserveCommand = new RelayCommand(OnReserveClick, () => GuestsNumber > 0);
            NextCommand = new RelayCommand(OnNextClick, () => imagesCount > 0);
            PreviousCommand = new RelayCommand(OnPreviousClick, () => imagesCount > 0);
            UndoReservationCommand = new RelayCommand(OnUndoReservationClick, () => UndoMessage.Type);
        }

        public TourReservationViewModel(User user, TourDTO tour, NavigationStore navigationStore, SimilarToursViewModel previousViewModel)
        {
            LoggedUser = user;
            SelectedTour = tour;
            this.navigationStore = navigationStore;
            UndoMessage = new Message();
            reservationExisted = reservationService.GetByUserIdAndTourId(LoggedUser.Id, SelectedTour.Id) != null;

            TourImages = imageRepository.GetByAssociationAndId(ImageAssociation.Tour, tour.Id);
            CurrentImage = TourImages[0];
            imagesCount = TourImages.Count;
            imageId = 0;

            Checkpoints = tourPointRepository.GetAllForTourString(SelectedTour.Id);
            Vouchers = voucherService.LoadVouchers(user.Id);
            SelectedVoucherName = Vouchers.First();

            ReserveCommand = new RelayCommand(OnReserveClick, () => GuestsNumber > 0);
            BackCommand = new NavigateCommand<SimilarToursViewModel>(this.navigationStore, () => previousViewModel);
            NextCommand = new RelayCommand(OnNextClick);
            PreviousCommand = new RelayCommand(OnPreviousClick);
            UndoReservationCommand = new RelayCommand(OnUndoReservationClick, () => UndoMessage.Type);
        }

        private void OnPreviousClick()
        {
            imageId = imageId - 1 < 0 ? imagesCount - 1 : imageId - 1;
            CurrentImage = TourImages[imageId];
        }

        private void OnNextClick()
        {
            imageId = imageId + 1 == imagesCount ? 0 : imageId + 1;
            CurrentImage = TourImages[imageId];
        }

        private void OnUndoReservationClick()
        {
            var undoReservation = reservationService.GetForUndo(LoggedUser.Id, SelectedTour.Id);
            if (reservationExisted)
            {
                undoReservation.GuestsNumber -= GuestsNumber;
                reservationService.Update(undoReservation);
            }
            else
            {
                reservationService.Delete(undoReservation.Id);
                attendanceService.DeleteLastAttendance(LoggedUser.Id, SelectedTour.Id);
            }

            SelectedTour.SpotsLeft += GuestsNumber;
            if (consumedTourVoucher != null)
            {
                voucherService.Create(consumedTourVoucher);
                Vouchers = voucherService.LoadVouchers(LoggedUser.Id);
            }
            _ = ShowMessageAndHide(new Message(true, "Reservation undone!"));
            UndoMessage = new Message();
        }

        private void OnReserveClick()
        {
            var tourCapacityLeft = SelectedTour.SpotsLeft;

            if (tourCapacityLeft == 0)
            {
                navigationStore.CurrentViewModel = new SimilarToursViewModel(LoggedUser, SelectedTour.LocationId, SelectedTour.Id, navigationStore, this);
            }
            else if (tourCapacityLeft < GuestsNumber)
            {
                _ = ShowMessageAndHide(new Message(false, "Unfortunately, we can't accept that many guests.\n" +
                                                          "Capacity left: " + tourCapacityLeft.ToString()));
            }
            else
            {
                
                if(!reservationExisted)
                {
                    var newReservation = new TourReservation(LoggedUser.Id, SelectedTour.Id, GuestsNumber, SelectedVoucherName != "Without voucher");
                    reservationService.Save(newReservation);
                    var newAttendance = new TourAttendance(LoggedUser.Id, SelectedTour.Id);
                    attendanceService.Save(newAttendance);
                }
                else
                {
                    var tourReservation = reservationService.GetByUserIdAndTourId(LoggedUser.Id, SelectedTour.Id);
                    tourReservation.GuestsNumber += GuestsNumber;
                    reservationService.Update(tourReservation);
                }


                SelectedTour.SpotsLeft -= GuestsNumber;
                if (SelectedVoucherName != "Without voucher")
                {
                    consumedTourVoucher = voucherService.GetEarliestByNameForUser(SelectedVoucherName, LoggedUser.Id);
                    voucherService.Delete(consumedTourVoucher.Id);
                    Vouchers = voucherService.LoadVouchers(LoggedUser.Id);
                    if(Vouchers.Count > 0) SelectedVoucherName = Vouchers.First();
                    
                }

                _ = ShowMessageAndHide(new Message(true, "Successful reservation"));
            }

        }
        private async Task ShowMessageAndHide(Message message)
        {
            Message = message;
            if (message.Type)
            {
                UndoMessage = new Message(true, "Undo");
            }

            await Task.Delay(15000);
            Message = new Message();
            UndoMessage = new Message();
        }
    }
}
