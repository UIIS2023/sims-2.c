using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views.Owner;

namespace Tourist_Project.WPF.ViewModels
{
    public class ShowRenovationsViewModel: INotifyPropertyChanged
    {
        private ObservableCollection<RenovationViewModel> scheduledRenovations;
        public ObservableCollection<RenovationViewModel> ScheduledRenovations
        {
            get => scheduledRenovations;
            set
            {
                if(value == scheduledRenovations) return;
                scheduledRenovations = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<RenovationViewModel> previousRenovations;
        public ObservableCollection<RenovationViewModel> PreviousRenovations
        {
            get => previousRenovations;
            set
            {
                if(value == previousRenovations) return;
                previousRenovations = value;
                OnPropertyChanged();
            }
        }
        public ICommand CancelCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        public RenovationViewModel SelectedRenovation { get; set; }

        private readonly RenovationService renovationService = new();
        private readonly IBindableBase bindableBase;
        public ShowRenovationsViewModel(IBindableBase bindableBase)
        {
            this.bindableBase = bindableBase;
            ScheduledRenovations = new ObservableCollection<RenovationViewModel>(renovationService.GetAll().Where(renovation => renovation.RenovatingSpan.StartingDate > DateTime.Now || renovation.RenovatingSpan.EndingDate > DateTime.Now).Select(renovation => new RenovationViewModel(renovation)));
            PreviousRenovations = new ObservableCollection<RenovationViewModel>(renovationService.GetAll().Where(renovation => renovation.RenovatingSpan.StartingDate < DateTime.Now && renovation.RenovatingSpan.EndingDate < DateTime.Now).Select(renovation => new RenovationViewModel(renovation)));
            CancelCommand = new RelayCommand(Cancel, CanCancel);
            ExitCommand = new RelayCommand(Exit);
        }

        public void Cancel()
        {
            renovationService.Delete(SelectedRenovation.Renovation.Id);
            ScheduledRenovations.Remove(SelectedRenovation);
        }

        public bool CanCancel()
        {
            return SelectedRenovation != null && (SelectedRenovation.Renovation.RenovatingSpan.StartingDate - DateTime.Now).Days > 5;
        }

        public void Exit()
        {
            bindableBase.CloseWindow();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    } 
}
