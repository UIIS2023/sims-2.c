using System.Windows;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for RateGuestWindow.xaml
    /// </summary>
    public partial class RateGuestWindow : Window, IBindableBase
    {
        public RateGuestWindow(Notification notification, OwnerMainWindowViewModel ownerMainWindowViewModel)
        {
            InitializeComponent();
            DataContext = new RateGuestViewModel(notification, this, ownerMainWindowViewModel);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
