using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum UserRole { owner, guide, guest1, guest2 }
    public class User : ISerializable, INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (value == id) return;
                id = value;
                OnPropertyChanged("Id");
            }
        }

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

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if(value == password) return;
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private UserRole role;
        public UserRole Role
        {
            get => role;
            set
            {
                if(value == role) return;
                role = value;
                OnPropertyChanged("Role");
            }
        }

        private string fullname;
        public string FullName
        {
            get => fullname;
            set
            {
                if(value == fullname) return;
                fullname = value;
                OnPropertyChanged("FullName");
            }
        }

        public DateTime birthDate;
        public DateTime BirthDate
        {
            get => birthDate;
            set
            {
                if(value == birthDate) return;
                birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }

        private bool isSuper;
        public bool IsSuper
        {
            get => isSuper;
            set
            {
                if(value == isSuper) return;
                isSuper = value;
                OnPropertyChanged("IsSuper");
            }
        }

        private bool isEmployed;

        public bool IsEmployed
        {
            get => isEmployed;
            set
            {
                if(value ==  isEmployed) return;
                isEmployed = value;
                OnPropertyChanged("Employed");
            }
        }

        private bool acquiredYearlyVoucher;

        public bool AcquiredYearlyVoucher
        {
            get => acquiredYearlyVoucher;
            set
            {
                if(value ==  acquiredYearlyVoucher) return;
                acquiredYearlyVoucher = value;
                OnPropertyChanged(nameof(AcquiredYearlyVoucher));
            }
        }

        private DateTime voucherAcquiredDate;

        public DateTime VoucherAcquiredDate
        {
            get => voucherAcquiredDate;
            set
            {
                if(value ==  voucherAcquiredDate) return;
                voucherAcquiredDate = value;
                OnPropertyChanged(nameof(VoucherAcquiredDate));
            }
        }

        public User() { }
        public User(int id, string username, string password, UserRole role, string fullName, DateTime birthDate, bool isSuper, bool isEmployed, bool acquiredYearlyVoucher, DateTime voucherAcquiredDate)
        {
            Id = id;
            Username = username;
            Password = password;
            Role = role;
            FullName = fullName;
            BirthDate = birthDate;
            IsSuper = isSuper;
            IsEmployed = isEmployed;
            AcquiredYearlyVoucher = acquiredYearlyVoucher;
            VoucherAcquiredDate = voucherAcquiredDate;
        }
        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Role.ToString(), FullName, BirthDate.ToString(), IsSuper.ToString(), IsEmployed.ToString(), AcquiredYearlyVoucher.ToString(), VoucherAcquiredDate.ToString() };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Role = Enum.Parse<UserRole>(values[3]);
            FullName = values[4];
            BirthDate = DateTime.Parse(values[5]);
            IsSuper = bool.Parse(values[6]);
            IsEmployed = bool.Parse(values[7]);
            AcquiredYearlyVoucher = bool.Parse(values[8]);
            VoucherAcquiredDate = DateTime.Parse(values[9]);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}