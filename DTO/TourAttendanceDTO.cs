using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourist_Project.Domain.Models;

namespace Tourist_Project.DTO
{
    public class TourAttendanceDTO
    {
        public string UserName { get; set; }
        public string PointName { get; set; }
        public Presence Presence { get; set; }

        public TourAttendanceDTO()
        {

        }

        public TourAttendanceDTO(string userName, string pointName, Presence presence)
        {
            UserName = userName;
            PointName = pointName;
            Presence = presence;
        }
    }
}
