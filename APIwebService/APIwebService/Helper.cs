using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace APIwebService
{
    public static class Helper
    {
        // bot access token
        public static AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer",
          "OGI4MGJkZTMtODI3OS00ZTcxLTk0YjUtNGMwODgzNzA2ODJjY2E3ZTYyNzQtYTk1");

        // bot's personId
        public static string myId = "Y2lzY29zcGFyazovL3VzL1BFT1BMRS9iZTc3Y2NlNS04YzE1LTQwMmQtODIyYy1kYjI2MjBhZjkzOTE";

        //// Mo's access token
        //public static AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer",
        //  "NzFlN2U5YWYtODRhMC00MzgxLTliZDQtMmRjNDk5MDk1NzdjYjZjZDE3YzItNmQ4");

        //// Mo's personId
        //public static string myId = "Y2lzY29zcGFyazovL3VzL1BFT1BMRS8xMmQzODU0My01NWE2LTRiMWEtOWQ3Ni1mNDk4ODhmMmM0YzQ";

        public static HttpClient GetClient(string address)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(address);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            //adding authentication
            client.DefaultRequestHeaders.Authorization = authHeader;

            return client;
        }
    }
}