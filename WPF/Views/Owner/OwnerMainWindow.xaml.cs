using System.Windows;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window, IBindableBase
    {
        public OwnerMainWindow(User user)
        {
            InitializeComponent();
            DataContext = new OwnerMainWindowViewModel(this);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
