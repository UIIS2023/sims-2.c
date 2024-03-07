using System;
using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

namespace Tourist_Project.Applications.UseCases
{ 
    public class GuestRateService
    {
        private static readonly Injector injector = new();

        private readonly IGuestRateRepository guestRateRepository = injector.CreateInstance<IGuestRateRepository>();
        
        private readonly IReservationRepository reservationRepository = injector.CreateInstance<IReservationRepository>();
        public GuestRateService()
        {
        }
        public GuestRating Create(GuestRating guestRating)
        {
            return guestRateRepository.Save(guestRating);
        }
        public List<GuestRating> GetAll()
        {
            return guestRateRepository.GetAll();
        }

        public GuestRating Get(int id)
        {
            return guestRateRepository.GetById(id);
        }
        public GuestRating Update(GuestRating guestRating)
        {
            return guestRateRepository.Update(guestRating);
        }
        public void Delete(int id)
        {
            guestRateRepository.Delete(id);
        }

        public void HasNewRatings()
        {
            foreach (var reservation in reservationRepository.GetAll())
            {
                var daysSinceCheckOut = DateTime.Now - reservation.CheckOut;
                if (Math.Abs(daysSinceCheckOut.Days) < 5 &&
                    guestRateRepository.GetAll().All(c => c.ReservationId != reservation.Id))
                {
                    Create(new GuestRating(reservation.Id, 0, 0, ""));
                }
            }
        }
    }

}