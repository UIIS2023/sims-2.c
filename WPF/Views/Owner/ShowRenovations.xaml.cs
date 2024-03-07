using System.Windows;
using Tourist_Project.WPF.ViewModels;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for ShowRenovations.xaml
    /// </summary>
    public partial class ShowRenovations : Window, IBindableBase
    {
        public ShowRenovations()
        {
            InitializeComponent();
            DataContext = new ShowRenovationsViewModel(this);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
