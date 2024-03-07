using System.Windows.Input;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.WPF.ViewModels.Owner
{
    public class ReplyOnForumViewModel
    {
        public Forum Forum { get; set; }
        public Comment Comment { get; set; }
        public User User { get; set; }
        public Comment NewComment { get; set; }

        private readonly ForumService forumService = new();
        private readonly CommentService commentService = new();
        private readonly UserService userService = new();

        public ICommand ReplyCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        private readonly IBindableBase bindableBase;

        public ReplyOnForumViewModel()
        {
        }
        public ReplyOnForumViewModel(Forum forum, Comment comment, IBindableBase bindableBase)
        {
            NewComment = new Comment();
            Forum = forum;
            Comment = comment;
            User = userService.GetOne(Comment.AuthorId);
            ReplyCommand = new RelayCommand(Reply);
            ExitCommand = new RelayCommand(Exit);
            this.bindableBase = bindableBase;
        }

        public void Reply()
        {
            NewComment.Author = Author.Owner;
            NewComment.AuthorId = App.LoggedInUser.Id;
            commentService.Create(NewComment);
            Forum.CommentsIds.Add(commentService.Create(NewComment).Id);
            forumService.Update(Forum);
            bindableBase.CloseWindow();
        }

        public void Exit()
        {
            bindableBase.CloseWindow();
        }
    }

}