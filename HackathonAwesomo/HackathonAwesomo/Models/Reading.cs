using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace HackathonAwesomo
{
    public class Patient
    {
        [Computed][Key][JsonIgnore]
        public string sqlId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string emergPhone { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string created { get; set; }
        public bool monitor { get; set; }
        public int lowHr { get; set; }
        public int highHr { get; set; }
    }

    public class ReadingWrapper
    {
        public List<Reading> Readings { get; set; }
    }

    public class Received
    {
        public long time { get; set; }
    }

    public class Reading
    {
        [Computed][Key][JsonIgnore]
        public string sqlid { get; set; }
        [JsonIgnore]
        public string device { get; set; }
        public string meaning { get; set; }
        public int value { get; set; }
        public long received { get; set; }
        [JsonIgnore][Computed]
        public DateTime time
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(Convert.ToDouble(received - 3600*7000)); }
        }
    }

    public class ReadingData : Reading
    {
        [Computed][Key]
        public string sqlid { get; set; }
        public string device { get; set; }
    }

    public class Monitor
    {
        
    }
}