using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.ViewModels;
using Tourist_Project.WPF.ViewModels.Guide;

namespace Tourist_Project.WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for TourLiveWindow.xaml
    /// </summary>
    public partial class TourLiveView : Window
    {

        public TourLiveView(Tour selectedTour)
        {
            InitializeComponent();
            DataContext = new TourLiveViewModel(selectedTour, this);
        }

    }
}
