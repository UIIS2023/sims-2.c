using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Tourist_Project.Applications;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Repositories;
using Tourist_Project.WPF.Views;

namespace Tourist_Project.WPF.ViewModels.Guide
{
    public class CreateTourViewModel : INotifyPropertyChanged
    {
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

        #region Service
        private LocationService locationService = new();
        private TourService tourService = new();
        private TourPointService tourPointService = new();
        private ImageService imageService = new();
        #endregion
        private int numberOfPoints = 0;
        private readonly Window window;
        private int imageId;

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
                OnPropertyChanged("Location");
            }
        }

        #endregion

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

        public CreateTourViewModel(Window window, User loggedInUser)
        {
            this.window = window;
            LoggedInUser = loggedInUser;

            Cities = new ObservableCollection<string>(locationService.GetAllCities());
            Countries = new ObservableCollection<string>(locationService.GetAllCountries());
            Checkpoints = new ObservableCollection<string>();
            Images = new ObservableCollection<string>();

            ImageForAdd = new Image();
            PointForAdd = new TourPoint();
            TourForAdd = new Tour();
            Location = new Location();

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
            TourForAdd.ImageId = imageId;
            tourService.Save(TourForAdd);

            if (TourForAdd.StartTime.Date == DateTime.Today.Date)
            {
                TodayToursViewModel.TodayTours.Add(TourForAdd);
            }

            window.Close();
        }

        private void AddImage()
        {
            ImageForAdd.Association = ImageAssociation.Tour;
            Images.Add(ImageForAdd.Url);
            imageService.Save(ImageForAdd);
            imageId = ImageForAdd.Id;
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

        private void CountryDropDownClosed()
        {
            Cities.Clear();
            foreach (var location in locationService.GetAll())
            {
                if (location.Country.Equals(Location.Country))
                    Cities.Add(location.City);
            }
        }
        private void CityDropDownClosed()
        {

            foreach (var location in locationService.GetAll())
            {
                if (location.City.Equals(Location.City))
                    Location.Country = location.Country;
            }
        }
    }
}