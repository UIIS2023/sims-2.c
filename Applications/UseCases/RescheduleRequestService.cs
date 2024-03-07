using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Applications.UseCases
{
    public class RescheduleRequestService
    {
        private static readonly Injector injector = new();

        private readonly IRescheduleRequestRepository rescheduleRequestRepository = injector.CreateInstance<IRescheduleRequestRepository>();


        public RescheduleRequestService()
        {
        }

        public RescheduleRequest Create(RescheduleRequest rescheduleRequest)
        {
            return rescheduleRequestRepository.Save(rescheduleRequest);
        }

        public List<RescheduleRequest> GetAll()
        {
            return rescheduleRequestRepository.GetAll();
        }

        public RescheduleRequest Get(int id)
        {
            return rescheduleRequestRepository.GetById(id);
        }

        public List<RescheduleRequest> GetByStatus(RequestStatus status)
        {
            return rescheduleRequestRepository.GetByStatus(status);
        }

        public RescheduleRequest Update(RescheduleRequest rescheduleRequest)
        {
            return rescheduleRequestRepository.Update(rescheduleRequest);
        }

        public void Delete(int id)
        {
           rescheduleRequestRepository.Delete(id);
        }
    }
}
