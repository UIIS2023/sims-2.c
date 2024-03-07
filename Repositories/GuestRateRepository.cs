using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class GuestRateRepository : IGuestRateRepository
    {
        private const string FilePath = "../../../Data/guestRatings.csv";
        private readonly Serializer<GuestRating> serializer;
        private List<GuestRating> guestReviews;
        public GuestRateRepository()
        {
            serializer = new Serializer<GuestRating>();
            guestReviews = serializer.FromCSV(FilePath);
        }
        public List<GuestRating> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public GuestRating Save(GuestRating guestRating)
        {
            guestRating.Id = NextId();
            guestReviews = serializer.FromCSV(FilePath);
            guestReviews.Add(guestRating);
            serializer.ToCSV(FilePath, guestReviews);
            return guestRating;
        }

        public int NextId()
        {
            guestReviews = serializer.FromCSV(FilePath);
            if (guestReviews.Count < 1)
            {
                return 1;
            }
            return guestReviews.Max(c => c.Id) + 1;
        }

        public void Delete(int id)
        {
            guestReviews = serializer.FromCSV(FilePath);
            var founded = guestReviews.Find(c => c.Id == id);
            guestReviews.Remove(founded);
            serializer.ToCSV(FilePath, guestReviews);
        }

        public GuestRating Update(GuestRating guestRating)
        {
            guestReviews = serializer.FromCSV(FilePath);
            var current = guestReviews.Find(c => c.Id == guestRating.Id);
            var index = guestReviews.IndexOf(current);
            guestReviews.Remove(current);
            guestReviews.Insert(index, guestRating);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, guestReviews);
            return guestRating;
        }

        public GuestRating GetById(int id)
        {
            return guestReviews.Find(c => c.Id == id);
        }
    }
}