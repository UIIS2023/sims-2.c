using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

namespace Tourist_Project.Applications.UseCases
{
    public class TourReservationService
    {
        private static readonly Injector injector = new();

        private readonly ITourReservationRepository repository = injector.CreateInstance<ITourReservationRepository>();
        private readonly UserService userService = new();

        public TourReservationService()
        {

        }

        public List<TourReservation> GetAllByTourId(int tourId)
        {
            return repository.GetAllByTourId(tourId);
        }
        public List<User> GetUsersOnTour(int tourId)
        {
            return repository.GetAllByTourId(tourId).Select(reservation => userService.GetOne(reservation.UserId)).ToList();
        }

        public int[] CountingTourists(int tourId)
        {
            var counterYounger = 0;
            var counterBetween = 0;
            var counterOlder = 0;
            foreach (var user in GetUsersOnTour(tourId))
            {
                switch (DateTime.Now.Subtract(user.BirthDate).TotalDays / 365.25)
                {
                    case < 18:
                        counterYounger++;
                        break;
                    case > 50:
                        counterOlder++;
                        break;
                    default:
                        counterBetween++;
                        break;
                }
            }
            return new int[] { counterYounger, counterBetween, counterOlder };
        }

        public double WithVoucherPercent(int tourId)
        {
            return (double)repository.NumberWithVoucher(tourId)/(double)GetUsersOnTour(tourId).Count * 100;
        }

        public double WithOutVoucherPercent(int tourId)
        {
            return (double)repository.NumberWithoutVoucher(tourId) / (double)GetUsersOnTour(tourId).Count * 100;
        }

        public int CountTourists(int tourId)
        {
            var counter = 0;
            foreach (var reservation in GetAllByTourId(tourId))
            {
                counter += reservation.GuestsNumber;
            }

            return counter;
        }

        public TourReservation? GetByUserIdAndTourId(int userId, int tourId)
        {
            return GetAll().Find(tr => tr.UserId == userId && tr.TourId == tourId);
        }


        public List<TourReservation> GetAll()
        {
            return repository.GetAll();
        }
        public void Save(TourReservation reservation)
        {
            repository.Save(reservation);
        }
        public void Delete(int id)
        {
            repository.Delete(id);
        }
        public void Update(TourReservation reservation)
        {
            repository.Update(reservation);
        }
        public TourReservation GetById(int id)
        {
            return repository.GetById(id);
        }

        public TourReservation GetForUndo(int userId, int tourId)
        {
            return repository.GetForUndo(userId, tourId);
        }
    }
}
