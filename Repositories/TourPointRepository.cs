using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class TourPointRepository : ITourPointRepository
    {
        private const string filePath = "../../../Data/tourPoints.csv";
        private readonly Serializer<TourPoint> serializer;
        private List<TourPoint> tourPoints;

        public TourPointRepository()
        {
            serializer = new Serializer<TourPoint>();
            tourPoints = serializer.FromCSV(filePath);
        }

        public List<TourPoint> GetAll()
        {
            return serializer.FromCSV(filePath);
        }

        public void Save(TourPoint tourPoint)
        {
            tourPoint.Id = NextId();
            tourPoints = serializer.FromCSV(filePath);
            tourPoints.Add(tourPoint);
            serializer.ToCSV(filePath, tourPoints);
        }

        public int NextId()
        {
            tourPoints = serializer.FromCSV(filePath);
            if (tourPoints.Count < 1)
            {
                return 1;
            }
            return tourPoints.Max(c => c.Id) + 1;
        }

        public TourPoint Update(TourPoint tourPoint)
        {
            tourPoints = serializer.FromCSV(filePath);
            TourPoint current = tourPoints.Find(t => t.Id == tourPoint.Id);
            int index = tourPoints.IndexOf(current);
            tourPoints.Remove(current);
            tourPoints.Insert(index, tourPoint);
            serializer.ToCSV(filePath, tourPoints);
            return current;
        }

        public List<TourPoint> GetAllForTour(int id)
        {
            return GetAll().FindAll(tourPoint => tourPoint.TourId == id);
        }

        public TourPoint GetOne(int id)
        {
            return GetAll().Find(tourPoint => tourPoint.Id == id);
        }

        public string GetAllForTourString(int id)
        {
            string retVal = string.Empty;
            foreach(TourPoint tourPoint in GetAllForTour(id))
            {
                retVal += tourPoint.Name + ", ";
            }

            if(retVal.Length > 2)
            {
                retVal = retVal.Remove(retVal.Length - 2, 2);
            }

            return retVal;
        }

    }
}
