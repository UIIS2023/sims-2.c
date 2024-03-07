using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface ITourRepository
    {
        public List<Tour> GetAll();
        public Tour GetOne(int id);
        public List<Tour> GetTodaysTours();
        public List<Tour> GetFutureTours();
        public List<Tour> GetPastTours();
        public void Save(Tour tour);
        public int NextId();
        public Tour Update(Tour tour);
        public List<Tour> GetAllByYear(int year, User loggedInUser);
        public List<Tour> GetYearAppointments(string name, int year, User loggedInUser);
        public List<Tour> GetAllTourAppointments(string name, User loggedInUser);
        public List<Tour> GetAllNotBegin();
        public List<Tour> GetAllNotBeginByGuide(int guideId);
        public List<Tour> GetEndedToursThisYear(int guideId);
        public List<string> GetTourLanguages();

        List<Tour> GetPastYearTours();
    }
}
