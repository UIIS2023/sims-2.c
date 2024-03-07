using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Domain.Models;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class AccommodationRatingRepository : IAccommodationRatingRepository
    {
        private const string FilePath = "../../../Data/accommodationRatings.csv";
        private readonly Serializer<AccommodationRating> serializer;
        private List<AccommodationRating> _accommodationRatings;

        public AccommodationRatingRepository()
        {
            serializer = new Serializer<AccommodationRating>();
            _accommodationRatings = serializer.FromCSV(FilePath);
        }

        public List<AccommodationRating> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public AccommodationRating Save(AccommodationRating accommodationRating)
        {
            accommodationRating.Id = NextId();
            _accommodationRatings = serializer.FromCSV(FilePath);
            _accommodationRatings.Add(accommodationRating);
            serializer.ToCSV(FilePath, _accommodationRatings);
            return accommodationRating;
        }

        public int NextId()
        {
            _accommodationRatings = serializer.FromCSV(FilePath);
            if (_accommodationRatings.Count < 1)
            {
                return 1;
            }
            return _accommodationRatings.Max(c => c.Id) + 1;
        }

        public AccommodationRating GetById(int id)
        {
            return _accommodationRatings.Find(c => c.Id == id);
        }


        public void Delete(int id)
        {
            _accommodationRatings = serializer.FromCSV(FilePath);
            var found = _accommodationRatings.Find(c => c.Id == id);
            _accommodationRatings.Remove(found);
            serializer.ToCSV(FilePath, _accommodationRatings);
        }
        public AccommodationRating Update(AccommodationRating accommodationRating)
        {
            _accommodationRatings = serializer.FromCSV(FilePath);
            var current = _accommodationRatings.Find(c => c.Id == accommodationRating.Id);
            var index = _accommodationRatings.IndexOf(current);
            _accommodationRatings.Remove(current);
            _accommodationRatings.Insert(index, accommodationRating);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, _accommodationRatings);
            return accommodationRating;
        }

    }
}
