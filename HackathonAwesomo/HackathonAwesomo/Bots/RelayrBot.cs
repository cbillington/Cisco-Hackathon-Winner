using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace HackathonAwesomo
{
    public static class RelayrBot
    {
        private const string BaseUri = "https://api.relayr.io/";

        public static List<Patient> GetAllPatients()
        {
            HttpClient getClient = GetClient();
            string uri = BaseUri + "users/" + Config.RelayrID + "/devices";
            getClient.BaseAddress = new Uri(uri);

            HttpResponseMessage response = getClient.GetAsync("").Result;
            var patients = response.Content.ReadAsAsync<List<Patient>>().Result;
            return patients;
        }

        public static void CheckPatients()
        {
            var patients = GetAllPatients();
            foreach (var patient in patients)
            {
                if (!patient.monitor)
                {
                    continue;
                }
                var readings = GetPatientReading(patient.id);
                bool notify = false;
                foreach (Reading reading in readings)
                {
                    switch (reading.meaning)
                    {
                        //case "BloodPressure":
                        //    if (reading.value > 0)
                        //    {
                        //        notify = true;
                        //    }
                        //    break;

                        case "HeartRate":
                            if (reading.value > patient.highHr || reading.value < patient.lowHr)
                            {
                                notify = true;
                            }
                            break;
                    }
                    reading.device = patient.id;
                    DBAccess.InsertReading(reading);
                }
                if (notify)
                {
                    NotifyEmergency(patient, readings);
                }
            }
        }

        private static void NotifyEmergency(Patient patient, List<Reading> readings)
        {
            string mapsLink = "https://www.google.ca/maps?q=" + patient.latitude + "," + patient.longitude;

            string emergencyMessage = "Emergency Notification: \n" +
                                      "Patient: " + patient.name + "\n" +
                                      readings[0].meaning + ": " + readings[0].value + "\n" +
                                      readings[1].meaning + ": " + readings[1].value + "\n" +
                                      "Location: " + mapsLink;

            //MobileBot.Notify(emergencyMessage);
            //SlackBot.SendMessage(emergencyMessage);
            //TropoBot.TextUser(patient.emergPhone, emergencyMessage);
        }

        public static Patient RegisterPatient(string name)
        {
            HttpClient postClient = GetClient();
            string uri = BaseUri + "devices/";
            postClient.BaseAddress = new Uri(uri);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
            request.Content = new StringContent(
                "{\"name\":\"" + name + "\"," +
                "\"owner\":\"" + Config.RelayrID + "\"," +
                "\"model\":\"" + Config.RelayrModel + "\"}",
                Encoding.UTF8, "application/json");

            HttpResponseMessage response = postClient.SendAsync(request).Result;
            var patient = response.Content.ReadAsAsync<Patient>().Result;
            return patient;
        }

        public static List<Reading> GetPatientReading(string userid)
        {
            var readings = new List<Reading>();
            HttpClient getClient = GetClient();
            string uri = BaseUri + "devices/" + userid + "/readings";
            getClient.BaseAddress = new Uri(uri);

            HttpResponseMessage response = getClient.GetAsync("").Result;
            if (response.IsSuccessStatusCode)
            {
                readings = response.Content.ReadAsAsync<ReadingWrapper>().Result.Readings.ToList();
            }
            return readings;
        }

        private static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Config.RelayrToken);
            return client;
        }
    }
}