using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.DTO
{
    public class TourReviewDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int KnowledgeGrade { get; set; }
        public int LanguageGrade { get; set; }
        public int InterestGrade { get; set; }
        public string Comment { get; set; }
        public string CheckpointName { get; set; }
        public ValidStatus Valid { get; set; }

        public TourReviewDTO()
        {

        }

        public TourReviewDTO(int id, string userName, int knowledgeGrade, int languageGrade, int interestGrade, string comment, string checkpointName, ValidStatus valid)
        {
            Id = id;
            UserName = userName;
            KnowledgeGrade = knowledgeGrade;
            LanguageGrade = languageGrade;
            InterestGrade = interestGrade;
            Comment = comment;
            CheckpointName = checkpointName;
            Valid = valid;
        }
    }
}
