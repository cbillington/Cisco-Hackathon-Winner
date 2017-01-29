using System;
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

        public static List<Member> GetMembersByTeam(string role)
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                return conn.Query<Member>(Config.getMembersByTeamQuery, new {role}).ToList();
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

        public static void InsertSensor(Sensor sensor)
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                conn.Insert(sensor);
            }
        }

        public static List<EmergencyCall> GetEmergencies()
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                return conn.Query<EmergencyCall>(Config.getEmergQuery).ToList();
            }
        }

        public static List<Sensor> GetSensorData()
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                return conn.Query<Sensor>(Config.getSensorQuery).ToList();
            }
        }
    }
}