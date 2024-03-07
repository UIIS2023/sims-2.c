using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum Status { NotBegin, Begin, End, Cancel }
    public class Tour : ISerializable, IDataErrorInfo
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int MaxGuestsNumber { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public int ImageId { get; set; }
        public Status Status { get; set; }
        public int UserId { get; set; }

        public Tour()
        {
            Duration = 1;
            MaxGuestsNumber = 1;
        }

        public Tour(int locationId, string description, string language, int guestNumber)
        {
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuestsNumber = guestNumber;
        }

        public Tour(string name, int locationId, string description, string language, int maxGuestsNumber, DateTime startTime, int duration, int imageId, int userId)
        {
            Name = name;
            LocationId = locationId;
            Description = description;
            Language = language;
            MaxGuestsNumber = maxGuestsNumber;
            StartTime = startTime;
            Duration = duration;
            this.ImageId = imageId;
            Status = Status.NotBegin;
            UserId = userId;
        }

        #region Serilization
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                LocationId.ToString(),
                Description,
                Language,
                MaxGuestsNumber.ToString(),
                StartTime.ToString(),
                Duration.ToString(),
                ImageId.ToString(),
                Status.ToString(),
                UserId.ToString(),
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            LocationId = int.Parse(values[2]);
            Description = values[3];
            Language = values[4];
            MaxGuestsNumber = int.Parse(values[5]);
            StartTime = DateTime.Parse(values[6]);
            Duration = int.Parse(values[7]);
            ImageId = int.Parse(values[8]);
            Status = Enum.Parse<Status>(values[9]);
            UserId = int.Parse(values[10]);
        }
        #endregion

        public override string ToString()
        {
            return Name;
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
                    case "Description" when string.IsNullOrEmpty(Description):
                        return "Description is required";
                    case "Language" when string.IsNullOrEmpty(Language):
                        return "Language is required";
                    case "StartTime" when string.IsNullOrEmpty(StartTime.ToString()):
                        return "Date is required";
                    case "MaxGuestsNumber" when MaxGuestsNumber < 1:
                        return "Max guest number is required";
                    case "Duration" when Duration < 1:
                        return "Duration is required";
                    default:
                        return null;
                }
            }
        }

        private readonly string[] validatedProperties = { "Name", "Description", "Language", "StartTime", "MaxGuestsNumber", "Duration" };

        public bool IsValid
        {
            get
            {
                return validatedProperties.All(property => this[property] == null);
            }
        }
    }
}
