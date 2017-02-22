using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace HackathonAwesomo
{
    public static class MobileBot
    {
        public static void Notify(string message)
        {
            string postAddress = "https://onesignal.com/api/v1/notifications/";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(postAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Config.OneSignalTokenOld);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
            request.Content = new StringContent(
                "{\"app_id\":\""+Config.OneSignalAppIdOld+"\"," +
                "\"contents\":{\"en\":\"" + message + "\"}," +
                "\"included_segments\":[\"All\"]}",
                Encoding.UTF8,
                "application/json"); //CONTENT-TYPE header

            var result = client.SendAsync(request).Result;
        }
    }
}