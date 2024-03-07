using System.Collections.Generic;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface IRescheduleRequestRepository
    {
        //treba da cuvam requestove u fajlu (cuvam requestId, stare i nove datume i reservationId), ICommand treba da mi otvori prozor koji vec imam i da mi na confirm dugme sacuva zahtev u fajl i to je to 
        public List<RescheduleRequest> GetAll();
        public RescheduleRequest Save(RescheduleRequest rescheduleRequest);
        public int NextId();
        public void Delete(int id);
        public RescheduleRequest Update(RescheduleRequest rescheduleRequest);
        public RescheduleRequest GetById(int id);
        public List<RescheduleRequest> GetByStatus(RequestStatus status);
    }
}
