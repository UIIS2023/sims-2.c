using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface INotificationGuestTwoRepository
    {
        public List<NotificationGuestTwo> GetAll();
        public int NextId();
        public NotificationGuestTwo Save(NotificationGuestTwo notification);
        public NotificationGuestTwo Update(NotificationGuestTwo notification);
        public void Delete(int id);
        List<NotificationGuestTwo> GetAllForUser(int userId);
    }
}
