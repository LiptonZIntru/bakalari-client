using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BakalariClient.Models;
using Newtonsoft.Json;

namespace BakalariClient.Services
{
    class JsonReaderService
    {
        private readonly string filename;
        public JsonReaderService(string filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// Read file and return contents
        /// </summary>
        /// <returns></returns>
        public Credential GetCredentials()
        {
            return JsonConvert.DeserializeObject<Credential>(File.ReadAllText(@"..\..\" + filename));
        }
    }
}
