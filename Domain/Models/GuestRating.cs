using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public class GuestRating : ISerializable, INotifyPropertyChanged
    {
        private int id;
        public int Id
        {
            get => id;
            set
            {
                if(value == id) return;
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private int reservationId;
        public int ReservationId
        {
            get => reservationId;
            set
            {
                if(value == reservationId) return;
                reservationId = value;
                OnPropertyChanged("ReservationId");
            }
        }

        private int cleanlinessGrade;
        public int CleanlinessGrade
        {
            get => cleanlinessGrade;
            set
            {
                if(value == cleanlinessGrade) return;
                cleanlinessGrade = value;
                OnPropertyChanged("CleanlinessGrade");
            }
        }

        private int ruleCompliance;
        public int RuleCompliance
        {
            get => ruleCompliance;
            set
            {
                if(value == ruleCompliance) return;
                ruleCompliance = value;
                OnPropertyChanged("RuleCompliance");
            }
        }

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                if(value == comment) return;
                comment = value;
                OnPropertyChanged("comment");
            }
        }
        public GuestRating() { }
        public GuestRating(int reservationId, int cleanlinessGrade, int ruleCompliance, string comment)
        {
            ReservationId = reservationId;
            CleanlinessGrade = cleanlinessGrade;
            RuleCompliance = ruleCompliance;
            Comment = comment;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(), 
                ReservationId.ToString(), 
                CleanlinessGrade.ToString(), 
                RuleCompliance.ToString(), 
                Comment
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            CleanlinessGrade = Convert.ToInt32(values[2]);
            RuleCompliance = Convert.ToInt32(values[3]);
            Comment = values[4];
        }
        //TODO
        public bool IsReviewed()
        {
            return CleanlinessGrade != 0 && RuleCompliance != 0 && !Comment.Equals("");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}