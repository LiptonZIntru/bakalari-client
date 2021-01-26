using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakalariClient.Models
{
    class Schedule
    {
        public List<ScheduleDay> ScheduleDays { get; set; }

        public List<ScheduleTime> ScheduleTimes { get; set; }
    }
}
