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
using Tourist_Project.WPF.ViewModels;
using Tourist_Project.WPF.ViewModels.Guide;

namespace Tourist_Project.WPF.Views.Guide
{
    /// <summary>
    /// Interaction logic for CreateTourByLanguageView.xaml
    /// </summary>
    public partial class CreateTourByLanguageView : Window
    {
        public CreateTourByLanguageView(User user, string language)
        {
            InitializeComponent();
            DataContext = new CreateTourByLanguageViewModel(this, user, language);
        }
    }
}
