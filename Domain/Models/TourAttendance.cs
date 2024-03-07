using System;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum Presence
    {
        No, Joined, Pending, Yes
    }
    public class TourAttendance : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public int CheckPointId { get; set; }
        public Presence Presence { get; set; }
        public User User { get; set; }
        public TourPoint TourPoint { get; set; }

        public TourAttendance()
        {
            TourPoint = new TourPoint();
            CheckPointId = -1;
        }

        public TourAttendance(int userId, int tourId)
        {
            this.UserId = userId;
            TourId = tourId;
            Presence = Presence.No;
            CheckPointId = -1;
            TourPoint = new TourPoint();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
                        {
                Id.ToString(),
                UserId.ToString(),
                TourId.ToString(),
                CheckPointId.ToString(),
                Presence.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            TourId = int.Parse(values[2]);
            CheckPointId = int.Parse(values[3]);
            Presence = Enum.Parse<Presence>(values[4]);
        }
    }
}
