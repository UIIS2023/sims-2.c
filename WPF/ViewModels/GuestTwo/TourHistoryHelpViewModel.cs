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
    public class TourHistoryHelpViewModel : ViewModelBase
    {
        public ICommand BackCommand { get; set; }
        public TourHistoryHelpViewModel(NavigationStore navigationStore, TourHistoryViewModel previousViewModel)
        {
            BackCommand = new NavigateCommand<TourHistoryViewModel>(navigationStore, () => previousViewModel);
        }
    }
}
