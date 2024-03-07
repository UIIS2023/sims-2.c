using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class VouchersViewModel : ViewModelBase
    {
        private readonly TourVoucherService voucherService = new();
        private int userId;
        private Message message;

        public ObservableCollection<TourVoucher> Vouchers { get; set; }
        public ICommand DownloadPDFCommand { get; set; }

        public Message Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public ICommand HelpCommand { get; }

        public VouchersViewModel(User user, NavigationStore navigationStore)
        {
            userId = user.Id;
            Vouchers = new ObservableCollection<TourVoucher>(voucherService.GetAllForUser(user.Id));
            DownloadPDFCommand = new RelayCommand(DownloadPDFClick, () => true);
            HelpCommand = new NavigateCommand<VouchersHelpViewModel>(navigationStore, () => new VouchersHelpViewModel(navigationStore, this));
        }

        private void DownloadPDFClick()
        {
            if (voucherService.GeneratePDFReport(userId))
            {
                _ = ShowMessageAndHide(new Message(true, "PDF report created successfully"));
            }
            else
            {
                _ = ShowMessageAndHide(new Message(false, "Error while generating PDF report"));
            }
        }

        private async Task ShowMessageAndHide(Message message)
        {
            Message = message;
            await Task.Delay(5000);
            Message = new Message();
        }
    }
}
