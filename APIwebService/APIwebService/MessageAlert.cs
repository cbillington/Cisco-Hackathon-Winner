using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIwebService
{
    public class MessageAlert
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
}