using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.WPF.Stores;
using Tourist_Project.WPF.ViewModels;

namespace Tourist_Project.WPF.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        private readonly Func<TViewModel> createViewModel;
        private readonly Func<bool> canExecute;

        public NavigateCommand(NavigationStore navigationStore, Func<TViewModel> createViewModel, Func<bool> canExecute = null)
        {
            this.navigationStore = navigationStore;
            this.createViewModel = createViewModel;
            this.canExecute = canExecute;

        }

        public override bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }
        public override void Execute(object parameter)
        {
            navigationStore.CurrentViewModel = createViewModel();
        }
    }
}
