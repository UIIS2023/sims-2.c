using System;
using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

namespace Tourist_Project.Applications.UseCases
{
    public class NotificationService
    {
        private static readonly Injector injector = new();

        private readonly INotificationRepository notificationRepository = injector.CreateInstance<INotificationRepository>();
        private readonly IAccommodationRatingRepository accommodationRatingRepository = injector.CreateInstance<IAccommodationRatingRepository>();
        private readonly IGuestRateRepository guestRateRepository = injector.CreateInstance<IGuestRateRepository>();
        private readonly IReservationRepository reservationRepository = injector.CreateInstance<IReservationRepository>();
        private readonly IForumRepository forumRepository = injector.CreateInstance<IForumRepository>();
        private readonly IAccommodationRepository accommodationRepository = injector.CreateInstance<IAccommodationRepository>();
        private readonly GuestRateService guestRateService = new();
        public NotificationService()
        {
            HasReviews();
            HasUnratedGuests();
            HasNewForum();
        }

        public Notification Create(Notification notification)
        {
            return notificationRepository.Save(notification);
        }

        public List<Notification> GetAll()
        {
            return notificationRepository.GetAll();
        }

        public List<Notification> GetAllByType(string type)
        {
            return notificationRepository.GetAllByType(type);
        }

        public Notification Get(int id)
        {
            return notificationRepository.GetById(id);
        }

        public Notification Update(Notification notification)
        {
            return notificationRepository.Update(notification);
        }

        public void Delete(int id)
        {
            notificationRepository.Delete(id);
        }

        public void HasReviews()
        {
            if (accommodationRatingRepository.GetAll().Count == 0) return;
            foreach (var accommodationRating in accommodationRatingRepository.GetAll())
            {
                if(GetAll().All(notification => notification.TypeId != accommodationRating.Id))
                    Create(new Notification("Reviews", false, accommodationRating.Id));
            }
        }
        public void HasUnratedGuests()
        {
            guestRateService.HasNewRatings();
            foreach (var guestRate in guestRateRepository.GetAll())
            {
                var reservation = reservationRepository.GetById(guestRate.ReservationId);
                var daysSinceCheckOut = DateTime.Now - reservation.CheckOut;
                if ((notificationRepository.GetAllByType("GuestRate").Count == 0 || notificationRepository.GetAllByType("GuestRate").All(c => c.TypeId != guestRate.Id)) && !guestRate.IsReviewed() && Math.Abs(daysSinceCheckOut.Days) < 5)
                {
                    Create(new Notification("GuestRate", true, guestRate.Id));
                }
                foreach (var notification in GetAllByType("GuestRate"))
                {
                    if (notificationRepository.GetAllByType("GuestRate").Any(c => c.TypeId == guestRate.Id) && Math.Abs(daysSinceCheckOut.Days) >= 5)
                        Delete(notification.Id);
                }
            }

        }

        public void HasNewForum()
        {
            if(forumRepository.GetAll().Count == 0) return;
            foreach (var forum in forumRepository.GetAll())
            {
                if ((notificationRepository.GetAllByType("Forum").Count == 0 || notificationRepository.GetAllByType("Forum").All(c => c.TypeId != forum.Id)) && accommodationRepository.GetLocationIds(App.LoggedInUser.Id).Contains(forum.LocationId))
                    Create(new Notification("Forum", false, forum.Id));
                
                /*
                if (GetAll().All(notification =>  notification.TypeId != forum.Id && accommodationRepository.GetLocationIds(App.LoggedInUser.Id).Contains(forum.LocationId)))
                */
            }
        }
    }

}