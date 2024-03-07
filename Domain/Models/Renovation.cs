using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Domain.Models;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain
{
    public class Renovation : ISerializable, INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if(value == id) return;
                id = value;
                OnPropertyChanged();
            }
        }
        private int accommodationId;
        public int AccommodationId
        {
            get => accommodationId;
            set
            {
                if (value == accommodationId) return;
                accommodationId = value;
                OnPropertyChanged();
            }
        }

        private DateSpan renovatingSpan;

        public DateSpan RenovatingSpan
        {
            get => renovatingSpan;
            set
            {
                if(value == renovatingSpan) return;
                renovatingSpan = value;
                OnPropertyChanged();
            }
        }

        private string description;

        public string Description
        {
            get => description;
            set
            {
                if(value == description) return;
                description = value;
                OnPropertyChanged();
            }
        }

        public Renovation()
        {
            RenovatingSpan = new DateSpan();
        }

        public Renovation(int accommodationId, DateSpan renovatingSpan, string description)
        {
            AccommodationId = accommodationId;
            RenovatingSpan = renovatingSpan;
            Description = description;
        }

        

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                RenovatingSpan.StartingDate.ToString("MM/dd/yyyy"),
                RenovatingSpan.EndingDate.ToString("MM/dd/yyyy"),
                Description
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            AccommodationId = Convert.ToInt32(values[1]);
            RenovatingSpan.StartingDate = DateTime.Parse(values[2]);
            RenovatingSpan.EndingDate = DateTime.Parse(values[3]);
            Description = values[4];
        }
    }

}