using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum RequestStatus { Pending, Confirmed, Declined}
    public class RescheduleRequest : ISerializable, INotifyPropertyChanged
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

        private DateTime oldBeginningDate;
        public DateTime OldBeginningDate
        {
            get => oldBeginningDate;
            set
            {
                if(value == oldBeginningDate) return;
                oldBeginningDate = value;
                OnPropertyChanged("OldBeginningDate");
            }
        }

        private DateTime oldEndDate;
        public DateTime OldEndDate
        {
            get => oldEndDate;
            set
            {
                if(value == oldEndDate) return;
                oldEndDate = value;
                OnPropertyChanged("OldEndDate");
            }
        }

        private DateTime newEndDate;
        public DateTime NewEndDate
        {
            get => newEndDate;
            set
            {
                if(value == newEndDate) return;
                newEndDate = value;
                OnPropertyChanged("NewEndDate");
            }
        }

        private DateTime newBeginningDate;
        public DateTime NewBeginningDate
        {
            get => newBeginningDate;
            set
            {
                if(value == newBeginningDate) return;
                newBeginningDate = value;
                OnPropertyChanged("NewBeginningDate");
            }
        }
        private int reservationId;
        public int ReservationId
        {
            get => reservationId;
            set
            {
                if (value == reservationId) return;
                reservationId = value;
                OnPropertyChanged("ReservationId");
            }
        }

        private RequestStatus status;
        public RequestStatus Status
        {
            get => status;
            set
            {
                if(value == status) return;
                status = value;
                OnPropertyChanged();
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
                OnPropertyChanged("Comment");
            }
        }

        public RescheduleRequest() { }
        public RescheduleRequest(DateTime oldBeginningDate, DateTime oldEndDate, DateTime newBeginningDate,DateTime newEndDate, int id, int reservationId, RequestStatus status, string comment)
        {
            Id = id;
            OldBeginningDate = oldBeginningDate;
            OldEndDate = oldEndDate;
            NewBeginningDate = newBeginningDate;
            NewEndDate = newEndDate;  
            ReservationId = reservationId;
            Status = status;
            Comment = comment;
        }
        public string[] ToCSV()
        {
            string[] csvValues = {
                Id.ToString(),
                OldBeginningDate.ToString(),
                OldEndDate.ToString(),
                NewBeginningDate.ToString(),
                NewEndDate.ToString(),
                ReservationId.ToString(),
                Status.ToString(),
                Comment
            };
            return csvValues;
        }

        public void FromCSV(string []values)
        {
            Id = Convert.ToInt32(values[0]);
            OldBeginningDate = Convert.ToDateTime(values[1]);
            OldEndDate = Convert.ToDateTime(values[2]);
            NewBeginningDate = Convert.ToDateTime(values[3]);
            NewEndDate = Convert.ToDateTime(values[4]);
            ReservationId = Convert.ToInt32(values[5]);
            Status = Enum.Parse<RequestStatus>(values[6]);
            Comment = values[7];
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
