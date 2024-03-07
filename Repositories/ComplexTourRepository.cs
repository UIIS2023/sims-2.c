using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class ComplexTourRepository : IComplexTourRepository
    {
        private const string filePath = "../../../Data/complexTours.csv";
        private readonly Serializer<ComplexTour> serializer;
        public List<ComplexTour> Tours;

        public ComplexTourRepository()
        {
            serializer = new Serializer<ComplexTour>();
            Tours = serializer.FromCSV(filePath);
        }
        public List<ComplexTour> GetAll()
        {
            return serializer.FromCSV(filePath);
        }

        public List<ComplexTour> GetAllForUser(int userId)
        {
            Tours = GetAll();
            return Tours.Where(tour => tour.UserId == userId).ToList();
        }

        public ComplexTour Save(ComplexTour complexTour)
        {
            Tours = GetAll();
            Tours.Add(complexTour);
            serializer.ToCSV(filePath, Tours);
            return complexTour;
        }

        public ComplexTour Update(ComplexTour complexTour)
        {
            Tours = GetAll();
            var current = Tours.Find(c => c.Id == complexTour.Id);
            var index = Tours.IndexOf(current);
            Tours.Remove(current);
            Tours.Insert(index, complexTour);
            serializer.ToCSV(filePath, Tours);
            return complexTour;
        }

        public void Delete(int complexTourId)
        {
            Tours = GetAll();
            var complexTourForDelete = Tours.Find(ct => ct.Id == complexTourId);
            Tours.Remove(complexTourForDelete);
            serializer.ToCSV(filePath, Tours);
        }

        public int NextId()
        {
            if (Tours.Count < 1)
            {
                return 1;
            }
            return Tours.Max(t => t.Id) + 1;
        }

        public List<ComplexTour> GetAllPendingForUser(int userId)
        {
            Tours = GetAll();
            return Tours.Where(tour => tour.UserId == userId && tour.Status == ComplexTourStatus.Pending).ToList();
        }

        public int GetUsersLatestRequestId(int loggedUserId)
        {
            return GetAll().Where(ct => ct.UserId == loggedUserId).Max(ct => ct.Id);
        }
    }
}
