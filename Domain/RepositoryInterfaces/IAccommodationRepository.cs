using System.Collections.Generic;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface IAccommodationRepository
    {
        public List<Accommodation> GetAll();
        public Accommodation Save(Accommodation accommodation);
        public int NextId();
        public void Delete(int id);
        public Accommodation Update(Accommodation accommodation);
        public Accommodation GetById(int id);
        public List<Accommodation> GetByUser(int userId);
        public List<int> GetLocationIds(int userId);
    }
}
