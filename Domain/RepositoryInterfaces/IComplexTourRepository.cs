using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface IComplexTourRepository
    {
        List<ComplexTour> GetAll();
        List<ComplexTour> GetAllForUser(int userId);
        ComplexTour Save(ComplexTour complexTour);
        ComplexTour Update(ComplexTour complexTour);
        void Delete(int complexTourId);
        int NextId();
        List<ComplexTour> GetAllPendingForUser(int userId);
        int GetUsersLatestRequestId(int loggedUserId);
    }
}
