using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class TourReservationRepository : ITourReservationRepository
    {
        private const string FilePath = "../../../Data/tourReservations.csv";
        private readonly Serializer<TourReservation> serializer = new();
        private List<TourReservation> reservations;

        public TourReservationRepository()
        {
            reservations = serializer.FromCSV(FilePath);
        }

        public List<TourReservation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public void Save(TourReservation tourReservation)
        {
            tourReservation.Id = NextId();
            reservations = GetAll();
            reservations.Add(tourReservation);
            serializer.ToCSV(FilePath, reservations);
        }

        public int NextId()
        {
            reservations = GetAll();
            if (reservations.Count < 1)
            {
                return 1;
            }
            return reservations.Max(r => r.Id) + 1;
        }

        public void Delete(int id)
        {
            reservations = GetAll();
            TourReservation reservation = reservations.Find(r => r.Id == id);
            reservations.Remove(reservation);
            serializer.ToCSV(FilePath, reservations);
        }

        public void Update(TourReservation reservation)
        {
            reservations = GetAll();
            var old = reservations.Find(r => r.Id == reservation.Id);
            int index = reservations.IndexOf(old);
            reservations.Remove(old);
            reservations.Insert(index, reservation);
            serializer.ToCSV(FilePath, reservations);
        }

        public TourReservation GetById(int id)
        {
            return GetAll().Find(r => r.Id == id);
        }
        public List<TourReservation> GetAllByTourId(int tourId)
        {
            return GetAll().FindAll(reservation => reservation.TourId == tourId);
        }

        public int NumberWithVoucher(int tourId)
        {
            return GetAllByTourId(tourId).FindAll(reservation => reservation.Voucher).Count;
        }

        public int NumberWithoutVoucher(int tourId)
        {
            return GetAllByTourId(tourId).FindAll(reservation => !reservation.Voucher).Count;
        }

        public TourReservation GetForUndo(int userId, int tourId)
        {
            var reservationId = GetAll().Where(tr => tr.UserId == userId && tr.TourId == tourId).Max(tr => tr.Id);
            return GetById(reservationId);
        }
    }
}

