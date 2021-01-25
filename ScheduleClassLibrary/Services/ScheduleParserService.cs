using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using ScheduleClassLibrary.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace ScheduleClassLibrary.Services
{
    class ScheduleParserService
    {
        private readonly string rawHtml;
        public Schedule Schedule;

        private static readonly string[] dayLabels = new string[5]
        {
            "Pondělí",
            "Úterý",
            "Středa",
            "Čtvrtek",
            "Pátek"
        };

        public ScheduleParserService(string rawHtml)
        {
            this.rawHtml = rawHtml;
        }

        /// <summary>
        /// Load schedule from html file
        /// </summary>
        /// <returns> Parsed Schedule object</returns>
        public Schedule ParseSchedule()
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(rawHtml);

            Schedule = new Schedule()
            {
                ScheduleDays = LoadDays(htmlDoc),
            };
            return Schedule;
        }

        /// <summary>
        /// Load single ScheduleDay
        /// </summary>
        /// <param name="html"> HTML document </param>
        /// <returns> List of ScheduleDays </returns>
        public List<ScheduleDay> LoadDays(HtmlDocument html)
        {
            List<ScheduleDay> scheduleDays = new List<ScheduleDay>();
            List<HtmlNode> days = html.DocumentNode.SelectNodes("//div[@class='day-row']").ToList();
            days = days
                .Select(day => day
                    .ChildNodes.Single(x => x.Name == "div")
                    .ChildNodes.Single(x => x.Name == "div"))
                .ToList();

            int i = 0;
            foreach (HtmlNode day in days)
            {
                string dateString = day
                    .ChildNodes.Single(x => x.Name == "div")
                    .ChildNodes.Single(x => x.Name == "div")
                    .ChildNodes.Single(x => x.Name == "span")
                    .InnerText;
                scheduleDays.Add(new ScheduleDay()
                {
                    DayName = dayLabels[i],
                    ScheduleSubjects = LoadSubjects(day),
                    Date = DateTime.ParseExact(dateString, "dd/M", CultureInfo.InvariantCulture),
                });

                i++;
            }

            return scheduleDays;
        }

        /// <summary>
        /// Parse subjects into list and returns it 
        /// </summary>
        /// <param name="day"> Single day in HTML format </param>
        /// <returns> List of ScheduleSubjects </returns>
        public List<ScheduleSubject> LoadSubjects(HtmlNode day)
        {
            List<ScheduleSubject> scheduleSubjects = new List<ScheduleSubject>();
            List<HtmlNode> subjects = day.ChildNodes.Where(x => x.Name == "span").ToList();

            foreach (HtmlNode subject in subjects)
            {
                scheduleSubjects.Add(LoadSubject(subject));
            }

            return scheduleSubjects;
        }

        /// <summary>
        /// Parse single subject
        /// </summary>
        /// <param name="subject"> Single subject in HTML format </param>
        /// <returns> ScheduleSubject object </returns>
        public ScheduleSubject LoadSubject(HtmlNode subject)
        {

            HtmlNode dataDiv = subject
                .ChildNodes
                .Single(x => x.Name == "div")
                .ChildNodes
                .Single(x => x.Name == "div");

            try
            {
                var a = dataDiv.GetClasses();
                if (dataDiv.GetClasses().FirstOrDefault() == null)
                {
                    return null;
                }
                else if (dataDiv.GetClasses().First() == "empty")
                {
                    return null;
                }

                var b = a;
            }
            catch (Exception e)
            {
                throw new Exception("Subject cannot be parsed");
            }

            string data = dataDiv.Attributes["data-detail"].Value;

            ScheduleSubject scheduleSubject = JsonConvert.DeserializeObject<ScheduleSubject>(data);
            scheduleSubject.ShortName = dataDiv
                .ChildNodes
                .Single(x => x.Name == "div")
                .ChildNodes
                .Where(x => x.Name == "div")
                .ToList()[1]
                .InnerText;

            return scheduleSubject;
        }
    }
}
