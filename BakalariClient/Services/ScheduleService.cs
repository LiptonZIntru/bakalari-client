using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using BakalariClient.Models;
using Newtonsoft.Json;

namespace BakalariClient.Services
{
    class ScheduleService
    {
        private readonly string url;
        public string rawHtml;
        private CookieContainer cookieContainer;
        public Schedule Schedule;

        public ScheduleService()
        {

        }

        public ScheduleService(CookieContainer cookieContainer, string url)
        {
            this.url = url;
            this.cookieContainer = cookieContainer;
        }

        /// <summary>
        /// Send request to Schedule url and returns raw HTML page
        /// </summary>
        /// <returns> HTML page </returns>
        public string GetHtmlPage()
        {
            HttpRequestService httpRequestService = new HttpRequestService();
            rawHtml = httpRequestService.Send(url, cookieContainer);
            return rawHtml;
        }

        /// <summary>
        /// Generate Schedule and return result
        /// </summary>
        /// <returns></returns>
        public Schedule GetSchedule()
        {
            ScheduleParserService scheduleParserService = new ScheduleParserService(rawHtml);

            Schedule = scheduleParserService.ParseSchedule();
            SaveScheduleToFile();
            return Schedule;
        }

        /// <summary>
        /// Save schedule to file
        /// </summary>
        /// <param name="filename"></param>
        public void SaveScheduleToFile(string filename = "schedule.json")
        {
            string jsonSchedule = JsonConvert.SerializeObject(Schedule/*, Formatting.Indented*/);
            File.WriteAllText(filename, jsonSchedule);
        }

        /// <summary>
        /// Load schedule from file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Schedule LoadScheduleFromFile(string filename = "schedule.json")
        {
            string stringSchedule = File.ReadAllText(filename);
            Schedule = JsonConvert.DeserializeObject<Schedule>(stringSchedule);
            return Schedule;
        }
    }
}
