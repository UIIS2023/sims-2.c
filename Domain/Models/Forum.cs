using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public class Forum : INotifyPropertyChanged, ISerializable
    {
        private int id;

        public int Id
        {
            get => id;
            set
            {
                if (value == id) return;
                id = value;
                OnPropertyChanged();
            }
        }

        private int locationId;
        public int LocationId
        {
            get => locationId;
            set
            {
                if (value == locationId) return;
                locationId = value;
                OnPropertyChanged();
            }
        }

        private string commentsIdsCsv;

        public string CommentsIdsCsv
        {
            get => commentsIdsCsv;
            set
            {
                if(value == commentsIdsCsv) return;
                commentsIdsCsv = value;
                OnPropertyChanged();
            }
        }

        private bool isClosed;
        public bool IsClosed
        {
            get => isClosed;
            set
            {
                if(value == isClosed) return;
                isClosed = value;
                OnPropertyChanged();
            }
        }

        private int userId;
        public int UserId
        {
            get => userId;
            set
            {
                if(userId == value) return;
                userId = value;
                OnPropertyChanged();
            }
        }

        public List<int> CommentsIds { get; set; } = new();

        public Forum() { }

        public Forum(int locationId, string commentsIdsCsv, bool isClosed, int userId)
        {
            LocationId = locationId;
            CommentsIdsCsv = commentsIdsCsv;
            IsClosed = isClosed;
            UserId = userId;
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] ToCSV()
        {
            CommentIdesToCsv();
            string[] csvValues =
            {
                Id.ToString(),
                LocationId.ToString(),
                IsClosed.ToString(),
                UserId.ToString(),
                CommentsIdsCsv
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            LocationId = Convert.ToInt32(values[1]);
            IsClosed = Convert.ToBoolean(values[2]);
            UserId = Convert.ToInt32(values[3]);
            CommentsIdsCsv = values[4];
            CommentsIdsFromCsv(CommentsIdsCsv);
        }

        public void CommentIdesToCsv()
        {
            if (CommentsIds.Count <= 0) return;
            CommentsIdsCsv = string.Empty;
            foreach (var commentIde in CommentsIds)
            {
                CommentsIdsCsv += commentIde + ",";
            }
            CommentsIdsCsv = CommentsIdsCsv.Remove(CommentsIdsCsv.Length - 1);
        }

        public void CommentsIdsFromCsv(string value)
        {
            var commentIdesCsv = value.Split(",");
            foreach (var commentIde in commentIdesCsv)
            {
                if (commentIde != string.Empty)
                    CommentsIds.Add(int.Parse(commentIde));
            }
        }
    }

}