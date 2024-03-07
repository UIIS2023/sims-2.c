using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum TourRequestStatus
    {
        Pending,
        Accepted,
        Denied
    };
    public class TourRequest : ISerializable
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public int GuestsNumber { get; set; }
        public DateTime FromDate { get; set; }
        public string FromDateStr => FromDate.ToShortDateString();
        public DateTime UntilDate { get; set; }
        public string UntilDateStr => UntilDate.ToShortDateString();
        public TourRequestStatus Status { get; set; } = TourRequestStatus.Pending;
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int ComplexTourId { get; set; }


        public TourRequest()
        {
            
        }

        public TourRequest(int locationId, string description, string language, int guestsNumber, DateTime fromDate, DateTime untilDate, int userId, DateTime createDate)
        {
            LocationId = locationId;
            Description = description;
            Language = language;
            GuestsNumber = guestsNumber;
            FromDate = fromDate;
            UntilDate = untilDate;
            UserId = userId;
            CreateDate = createDate;
            ComplexTourId = -1;
        }

        public TourRequest(int locationId, string description, string language, int guestsNumber, DateTime fromDate, DateTime untilDate, int userId, DateTime createDate, int complexTourId)
        {
            LocationId = locationId;
            Description = description;
            Language = language;
            GuestsNumber = guestsNumber;
            FromDate = fromDate;
            UntilDate = untilDate;
            UserId = userId;
            CreateDate = createDate;
            ComplexTourId = complexTourId;
        }

        public string[] ToCSV()
        {
            return new[]
            {
                Id.ToString(), LocationId.ToString(), Description, Language, GuestsNumber.ToString(),
                FromDate.ToString(), UntilDate.ToString(), Status.ToString(), UserId.ToString(), CreateDate.ToString(), ComplexTourId.ToString()
            };
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            LocationId = int.Parse(values[1]);
            Description = values[2];
            Language = values[3];
            GuestsNumber = int.Parse(values[4]);
            FromDate = DateTime.Parse(values[5]);
            UntilDate = DateTime.Parse(values[6]);
            Status = Enum.Parse<TourRequestStatus>(values[7]);
            UserId = int.Parse(values[8]);
            CreateDate = DateTime.Parse(values[9]);
            ComplexTourId = int.Parse(values[10]);
        }
    }
}
