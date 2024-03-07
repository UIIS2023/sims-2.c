using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.Serializer;

namespace Tourist_Project.Repositories
{
    public class RenovationRecommendationRepository : IRenovationRecommendationRepository
    {
        private const string FilePath = "../../../Data/renovationRecommendation.csv";
        private readonly Serializer<RenovationRecommendation> serializer = new();
        private List<RenovationRecommendation> recommendations;

        public RenovationRecommendationRepository()
        {
            recommendations = serializer.FromCSV(FilePath);
        }
        public List<RenovationRecommendation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public RenovationRecommendation Save(RenovationRecommendation renovation)
        {
            renovation.Id = NextId();
            recommendations = serializer.FromCSV(FilePath);
            recommendations.Add(renovation);
            serializer.ToCSV(FilePath, recommendations);
            return renovation;
        }

        public int NextId()
        {
            recommendations = serializer.FromCSV(FilePath);
            if (recommendations.Count < 1)
            {
                return 1;
            }
            return recommendations.Max(c => c.Id) + 1;
        }

        public void Delete(int id)
        {
            recommendations = serializer.FromCSV(FilePath);
            var founded = recommendations.Find(c => c.Id == id);
            recommendations.Remove(founded);
            serializer.ToCSV(FilePath, recommendations);
        }

        public RenovationRecommendation Update(RenovationRecommendation recommendation)
        {
            recommendations = serializer.FromCSV(FilePath);
            var current = recommendations.Find(c => c.Id == recommendation.Id);
            var index = recommendations.IndexOf(current);
            recommendations.Remove(current);
            recommendations.Insert(index, recommendation);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, recommendations);
            return recommendation;
        }

        public RenovationRecommendation GetById(int id)
        {
            return recommendations.Find(c => c.Id == id);
        }
    } 
}
