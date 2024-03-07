using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface ITourAttendanceRepository
    {
        public List<TourAttendance> GetAll();
        public void Save(TourAttendance tourAttendance);
        public int NextId();
        public void Update(TourAttendance tourAttendance);
        public void Delete(int id);
        public List<TourAttendance> GetAllByTourId(int tourId);

        TourAttendance GetForUndo(int loggedUserId, int selectedTourId);
    }
}
