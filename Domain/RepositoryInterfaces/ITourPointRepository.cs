using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface ITourPointRepository
    {
        public List<TourPoint> GetAll();
        public void Save(TourPoint tourPoint);
        public int NextId();
        public TourPoint Update(TourPoint tourPoint);
        public List<TourPoint> GetAllForTour(int id);
        public TourPoint GetOne(int id);
    }
}
