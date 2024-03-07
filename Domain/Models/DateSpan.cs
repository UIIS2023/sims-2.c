using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tourist_Project.Domain.Models
{
    public class DateSpan : INotifyPropertyChanged
    {
        private DateTime startingDate;

        public DateTime StartingDate
        {
            get => startingDate;
            set
            {
                if (startingDate == value) return;
                startingDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime endingDate;

        public DateTime EndingDate
        {
            get => endingDate;
            set
            {
                if (endingDate == value) return;
                endingDate = value;
                OnPropertyChanged();
            }
        }

        public DateSpan()
        {
        }

        public DateSpan(DateTime startingDate, DateTime endingDate)
        {
            StartingDate = startingDate;
            EndingDate = endingDate;
        }

/*
        public string[] ToCSV()
        {
            string[] csvValues = {
                StartingDate.ToString("MM/dd/yyyy"),
                EndingDate.ToString("MM/dd/yyyy")
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            StartingDate = DateTime.Parse(values[1]);
            EndingDate = DateTime.Parse(values[2]);
        }*/
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}