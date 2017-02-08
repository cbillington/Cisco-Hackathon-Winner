using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackathonBEES
{
    public class Tweets
    {
        public List<Tweet> statuses { get; set; }
    }

    public class Tweet
    {
        public string created_at { get; set; }
        public string id_str { get; set; }
        public string text { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string screen_name { get; set; }
        public string url { get; set; }
        public int followers_count { get; set; }
    }
}