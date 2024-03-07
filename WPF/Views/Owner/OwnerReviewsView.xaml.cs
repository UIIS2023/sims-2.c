using System.Windows;
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for OwnerReviewsView.xaml
    /// </summary>
    public partial class OwnerReviewsView : Window, IBindableBase
    {
        public OwnerReviewsView(OwnerMainWindowViewModel ownerMainWindowViewModel)
        {
            InitializeComponent();
            DataContext = new OwnerReviewsViewModel(this, ownerMainWindowViewModel);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
