using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourist_Project.Domain.Models;
using Tourist_Project.DTO;
using Tourist_Project.Repositories;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;

namespace Tourist_Project.WPF.ViewModels
{
    public class MyTourPreviewViewModel : ViewModelBase
    {
        private readonly TourPointRepository tourPointRepository = new();
        private readonly ImageRepository imageRepository = new();
        public TourDTO SelectedTour { get; set; }
        public string Checkpoints { get; set; }
        public List<Image> TourImages { get; set; }
        private int imageId;
        private readonly int imagesCount;
        private Image currentImage;
        public Image CurrentImage
        {
            get => currentImage;
            set
            {
                if (value != currentImage)
                {
                    currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }
        public ICommand BackCommand {  get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand PreviousCommand { get; set; }
        public ICommand HelpCommand { get; set; }


        public MyTourPreviewViewModel(TourDTO tour, NavigationStore navigationStore, MyToursViewModel previousViewModel)
        {
            SelectedTour = tour;
            Checkpoints = tourPointRepository.GetAllForTourString(tour.Id);

            TourImages = imageRepository.GetByAssociationAndId(ImageAssociation.Tour, tour.Id);
            imagesCount = TourImages.Count;
            if (imagesCount > 0)
            {
                CurrentImage = TourImages[0];
                imageId = 0;
            }
            else
            {
                CurrentImage = new Image("/Images/No images to show.jpg");
            }

            BackCommand = new NavigateCommand<MyToursViewModel>(navigationStore, () => previousViewModel);
            NextCommand = new RelayCommand(OnNextClick, () => imagesCount > 0);
            PreviousCommand = new RelayCommand(OnPreviousClick, () => imagesCount > 0);
            HelpCommand = new NavigateCommand<MyToursHelpViewModel>(navigationStore, () => new MyToursHelpViewModel(navigationStore, this));
        }
        public MyTourPreviewViewModel(TourDTO tour, NavigationStore navigationStore, NotificationsViewModel previousViewModel)
        {
            SelectedTour = tour;
            Checkpoints = tourPointRepository.GetAllForTourString(tour.Id);

            TourImages = imageRepository.GetByAssociationAndId(ImageAssociation.Tour, tour.Id);
            imagesCount = TourImages.Count;
            if (imagesCount > 0)
            {
                CurrentImage = TourImages[0];
                imageId = 0;
            }
            else
            {
                CurrentImage = new Image("/Images/No images to show.jpg");
            }

            BackCommand = new NavigateCommand<NotificationsViewModel>(navigationStore, () => previousViewModel);
            NextCommand = new RelayCommand(OnNextClick, () => imagesCount > 0);
            PreviousCommand = new RelayCommand(OnPreviousClick, () => imagesCount > 0);
            HelpCommand = new NavigateCommand<MyToursHelpViewModel>(navigationStore, () => new MyToursHelpViewModel(navigationStore, this));
        }


        private void OnPreviousClick()
        {
            imageId = imageId - 1 < 0 ? imagesCount - 1 : imageId - 1;
            CurrentImage = TourImages[imageId];
        }

        private void OnNextClick()
        {
            imageId = imageId + 1 == imagesCount ? 0 : imageId + 1;
            CurrentImage = TourImages[imageId];
        }
    }
}
