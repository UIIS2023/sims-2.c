using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.ViewModels.Owner;

public class ForumViewModel : INotifyPropertyChanged
{
    private Forum forum;
    public Forum Forum
    {
        get => forum;
        set
        {
            if (value == forum) return;
            forum = value;
            OnPropertyChanged();
        }
    }
    private User user;
    public User User
    {
        get => user;
        set
        {
            if(value == user) return;
            user = value;
            OnPropertyChanged();
        }
    }

    private Location location;
    public Location Location
    {
        get => location;
        set
        {
            if (value == location) return;
            location = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Accommodation> accommodations;
    public ObservableCollection<Accommodation> Accommodations
    {
        get => accommodations;
        set
        {
            if (value == accommodations) return;
            accommodations = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<CommentViewModel> comments;

    public ObservableCollection<CommentViewModel> Comments
    {
        get => comments;
        set
        {
            if(comments == value) return;
            comments = value;
            OnPropertyChanged();
        }
    }

    public CommentViewModel SelectedComment { get; set; }

    private readonly AccommodationService accommodationService = new();
    private readonly LocationService locationService = new();
    private readonly ForumService forumService = new();
    private readonly UserService userService = new();
    private readonly CommentService commentService = new();

    public ForumViewModel(int forumId)
    {
        Forum = forumService.Get(forumId);
        User = userService.GetOne(Forum.UserId);
        Location = locationService.Get(Forum.LocationId);
        Accommodations = new ObservableCollection<Accommodation>(accommodationService.GetAll().Where(accommodation => accommodation.LocationId == Forum.LocationId));
        Comments = new ObservableCollection<CommentViewModel>(commentService.GetAll().Where(comment => Forum.CommentsIds.Contains(comment.Id)).Select(comment => new CommentViewModel(comment, accommodations.Count != 0, Forum)));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
