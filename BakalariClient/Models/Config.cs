using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BakalariClient.Models
{
    class Config
    {
        [JsonProperty(PropertyName = "display_next_week_from_saturday")]
        public bool? DisplayNextWeekFromSaturday { get; set; }

        [JsonProperty(PropertyName = "credentials", Required = Required.Always)]
        public Credential Credential { get; set; }

        [JsonProperty(PropertyName = "domain", Required = Required.Always)]
        public string Domain
        {
            get => _Domain;
            set
            {
                if (value.EndsWith("/login"))
                {
                    _Domain = value.Substring(0, value.Length - 6);
                }
                else if (value.EndsWith("/"))
                {
                    _Domain = value.Substring(0, value.Length - 1);
                }
                else
                {
                    _Domain = value;
                }
            }
        }

        [JsonIgnore]
        private string _Domain { get; set; }

        [JsonIgnore]
        public string LoginUrl
        {
            get => _Domain + "/login";
        }

        [JsonIgnore]
        public string ScheduleUrl
        {
            get => _Domain + "/next/rozvrh.aspx";
        }
    }
}
