using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public class TourVoucher : ISerializable
    {
        public int Id { get; set; }
        public int TouristId { get; set; }
        public int TourId { get; set; }
        public DateTime ExpireDate { get; set; }
        public string ExpireDateStr => ExpireDate.ToShortDateString();
        public int GuideId { get; set; }
        public string Name { get; set; }
        public string WayAcquired { get; set; }

        public TourVoucher() 
        {
            Name = "Without voucher";
        }

        public TourVoucher(int touristsId, int tourId, string wayAcquired, int guideId = -1)
        {
            TouristId = touristsId;
            TourId = tourId;
            ExpireDate = DateTime.Now.AddYears(1).Date;
            GuideId = guideId;
            WayAcquired = wayAcquired;

            var random = new Random();
            Name = random.Next(3) switch
            {
                0 => "Discount 15e",
                1 => "Free tour",
                _ => "Discount 30%",
            };
        }

        public TourVoucher(int touristsId, string wayAcquired, DateTime expireDate, int guideId = -1, int tourId = -1)
        {
            TouristId = touristsId;
            TourId = tourId;
            ExpireDate = expireDate;
            GuideId = guideId;
            WayAcquired = wayAcquired;

            var random = new Random();
            Name = random.Next(3) switch
            {
                0 => "Discount 15e",
                1 => "Free tour",
                _ => "Discount 30%",
            };
        }

        public string[] ToCSV()
        {
            string[] cssValues =
            {
                Id.ToString(),
                TouristId.ToString(),
                TourId.ToString(),
                ExpireDate.ToString(),
                GuideId.ToString(),
                Name,
                WayAcquired
            };

            return cssValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TouristId = int.Parse(values[1]);
            TourId = int.Parse(values[2]);
            ExpireDate = DateTime.Parse(values[3]);
            GuideId = int.Parse(values[4]);
            Name = values[5];
            WayAcquired = values[6];
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
