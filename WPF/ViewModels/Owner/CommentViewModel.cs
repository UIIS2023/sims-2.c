using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views.Owner;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class CommentViewModel : INotifyPropertyChanged
    {
        private Comment comment;

        public Comment Comment
        {
            get => comment;
            set
            {
                if(comment == value) return;
                comment = value;
                OnPropertyChanged();
            }
        }

        private User user;
        public User User
        {
            get => user;
            set
            {
                if(user == value) return;
                user = value;
                OnPropertyChanged();
            }
        }

        public bool HasAccommodations { get; set; }
        public int LocationId { get; set; }
        public Forum Forum { get; set; }

        private readonly UserService userService = new();
        private readonly ReservationService reservationService = new();
        private readonly CommentService commentService = new();

        public ICommand ReplyCommand { get; set; } 
        public ICommand ReportCommand { get; set; } 

        public CommentViewModel() { }

        public CommentViewModel(Comment comment, bool hasAccommodations, Forum forum)
        {
            Comment = comment;
            User = userService.GetOne(Comment.AuthorId);
            ReplyCommand = new RelayCommand(Reply, CanReply);
            ReportCommand = new RelayCommand(Report, CanReport);
            HasAccommodations = hasAccommodations;
            Forum = forum;
            LocationId = forum.LocationId;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Reply()
        {
            var replyOnForum = new ReplyOnForum(Forum, Comment);
            replyOnForum.ShowDialog();
        }

        public void Report()
        {
            Comment.ReportNo++;
            commentService.Update(Comment);
        }

        public bool CanReply()
        {
            return HasAccommodations && !Forum.IsClosed;
        }

        public bool CanReport()
        {
            return Comment.Author != Author.Owner && reservationService.WasOnLocation(Comment.AuthorId, LocationId) && !Forum.IsClosed;
        }
    }

}