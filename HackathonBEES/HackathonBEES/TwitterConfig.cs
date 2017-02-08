using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace HackathonBEES
{
    public static class TwitterConfig
    {
        public static AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAAPxnzAAAAAAAvl9uhUNMSY4txqtLh4H02a2TXD8%3DvMjSCgmrSONxIl0iUo3rnyOrE6PPkC0rYB0YTTtyQYOoyho52T");

        public static HttpClient GetClient(string address)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(address);

            // Add an Accept header for JSON format.
            //client.DefaultRequestHeaders.Accept.Add(
                //new MediaTypeWithQualityHeaderValue("application/json"));

            //adding authentication
            client.DefaultRequestHeaders.Authorization = authHeader;

            return client;
        }
    }
}