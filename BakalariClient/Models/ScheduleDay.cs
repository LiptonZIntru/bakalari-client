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

        public string DayNameShort
        {
            get => DayName.Substring(0, 2);
        }

        public List<ScheduleSubject> ScheduleSubjects { get; set; }
        public DateTime Date { get; set; }

        public string DateShort
        {
            get => Date.ToString("dd.MM.");
        }
    }
}
