using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Stores;
using Tourist_Project.WPF.ViewModels;

namespace Tourist_Project.WPF.Views.GuestTwo
{
    /// <summary>
    /// Interaction logic for GuestTwoView.xaml
    /// </summary>
    public partial class GuestTwoView : Window
    {
        public GuestTwoView(User user)
        {
            InitializeComponent();
            var navigationStore = new NavigationStore();
            navigationStore.CurrentViewModel = new HomeViewModel(user, navigationStore);
            DataContext = new GuestTwoViewModel(user, navigationStore, this);
        }
    }
}
