using System.Collections.Generic;
using System.Linq;
using Tourist_Project.Applications.UseCases;
using Tourist_Project.Domain.Models;

public class AccommodationStatisticsService
{
    private readonly ReservationService reservationService = new();
    private readonly RenovationRecommendationService renovationRecommendationService = new();
    private readonly RescheduleRequestService rescheduleRequestService = new();
	public AccommodationStatisticsService()
	{
	}

    public int GetTotalReservation(int accommodationId, int year, int month)
    {
        if(month == 0)
            return reservationService.GetAllByAccommodation(accommodationId).Count(reservation => reservation.CheckIn.Year == year);
        else
            return reservationService.GetAllByAccommodation(accommodationId).Count(reservation => reservation.CheckIn.Year == year && reservation.CheckIn.Month == month);

    }

    public int GetTotalCancelledReservations(int accommodationId, int year, int month)
    {
        if(month == 0)
            return reservationService.GetAllByAccommodation(accommodationId).Count(reservation => reservation.Status == ReservationStatus.Cancelled && reservation.CheckIn.Year == year);
        else
            return reservationService.GetAllByAccommodation(accommodationId).Count(reservation => reservation.Status == ReservationStatus.Cancelled && reservation.CheckIn.Year == year && reservation.CheckIn.Month == month);

    }

    public int GetTotalRescheduledReservations(int accommodationId, int year, int month)
    {
        if(month == 0)
            return (from reservation in reservationService.GetAllByAccommodation(accommodationId) from rescheduleRequest in rescheduleRequestService.GetAll() where reservation.Id == rescheduleRequest.ReservationId && reservation.CheckIn.Year == year select reservation).Count();
        else
            return (from reservation in reservationService.GetAllByAccommodation(accommodationId) from rescheduleRequest in rescheduleRequestService.GetAll() where reservation.Id == rescheduleRequest.ReservationId && reservation.CheckIn.Year == year && reservation.CheckIn.Month == month select reservation).Count();

    }

    public int GetTotalRenovationRecommendations(int accommodationId, int year, int month)
    {
        if(month == 0) 
            return (from reservation in reservationService.GetAllByAccommodation(accommodationId) from recommendation in renovationRecommendationService.GetAll() where reservation.Id == recommendation.ReservationId && reservation.CheckIn.Year == year select reservation).Count();
        else
            return (from reservation in reservationService.GetAllByAccommodation(accommodationId) from recommendation in renovationRecommendationService.GetAll() where reservation.Id == recommendation.ReservationId && reservation.CheckIn.Year == year && reservation.CheckIn.Month == month select reservation).Count();
            
    }

    public List<int> GetYears(int accommodationId)
    {
        List<int> years = new();
        foreach (var reservation in reservationService.GetAllByAccommodation(accommodationId))
        {
            if (!years.Contains(reservation.CheckIn.Year))
                years.Add(reservation.CheckIn.Year);
        }

        return years;
    }

    public int GetOccupancy(int accommodationId, int year, int month)
    {
        if(month == 0)
            return reservationService.GetAllByAccommodation(accommodationId).Where(reservation => reservation.CheckIn.Year == year).Sum(reservation => (reservation.CheckOut - reservation.CheckIn).Days);
        else
            return reservationService.GetAllByAccommodation(accommodationId).Where(reservation => reservation.CheckIn.Year == year && reservation.CheckIn.Month == month).Sum(reservation => (reservation.CheckOut - reservation.CheckIn).Days);

    }
    
    public int GetMostOccupiedYear(int accommodationId)
    {
        var mostOccupiedYear = 0;
        foreach (var year in GetYears(accommodationId).Where(year => mostOccupiedYear < GetOccupancy(accommodationId, year, 0)))
        {
            mostOccupiedYear = year;
        }

        return mostOccupiedYear;
    }
    public int GetMostOccupiedMonth(int accommodationId, int year)
    {
        var mostOccupiedMonth = 0;
        for (var i = 1; i < 13; i++)
        {
            if (mostOccupiedMonth < GetOccupancy(accommodationId, year, i))
                mostOccupiedMonth = i;
        }

        return mostOccupiedMonth;
    }
}
