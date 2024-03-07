using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class ScheduleRenovationViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private ObservableCollection<AccommodationViewModel> accommodationViewModel;

        public ObservableCollection<AccommodationViewModel> AccommodationViewModel
        {
            get => accommodationViewModel;
            set
            {
                if(value == accommodationViewModel) return;
                accommodationViewModel = value;
                OnPropertyChanged();
            }
        }

        private int renovationLength;

        public int RenovationLength
        {
            get => renovationLength;
            set
            {
                if(value == renovationLength) return;
                renovationLength = value;
                OnPropertyChanged();
            }
        }

        private DateSpan requestedDateSpan;
        public DateSpan RequestedDateSpan
        {
            get => requestedDateSpan;
            set
            {
                if(value == requestedDateSpan) return;
                requestedDateSpan = value;
                OnPropertyChanged();
            }
        }

        private string description;

        public string Description
        {
            get => description;
            set
            {
                if (value == description) return;
                description = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DateSpan> possibleDateSpans;

        public ObservableCollection<DateSpan> PossibleDateSpans
        {
            get => possibleDateSpans;
            set
            {
                if(value == possibleDateSpans) return;
                possibleDateSpans = value;
                OnPropertyChanged();
            }
        }

        #region DemoFields

        private DatePicker startingDate;
        private DatePicker endingDate;
        private TextBox length;
        private TextBox descriptionTextBox;
        private DataGrid timeSpans;
        private Button findButton;

        #endregion

        public DateSpan SelectedDateSpan { get; set; }

        private readonly RenovationService renovationService = new ();

        private readonly IBindableBase bindableBase;

        public ICommand FindCommand { get; set; }
        public ICommand RenovateCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand DemoCommand { get; set; }
        public ScheduleRenovationViewModel() {}

        public ScheduleRenovationViewModel(AccommodationViewModel accommodationViewModel, IBindableBase bindableBase)
        {
            this.bindableBase = bindableBase;
            AccommodationViewModel = new ObservableCollection<AccommodationViewModel> { accommodationViewModel };
            FindCommand = new RelayCommand(Find, CanFind);
            RenovateCommand = new RelayCommand(Renovate, CanRenovate);
            CloseCommand = new RelayCommand(Close);
            DemoCommand = new RelayCommand(async() => await Demo());
            RequestedDateSpan = new DateSpan(DateTime.Now, DateTime.Now);
            PossibleDateSpans = new ObservableCollection<DateSpan>();
        }

        #region CommandLogic

        public void Find()
        {
            foreach (var dateSpan in renovationService.FindDateSpans(RequestedDateSpan, Convert.ToInt32(renovationLength), AccommodationViewModel.First().Accommodation.Id))
            {
                possibleDateSpans.Add(dateSpan);
            }
        }

        public bool CanFind()
        {
            return int.TryParse(RenovationLength.ToString(), out _) && RequestedDateSpan.EndingDate > RequestedDateSpan.StartingDate && Convert.ToInt32(RenovationLength) < (RequestedDateSpan.EndingDate - RequestedDateSpan.StartingDate).Days && RequestedDateSpan.StartingDate >= DateTime.Now && RequestedDateSpan.EndingDate > DateTime.Now;
        }

        public void Renovate()
        {
            SelectedDateSpan ??= possibleDateSpans.FirstOrDefault();
            renovationService.Create(new Renovation(AccommodationViewModel.First().Accommodation.Id, SelectedDateSpan, Description));
            bindableBase.CloseWindow();
        }

        public bool CanRenovate()
        {
            return SelectedDateSpan != null && IsValid;
        }

        public void Close()
        {
            bindableBase.CloseWindow();
        }
        
        #endregion


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #region Validation

        public string Error { get; }

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Renovation Length":
                        {
                            if (string.IsNullOrWhiteSpace(renovationLength.ToString()))
                                return "Length is required!";
                            if (renovationLength <= 0)
                                return "Length must be positive value!";
                            break;
                        }
                    case "Description":
                        {
                            if (string.IsNullOrWhiteSpace(description))
                                return "Description is required!";
                            break;
                        }
                    case "Ending date":
                        {
                            if (RequestedDateSpan.EndingDate < DateTime.Now)
                                return "Ending date must be in future!";
                            if (RequestedDateSpan.EndingDate < RequestedDateSpan.StartingDate)
                                return "Ending date must be after starting date!";
                            break;
                        }
                    case "Starting date":
                        {
                            if (RequestedDateSpan.StartingDate < DateTime.Now)
                                return "Starting date must be in future!";
                            break;
                        }
                    default:
                        return null;
                }

                return null;
            }
        }

        private readonly string[] validatedProperties = { "Renovation Length", "Ending date", "Starting date", "Description" };

        public bool IsValid
        {
            get
            {
                return validatedProperties.All(property => this[property] == null);
            }
        }

        #endregion

        public void SetControls(DatePicker startingDate, DatePicker endingDate, TextBox length,
            TextBox descriptionTextBox, DataGrid timeSpans, Button findButton)
        {
            this.startingDate = startingDate;
            this.endingDate = endingDate;
            this.length = length;
            this.descriptionTextBox = descriptionTextBox;
            this.timeSpans = timeSpans;
            this.findButton = findButton;
        }

        public async Task Demo()
        {
            startingDate.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            startingDate.SelectedDate = DateTime.Now.AddDays(1);

            endingDate.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            endingDate.SelectedDate = DateTime.Now.AddMonths(2);

            length.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            length.Text = "5";

            const string demoName = "Demo accommodation";
            StringBuilder nameBuilder = new();
            descriptionTextBox.Focus();
            foreach (var t in demoName)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.5));
                descriptionTextBox.Text = nameBuilder.Append(t).ToString();
            }

            findButton.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            Find();

            timeSpans.Focus();
            await Task.Delay(TimeSpan.FromSeconds(1));
            timeSpans.SelectedItem = PossibleDateSpans.First();
        }
    }
}
