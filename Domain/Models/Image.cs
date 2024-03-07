using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum ImageAssociation
    {
        Accommodation, Tour, TourReview
    }
    public class Image : ISerializable, INotifyPropertyChanged, IDataErrorInfo
    {
        public int Id { get; set; }
        public ImageAssociation Association { get; set; }
        public int AssociationId { get; set; }
        public string Url { get; set; }

        public Image() { }
        public Image(string url)
        {
            Url = url;
        }
        public Image(string url, ImageAssociation association, int associationId)
        {
            Url = url;
            Association = association;
            AssociationId = associationId;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Association.ToString(),
                AssociationId.ToString(),
                Url
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Association = Enum.Parse<ImageAssociation>(values[1]);
            AssociationId = int.Parse(values[2]);
            Url = values[3];
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Url" when string.IsNullOrEmpty(Url):
                        return "Url is required";
                    default:
                        return null;
                }
            }
        }

        private readonly string[] validatedProperties = { "Url" };

        public bool IsValid
        {
            get
            {
                return validatedProperties.All(property => this[property] == null);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}