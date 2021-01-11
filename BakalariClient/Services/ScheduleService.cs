using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BakalariClient.Services
{
    class ScheduleService
    {
        private readonly string url;
        private CookieContainer cookieContainer;
        public string RawHtml;

        public ScheduleService(CookieContainer cookieContainer)
        {
            url = ConfigurationManager.AppSettings["scheduleUrl"];
            this.cookieContainer = cookieContainer;
        }

        /// <summary>
        /// Send request to Schedule url and returns raw HTML page
        /// </summary>
        /// <returns> HTML page </returns>
        public async Task<string> GetSchedule()
        {
            HttpRequestService httpRequestService = new HttpRequestService();
            HttpResponseMessage httpResponseMessage = await httpRequestService.Send(url, cookieContainer);

            RawHtml = await httpResponseMessage.Content.ReadAsStringAsync();
            return RawHtml;
        }
    }
}
