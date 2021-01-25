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
        public LoginService(string url)
        {
            cookieContainer = new CookieContainer();
            this.url = url;
        }

        /// <summary>
        /// Login user, set and return login cookies
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns> Login cookies </returns>
        public CookieContainer Authorize(string username, string password)
        {
            Credential credentials = new Credential()
            {
                Username = username,
                Password = password
            };
            return Authorize(credentials);
        }

        /// <summary>
        /// Login user, set and return login cookies
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns> Login cookies </returns>
        public CookieContainer Authorize(Credential credentials)
        {
            HttpRequestService httpRequestService = new HttpRequestService();
            httpRequestService.Send(url, cookieContainer, credentials, false);
            return cookieContainer;
        }
    }
}
