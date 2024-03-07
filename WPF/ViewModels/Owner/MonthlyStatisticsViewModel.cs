using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.ViewModels.Owner;

public class MonthlyStatisticsViewModel : INotifyPropertyChanged
{
    public AccommodationViewModel AccommodationViewModel { get; set; }

    private ObservableCollection<AccommodationStatistics> accommodationStatistics;
    public ObservableCollection<AccommodationStatistics> AccommodationStatistics
    {
        get => accommodationStatistics;
        set
        {
            if (value == accommodationStatistics) return;
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
            if (statsChart == value) return;
            statsChart = value;
            OnPropertyChanged();
        }
    }

    private readonly AccommodationStatisticsService accommodationStatisticsService = new();
    public string MostOccupiedMonth { get; set; }
    public string[] Labels { get; set; }
    public Func<double, string> Formatter { get; set; }

    public ICommand CloseCommand { get; set; }

    private readonly IBindableBase bindableBase;
    public MonthlyStatisticsViewModel(AccommodationViewModel accommodationViewModel, int year, IBindableBase bindableBase)
    {
        this.bindableBase = bindableBase;

        CloseCommand = new RelayCommand(Close);

        AccommodationViewModel = accommodationViewModel;
        InitializeStatistics(year);
        ChartInitialization();
    }

    private void InitializeStatistics(int year)
    {
        AccommodationStatistics = new ObservableCollection<AccommodationStatistics>();
        for (var i = 1; i < 13; i++)
        {
            AccommodationStatistics.Add(new AccommodationStatistics(
                accommodationStatisticsService.GetTotalReservation(AccommodationViewModel.Accommodation.Id, year,
                    i),
                accommodationStatisticsService.GetTotalCancelledReservations(AccommodationViewModel.Accommodation.Id,
                    year, i),
                accommodationStatisticsService.GetTotalRescheduledReservations(AccommodationViewModel.Accommodation.Id,
                    year, i),
                accommodationStatisticsService.GetTotalRenovationRecommendations(AccommodationViewModel.Accommodation.Id,
                    year, i),
                accommodationStatisticsService.GetOccupancy(AccommodationViewModel.Accommodation.Id, year, i),
                i.ToString()));
        }

        MostOccupiedMonth = ConvertToMonth(accommodationStatisticsService
            .GetMostOccupiedMonth(AccommodationViewModel.Accommodation.Id, year).ToString());
    }

    private void ChartInitialization()
    {
        StatsChart = new SeriesCollection();

        foreach (var accommodationStatistic in AccommodationStatistics)
        {
            StatsChart.Add(new ColumnSeries
            {
                Title = ConvertToMonth(accommodationStatistic.Period),
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
        Labels = new[]
        {
            "Reservations", "Cancelled \nReservations", "Rescheduled \nReservations", "Renovation \nRecommendations", "Occupancy"
        };
        Formatter = value => value.ToString("N");
    }

    private static string ConvertToMonth(string period)
    {
        return period switch
        {
            "1" => "January",
            "2" => "February",
            "3" => "March",
            "4" => "April",
            "5" => "May",
            "6" => "June",
            "7" => "July",
            "8" => "August",
            "9" => "September",
            "10" => "October",
            "11" => "November",
            "12" => "December"
        };
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
