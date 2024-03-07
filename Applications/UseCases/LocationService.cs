using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using Tourist_Project.WPF.ViewModels;
using Tourist_Project.WPF.ViewModels.Guide;

namespace Tourist_Project.Applications.UseCases
{
    public class LocationService
    {
        private static readonly Injector injector = new();

        private readonly ILocationRepository locationRepository =
            injector.CreateInstance<ILocationRepository>();

        public LocationService() 
        {
        }
        public List<Location> GetAll()
        {
            return locationRepository.GetAll();
        }

        public Location Get(int id)
        {
            return locationRepository.GetById(id);
        }

        public int GetId(string city, string country)
        {
            return locationRepository.GetId(city, country);
        }

        public void InitializeCitiesAndCountries()
        {
            foreach (var location in GetAll())
            {
                CreateTourViewModel.Cities.Add(location.City);
                if (!CreateTourViewModel.Countries.Contains(location.Country))
                    CreateTourViewModel.Countries.Add(location.Country);
            }
        }

        public List<string> GetAllCities()
        {
            List<string> cities = new();
            foreach (var location in GetAll())
            {
                cities.Add(location.City);
            }
            return cities;
        }

        public List<string> GetAllCountries()
        {
            List<string> countries = new();
            foreach (var location in GetAll())
            {
                if (!countries.Contains(location.Country))
                {
                    countries.Add(location.Country);
                }
            }
            return countries;
        }

        public Location GetLocation(Tour tour)
        {
            return GetAll().Find(x => x.Id == tour.LocationId);
        }

        public ObservableCollection<string> GetCitiesFromCountry(string country)
        {
            var cities = new ObservableCollection<string>();
            foreach(var location in GetAll())
            {
                if(location.Country == country)
                {
                    cities.Add(location.City);
                }
            }
            return cities;
        }

        public string GetCountryFromCity(string selectedCity)
        {
            string retVal = string.Empty;
            foreach(var location in GetAll())
            {
                if(location.City == selectedCity)
                {
                    retVal = location.Country;
                }
            }
            return retVal;
        }
    }
}
