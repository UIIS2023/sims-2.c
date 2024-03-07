using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.ViewModels;
using Tourist_Project.WPF.ViewModels.Guide;

namespace Tourist_Project.WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for TouristListWindow.xaml
    /// </summary>
    public partial class TouristListView : Window
    {
        public TouristListView(TourPoint selectedTourPoint, Tour tour)
        {
            InitializeComponent();
            DataContext = new TouristListViewModel(selectedTourPoint, tour, this);
            
        }
    }
}
