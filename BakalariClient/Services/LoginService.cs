using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BakalariClient.Models;
using Newtonsoft.Json;

namespace BakalariClient.Services
{
    class LoginService
    {
        public CookieContainer cookieContainer;
        private readonly string url;
        public LoginService()
        {
            cookieContainer = new CookieContainer();
            url = ConfigurationManager.AppSettings["loginUrl"];
        }

        /// <summary>
        /// Login user, set and return login cookies
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> Login cookies </returns>
        public async Task<CookieContainer> Authorize(string username, string password)
        {
            Credential credentials = new Credential()
            {
                Username = username,
                Password = password
            };
            return await Authorize(credentials);
        }

        /// <summary>
        /// Login user, set and return login cookies
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns> Login cookies </returns>
        public async Task<CookieContainer> Authorize(Credential credentials)
        {
            HttpRequestService httpRequestService = new HttpRequestService();
            await httpRequestService.Send(url, cookieContainer, credentials);

            return cookieContainer;
        }
    }
}
