using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{

    public class NotificationRepository : INotificationRepository
    {

        private const string filePath = "../../../Data/notifications.csv";
        private readonly Serializer<Notification> serializer;
        public List<Notification> notifications;
        public NotificationRepository()
        {
            serializer = new Serializer<Notification>();
            notifications = serializer.FromCSV(filePath);
        }

        public List<Notification> GetAll()
        {
            return serializer.FromCSV(filePath);
        }

        public int NextId()
        {
            notifications = serializer.FromCSV(filePath);
            if (notifications.Count < 1)
            {
                return 1;
            }
            return notifications.Max(c => c.Id) + 1;
        }

        public List<Notification> GetAllByType(string type)
        {
            return notifications.Where(notification => notification.Type.Equals(type)).ToList();
        }

        public Notification GetById(int id)
        {
            return notifications.Find(x => x.Id == id);
        }

        public Notification Save(Notification notification)
        {
            notification.Id = NextId();
            notifications = serializer.FromCSV(filePath);
            notifications.Add(notification);
            serializer.ToCSV(filePath, notifications);
            return notification;
        }

        public Notification Update(Notification notification)
        {
            notifications = serializer.FromCSV(filePath);
            var current = notifications.Find(c => c.Id == notification.Id);
            var index = notifications.IndexOf(current);
            notifications.Remove(current);
            notifications.Insert(index, notification);       // keep ascending order of ids in file 
            serializer.ToCSV(filePath, notifications);
            return notification;
        }

        public void Delete(int id)
        {
            notifications = serializer.FromCSV(filePath);
            var founded = notifications.Find(c => c.Id == id);
            notifications.Remove(founded);
            serializer.ToCSV(filePath, notifications);
        }
    }

}