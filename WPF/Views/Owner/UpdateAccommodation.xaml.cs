using System.Windows;
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for UpdateAccommodation.xaml
    /// </summary>
    public partial class UpdateAccommodation : Window, IBindableBase
    {
        public UpdateAccommodation(AccommodationViewModel accommodationViewModel, OwnerMainWindowViewModel ownerMainWindowViewModel)
        {
            InitializeComponent();
            DataContext = new UpdateAccommodationViewModel(this, accommodationViewModel, ownerMainWindowViewModel);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
