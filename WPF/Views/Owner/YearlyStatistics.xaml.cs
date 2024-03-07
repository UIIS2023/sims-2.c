using System.Windows;
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for YearlyStatistics.xaml
    /// </summary>
    public partial class YearlyStatistics : Window, IBindableBase
    {
        public YearlyStatistics(AccommodationViewModel accommodationViewModel)
        {
            InitializeComponent();
            DataContext = new YearlyStatisticsViewModel(accommodationViewModel, this);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
