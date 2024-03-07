using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels
{
    public class WizardViewModel
    {
        public ICommand SaveWizardSettingsCommand => new RelayCommand(SaveWizardSettings);

        private void SaveWizardSettings()
        {
            Properties.Settings.Default.ShowWizard = false;
            Properties.Settings.Default.Save();
        }
    }
}
