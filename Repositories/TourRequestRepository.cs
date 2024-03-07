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
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string filePath = "../../../Data/tourRequests.csv";
        private readonly Serializer<TourRequest> serializer;
        public List<TourRequest> Requests;

        public TourRequestRepository()
        {
            serializer = new Serializer<TourRequest>();
            Requests = serializer.FromCSV(filePath);
        }

        public List<TourRequest> GetAll()
        {
            return serializer.FromCSV(filePath);
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            Requests = serializer.FromCSV(filePath);
            Requests.Add(tourRequest);
            serializer.ToCSV(filePath, Requests);
            return tourRequest;
        }

        public int NextId()
        {
            Requests = serializer.FromCSV(filePath);
            if (Requests.Count < 1)
            {
                return 1;
            }
            return Requests.Max(c => c.Id) + 1;
        }

        public TourRequest Update(TourRequest tourRequest)
        {
            Requests = serializer.FromCSV(filePath);
            var current = Requests.Find(c => c.Id == tourRequest.Id);
            var index = Requests.IndexOf(current);
            Requests.Remove(current);
            Requests.Insert(index, tourRequest);
            serializer.ToCSV(filePath, Requests);
            return tourRequest;
        }

        public void Delete(int id)
        {
            Requests = GetAll();
            var found = Requests.Find(x => x.Id == id);
            Requests.Remove(found);
            serializer.ToCSV(filePath, Requests);
        }

        public TourRequest GetById(int id)
        {
            return serializer.FromCSV(filePath).Find(tr => tr.Id == id);
        }

        public List<TourRequest> GetAllForUser(int userId)
        {
            return serializer.FromCSV(filePath).Where(tr => tr.UserId == userId && tr.ComplexTourId == -1).ToList();
        }

        public List<int> GetAllRequestedLocations(int userId)
        {
            var retVal = new List<int>();
            foreach (var requestedLocationId in GetAll().Where(r => r.UserId == userId && r.ComplexTourId == -1).Select(r => r.LocationId).Distinct())
            {
                retVal.Add(requestedLocationId);
            }
            return retVal;
        }

        public List<string> GetAllRequestedLanguages(int userId)
        {
            var retVal = new List<string>();
            foreach (var request in GetAll().Where(r => r.UserId == userId && r.ComplexTourId == -1))
            {
                if (retVal.Contains(request.Language)) continue;
                retVal.Add(request.Language);
            }
            return retVal;
        }
        public List<TourRequest> GetAllPending()
        {
            return GetAll().FindAll(tr => tr.Status == TourRequestStatus.Pending);
        }

        public List<TourRequest> GetAllLastYear()
        {
            return GetAll().FindAll(request => request.CreateDate >= DateTime.Now.AddYears(-1) && request.ComplexTourId == -1);
        }

        public List<TourRequest> GetAllForComplexTour(int complexTourId)
        {
            return GetAll().Where(tr => tr.ComplexTourId == complexTourId).ToList();
        }

        public void DenyAllForComplexTour(int complexTourId)
        {
            var requestsFromComplexTour = GetAllForComplexTour(complexTourId);
            foreach (var request in requestsFromComplexTour)
            {
                request.Status = TourRequestStatus.Denied;
                Update(request);
            }
        }

        public List<TourRequest> GetAllPendingForUser(int loggedUserId)
        {
            return GetAll().Where(r =>
                r.UserId == loggedUserId && r.Status == TourRequestStatus.Pending && r.ComplexTourId == -1).ToList();
        }

        public List<TourRequest> GetForSelectedYear(int userId, string year)
        {
            return year.Equals("All time") ? GetAllForUser(userId) : GetAll().Where(r => r.UntilDate.Year == int.Parse(year) && r.ComplexTourId == -1).ToList();
        }

        public int GetUsersLatestRequestId(int loggedUserId)
        {
            return GetAllForUser(loggedUserId).Max(tr => tr.Id);
        }

        public List<string> GetAllYears(int userId)
        {
            var years = GetAllForUser(userId).Select(request => request.CreateDate.Year.ToString()).ToList();
            years.Add("Overall");
            return years.Distinct().ToList();
        }
    }
}

