using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class RescheduleRequestRepository : IRescheduleRequestRepository
    {
        private const string FilePath = "../../../Data/rescheduleRequests.csv";
        private readonly Serializer<RescheduleRequest> serializer;
        private List<RescheduleRequest> rescheduleRequests;


        public RescheduleRequestRepository()
        {
            serializer = new Serializer<RescheduleRequest>();
            rescheduleRequests = serializer.FromCSV(FilePath);
        }
        public List<RescheduleRequest> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public RescheduleRequest Save(RescheduleRequest rescheduleRequest)
        {
            rescheduleRequest.Id = NextId();
            rescheduleRequests = serializer.FromCSV(FilePath);
            rescheduleRequests.Add(rescheduleRequest);
            serializer.ToCSV(FilePath, rescheduleRequests);
            return rescheduleRequest;
        }

        public int NextId()
        {
            rescheduleRequests = serializer.FromCSV(FilePath);
            if (rescheduleRequests.Count < 1)
            {
                return 1;
            }
            return rescheduleRequests.Max(c => c.Id) + 1;
        }

        public void Delete(int id)
        {
            rescheduleRequests = serializer.FromCSV(FilePath);
            var found = rescheduleRequests.Find(c => c.Id == id);
            rescheduleRequests.Remove(found);
            serializer.ToCSV(FilePath, rescheduleRequests);
        }

        public RescheduleRequest Update(RescheduleRequest rescheduleRequest)
        {
            rescheduleRequests = serializer.FromCSV(FilePath);
            var current = rescheduleRequests.Find(c => c.Id == rescheduleRequest.Id);
            var index = rescheduleRequests.IndexOf(current);
            rescheduleRequests.Remove(current);
            rescheduleRequests.Remove(current);
            rescheduleRequests.Insert(index, rescheduleRequest);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, rescheduleRequests);
            return rescheduleRequest;
        }
        public RescheduleRequest GetById(int id)
        {
            return rescheduleRequests.Find(c => c.Id == id);
        }

        public List<RescheduleRequest> GetByStatus(RequestStatus status)
        {
            return rescheduleRequests.FindAll(c => c.Status == status);
        }


    }
}
