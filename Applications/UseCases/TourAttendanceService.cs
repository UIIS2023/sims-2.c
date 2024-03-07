using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Repositories;
using Tourist_Project.WPF.ViewModels;
using Tourist_Project.WPF.ViewModels.Guide;

namespace Tourist_Project.Applications.UseCases
{
    public class TourAttendanceService
    {
        private static readonly Injector injector = new();

        private readonly ITourAttendanceRepository attendanceRepository = injector.CreateInstance<ITourAttendanceRepository>();
        private readonly ITourRepository tourRepository = injector.CreateInstance<ITourRepository>();

        private readonly UserService userService = new();
        private readonly TourPointService tourPointService = new();

        public TourAttendanceService()
        {
        }

        public void UpdateCollection(TourAttendance selectedTourAttendance, TourPoint selectedTourPoint)
        {
            var tourAttendances = TouristListViewModel.TourAttendances;
            attendanceRepository.Update(selectedTourAttendance);
            tourAttendances.Clear();
            foreach (var attendance in GetAllByTourId(selectedTourPoint.TourId))
            {
                tourAttendances.Add(attendance);
            }
            foreach (var attendace in tourAttendances)
            {
                attendace.User = userService.GetOne(attendace.UserId);
                attendace.TourPoint = tourPointService.GetOne(attendace.CheckPointId);
            }
        }
        public List<TourAttendance> GetAllByTourId(int tourId)
        {
            return attendanceRepository.GetAllByTourId(tourId);
        }

        public List<TourAttendance> GetAll()
        {
            return attendanceRepository.GetAll();
        }

        public void Save(TourAttendance tourAttendance)
        {
            attendanceRepository.Save(tourAttendance);
        }

        public void Update(TourAttendance tourAttendance)
        {
            attendanceRepository.Update(tourAttendance);
        }

        public void Delete(int tourId)
        {
            attendanceRepository.Delete(tourId);
        }

        public TourAttendance GetByTourIdAndUserId(int tourId, int userId)
        {
            return GetAll().Find(t => t.TourId == tourId && t.UserId == userId);
        }

        public bool WasUserPresent(int userId, int tourId)
        {
            var attendance = GetAll().Find(t => t.TourId == tourId && t.UserId == userId);
            return attendance != null && attendance.Presence == Presence.Yes;
        }

        public int GetUsersPastYearAttendancesCount(int userId)
        {
            var retVal = 0;
            var pastYearTours = tourRepository.GetPastYearTours();
            foreach (var tour in pastYearTours)
            {
                if (WasUserPresent(userId, tour.Id))
                {
                    retVal++;
                }
            }
            return retVal;
        }

        public void DeleteLastAttendance(int loggedUserId, int selectedTourId)
        {
            var lastAttendance = attendanceRepository.GetForUndo(loggedUserId, selectedTourId);
            attendanceRepository.Delete(lastAttendance.Id);
        }
    }
}
