using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.DTO;

namespace Tourist_Project.Repositories
{
    public class AccommodationDtoRepository
    {
        private static readonly Injector injector = new();

        private readonly IImageRepository imageRepository = injector.CreateInstance<IImageRepository>();
        private readonly ILocationRepository locationRepository = injector.CreateInstance<ILocationRepository>();
        private readonly IAccommodationRepository accommodationRepository = injector.CreateInstance<IAccommodationRepository>();
        private readonly List<AccommodationDTO> accommodationDtos = new();
        public List<AccommodationDTO> LoadAll()
        {
            accommodationDtos.Clear();
            foreach (var accommodation in accommodationRepository.GetAll())
            {
                foreach (var location in locationRepository.GetAll())
                {
                    foreach (var image in imageRepository.GetAll().Where(image => accommodation.LocationId == location.Id && accommodation.ImageId == image.Id))
                    {
                        accommodationDtos.Add(new AccommodationDTO(accommodation, location, image));
                    }
                }
            }
            return accommodationDtos;
        }
    }
}