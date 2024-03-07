using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views.Owner;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class CancelRescheduleViewModel
    {
        public ReschedulingReservationViewModel RescheduleRequestViewModel { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        private readonly IBindableBase bindableBase;
        private readonly RescheduleRequestService rescheduleRequestService = new();
        public OwnerMainWindowViewModel ownerMainWindowViewModel;

        public CancelRescheduleViewModel(IBindableBase bindableBase, OwnerMainWindowViewModel ownerMainWindowViewModel, ReschedulingReservationViewModel reschedulingReservationViewModel)
        {
            RescheduleRequestViewModel = reschedulingReservationViewModel;
            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
            this.ownerMainWindowViewModel = ownerMainWindowViewModel;
            this.bindableBase = bindableBase;
        }
        #region Commands
        public void Confirm()
        {
            RescheduleRequestViewModel.RescheduleRequest.Status = RequestStatus.Declined;
            rescheduleRequestService.Update(RescheduleRequestViewModel.RescheduleRequest);
            ownerMainWindowViewModel.RescheduleRequestUpdate(RescheduleRequestViewModel);
            bindableBase.CloseWindow();
        }

        public void Cancel()
        {
            bindableBase.CloseWindow();
        }
        #endregion
    }

}