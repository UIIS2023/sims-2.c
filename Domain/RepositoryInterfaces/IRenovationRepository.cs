using System.Collections.Generic;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface IRenovationRepository
    {
        public List<Renovation> GetAll();
        public Renovation Save(Renovation renovation);
        public int NextId();
        public void Delete(int id);
        public Renovation Update(Renovation renovation);
        public Renovation GetById(int id);
    }

}