using System.Collections.Generic;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

public class RenovationRecommendationService
{
    private static readonly Injector injector = new();
    private readonly IRenovationRecommendationRepository renovationRecommendationRepository = injector.CreateInstance<IRenovationRecommendationRepository>();

    public RenovationRecommendationService()
    {
    }

    public RenovationRecommendation Create(RenovationRecommendation renovationRecommendation)
    {
        return renovationRecommendationRepository.Save(renovationRecommendation);
    }

    public List<RenovationRecommendation> GetAll()
    {
        return renovationRecommendationRepository.GetAll();
    }

    public RenovationRecommendation Get(int id)
    {
        return renovationRecommendationRepository.GetById(id);
    }

    public void Delete(int id)
    {
        renovationRecommendationRepository.Delete(id);
    }

    public RenovationRecommendation Update(RenovationRecommendation renovationRecommendation)
    {
        return renovationRecommendationRepository.Update(renovationRecommendation);
    }
}
