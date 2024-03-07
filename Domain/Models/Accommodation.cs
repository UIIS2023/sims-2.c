using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum AccommodationType { Apartment, House, Cottage }
    public class Accommodation : ISerializable, INotifyPropertyChanged, IDataErrorInfo
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
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (value == name) return;
                name = value;
                OnPropertyChanged("Name");
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
                OnPropertyChanged("LocationId");
            }
        }

        private AccommodationType type;
        public AccommodationType Type
        {
            get => type;
            set
            {
                if (value == type) return;
                type = value;
                OnPropertyChanged("Type");
            }
        }

        private int maxGuestNum;
        public int MaxGuestNum
        {
            get => maxGuestNum;
            set
            {
                if (value == maxGuestNum) return;
                maxGuestNum = value;
                OnPropertyChanged("MaxGuestNum");
            }
        }

        private int minStayingDays;
        public int MinStayingDays
        {
            get => minStayingDays;
            set
            {
                if (value == minStayingDays) return;
                minStayingDays = value;
                OnPropertyChanged("MinStayingDays");
            }
        }

        private int cancellationThreshold;
        public int CancellationThreshold
        {
            get => cancellationThreshold;
            set
            {
                if (value == cancellationThreshold) return;
                cancellationThreshold = value;
                OnPropertyChanged("CancellationThreshold");
            }
        }

        private int imageId;
        public int ImageId
        {
            get => imageId;
            set
            {
                if (value == imageId) return;
                imageId = value;
                OnPropertyChanged("ImageId");
            }
        }

        private int userId;
        public int UserId
        {
            get => userId;
            set
            {
                if (value == userId) return;
                userId = value;
                OnPropertyChanged("UserId");
            }
        }

        private string imageIdsCsv;
        public string ImageIdsCsv
        {
            get => imageIdsCsv;
            set
            {
                if (value == imageIdsCsv) return;
                imageIdsCsv = value;
                OnPropertyChanged("ImageIdsCsv");
            }
        }
        public List<int> ImageIds { get; set; } = new();

        private bool isRecentlyRenovated;
        public bool IsRecentlyRenovated
        {
            get => isRecentlyRenovated;
            set
            {
                if(value == isRecentlyRenovated) return;
                isRecentlyRenovated = value;
                OnPropertyChanged();
            }
        }

        public Accommodation() { }
        public Accommodation(string name, int locationId, AccommodationType type, int maxGuestNum, int minStayingDays, int daysBeforeCancel, int imageId, string imageIdes, int userId, bool isRecentlyRenovated)
        {
            Name = name;
            LocationId = locationId;
            Type = type;
            MaxGuestNum = maxGuestNum;
            MinStayingDays = minStayingDays;
            CancellationThreshold = daysBeforeCancel;
            ImageId = imageId;
            ImageIdsCsv = imageIdes;
            UserId = userId;
            IsRecentlyRenovated = isRecentlyRenovated;
        }
        public string[] ToCSV()
        {
            ImageIdesToCsv();
            string[] csvValues = {
                Id.ToString(),
                UserId.ToString(),
                Name,
                LocationId.ToString(),
                Type.ToString(),
                MaxGuestNum.ToString(),
                MinStayingDays.ToString(),
                CancellationThreshold.ToString(),
                ImageId.ToString(),
                ImageIdsCsv,
                IsRecentlyRenovated.ToString()
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            UserId = Convert.ToInt32(values[1]);
            Name = values[2];
            LocationId = Convert.ToInt32(values[3]);
            Type = Enum.Parse<AccommodationType>(values[4]);
            MaxGuestNum = Convert.ToInt32(values[5]);
            MinStayingDays = Convert.ToInt32(values[6]);
            CancellationThreshold = Convert.ToInt32(values[7]);
            ImageId = Convert.ToInt32(values[8]);
            ImageIdsCsv = values[9];
            IsRecentlyRenovated = Convert.ToBoolean(values[10]);
            ImageIdesFromCsv(ImageIdsCsv);
        }
        public void ImageIdesToCsv()
        {
            if (ImageIds.Count <= 0) return;
            ImageId = ImageIds.First();
            ImageIdsCsv = string.Empty;
            foreach (var imageIde in ImageIds)
            {
                ImageIdsCsv += imageIde + ",";
            }
            ImageIdsCsv = ImageIdsCsv.Remove(ImageIdsCsv.Length - 1);
        }
        //TODO
        public void ImageIdesFromCsv(string value)
        {
            var imageIdesCsv = value.Split(",");
            foreach (var imageIde in imageIdesCsv)
            {
                if (imageIde != string.Empty)
                    ImageIds.Add(int.Parse(imageIde));
            }
        }


        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name" when string.IsNullOrEmpty(Name):
                        return "Name is required";
                    case "AccommodationType" when string.IsNullOrEmpty(Type.ToString()):
                        return "Type is required";
                    case "MaxNoGuests" when MaxGuestNum < 1:
                        return "Max is required";
                    case "MinStayDay" when MinStayingDays < 1:
                        return "Min is required";
                    case "CancelThreshold" when CancellationThreshold < 1:
                        return "Cancel is required";
                    default:
                        return null;
                }
            }
        }

        private readonly string[] validatedProperties = { "Name", "AccommodationType", "MaxNoGuests", "MinStayDay", "CancelThreshold" };

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
