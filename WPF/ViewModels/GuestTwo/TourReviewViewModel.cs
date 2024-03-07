using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.DTO;
using Tourist_Project.Repositories;
using Tourist_Project.WPF.Commands;
using Tourist_Project.WPF.Stores;
using Tourist_Project.WPF.ViewModels.Guide;
using Tourist_Project.WPF.Views;

namespace Tourist_Project.WPF.ViewModels
{
    public class TourReviewViewModel : ViewModelBase
    {
        private readonly TourReviewService reviewService = new();
        private readonly ImageService imageService = new();
        private readonly TourAttendanceService attendanceService = new();
        private readonly TourPointRepository tourPointRepository = new();
        private int knowledgeRating;

        public int KnowledgeRating
        {
            get => knowledgeRating;
            set
            {
                knowledgeRating = value;
                OnPropertyChanged();
            }
        }

        private int languageRating;

        public int LanguageRating
        {
            get => languageRating;
            set
            {
                languageRating = value;
                OnPropertyChanged();
            }
        }

        private int entertainmentRating;

        public int EntertainmentRating
        {
            get => entertainmentRating;
            set
            {
                entertainmentRating = value;
                OnPropertyChanged();
            }
        }
        public string Comment { get; set; }

        private string imageURL;
        public string ImageURL
        {
            get => imageURL;
            set
            {
                if(imageURL != value)
                {
                    imageURL = value;
                    OnPropertyChanged(nameof(ImageURL));
                }
            }
        }
        public List<Image> Images { get; set; }

        public User LoggedInUser { get; set; }
        private readonly NavigationStore navigationStore;
        public TourDTO SelectedTour { get; set; }
        public string Checkpoints { get; set; }

        private Message message;
        public Message Message
        {
            get { return message; }
            set
            {
                if(message != value)
                {
                    message = value;
                    OnPropertyChanged(nameof(Message));
                }
            }
        }

        private Message undoMessage;
        public Message UndoMessage
        {
            get => undoMessage;
            set
            {
                undoMessage = value;
                OnPropertyChanged(nameof(UndoMessage));
            }
        }

        public ICommand RateCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand UndoReviewCommand { get; set; }
        public ICommand HelpCommand { get; }

        public TourReviewViewModel(User user, TourDTO tour, NavigationStore navigationStore, TourHistoryViewModel previousViewModel)
        {
            LoggedInUser = user;
            SelectedTour = tour;
            Checkpoints = tourPointRepository.GetAllForTourString(SelectedTour.Id);
            this.navigationStore = navigationStore;
            UndoMessage = new Message();

            Comment = string.Empty;
            ImageURL = string.Empty;
            Images = new List<Image>();
            Message = new Message();

            RateCommand = new RelayCommand(OnRateClick, CanRate);
            AddCommand = new RelayCommand(OnAddClick, CanAdd);
            UndoReviewCommand = new RelayCommand(OnUndoReviewClick, () => UndoMessage.Type);
            BackCommand = new NavigateCommand<TourHistoryViewModel>(this.navigationStore, () => previousViewModel);
            HelpCommand = new NavigateCommand<TourReviewHelpViewModel>(navigationStore, () => new TourReviewHelpViewModel(navigationStore, this));
        }

        private void OnUndoReviewClick()
        {
            reviewService.UndoLatestReview(LoggedInUser.Id, SelectedTour.Id);
            _ = ShowMessageAndHide(new Message(true, "Review undone!"));
            UndoMessage = new Message();
        }

        private bool CanAdd()
        {
            return ImageURL != string.Empty;
        }

        private void OnAddClick()
        {
            Image image = new(ImageURL);
            Images.Add(image);
            ImageURL = string.Empty;
        }

        private bool CanRate()
        {
            return KnowledgeRating != 0 && LanguageRating != 0 && EntertainmentRating != 0 && Comment != string.Empty && Comment.Length >= 15;
        }

        private async Task ShowMessageAndHide(Message message)
        {
            Message = message;
            if (message.Type)
            {
                UndoMessage = new Message(true, "Undo");
            }

            await Task.Delay(15000);
            Message = new Message();
            UndoMessage = new Message();
        }

        private void OnRateClick()
        {
            if(attendanceService.WasUserPresent(LoggedInUser.Id, SelectedTour.Id))
            {
                if(reviewService.DidUserReview(LoggedInUser.Id, SelectedTour.Id))
                {
                    _ = ShowMessageAndHide(new Message(false, "You already reviewed this tour"));
                }
                else
                {
                    TourReview tourReview = new(LoggedInUser.Id, SelectedTour.Id, KnowledgeRating, LanguageRating, EntertainmentRating, Comment);
                    reviewService.Save(tourReview);

                    foreach (var image in Images)
                    {
                        imageService.Save(image);
                    }

                    _ = ShowMessageAndHide(new Message(true, "Successfully rated the tour"));
                }
            }
            else
            {
                _ = ShowMessageAndHide(new Message(false, "You weren't present on this tour, so you can't review it!"));
            }            
        }
    }
}
