using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Mathematics.Interop;
using Tourist_Project.Domain.Models;
using Tourist_Project.Domain.RepositoryInterfaces;
using System.Globalization;
using Tourist_Project.WPF.Converters;

namespace Tourist_Project.Applications.UseCases
{
    public class TourRequestService
    {
        private static readonly Injector injector = new();
        private readonly ITourRequestRepository requestRepository = injector.CreateInstance<ITourRequestRepository>();

        private readonly TourService tourService = new();
        private readonly LocationService locationService = new();

        private readonly MonthConverter monthConverter = new();

        public TourRequestService()
        {
        }

        public List<TourRequest> GetAll()
        {
            return requestRepository.GetAll();
        }

        public TourRequest Save(TourRequest tourRequest)
        {
            return requestRepository.Save(tourRequest);
        }

        public TourRequest Update(TourRequest tourRequest)
        {
            return requestRepository.Update(tourRequest);
        }

        public void Delete(int id)
        {
            requestRepository.Delete(id);
        }

        public TourRequest GetById(int id)
        {
            return requestRepository.GetById(id);
        }

        public List<TourRequest> GetAllForUser(int userId)
        {
            return requestRepository.GetAllForUser(userId);
        }

        public List<TourRequest> GetAllForComplexTour(int complexTourId)
        {
            var retVal = requestRepository.GetAllForComplexTour(complexTourId);
            foreach (var request in retVal)
            {
                request.Location = locationService.Get(request.LocationId);
            }
            return retVal;
        }

        public void UpdateInvalidRequests(int loggedUserId)
        {
            var requests = requestRepository.GetAllPendingForUser(loggedUserId);
            foreach (var request in requests)
            {
                if (DateTime.Now < request.UntilDate.AddDays(-2).Date) continue;

                request.Status = TourRequestStatus.Denied;
                requestRepository.Update(request);
            }
        }

        public double GetAcceptedPercentage(int userId, string statYear)
        {
            var requests = requestRepository.GetAllForUser(userId);
            double acceptedCount = 0;

            if (statYear == "All time")
            {
                foreach (var request in requests)
                {
                    if (request.Status == TourRequestStatus.Accepted)
                    {
                        acceptedCount++;
                    }
                }
                return acceptedCount > 0 ? acceptedCount / requests.Count * 100 : 0;
            }

            double selectedYearCount = 0;
            foreach (var request in requests.Where(r =>  r.UntilDate.Year == int.Parse(statYear) && r.ComplexTourId == -1))
            {
                selectedYearCount++;
                if (request.Status == TourRequestStatus.Accepted)
                {
                    acceptedCount++;
                }
            }
            return acceptedCount > 0 ? acceptedCount / selectedYearCount * 100 : 0;

        }

        public double GetDeniedPercentage(int userId, string statYear)
        {
            var requests = requestRepository.GetAllForUser(userId);
            double deniedCount = 0;

            if (statYear == "All time")
            {
                foreach (var request in requests)
                {
                    if (request.Status == TourRequestStatus.Denied)
                    {
                        deniedCount++;
                    }
                }
                return deniedCount > 0 ? deniedCount / requests.Count * 100 : 0;
            }

            double selectedYearCount = 0;
            foreach (var request in requests.Where(r => r.UntilDate.Year == int.Parse(statYear) && r.ComplexTourId == -1))
            {
                selectedYearCount++;
                if (request.Status == TourRequestStatus.Denied)
                {
                    deniedCount++;
                }
            }
            return deniedCount > 0 ? deniedCount / selectedYearCount * 100 : 0;

        }

        public double GetAverageGuests(int userId, string statYear)
        {
            var requests = requestRepository.GetAllForUser(userId).Where(r => r.Status == TourRequestStatus.Accepted).ToList();
            double guestsCount = 0;

            if (statYear == "All time")
            {
                foreach (var request in requests)
                {
                    guestsCount += request.GuestsNumber;
                }

                return requests.Count > 0 ? guestsCount / requests.Count : 0;
            }

            double selectedYearGuestsCount = 0;
            foreach (var request in requests.Where(r => r.FromDate.Year == int.Parse(statYear)))
            {
                selectedYearGuestsCount++;
                guestsCount += request.GuestsNumber;
            } 
            return selectedYearGuestsCount > 0 ? guestsCount / selectedYearGuestsCount : 0;

        }

        public List<string> GetAllRequestedLanguages(int userId)
        {
            return requestRepository.GetAllRequestedLanguages(userId);
        }

        public int GetLanguageRequestCount(int userId, string language)
        {
            var count = 0;
            foreach (var request in requestRepository.GetAllForUser(userId))
            {
                if (request.Language == language)
                {
                    count++;
                }
            }

            return count;
        }

