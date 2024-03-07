using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views.Owner;

namespace Tourist_Project.WPF.ViewModels.Owner;

public class OwnerReviewsViewModel : INotifyPropertyChanged
{
    private ObservableCollection<AccommodationRatingViewModel> reviews;
    public ObservableCollection<AccommodationRatingViewModel> Reviews
    {
        get => reviews;
        set
        {
            if(value == reviews) return;
            reviews = value;
            OnPropertyChanged("Reviews");
        }
    }
    private readonly AccommodationRatingService ratingService = new();
    public AccommodationRatingViewModel SelectedReview { get; set; }
    public OwnerMainWindowViewModel OwnerMainWindowViewModel;
    public ICommand CancelCommand { get; set; }
    public ICommand UpdateCommand { get; set; }
    public ICommand RenovateCommand { get; set; }

    private readonly IBindableBase bindableBase;
    public OwnerReviewsViewModel(IBindableBase bindableBase, OwnerMainWindowViewModel ownerMainWindowViewModel)
    {
        OwnerMainWindowViewModel = ownerMainWindowViewModel;
        Reviews = new ObservableCollection<AccommodationRatingViewModel>(ratingService.GetAll().Select(rating => new AccommodationRatingViewModel(rating, OwnerMainWindowViewModel)));
        CancelCommand = new RelayCommand(Cancel);
        UpdateCommand = new RelayCommand(Update, CanUpdate);
        RenovateCommand = new RelayCommand(Renovate, CanUpdate);
        this.bindableBase = bindableBase;
    }
    public void Update()
    {
        var updateWindow = new UpdateAccommodation(new AccommodationViewModel(SelectedReview.Accommodation), OwnerMainWindowViewModel);
        updateWindow.Show();
    }

    public bool CanUpdate()
    {
        return SelectedReview != null;
    }

    public void Renovate()
    {
        var renovateWindow = new ScheduleRenovation(new AccommodationViewModel(SelectedReview.Accommodation));
        renovateWindow.ShowDialog();
    }

    public void Cancel()
    {
        bindableBase.CloseWindow();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}