using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace BakalariClient.Services
{
    class ScheduleService
    {
        private readonly string url;
        public string rawHtml;
        private CookieContainer cookieContainer;
        public Schedule Schedule;

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

        // TODO: add comment
        public Schedule GetSchedule()
        {
            ScheduleParserService scheduleParserService = new ScheduleParserService(rawHtml);

            // TODO: add header of schedule

            Schedule = scheduleParserService.ParseSchedule();
            return Schedule;
        }
    }
}