        public SeriesCollection GetLanguageSeriesCollection(int userId)
        {
            var retVal = new SeriesCollection();

            foreach (var requestedLanguage in GetAllRequestedLanguages(userId))
            {
                retVal.Add(new PieSeries { Title = requestedLanguage, Values = new ChartValues<ObservableValue> { new(GetLanguageRequestCount(userId, requestedLanguage)) } });
            }

            return retVal;
        }

        public List<Location> GetAllRequestedLocations(int userId)
        {
            var retVal = new List<Location>();
            foreach (var locationId in requestRepository.GetAllRequestedLocations(userId))
            {
                retVal.Add(locationService.Get(locationId));
            }

            return retVal;
        }

        public int GetLocationRequestCount(int userId, int locationId)
        {
            var count = 0;
            foreach (var request in GetAllForUser(userId))
            {
                if (request.LocationId == locationId)
                {
                    count++;
                }
            }

            return count;
        }

        public SeriesCollection GetLocationSeriesCollection(int userId)
        {
            var retVal = new SeriesCollection();

            foreach (var location in GetAllRequestedLocations(userId))
            {
                retVal.Add(new PieSeries {Title = location.ToString(), Values = new ChartValues<ObservableValue> {new(GetLocationRequestCount(userId, location.Id))}});
            }

            return retVal;
        }
        public List<TourRequest> GetAllPending()
        {
            return requestRepository.GetAllPending();
        }

