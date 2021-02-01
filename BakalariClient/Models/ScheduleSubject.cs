using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BakalariClient.Models
{
    class ScheduleSubject
    {

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "shortname")]
        public string ShortName { get; set; }

        [JsonProperty(PropertyName = "teacher")]
        public string Teacher { get; set; }

        [JsonProperty(PropertyName = "room")]
        public string ClassLocation { get; set; }

        [JsonProperty(PropertyName = "group")]
        public string Group { get; set; }

        [JsonProperty(PropertyName = "theme")]
        public string LessonSubject { get; set; }

        [JsonProperty(PropertyName = "changeinfo")]
        public string ChangeInfo { get; set; }

        [JsonProperty(PropertyName = "notice")]
        public string Notice { get; set; }

        [JsonProperty(PropertyName = "absencetext")]
        public string AbsenceText { get; set; }

        [JsonProperty(PropertyName = "identcode")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "begin")]
        public DateTime Begin { get; private set; }

        [JsonProperty(PropertyName = "end")]
        public DateTime End { get; private set; }

        [JsonProperty(PropertyName = "begin_shortened")]
        public string BeginShortened { get; private set; }

        [JsonProperty(PropertyName = "end_shortened")]
        public string EndShortened { get; private set; }

        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; private set; }


        [JsonProperty(PropertyName = "subjecttext")]
        public string AdditionalInformation
        {
            set
            {
                if (value != null)
                {
                    string[] data = value.Split('|');

                    string date = data[1].Trim();
                    string[] time = new string[2];
                    time = data[2]
                        .Split('(')[1]
                        .Split(')')[0]
                        .Replace(" ", "")
                        .Split('-');

                    TimeSpan beginTimeSpan = TimeSpan.ParseExact(time[0], "h\\:mm", CultureInfo.InvariantCulture);
                    TimeSpan endTimeSpan = TimeSpan.ParseExact(time[1], "h\\:mm", CultureInfo.InvariantCulture);

                    BeginShortened = time[0];
                    EndShortened = time[1];
                    Begin = DateTime.ParseExact($"{date} {time[0]}", "ddd d/M H\\:mm", CultureInfo.InvariantCulture);
                    End = DateTime.ParseExact($"{date} {time[1]}", "ddd d/M H\\:mm", CultureInfo.InvariantCulture);

                    Name = data[0].Trim();
                    Duration = (endTimeSpan - beginTimeSpan).ToString("hh\\:mm");
                }
            }
        }
    }
}
