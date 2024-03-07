using System.Collections.Generic;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface IReservationRepository
    {
        public List<Reservation> GetAll();
        public Reservation Save(Reservation reservation);
        public int NextId();
        public void Delete(int id);
        public Reservation Update(Reservation reservation);
        public Reservation GetById(int id);

        public List<Reservation> GetByAccommodation(Accommodation accommodation);
    }

}