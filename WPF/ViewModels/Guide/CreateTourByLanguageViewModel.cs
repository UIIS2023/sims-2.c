using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels.Guide
{
    public class CreateTourByLanguageViewModel : INotifyPropertyChanged
    {
        #region Service
        private LocationService locationService = new();
        private TourService tourService = new();
        private TourPointService tourPointService = new();
        private ImageService imageService = new();
        #endregion

        #region ObservableCollection
        public static ObservableCollection<string> Countries { get; set; }
        public static ObservableCollection<string> Cities { get; set; }
        private ObservableCollection<string> checkpoints;
        public ObservableCollection<string> Checkpoints
        {
            get { return checkpoints; }
            set
            {
                checkpoints = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> images;

        public ObservableCollection<string> Images
        {
            get { return images; }
            set
            {
                images = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ObjectForAdd
        private Tour tourForAdd;
        public Tour TourForAdd

        {
            get { return tourForAdd; }
            set
            {
                tourForAdd = value;
                OnPropertyChanged("TourForAdd");
            }
        }
        private Image imageForAdd;
        public Image ImageForAdd
        {
            get { return imageForAdd; }
            set
            {
                imageForAdd = value;
                OnPropertyChanged("ImageForAdd");
            }
        }
        private TourPoint pointForAdd;
        public TourPoint PointForAdd
        {
            get { return pointForAdd; }
            set
            {
                pointForAdd = value;
                OnPropertyChanged("PointForAdd");
            }
        }
        private Location location;
        public Location Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged("PointForAdd");
            }
        }

        #endregion

        private int numberOfPoints = 0;
        private readonly Window window;
        public User LoggedInUser { get; set; }

        private string selectedLink;
        public string SelectedLink
        {
            get { return selectedLink; }
            set
            {
                selectedLink = value;
                OnPropertyChanged("SelectedLink");
            }
        }

        private string selectedCheckpoint;

        public string SelectedCheckpoint
        {
            get { return selectedCheckpoint; }
            set
            {
                selectedCheckpoint = value;
                OnPropertyChanged("SelectedCheckpoint");
            }
        }

        #region Command
        public ICommand CreateCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand AddImageCommand { get; set; }
        public ICommand AddCheckpointCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }
        public ICommand DeleteCheckpointCommand { get; set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CreateTourByLanguageViewModel(Window window, User loggedInUser, string language)
        {
            this.window = window;
            LoggedInUser = loggedInUser;

            Cities = new ObservableCollection<string>(locationService.GetAllCities());
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            Checkpoints = new ObservableCollection<string>();
            Images = new ObservableCollection<string>();

            ImageForAdd = new();
            PointForAdd = new();
            TourForAdd = new();
            TourForAdd.Language = language;
            Location = new();

            CreateCommand = new RelayCommand(Create, CanCreate);
            CancelCommand = new RelayCommand(Cancel);
            AddImageCommand = new RelayCommand(AddImage);
            AddCheckpointCommand = new RelayCommand(AddCheckpoint);
            DeleteImageCommand = new RelayCommand(DeleteImage, CanDelete);
            DeleteCheckpointCommand = new RelayCommand(DeleteCheckpoint, CanDeleteCheckpoint);
        }

        private bool CanDeleteCheckpoint()
        {
            return SelectedCheckpoint != null;
        }

        private void DeleteCheckpoint()
        {
            Checkpoints.Remove(SelectedCheckpoint);
        }

        private bool CanDelete()
        {
            return SelectedLink != null;
        }

        private void DeleteImage()
        {
            Images.Remove(SelectedLink);
        }

        private void Cancel()
        {
            window.Close();
        }

        private bool CanCreate()
        {
            return numberOfPoints >= 2 && TourForAdd.IsValid;
        }

        private void Create()
        {
            TourForAdd.LocationId = locationService.GetId(Location.City, Location.Country);
            TourForAdd.UserId = LoggedInUser.Id;
            tourService.Save(TourForAdd);

            if (TourForAdd.StartTime.Date == DateTime.Today.Date)
            {
                TodayToursViewModel.TodayTours.Add(TourForAdd);
            }

            window.Close();
        }

        private void AddImage()
        {
            Images.Add(ImageForAdd.Url);
            imageService.Save(ImageForAdd);
            ImageForAdd = new Image();
        }

        private void AddCheckpoint()
        {
            Checkpoints.Add(PointForAdd.Name);
            PointForAdd.TourId = tourService.NexttId();
            tourPointService.Save(PointForAdd);
            PointForAdd = new TourPoint();
            numberOfPoints++;
        }
    }
}
