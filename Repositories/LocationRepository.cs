using System.Collections.Generic;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private const string filePath = "../../../Data/locations.csv";
        private readonly Serializer<Location> serializer;
        private readonly List<Location> locations;

        public LocationRepository()
        {
            serializer = new Serializer<Location>();
            locations = serializer.FromCSV(filePath);
        }

        public List<Location> GetAll()
        {
            return serializer.FromCSV(filePath);
        }

        public int GetId(string city, string country)
        {
            return locations.Find(x => x.City == city && x.Country == country).Id;
        }
        public Location GetById(int id)
        {
            return locations.Find(c => c.Id == id);
        }
    }
}
