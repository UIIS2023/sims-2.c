using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Tourist_Project.Serializer;

namespace Tourist_Project.Domain.Models
{
    public enum ValidStatus
    {
        NotValid, Pending, Valid
    }
    public class TourReview : ISerializable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public int KnowledgeRating { get; set; }
        public int LanguageRating { get; set; }
        public int EntertainmentRating { get; set; }
        public string Comment { get; set; }
        public ValidStatus Valid { get; set; }

        public TourReview()
        {
            Comment = string.Empty;
        }

        public TourReview(int userId, int tourId, int knowledgeGrade, int languageGrade, int interestGrade, string comment)
        {
            UserId = userId;
            TourId = tourId;
            KnowledgeRating = knowledgeGrade;
            LanguageRating = languageGrade;
            EntertainmentRating = interestGrade;
            Comment = comment;
            Valid = ValidStatus.NotValid;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                UserId.ToString(),
                TourId.ToString(),
                KnowledgeRating.ToString(),
                LanguageRating.ToString(),
                EntertainmentRating.ToString(),
                Comment,
                Valid.ToString()
            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            TourId = int.Parse(values[2]);
            KnowledgeRating = int.Parse(values[3]);
            LanguageRating = int.Parse(values[4]);
            EntertainmentRating = int.Parse(values[5]);
            Comment = values[6];
            Valid = Enum.Parse<ValidStatus>(values[7]);
        }
    }
}
