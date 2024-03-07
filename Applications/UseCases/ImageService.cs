using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;

namespace Tourist_Project.Applications.UseCases
{
    public class ImageService
    {
        private static readonly Injector injector = new();

        private readonly IImageRepository imageRepository =
            injector.CreateInstance<IImageRepository>();
        public ImageService()
        {
        }

        public Image Create(Image image)
        {
            return imageRepository.Save(image);
        }

        public List<Image> GetAll()
        {
            return imageRepository.GetAll();
        }

        public Image Get(int id)
        {
            return imageRepository.GetById(id);
        }

        public Image? GetByUrl(string url)
        {
            return imageRepository.GetByUrl(url);
        }

        public Image Update(Image image)
        {
            return imageRepository.Update(image);
        }

        public void Delete(int id)
        {
            imageRepository.Delete(id);
        }

        public void Save(Image image)
        {
            imageRepository.Save(image);
        }

        public List<int> CreateImages(string url)
        {
            var ids = new List<int>();
            var urls = url.Split(",");
            foreach (var imageUrl in urls)
            {
                if (imageRepository.GetByUrl(imageUrl) != null)
                {
                    ids.Add(imageRepository.GetByUrl(imageUrl).Id);
                }
                else
                {
                    Image newImage = new(imageUrl);
                    var savedImage = Create(newImage);
                    ids.Add(savedImage.Id);
                } 
            }
            return ids;
        }
        public string? FormIdesString(string url)
        {
            var ids = CreateImages(url);
            if (ids.Count <= 0) return null;
            var ides = ids.Aggregate(string.Empty, (current, imageId) => current + (imageId + ","));
            ides = ides.Remove(ides.Length - 1);
            return ides;
        }

        public List<Image> GetAllByTourReview(int reviewId)
        {
            return imageRepository.GetAllByTourReview(reviewId);
        }
    }

}