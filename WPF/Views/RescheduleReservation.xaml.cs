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
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views
{
    /// <summary>
    /// Interaction logic for RescheduleReservation.xaml
    /// </summary>
    public partial class RescheduleReservation : Window
    {
        public RescheduleReservation()
        {
            InitializeComponent();
            this.DataContext = new RescheduleReservationViewModel(this);
        }
    }
}
