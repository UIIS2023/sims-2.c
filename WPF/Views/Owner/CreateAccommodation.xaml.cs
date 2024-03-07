using System.Windows;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for CreateAccommodation.xaml
    /// </summary>
    public partial class CreateAccommodation : Window, IBindableBase
    {
        public CreateAccommodation(User user, OwnerMainWindowViewModel ownerMainWindowViewModel)
        {
            InitializeComponent();
            DataContext = new CreateAccommodationViewModel(user, this, ownerMainWindowViewModel);
            ((CreateAccommodationViewModel)DataContext).SetControls(Name, Country, City, Accommodation, House, Cottage, MaxGuests, MinStayingDays, CancellationThreshold, Url );
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
