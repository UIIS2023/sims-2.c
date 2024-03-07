using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views.Owner;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class YearlyStatisticsViewModel : INotifyPropertyChanged
    {
        public AccommodationViewModel AccommodationViewModel { get; set; }

        private ObservableCollection<AccommodationStatistics> accommodationStatistics;
        public ObservableCollection<AccommodationStatistics> AccommodationStatistics
        {
            get => accommodationStatistics;
            set
            {
                if(value == accommodationStatistics) return;
                accommodationStatistics = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection statsChart;

        public SeriesCollection StatsChart
        {
            get => statsChart;
            set
            {
                if(statsChart == value) return;
                statsChart = value;
                OnPropertyChanged();
            }
        }

        private readonly AccommodationStatisticsService accommodationStatisticsService = new();
        public List<int> Years { get; set; }
        public int MostOccupiedYear { get; set; }

        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public AccommodationStatistics SelectedStatistics { get; set; }
        public ICommand SwitchToMonthlyCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private readonly IBindableBase bindableBase;

        public YearlyStatisticsViewModel(AccommodationViewModel accommodationViewModel, IBindableBase bindableBase)
        {
            this.bindableBase = bindableBase;
            AccommodationViewModel = accommodationViewModel;
            Years = accommodationStatisticsService.GetYears(AccommodationViewModel.Accommodation.Id);
            AccommodationStatistics = new ObservableCollection<AccommodationStatistics>();
            foreach (var year in Years)
            {
                AccommodationStatistics.Add(new AccommodationStatistics(accommodationStatisticsService.GetTotalReservation(accommodationViewModel.Accommodation.Id, year, 0),
                    accommodationStatisticsService.GetTotalCancelledReservations(accommodationViewModel.Accommodation.Id, year, 0),
                    accommodationStatisticsService.GetTotalRescheduledReservations(accommodationViewModel.Accommodation.Id, year, 0),
                    accommodationStatisticsService.GetTotalRenovationRecommendations(accommodationViewModel.Accommodation.Id, year, 0),
                    accommodationStatisticsService.GetOccupancy(accommodationViewModel.Accommodation.Id, year, 0),
                    year.ToString()));
            }
            MostOccupiedYear = accommodationStatisticsService.GetMostOccupiedYear(AccommodationViewModel.Accommodation.Id);
            SwitchToMonthlyCommand = new RelayCommand(SwitchToMonthly, CanSwitch);
            CloseCommand = new RelayCommand(Close);

            ChartInitialization();
        }

        private void ChartInitialization()
        {
            StatsChart = new SeriesCollection();

            foreach (var accommodationStatistic in AccommodationStatistics)
            {
                StatsChart.Add(new ColumnSeries
                {
                    Title = accommodationStatistic.Period,
                    Values = new ChartValues<int>
                    {
                        accommodationStatistic.TotalReservations,
                        accommodationStatistic.CancelledReservations,
                        accommodationStatistic.RescheduledReservations,
                        accommodationStatistic.RescheduledReservations,
                        accommodationStatistic.Occupancy
                    }
                });
            }
            Labels = new []
            {
                "Reservations", "Cancelled \nReservations", "Rescheduled \nReservations", "Renovation \nRecommendations", "Occupancy"
            };
            Formatter = value => value.ToString("N");
        }

        public void SwitchToMonthly()
        {
            var monthlyStatistics = new MonthlyStatistics(AccommodationViewModel, Convert.ToInt32(SelectedStatistics.Period));
            monthlyStatistics.ShowDialog();
        }

        public bool CanSwitch()
        {
            return true;
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