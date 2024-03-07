using System;
using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

namespace Tourist_Project.Applications.UseCases
{
    public class ReservationService
    {
        private static readonly Injector injector = new();

        private readonly IReservationRepository reservationRepository = injector.CreateInstance<IReservationRepository>();
        private readonly AccommodationService accommodationService = new();

        public ReservationService()
        {
        }

        public Reservation Create(Reservation reservation)
        {
            return reservationRepository.Save(reservation);
        }

        public List<Reservation> GetAll()
        {
            return reservationRepository.GetAll();
        }

        public List<Reservation> GetAllByAccommodation(int accommodationId)
        {
            return GetAll().Where(reservation => reservation.AccommodationId == accommodationId).ToList();
        }

        public Reservation Get(int id)
        {
            return reservationRepository.GetById(id);
        }

        public Reservation Update(Reservation reservation)
        {
            return reservationRepository.Update(reservation);
        }

        public void Delete(int id)
        {
            reservationRepository.Delete(id);
        }

        public List<Reservation> FindReservationsForAccommodation(Accommodation selectedAccommodation)
        {
            return reservationRepository.GetByAccommodation(selectedAccommodation);
        }

        public List<Reservation> GetAllOrderedInDateSpan(DateSpan dateSpan, int accommodationId)
        {
            return GetAllByAccommodation(accommodationId).Where(reservation =>
                (reservation.CheckIn > dateSpan.StartingDate && reservation.CheckIn < dateSpan.EndingDate) ||
                (reservation.CheckOut > dateSpan.StartingDate && reservation.CheckOut < dateSpan.EndingDate)).OrderBy(o=>o.CheckIn).ToList();
        }

        public bool WasOnLocation(int userId, int locationId)
        {
            return GetAll().Any(reservation => reservation.GuestId == userId && accommodationService.Get(reservation.AccommodationId).LocationId == locationId);
        }
        
        public List<Reservation> GetReservationsOnLocation(int locationId)
        {
            return GetAll().Where(reservation => accommodationService.GetAccommodationIds(locationId).Contains(reservation.AccommodationId)).ToList();
        }

        public int GetOccupancyOnLocation(int locationId)
        {
            return GetReservationsOnLocation(locationId)
                .Where(reservation => (DateTime.Now - reservation.CheckIn).Days < 100)
                .Sum(reservation => (reservation.CheckOut - reservation.CheckIn).Days);
        }
    }

}
