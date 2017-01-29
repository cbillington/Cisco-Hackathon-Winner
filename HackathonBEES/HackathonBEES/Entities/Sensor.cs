using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper.Contrib.Extensions;

namespace HackathonBEES
{
    public class Sensor
    {
        [Computed][Key]
        public int sqlId { get; set; }
        public string LSD { get; set; }
        public string temperature { get; set; }
        public string pressure { get; set; }
        public string status { get; set; }
    }

    public class ReadingWrapper
    {
        public List<Reading> readings { get; set; }
    }

    public class Reading
    {
        public int value { get; set; }
    }
}