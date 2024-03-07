using System.Windows;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.ViewModels.Owner;

namespace Tourist_Project.WPF.Views.Owner
{
    /// <summary>
    /// Interaction logic for ReplyOnForum.xaml
    /// </summary>
    public partial class ReplyOnForum : Window, IBindableBase
    {
        public ReplyOnForum(Forum forum, Comment comment)
        {
            InitializeComponent();
            DataContext = new ReplyOnForumViewModel(forum, comment, this);
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}
