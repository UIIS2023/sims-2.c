using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum Author
    {
        Owner,
        Guest
    }
    public class Comment : INotifyPropertyChanged, ISerializable
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

        private string commentText;

        public string CommentText
        {
            get => commentText;
            set
            {
                if(value == commentText) return;
                commentText = value;
                OnPropertyChanged();
            }
        }

        private int reportNo;
        public int ReportNo
        {
            get => reportNo;
            set
            {
                if(reportNo == value) return;
                reportNo = value;
                OnPropertyChanged();
            }
        }

        private Author author;
        public Author Author
        {
            get => author;
            set
            {
                if(value == author) return;
                author = value;
                OnPropertyChanged();
            }
        }

        private int authorId;

        public int AuthorId
        {
            get => authorId;
            set
            {
                if (value == authorId) return;
                authorId = value;
                OnPropertyChanged();
            }
        }
        public Comment() { }

        public Comment(string commentText, int reportNo, Author author, int authorId)
        {
            CommentText = commentText;
            ReportNo = reportNo;
            Author = author;
            AuthorId = authorId;
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
                CommentText,
                ReportNo.ToString(),
                AuthorId.ToString(),
                Author.ToString()
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            CommentText = values[1];
            ReportNo = Convert.ToInt32(values[2]);
            AuthorId = Convert.ToInt32(values[3]);
            Author = Enum.Parse<Author>(values[4]);
        }
    }

}