using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace HackathonAwesomo
{
    public static class TropoBot
    {
        private const string BaseUri = "https://api.tropo.com/1.0/sessions";
        public static void NotifyUser(string number, string message, string method = "SMS")
        {
            HttpClient textClient = new HttpClient();
            textClient.BaseAddress = new Uri(BaseUri);
            textClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            string phoneList = "[\"" + number + "\"]";

            var textClientContent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("token", Config.TropoToken),
                    new KeyValuePair<string, string>("msg", message),
                    new KeyValuePair<string, string>("networkToUse", method), //INUM or SMS
                    new KeyValuePair<string, string>("callerNumber", Config.FromNumber), 
                    new KeyValuePair<string, string>("numbersToDial", phoneList)
                });
            var response = textClient.PostAsync("", textClientContent);
        }
    }
}