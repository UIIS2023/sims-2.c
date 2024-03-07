using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;
using System;

namespace Tourist_Project.Repositories
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private const string FilePath = "../../../Data/accommodations.csv";
        private readonly Serializer<Accommodation> serializer;
        private List<Accommodation> accommodations;
        public AccommodationRepository()
        {
            serializer = new Serializer<Accommodation>();
            accommodations = serializer.FromCSV(FilePath);
        }
        public List<Accommodation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Accommodation Save(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            accommodations = serializer.FromCSV(FilePath);
            accommodations.Add(accommodation);
            serializer.ToCSV(FilePath, accommodations);
            return accommodation;
        }

        public int NextId()
        {
            accommodations = serializer.FromCSV(FilePath);
            if (accommodations.Count < 1)
            {
                return 1;
            }
            return accommodations.Max(c => c.Id) + 1;
        }

        public void Delete(int id)
        {
            accommodations = serializer.FromCSV(FilePath);
            var founded = accommodations.Find(c => c.Id == id);
            accommodations.Remove(founded);
            serializer.ToCSV(FilePath, accommodations);
        }

        public Accommodation Update(Accommodation accommodation)
        {
            accommodations = serializer.FromCSV(FilePath);
            var current = accommodations.Find(c => c.Id == accommodation.Id);
            var index = accommodations.IndexOf(current);
            accommodations.Remove(current);
            accommodations.Insert(index, accommodation);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, accommodations);
            return accommodation;
        }
        public Accommodation GetById(int id)
        {
            return accommodations.Find(c => c.Id == id);
        }

        public List<Accommodation> GetByUser(int userId)
        {
            return accommodations.FindAll(accommodation => accommodation.UserId == userId);
        }

        public List<int> GetLocationIds(int userId)
        {
            var locations = new List<int>();
            foreach (var accommodation in GetByUser(userId).Where(accommodation => !locations.Contains(accommodation.LocationId)))
            {
                locations.Add(accommodation.LocationId);
            }
            return locations;
        }
    }
}