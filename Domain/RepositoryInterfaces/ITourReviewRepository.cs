using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;
using Tourist_Project.WPF.Views;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface ITourReviewRepository
    {
        public void Save(TourReview tourReview);
        public TourReview Update(TourReview tourReview);
        public int NextId();
        public TourReview GetOne(int id);
        public List<TourReview> GetAllByTourId(int id);
        public List<TourReview> GetAll();
        int GetByUserAndTour(int userId, int tourId);
        void Delete(int reviewId);
    }
}
