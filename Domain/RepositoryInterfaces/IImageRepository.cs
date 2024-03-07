using System.Collections.Generic;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.Domain.RepositoryInterfaces
{
    public interface IImageRepository
    {
        public List<Image> GetAll();
        public Image Save(Image image);
        public int NextId();
        public void Delete(int id);
        public Image Update(Image image);
        public Image GetById(int id);
        public Image? GetByUrl(string url);
        public List<Image> GetByAssociationAndId(ImageAssociation association, int associationId);
        public List<Image> GetAllByTourReview(int reviewId);
    }

}