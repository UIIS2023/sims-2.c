using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.DTO;


namespace Tourist_Project.Applications.UseCases
{

    public class TourReviewService
    {
        private static readonly Injector injector = new();

        private readonly ITourReviewRepository repository = injector.CreateInstance<ITourReviewRepository>();
        private readonly IUserRepository userRepository = injector.CreateInstance<IUserRepository>();
        private readonly ITourRepository tourRepository = injector.CreateInstance<ITourRepository>();
        private readonly TourPointService tourPointService = new();
        public TourReviewService()
        {

        }

        public TourReview Update(TourReview tourReview)
        {
            return repository.Update(tourReview);
        }

        public TourReview GetOne(int id)
        {
            return repository.GetOne(id);
        }

        public List<TourReview> GetAllByTourId(int tourId)
        {
            return repository.GetAllByTourId(tourId);
        }

        public void Save(TourReview tourReview)
        {
            repository.Save(tourReview);
        }
        
        public List<TourReviewDTO> GetAllReviewDtos(int tourId)
        {
            List<TourReviewDTO> dtos = new();
            foreach (var review in GetAllByTourId(tourId))
            {

                var dto = new TourReviewDTO(review.Id, userRepository.GetFullName(review.UserId),review.KnowledgeRating, review.LanguageRating, review.EntertainmentRating, review.Comment, tourPointService.GetCheckpointName(review.UserId, tourId), review.Valid);
                dtos.Add(dto);
            }

            return dtos;
        }

        public bool DidUserReview(int userId, int tourId)
        {
            return repository.GetAll().Find(tr => tr.UserId == userId && tr.TourId == tourId) != null;
        }

        public List<string> GetSuperLanguages(int guideId)
        {
            var superLanguages = new List<string>();

            foreach (var language in tourRepository.GetTourLanguages())
            {
                if (GetLanguageRating(guideId, language) >= 9.0 && tourRepository.GetEndedToursThisYear(guideId).FindAll(t => t.Language == language).Count >= 20)
                {
                    superLanguages.Add(language);
                }
            }

            return superLanguages;
        }

        private double GetAverageRating(int tourId)
        {
            var ratingSum = 0;
            var ratingCounter = 0;

            foreach (var review in repository.GetAllByTourId(tourId))
            {
                var averageReviewRating = (review.EntertainmentRating + review.KnowledgeRating + review.LanguageRating) / 3;
                ratingSum += averageReviewRating;
                ratingCounter++;
            }

            if (ratingCounter > 0)
            {
                return ratingSum/ratingCounter;
            }

            return 0.0;
        }

        private double GetLanguageRating(int guideId, string language)
        {
            var ratingSum = 0.0;
            var tourCounter = 0;

            foreach (var tour in tourRepository.GetEndedToursThisYear(guideId).FindAll(t => t.Language == language))
            {
                if(GetAverageRating(tour.Id) == 0.0) continue;
                ratingSum += GetAverageRating(tour.Id);
                tourCounter++;
            }

            return ratingSum / tourCounter;
        }

        public void UndoLatestReview(int userId, int tourId)
        {
            var reviewId = repository.GetByUserAndTour(userId, tourId);
            repository.Delete(reviewId);
        }
    }
}
