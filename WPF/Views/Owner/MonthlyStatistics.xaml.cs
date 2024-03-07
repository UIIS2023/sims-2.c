using System.Windows;
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for MonthlyStatistics.xaml
    /// </summary>
    public partial class MonthlyStatistics : Window, IBindableBase
    {
        public MonthlyStatistics(AccommodationViewModel accommodationViewModel, int year)
        {
            InitializeComponent();
            DataContext = new MonthlyStatisticsViewModel(accommodationViewModel, year, this);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
