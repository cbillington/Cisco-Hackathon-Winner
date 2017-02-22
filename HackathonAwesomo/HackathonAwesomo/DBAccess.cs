using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using Dapper.Contrib.Extensions;

namespace HackathonAwesomo
{
    public static class DBAccess
    {
        public static void InsertProfile(Profile profile)
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                conn.Insert(profile);
            }
        }

        public static Patient GetPatientDetails(string id)
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                return conn.QuerySingle<Patient>(Config.GetPatientsDetailsQuery, new {id});
            }
        }

        public static List<Patient> GetPatients()
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                return conn.Query<Patient>(Config.GetPatientsQuery).ToList();
            }
        }

        public static List<Reading> GetReadings(string device, string meaning)
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                return conn.Query<Reading>(Config.GetReadingsQuery, new {meaning, device}).ToList();
            }
        }

        public static void InsertPatient(Patient newPatient)
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                conn.Insert(newPatient);
            }
        }

        public static void UpdatePatient(Patient patient)
        {
            patient.sqlId = GetPatientDetails(patient.id).sqlId;
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Update(patient);
            }
        }

        public static void InsertReading(Reading reading)
        {
            using (SqlConnection conn = new SqlConnection(Config.connectionString))
            {
                conn.Open();
                var time = conn.QuerySingle<Received>(Config.GetLastReading, new {reading.meaning, reading.device}).time;
                if (time < reading.received)
                {
                    conn.Insert(reading);
                    _default.DataUpdated = true;
                }
            }
        }
    }
}