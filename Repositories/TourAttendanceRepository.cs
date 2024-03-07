using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class TourAttendanceRepository : ITourAttendanceRepository
    {
        private const string filePath = "../../../Data/tourAttendance.csv";
        private readonly Serializer<TourAttendance> serializer;
        private List<TourAttendance> tourAttendances;

        public TourAttendanceRepository()
        {
            serializer = new Serializer<TourAttendance>();
            tourAttendances = serializer.FromCSV(filePath);
        }

        public List<TourAttendance> GetAll()
        {
            return serializer.FromCSV(filePath);
        }

        public void Save(TourAttendance tourAttendance)
        {
            tourAttendance.Id = NextId();
            tourAttendances = serializer.FromCSV(filePath);
            tourAttendances.Add(tourAttendance);
            serializer.ToCSV(filePath, tourAttendances);
        }

        public int NextId()
        {
            tourAttendances = serializer.FromCSV(filePath);
            if (tourAttendances.Count < 1)
            {
                return 1;
            }
            return tourAttendances.Max(c => c.Id) + 1;
        }

        public void Update(TourAttendance tourAttendance)
        {
            tourAttendances = serializer.FromCSV(filePath);

            foreach(TourAttendance oldAttendance in tourAttendances)
            {
                if(oldAttendance.Id == tourAttendance.Id)
                {
                    oldAttendance.Presence = tourAttendance.Presence;
                    oldAttendance.CheckPointId = tourAttendance.CheckPointId;
                    oldAttendance.TourPoint = tourAttendance.TourPoint;
                }
            }

            serializer.ToCSV(filePath, tourAttendances);
        }

        public void Delete(int id)
        {
            tourAttendances = GetAll();
            var attendanceFoDelete = tourAttendances.Find(ta => ta.Id == id);
            tourAttendances.Remove(attendanceFoDelete);
            serializer.ToCSV(filePath, tourAttendances);
        }

        public List<TourAttendance> GetAllByTourId(int tourId)
        {
            return GetAll().FindAll(reservation => reservation.TourId == tourId);
        }

        public TourAttendance GetForUndo(int loggedUserId, int selectedTourId)
        {
            var undoAttendanceId = GetAll().Where(ta => ta.UserId == loggedUserId && ta.TourId == selectedTourId).Max(ta => ta.Id);
            return GetAll().Find(ta => ta.Id == undoAttendanceId);
        }
    }
}
