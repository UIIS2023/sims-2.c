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
using Tourist_Project.WPF.Views;

namespace Tourist_Project.WPF.ViewModels
{
    public class TourHistoryViewModel : ViewModelBase
    {
        private readonly TourService tourService = new();
        public ObservableCollection<TourDTO> Tours { get; set; }
        public TourDTO SelectedTour { get; set; }
        public User LoggedInUser { get; set; }
        private readonly NavigationStore navigationStore;
        public ICommand ReviewCommand { get; set; }
        public ICommand HelpCommand { get; }

        public TourHistoryViewModel(User user, NavigationStore navigationStore)
        {
            LoggedInUser = user;
            this.navigationStore = navigationStore;

            Tours = new ObservableCollection<TourDTO>(tourService.GetAllPastTours(LoggedInUser.Id));

            ReviewCommand = new NavigateCommand<TourReviewViewModel>(this.navigationStore, () => new TourReviewViewModel(user, SelectedTour, this.navigationStore, this), CanReview);
            HelpCommand = new NavigateCommand<TourHistoryHelpViewModel>(navigationStore, () => new TourHistoryHelpViewModel(navigationStore, this));
        }

        private bool CanReview()
        {
            return SelectedTour != null;
        }
    }
}
