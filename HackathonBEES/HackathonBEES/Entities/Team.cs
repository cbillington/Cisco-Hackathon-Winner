using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper.Contrib.Extensions;

namespace HackathonBEES
{
    public class Team
    {

    }

    public class Room
    {
        public string id { get; set; }
        public string teamId { get; set; }
    }

    public class MemberData
    {
        public List<Member> items { get; set; }
    }

    public class Member
    {
        [Computed][Key]
        public int sqlId { get; set; }
        public string id { get; set; }
        public string personId { get; set; }
        public string personEmail { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string role { get; set; }
    }
}