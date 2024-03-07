using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourist_Project.Domain.Models
{
    public class RequestStatistics
    {
        public string Time { get; set; }
        public int Statistics { get; set; }

        public RequestStatistics()
        {

        }

        public RequestStatistics(string time, int statistics)
        {
            Time = time;
            Statistics = statistics;
        }
    }
}
