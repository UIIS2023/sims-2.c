using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class NotificationGuestTwoRepository : INotificationGuestTwoRepository
    {
        private const string filePath = "../../../Data/notificationsGuestTwo.csv";
        private readonly Serializer<NotificationGuestTwo> serializer;
        public List<NotificationGuestTwo> notifications;

        public NotificationGuestTwoRepository()
        {
            serializer = new Serializer<NotificationGuestTwo>();
            notifications = serializer.FromCSV(filePath);
        }

        public void Delete(int id)
        {
            notifications = GetAll();
            var found = notifications.Find(x => x.Id == id);
            notifications.Remove(found);
            serializer.ToCSV(filePath, notifications);
        }

        public List<NotificationGuestTwo> GetAllForUser(int userId)
        {
            notifications = GetAll();
            return notifications.Where(notification => notification.UserId == userId).ToList();
        }

        public List<NotificationGuestTwo> GetAll()
        {
            return serializer.FromCSV(filePath);
        }

        public int NextId()
        {
            notifications = GetAll();
            if (notifications.Count < 1)
            {
                return 1;
            }
            return notifications.Max(c => c.Id) + 1;
        }

        public NotificationGuestTwo Save(NotificationGuestTwo notification)
        {
            notification.Id = NextId();
            notifications = serializer.FromCSV(filePath);
            notifications.Add(notification);
            serializer.ToCSV(filePath, notifications);
            return notification;
        }

        public NotificationGuestTwo Update(NotificationGuestTwo notification)
        {
            notifications = serializer.FromCSV(filePath);
            var current = notifications.Find(c => c.Id == notification.Id);
            var index = notifications.IndexOf(current);
            notifications.Remove(current);
            notifications.Insert(index, notification);
            serializer.ToCSV(filePath, notifications);
            return notification;
        }
    }
}
