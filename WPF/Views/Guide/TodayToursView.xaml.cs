using System.Collections.ObjectModel;
using System.Windows;
using Tourist_Project.Domain.Models;
using Tourist_Project.Repositories;
using Tourist_Project.WPF.ViewModels;
using Tourist_Project.WPF.ViewModels.Guide;

namespace Tourist_Project.WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for GuideShowWindow.xaml
    /// </summary>
    public partial class TodayToursView : Window
    {
        public TodayToursView(User LoggedInUser)
        {
            InitializeComponent();
            DataContext = new TodayToursViewModel(this, LoggedInUser);
        }
    }
}
