using System.Collections.Generic;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{ 
    public interface IGuestRateRepository
    {
        public List<GuestRating> GetAll();
        public GuestRating Save(GuestRating guestRating);
        public int NextId();
        public void Delete(int id);
        public GuestRating Update(GuestRating guestRating);
        public GuestRating GetById(int id);
    }

}