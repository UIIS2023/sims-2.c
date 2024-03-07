using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Applications.UseCases
{
    public class ComplexTourService
    {
        private static readonly Injector injector = new();
        private readonly IComplexTourRepository complexTourRepository = injector.CreateInstance<IComplexTourRepository>();
        private readonly ITourRequestRepository tourRequestRepository = injector.CreateInstance<ITourRequestRepository>();
        public ComplexTourService()
        {
        }

        public List<ComplexTour> GetAll()
        {
            return complexTourRepository.GetAll();
        }

        public List<ComplexTour> GetAllForUser(int userId)
        {
            return complexTourRepository.GetAllForUser(userId);
        }

        public List<ComplexTour> GetAllPendingForUser(int userId)
        {
            return complexTourRepository.GetAllPendingForUser(userId);
        }

        public ComplexTour Save(ComplexTour complexTour)
        {
            return complexTourRepository.Save(complexTour);
        }

        public ComplexTour Update(ComplexTour complexTour)
        {
            return complexTourRepository.Update(complexTour);
        }

        public int GetNextRequestId()
        {
            return complexTourRepository.NextId();
        }

        public void UpdateComplexTourStatusesForUser(int userId)
        {
            var complexTours = GetAllPendingForUser(userId);
            foreach (var complexTour in complexTours)
            {
                var tourRequests = tourRequestRepository.GetAllForComplexTour(complexTour.Id);
                var areAllAccepted = tourRequests.All(tourRequest => tourRequest.Status == TourRequestStatus.Accepted);

                if (areAllAccepted)
                {
                    complexTour.Status = ComplexTourStatus.Accepted;
                    Update(complexTour);
                    continue;
                }
                
                var firstTour = tourRequests.Find(tr => tr.UntilDate == tourRequests.Min(tr => tr.UntilDate));
                if (firstTour.UntilDate.Date <= DateTime.Today.AddDays(2).Date)
                {
                    var areAllPending = tourRequests.All(tourRequest => tourRequest.Status == TourRequestStatus.Pending);

                    if (areAllPending)
                    {
                        complexTour.Status = ComplexTourStatus.Denied;
                        Update(complexTour);
                        tourRequestRepository.DenyAllForComplexTour(complexTour.Id);
                    }
                }
            }
        }

        public void UndoLatestRequest(int loggedUserId)
        {
            var complexTourForDeleteId = complexTourRepository.GetUsersLatestRequestId(loggedUserId);
            var tourRequests = tourRequestRepository.GetAllForComplexTour(complexTourForDeleteId);
            foreach (var request in tourRequests)
            {
                tourRequestRepository.Delete(request.Id);
            }
            complexTourRepository.Delete(complexTourForDeleteId);
        }
    }
}
