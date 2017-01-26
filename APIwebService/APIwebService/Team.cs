using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace APIwebService
{
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
        public string personId { get; set; }
        public string personEmail { get; set; }
        public string personDisplayName { get; set; }
    }
}