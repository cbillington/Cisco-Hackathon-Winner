using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dapper.Contrib.Extensions;

namespace HackathonAwesomo
{
    public class MemberList
    {
        public List<Member> members { get; set; }
    }

    public class Member
    {
        public string id { get; set; }
        public string name { get; set; }
        public Profile profile { get; set; }
        public bool is_bot { get; set; }

        public void InsertDB()
        {
            if (!is_bot && profile.email != null)
            {
                profile.id = id;
                profile.name = name;
                DBAccess.InsertProfile(profile);
            }
        }
    }

    public class Profile
    {
        [Computed][Key]
        public int sqlid { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string real_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }
}