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
    public class ComplexToursHelpViewModel : ViewModelBase
    {
        public ICommand BackCommand { get; set; }
        public ComplexToursHelpViewModel(NavigationStore navigationStore, ComplexToursViewModel previousViewModel)
        {
            BackCommand = new NavigateCommand<ComplexToursViewModel>(navigationStore, () => previousViewModel);
        }
    }
}
