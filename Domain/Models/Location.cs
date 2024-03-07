using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public class Location : ISerializable, INotifyPropertyChanged, IDataErrorInfo
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

        private string city;
        public string City
        {
            get => city;
            set
            {
                if(value == city) return;
                city = value;
                OnPropertyChanged("City");
            }
        }

        private string country;
        public string Country
        {
            get => country;
            set
            {
                if(value == country) return;
                country = value;
                OnPropertyChanged("Country");
            }
        }
        public Location() { }
        public Location(string city, string country)
        {
            City = city;
            Country = country;
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                City,
                Country
            };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            City = values[1];
            Country = values[2];
        }
        public override string ToString()
        {
            return City + ", " + Country;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Country" when string.IsNullOrEmpty(Country):
                        return "Country is required";
                    case "City" when string.IsNullOrEmpty(City):
                        return "City is required";
                    default:
                        return null;
                }
            }
        }

        private readonly string[] validatedProperties = { "Country", "City" };

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