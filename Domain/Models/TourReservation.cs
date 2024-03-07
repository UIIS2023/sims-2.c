using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public int GuestsNumber { get; set; }
        public bool Voucher { get; set; }

        public TourReservation()
        {
        }

        public TourReservation(int userId, int tourId, int guestsNumber, bool voucher)
        {
            UserId = userId;
            TourId = tourId;
            GuestsNumber = guestsNumber;
            Voucher = voucher;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                TourId.ToString(),
                GuestsNumber.ToString(),
                Voucher.ToString(),
            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            TourId = int.Parse(values[2]);
            GuestsNumber = int.Parse(values[3]);
            Voucher = bool.Parse(values[4]);
        }
    }
}
