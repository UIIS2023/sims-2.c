using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    internal interface IAccommodationRatingRepository
    {
        public List<AccommodationRating> GetAll();
        public AccommodationRating Save(AccommodationRating accommodationRating);
        public int NextId();
        public void Delete(int id);
        public AccommodationRating Update(AccommodationRating accommodationRating);
        public AccommodationRating GetById(int id);

    }
}
