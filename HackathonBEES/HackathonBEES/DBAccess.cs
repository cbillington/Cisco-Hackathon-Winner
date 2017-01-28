using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace HackathonBEES
{
    public static class DBAccess
    {
        public static void InsertMember(Member member)
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                conn.Insert(member);
            }
        }

        public static List<Member> GetMembers()
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                return conn.Query<Member>(Config.getMembersQuery).ToList();
            }
        }

        public static void InsertEmergencyCall(EmergencyCall call)
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                conn.Insert(call);
            }
        }
    }
}