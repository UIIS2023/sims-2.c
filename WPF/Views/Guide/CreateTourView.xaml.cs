using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Tourist_Project.Domain.Models;
using Tourist_Project.Repositories;
using Image = Tourist_Project.Domain.Models.Image;
using Tourist_Project.WPF.ViewModels;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.WPF.ViewModels.Guide;

namespace Tourist_Project.WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for CreateTour.xaml
    /// </summary>
    public partial class CreateTourView : Window
    {
        public CreateTourView(User LoggedInUser)
        {
            InitializeComponent();
            DataContext = new CreateTourViewModel(this, LoggedInUser);
        }
    }
}
