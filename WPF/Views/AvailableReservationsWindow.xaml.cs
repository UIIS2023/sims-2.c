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
using Tourist_Project.WPF.ViewModels;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.Views
{
    /// <summary>
    /// Interaction logic for AvailableReservationsWindow.xaml
    /// </summary>
    public partial class AvailableReservationsWindow : Window
    {
        public AvailableReservationsWindow(Accommodation selectedAccommodation, DateTime from, DateTime to, int stayingDays, int guestsNum)
        {
            InitializeComponent();
            this.DataContext = new AvailableReservationsViewModel(selectedAccommodation,from, to, stayingDays, guestsNum);
        }
    }
}
