using System.Collections.Generic;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface IRenovationRecommendationRepository
    {
        public List<RenovationRecommendation> GetAll();
        public RenovationRecommendation Save(RenovationRecommendation renovation);
        public int NextId();
        public void Delete(int id);
        public RenovationRecommendation Update(RenovationRecommendation recommendation);
        public RenovationRecommendation GetById(int id);
    } 
}
