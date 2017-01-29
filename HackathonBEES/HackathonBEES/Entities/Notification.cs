using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper.Contrib.Extensions;

namespace HackathonBEES
{
    public class Notification
    {
        public string id { get; set; }
        public string name { get; set; }
        public string resource { get; set; }
        public string filter { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string id { get; set; }

        public string roomId { get; set; }

        public string personId { get; set; }

        public string personEmail { get; set; }

        public string created { get; set; }

    }

    public class Message
    {
        public string text { get; set; }
        public string roomId { get; set; }
        public string type { get; set; }
    }

    public class Emergency
    {
        public EmergencyCall result { get; set; }
    }

    public class EmergencyCall
    {
        [Computed][Key]
        public string sqlId { get; set; }
        public string transcription { get; set; }
        public string identifier { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
    }

    public class TemperatureObject
    {
        public Temperature main { get; set; }
    }

    public class Temperature
    {
        public string temp { get; set; }
    }
}