using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface ITourRequestRepository
    {
        public List<TourRequest> GetAll();
        public TourRequest Save(TourRequest tourRequest);
        public int NextId();
        public TourRequest Update(TourRequest tourRequest);
        public void Delete(int id);
        public TourRequest GetById(int id);
        List<TourRequest> GetAllForUser(int userId);
        List<int> GetAllRequestedLocations(int userId);
        List<string> GetAllRequestedLanguages(int userId);
        public List<TourRequest> GetAllPending();
        public List<TourRequest> GetAllLastYear();
        List<TourRequest> GetAllForComplexTour(int complexTourId);
        void DenyAllForComplexTour(int complexTourId);
        List<TourRequest> GetAllPendingForUser(int loggedUserId);
        List<TourRequest> GetForSelectedYear(int userId, string year);
        int GetUsersLatestRequestId(int loggedUserId);
        public List<string> GetAllYears(int userId);
    }
}