        public bool IsAlreadyBooked(DateTime bookingDateTime, int duration)
        {
            foreach (var date in tourService.GetBookedHours())
            {
                if (CheckTourOverlap(bookingDateTime, duration, date))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckTourOverlap(DateTime bookingDateTime, int duration, DateSpan date)
        {
            return bookingDateTime.CompareTo(date.StartingDate) <= 0 &&
                   bookingDateTime.AddHours(duration).CompareTo(date.EndingDate) >= 0 ||
                   date.StartingDate.CompareTo(bookingDateTime.AddHours(duration)) <= 0 &&
                   date.EndingDate.CompareTo(bookingDateTime) >= 0;
        }

        public List<TourRequest> SearchRequests(string filter, string searchText)
        {
            switch (filter)
            {
                case "Language":
                    return GetAllPending().FindAll(request => request.Language == searchText);
                case "Location":
                    {
                        var result = searchText.Split(",");
                        var city = result[0];
                        var country = result[1];
                        var locationId = locationService.GetId(city, country);
                        return GetAllPending().FindAll(request => request.LocationId == locationId);
                    }
                default:
                    {
                        var result = searchText.Split("-");
                        var fromDate = DateTime.Parse(result[0]);
                        var untilDate = DateTime.Parse(result[1]);
                        return GetAllPending().FindAll(request =>
                            request.FromDate.CompareTo(fromDate) >= 0 && request.UntilDate.CompareTo(untilDate) <= 0);
                    }
            }
        }

        public Location FindMostRequestedLocation()
        {
            var mostRequested = 0;
            var maxRequests = 0;
            var requestsNumber = 0;
            foreach (var location in locationService.GetAll())
            {
                requestsNumber += requestRepository.GetAllLastYear().Count(request => request.LocationId == location.Id);

                if (requestsNumber > maxRequests)
                {
                    mostRequested = location.Id;
                    maxRequests = requestsNumber;
                }

                requestsNumber = 0;
            }

            return locationService.Get(mostRequested);
        }

        public string FindMOstRequestedLanguage()
        {
            var mostRequested = 0;
            var maxRequests = 0;
            var language = "";
            var requestsNumber = 0;
            foreach (var request in requestRepository.GetAll())
            {
                requestsNumber += requestRepository.GetAllLastYear().Count(tourRequest => tourRequest.Language == request.Language);

                if (requestsNumber > maxRequests)
                {
                    language = request.Language;
                    maxRequests = requestsNumber;
                }

                requestsNumber = 0;
            }
            return language;
        }

        public List<RequestStatistics> GetRequestStatistics(string filter, string search, string selectedYear)
        {
            return filter == "Location" ? GetStatisticsForLocation(search, selectedYear) : GetStatisticsForLanguage(search, selectedYear);
        }

        private List<RequestStatistics> GetStatisticsForLocation(string search, string selectedYear)
        {
            var location = search.Split(",");
            var city = location[0];
            var country = location[1];
            var locationId = locationService.GetId(city, country);
            var requestCounter = 0;

            List<RequestStatistics> statistics = new();

            statistics = selectedYear == "Overall" ? GetStatisticsForYearsByLocation(locationId) : GetStatisticsForMonthsByLocation(selectedYear, locationId);

            return statistics;
        }

        private List<RequestStatistics> GetStatisticsForMonthsByLocation(string selectedYear, int locationId)
        {
            var requestCounter = 0;
            List<RequestStatistics> statistics = new();
            var year = int.Parse(selectedYear);

            for (var i = 1; i <= 12; i++)
            {
                foreach (var request in requestRepository.GetAll()
                             .Where(request => request.CreateDate.Year == year && request.LocationId == locationId))
                {
                    if (request.CreateDate.Month == i)
                    {
                        requestCounter++;
                    }
                }

                statistics.Add(new(monthConverter.Convert(i.ToString(), typeof(string), null, CultureInfo.CurrentCulture) as string, requestCounter));
                requestCounter = 0;
            }

            return statistics;
        }

        private List<RequestStatistics> GetStatisticsForYearsByLocation(int locationId1)
        {

            var requestCounter = 0;
            List<RequestStatistics> statistics = new();


            for (var i = DateTime.Now; i >= DateTime.Now.AddYears(-4); i = i.AddYears(-1))
            {
                requestCounter += requestRepository.GetAll()
                    .Count(request => request.CreateDate.Year == i.Year && request.LocationId == locationId1);
                statistics.Add(new(i.Year.ToString(), requestCounter));

                requestCounter = 0;
            }

            return statistics;
        }

        private List<RequestStatistics> GetStatisticsForLanguage(string search, string selectedYear)
        {
            return  selectedYear == "Overall" ? GetStatisticsForYearsByLanguage(search) : GetStatisticsForMonthsByLanguage(selectedYear, search);
        }

        private List<RequestStatistics> GetStatisticsForMonthsByLanguage(string selectedYear, string search)
        {
            var requestCounter = 0;
            var statistics = new List<RequestStatistics>();

            var year = int.Parse(selectedYear);
            for (var i = 1; i <= 12; i++)
            {
                foreach (var request in requestRepository.GetAll().Where(request => request.CreateDate.Year == year && request.Language == search))
                {
                    if (request.CreateDate.Month == i)
                    {
                        requestCounter++;

                    }
                }
                statistics.Add(new(monthConverter.Convert(i.ToString(), typeof(string), null, CultureInfo.CurrentCulture) as string, requestCounter));
                requestCounter = 0;
            }

            return statistics;
        }

        private List<RequestStatistics> GetStatisticsForYearsByLanguage(string search)
        {
            var requestCounter = 0;
            List<RequestStatistics> statistics = new();

            for (var i = DateTime.Now; i >= DateTime.Now.AddYears(-4); i = i.AddYears(-1))
            {
                requestCounter += requestRepository.GetAll().Count(request => request.Language == search && request.CreateDate.Year == i.Year);
                statistics.Add(new(i.Year.ToString(), requestCounter));
                requestCounter = 0;
            }

            return statistics;
        }

        public List<TourRequest> GetForSelectedYear(int userId, string year)
        {
            return requestRepository.GetForSelectedYear(userId, year);
        }

        public void UndoLatestRequest(int loggedUserId)
        {
            var usersLatestRequestId = requestRepository.GetUsersLatestRequestId(loggedUserId);
            requestRepository.Delete(usersLatestRequestId);
        }

        public SeriesCollection GetLocationOverallCollection(string search)
        {
            var retVal = new SeriesCollection();

            var location = search.Split(",");
            var city = location[0];
            var country = location[1];
            var locationId = locationService.GetId(city, country);

            foreach (var request in GetStatisticsForYearsByLocation(locationId))
            {
                if(request.Statistics == 0) continue;

                retVal.Add(new PieSeries { Title = request.Time, Values = new ChartValues<int> { request.Statistics } });
            }
            
            return retVal;
        }

        public SeriesCollection GenerateLocationYearCollection(string search, string selectedYear)
        {
            var retVal = new SeriesCollection();

            var location = search.Split(",");
            var city = location[0];
            var country = location[1];
            var locationId = locationService.GetId(city, country);

            foreach (var request in GetStatisticsForMonthsByLocation(selectedYear, locationId))
            {
                if(request.Statistics == 0) continue;

                retVal.Add(new PieSeries { Title = request.Time, Values = new ChartValues<int> { request.Statistics } });
            }

            return retVal;
        }

        public SeriesCollection GenerateLanguageOverallCollection(string search)
        {
            var retVal = new SeriesCollection();

            foreach (var request in GetStatisticsForYearsByLanguage(search))
            {
                if(request.Statistics == 0) continue;

                retVal.Add(new PieSeries { Title = request.Time, Values = new ChartValues<int> { request.Statistics } });
            }

            return retVal;
        }

        public SeriesCollection GenerateLanguageYearCollection(string search, string selectedYear)
        {
            var retVal = new SeriesCollection();

            foreach (var request in GetStatisticsForMonthsByLanguage(selectedYear, search))
            {
                if(request.Statistics == 0) continue;

                retVal.Add(new PieSeries { Title = request.Time, Values = new ChartValues<int> { request.Statistics } });   
            }

            return retVal;
        }

        public List<string> GetAllYears(int userId)
        {
            return requestRepository.GetAllYears(userId);
        }
    }
}
