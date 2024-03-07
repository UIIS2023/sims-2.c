using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum ComplexTourStatus
    {
        Pending,
        Accepted,
        Denied
    };
    public class ComplexTour : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ComplexTourStatus Status { get; set; }
        public ComplexTour() { }

        public ComplexTour(int complexTourId, int loggedUserId)
        {
            Id = complexTourId;
            UserId = loggedUserId;
            Status = ComplexTourStatus.Pending;
        }

        public string[] ToCSV()
        {
            string[] retVal = { Id.ToString(), UserId.ToString(), Status.ToString() };
            return retVal;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            Status = Enum.Parse<ComplexTourStatus>(values[2]);
        }
    }
}
