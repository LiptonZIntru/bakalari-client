using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace ScheduleClassLibrary.Services
{
    class HttpRequestService
    {
        /// <summary>
        /// Send request to specified url with specified cookies and body
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="body"></param>
        /// <param name="isGet"></param>
        /// <returns> Response from request </returns>
        public string Send(string url, CookieContainer cookieContainer, object body = null, bool isGet = true)
        {
            string result;
            string json;
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp(url);
            httpWebRequest.Method = "GET";
            httpWebRequest.CookieContainer = cookieContainer;

            if (body == null)
            {
                body = new { };
            }

            if (!isGet)
            {
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    json = JsonConvert.SerializeObject(body);
                    streamWriter.Write(json);
                }
            }

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                return result;//.Replace("\n", "").Replace("\r", "");
            }
            catch (Exception e)
            {
                MessageBox.Show("Server unavailable\n\n" + e.ToString());
                throw new Exception("Server unavailable");
            }
        }
    }
}
