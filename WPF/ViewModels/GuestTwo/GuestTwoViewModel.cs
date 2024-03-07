using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;
using Tourist_Project.WPF.Views;
using Tourist_Project.WPF.Views.GuestTwo;

namespace Tourist_Project.WPF.ViewModels
{
    public class GuestTwoViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        private readonly TourVoucherService voucherService = new();
        private readonly TourRequestService requestService = new();
        private readonly ComplexTourService complexTourService = new();
        private GuestTwoView guestTwoWindow;
        private bool tooltipVisibility;

        public bool TooltipVisibility
        {
            get => tooltipVisibility;
            set
            {
                tooltipVisibility = value;
                OnPropertyChanged();
            }
        }
        public ViewModelBase CurrentViewModel => navigationStore.CurrentViewModel;
        public User LoggedUser { get; set; }
        public ICommand HomeCommand { get; set; }
        public ICommand MyToursCommand { get; set; }
        public ICommand TourHistoryCommand { get; set; }
        public ICommand RequestsStatsCommand { get; set; }
        public ICommand VouchersCommand { get; set; }
        public ICommand SignOutCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand NotificationsCommand { get; set; }
        public ICommand ComplexToursCommand { get; set; }
        public ICommand ShowWizardCommand => new RelayCommand(ShowWizard);
        public ICommand SaveSettingsCommand => new RelayCommand(SaveSettings);
        public ICommand UpdateDataCommand => new RelayCommand(UpdateData);

        public GuestTwoViewModel(User user, NavigationStore navigationStore, GuestTwoView guestTwoWindow)
        {
            LoggedUser = user;
            this.navigationStore = navigationStore;
            this.guestTwoWindow = guestTwoWindow;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            TooltipVisibility = true;

            HomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(user, navigationStore));
            MyToursCommand = new NavigateCommand<MyToursViewModel>(navigationStore, () => new MyToursViewModel(user, navigationStore));
            TourHistoryCommand = new NavigateCommand<TourHistoryViewModel>(navigationStore, () => new TourHistoryViewModel(user, navigationStore));
            RequestsStatsCommand = new NavigateCommand<RequestsStatsViewModel>(navigationStore, () => new RequestsStatsViewModel(user, navigationStore));
            VouchersCommand = new NavigateCommand<VouchersViewModel>(navigationStore, () => new VouchersViewModel(user, navigationStore));
            NotificationsCommand = new NavigateCommand<NotificationsViewModel>(navigationStore, () => new NotificationsViewModel(user, navigationStore));
            ComplexToursCommand = new NavigateCommand<ComplexToursViewModel>(navigationStore, () => new ComplexToursViewModel(user, navigationStore));
            SignOutCommand = new RelayCommand(SignOutClick);
            ExitCommand = new RelayCommand(ExitClick);

        }

        private void UpdateData()
        {
            voucherService.DeleteInvalidVouchers(LoggedUser.Id);
            voucherService.ClaimFiveToursInAYearVoucher(LoggedUser.Id);
            requestService.UpdateInvalidRequests(LoggedUser.Id);
            complexTourService.UpdateComplexTourStatusesForUser(LoggedUser.Id);
        }

        private void ShowWizard()
        {
            if (Properties.Settings.Default.ShowWizard)
            {
                WizardWindow wizardWindow = new();
                wizardWindow.ShowDialog();
            }
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }

        private void ExitClick()
        {
            guestTwoWindow.Close();
        }

        private void SignOutClick()
        {
            var LoginWindow = new LoginWindow();
            LoginWindow.Show();
            guestTwoWindow.Close();
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
