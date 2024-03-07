using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Tourist_Project.Domain.Models;
using Tourist_Project.Repositories;
using Tourist_Project.WPF.Views.GuestTwo;
using Tourist_Project.WPF.Views.Guide;
using Tourist_Project.WPF.Views.Owner;

namespace Tourist_Project.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, INotifyPropertyChanged
    {
        private readonly UserRepository repository = new();
        private string username;

        public string Username
        {
            get => username;
            set
            {
                if(value == username) return;
                username = value;
                OnPropertyChanged("Username");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SignInCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = this;
            SignInCommand = new RelayCommand(SignIn, CanSignIn);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void SignIn()
        {
            var user = repository.GetByUsername(Username);
            if (user != null)
            {
                if (user.Password == txtPassword.Password)
                {
                    Tourist_Project.App.LoggedInUser = user;
                    switch (user.Role)
                    {
                        case UserRole.owner:
                        {
                            var ownerShowWindow = new OwnerMainWindow(user);
                            ownerShowWindow.Show();
                            Close();
                            break;
                        }
                        case UserRole.guest1:
                        {
                            var guestOne = new GuestOne(user);
                            guestOne.Show();
                            Close();
                            break;
                        }
                        case UserRole.guest2:
                        {
                            var guestTwoWindow = new GuestTwoView(user);
                            guestTwoWindow.Show();
                            Close();
                            break;
                        }
                        case UserRole.guide:
                        {
                            var guideShowWindow = new TodayToursView(user);
                            guideShowWindow.Show();
                            Close();
                            break;
                        }
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }

        }

        public bool CanSignIn()
        {
            return !string.IsNullOrWhiteSpace(Username);
        }

        public void Cancel()
        {
            Close();
        }
    }
}
