using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

namespace Tourist_Project.Applications.UseCases
{
    public class AccommodationRatingService 
    {
        private static readonly Injector injector = new();
        private readonly IAccommodationRatingRepository _accommodationRatingRepository = injector.CreateInstance<IAccommodationRatingRepository>();

        public AccommodationRatingService()
        {
        }

        public AccommodationRating Create(AccommodationRating accommodationRating)
        {
            return _accommodationRatingRepository.Save(accommodationRating);
        }

        public List<AccommodationRating> GetAll()
        {
            return _accommodationRatingRepository.GetAll();
        }

        public AccommodationRating Get(int id)
        {
            return _accommodationRatingRepository.GetById(id);
        }

        public void Delete(int id)
        {
            _accommodationRatingRepository.Delete(id);
        }

        public AccommodationRating Update(AccommodationRating accommodationRating)
        {
            return _accommodationRatingRepository.Update(accommodationRating);
        }
        public double getRating()
        {
            return (double)_accommodationRatingRepository.GetAll().Sum(accommodationRating => accommodationRating.AccommodationGrade + accommodationRating.Cleanness + accommodationRating.OwnerRating) / (_accommodationRatingRepository.GetAll().Count * 3);
        }
    }
}
