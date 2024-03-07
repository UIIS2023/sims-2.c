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
    public class ComplexToursViewModel : ViewModelBase
    {
        private readonly ComplexTourService complexTourService = new();
        private readonly TourRequestService tourRequestService = new();
        private ComplexTour selectedComplexTour;
        private ObservableCollection<TourRequest> tourRequests;

        public User LoggedUser { get; set; }

        public ComplexTour SelectedComplexTour
        {
            get => selectedComplexTour;
            set
            {
                selectedComplexTour = value;
                TourRequests = new ObservableCollection<TourRequest>(tourRequestService.GetAllForComplexTour(value.Id));
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ComplexTour> ComplexTours { get; set; }

        public ObservableCollection<TourRequest> TourRequests
        {
            get => tourRequests;
            set
            {
                tourRequests = value;
                OnPropertyChanged();
            }
        }
        public ICommand RequestComplexTourCommand { get; set; }
        public ICommand HelpCommand { get; }

        public ComplexToursViewModel(User user, NavigationStore navigationStore)
        {
            LoggedUser = user;

            ComplexTours = new ObservableCollection<ComplexTour>(complexTourService.GetAllForUser(user.Id));
            if (ComplexTours.Count > 0)
            {
                SelectedComplexTour = ComplexTours[0];
            }
            

            RequestComplexTourCommand = new NavigateCommand<RequestComplexTourViewModel>(navigationStore,
                () => new RequestComplexTourViewModel(user, navigationStore));

            HelpCommand = new NavigateCommand<ComplexToursHelpViewModel>(navigationStore, () => new ComplexToursHelpViewModel(navigationStore, this));

        }

    }
}
