using System.Collections.ObjectModel;
using System.Windows;
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for Recommendations.xaml
    /// </summary>
    public partial class Recommendations : Window, IBindableBase
    {
        public Recommendations(ObservableCollection<LocationStatisticsViewModel> reservations, ObservableCollection<LocationStatisticsViewModel> occupancy)
        {
            InitializeComponent();
            DataContext = new RecommendationViewModel(reservations, occupancy, this);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
