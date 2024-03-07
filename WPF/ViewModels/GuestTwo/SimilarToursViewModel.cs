using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class SimilarToursViewModel : ViewModelBase
    {
        private readonly TourService tourService = new();
        public ObservableCollection<TourDTO> Tours { get; set; }
        public TourDTO SelectedTour { get; set; }
        public ICommand ReserveCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand HelpCommand { get; }

        public SimilarToursViewModel(User user, int locationId, int tourId, NavigationStore navigationStore, TourReservationViewModel previousViewModel)
        {            
            Tours = new ObservableCollection<TourDTO>(tourService.GetSimilarTours(locationId, tourId));

            ReserveCommand = new NavigateCommand<TourReservationViewModel>(navigationStore, () => new TourReservationViewModel(user, SelectedTour, navigationStore, this), CanReserve);
            BackCommand = new NavigateCommand<TourReservationViewModel>(navigationStore, () => previousViewModel);
            HelpCommand = new NavigateCommand<SimilarToursHelpViewModel>(navigationStore, () => new SimilarToursHelpViewModel(navigationStore, this));
        }

        private bool CanReserve()
        {
            return SelectedTour != null;
        }
    }
}
