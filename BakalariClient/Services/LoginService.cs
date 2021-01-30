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
            string response = "";
            try
            {
                HttpRequestService httpRequestService = new HttpRequestService();
                response = httpRequestService.Send(url, cookieContainer, credentials, false).Substring(48, 30);
            }
            catch
            {
                new LogService().Add("Wrong URL~");
                throw new Exception("Wrong URL~");
            }
            

            if(response == "Bakalari - login to the system")
            {
                new LogService().Add("Wrong credentials!");
                throw new Exception("Wrong credentials!");
            }

            return cookieContainer;
        }
    }
}
