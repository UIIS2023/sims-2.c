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
using Tourist_Project.Repositories;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;

namespace Tourist_Project.WPF.ViewModels
{
    public class TourLiveGuestViewModel : ViewModelBase
    {
        public TourDTO SelectedTour { get; set; }
        public string Checkpoints { get; set; }
        public ObservableCollection<TourPoint> TourPoints { get; set; }

        private readonly TourPointRepository tourPointRepository = new();
        private readonly NavigationStore navigationStore;
        public ICommand BackCommand { get; set; }
        public ICommand HelpCommand { get; }

        public TourLiveGuestViewModel(TourDTO tour, NavigationStore navigationStore, MyToursViewModel previousViewModel)
        {
            SelectedTour = tour;
            this.navigationStore = navigationStore;
            Checkpoints = tourPointRepository.GetAllForTourString(SelectedTour.Id);
            TourPoints = new ObservableCollection<TourPoint>(tourPointRepository.GetAllForTour(SelectedTour.Id));

            BackCommand = new NavigateCommand<MyToursViewModel>(this.navigationStore, () => previousViewModel);
            HelpCommand = new NavigateCommand<MyToursHelpViewModel>(navigationStore, () => new MyToursHelpViewModel(navigationStore, this));
        }
    }
}
