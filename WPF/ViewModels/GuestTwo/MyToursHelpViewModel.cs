using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;

namespace Tourist_Project.WPF.ViewModels
{
    public class MyToursHelpViewModel : ViewModelBase
    {
        public ICommand BackCommand { get; set; }
        public MyToursHelpViewModel(NavigationStore navigationStore, MyToursViewModel previousViewModel)
        {
            BackCommand = new NavigateCommand<MyToursViewModel>(navigationStore, () => previousViewModel);
        }
        public MyToursHelpViewModel(NavigationStore navigationStore, MyTourPreviewViewModel previousViewModel)
        {
            BackCommand = new NavigateCommand<MyTourPreviewViewModel>(navigationStore, () => previousViewModel);
        }
        public MyToursHelpViewModel(NavigationStore navigationStore, TourLiveGuestViewModel previousViewModel)
        {
            BackCommand = new NavigateCommand<TourLiveGuestViewModel>(navigationStore, () => previousViewModel);
        }
    }
}
