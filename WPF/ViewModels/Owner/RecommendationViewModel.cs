using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class RecommendationViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<LocationStatisticsViewModel> locationStatisticsByReservation;
        public ObservableCollection<LocationStatisticsViewModel> LocationStatisticsByReservation
        {
            get => locationStatisticsByReservation;
            set
            {
                if(locationStatisticsByReservation == value) return;
                locationStatisticsByReservation = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<LocationStatisticsViewModel> locationStatisticsByOccupancy;
        public ObservableCollection<LocationStatisticsViewModel> LocationStatisticsByOccupancy
        {
            get => locationStatisticsByReservation;
            set
            {
                if(locationStatisticsByReservation == value) return;
                locationStatisticsByReservation = value;
                OnPropertyChanged();
            }
        }
        private SeriesCollection reservationChart;
        public SeriesCollection ReservationChart
        {
            get => reservationChart;
            set
            {
                if (reservationChart == value) return;
                reservationChart = value;
                OnPropertyChanged();
            }
        }

        public string[] Labels { get; set; }

        private readonly IBindableBase bindableBase;
        public ICommand CloseCommand { get; set; }

        public RecommendationViewModel(ObservableCollection<LocationStatisticsViewModel> locationStatisticsByReservation, ObservableCollection<LocationStatisticsViewModel> locationStatisticsByOccupancy, IBindableBase bindableBase)
        {
            this.bindableBase = bindableBase;
            CloseCommand = new RelayCommand(Close);
            Labels = new string[] { };
            LocationStatisticsByReservation = locationStatisticsByReservation;
            LocationStatisticsByOccupancy = locationStatisticsByOccupancy;
            ReservationChart = new SeriesCollection();
            foreach (var locationStatisticsViewModel in LocationStatisticsByReservation)
            {
                ReservationChart.Add( new ColumnSeries
                {
                    Title = locationStatisticsViewModel.Location.ToString(),
                    Values = new ChartValues<int>()
                    {
                        locationStatisticsViewModel.ReservationNo
                    }
                });
            }
        }

        public void Close()
        {
            bindableBase.CloseWindow();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}