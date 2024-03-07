using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tourist_Project.Domain;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

namespace Tourist_Project.Applications.UseCases
{
    public class RenovationService
    {
        private static readonly Injector injector = new();

        private readonly IRenovationRepository renovationRepository = injector.CreateInstance<IRenovationRepository>();

        private readonly ReservationService reservationService = new();
        public RenovationService() { }

        public Renovation Create(Renovation renovation)
        {
            return renovationRepository.Save(renovation);
        }

        public List<Renovation> GetAll()
        {
            return renovationRepository.GetAll();
        }

        public Renovation Get(int id)
        {
            return renovationRepository.GetById(id);
        }
        public Renovation Update(Renovation renovation)
        {
            return renovationRepository.Update(renovation);
        }

        public void Delete(int id)
        {
            renovationRepository.Delete(id);
        }

        public ObservableCollection<DateSpan> FindDateSpans(DateSpan requestedDateSpan, int length, int accommodationId)
        {
            ObservableCollection<DateSpan> possibleDateSpans = new();

            foreach (var reservation in reservationService.GetAllOrderedInDateSpan(requestedDateSpan, accommodationId))
            {
                if (requestedDateSpan.StartingDate > requestedDateSpan.EndingDate) return possibleDateSpans;
                var daysToReservation = reservation.CheckIn - requestedDateSpan.StartingDate;
                if (daysToReservation.Days < length)
                {
                    if(requestedDateSpan.StartingDate < reservation.CheckOut )
                        requestedDateSpan.StartingDate = reservation.CheckOut;
                    continue;
                }

                while ((reservation.CheckIn - requestedDateSpan.StartingDate).Days > length)
                    AddNewSpan(requestedDateSpan, length, possibleDateSpans);
            }

            if(reservationService.GetAllOrderedInDateSpan(requestedDateSpan,accommodationId).Count != 0)
                requestedDateSpan.StartingDate = reservationService.GetAllOrderedInDateSpan(requestedDateSpan, accommodationId).Last().CheckOut;

            while ((requestedDateSpan.EndingDate - requestedDateSpan.StartingDate).Days > length)
            {
                AddNewSpan(requestedDateSpan, length, possibleDateSpans);
            }
            
            return possibleDateSpans;
        }

        private static void AddNewSpan(DateSpan requestedDateSpan, int length, ObservableCollection<DateSpan> possibleDateSpans)
        {

            possibleDateSpans.Add(new DateSpan(requestedDateSpan.StartingDate, requestedDateSpan.StartingDate.AddDays(length)));
            requestedDateSpan.StartingDate = requestedDateSpan.StartingDate.AddDays(length);
        }
    }

}