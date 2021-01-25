using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakalariClient.Models
{
    class ScheduleDay
    {
        public string DayName { get; set; }
        public List<ScheduleSubject> ScheduleSubjects { get; set; }
        public DateTime Date { get; set; }
    }
}
