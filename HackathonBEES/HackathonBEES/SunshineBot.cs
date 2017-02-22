using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HackathonBEES
{
    public static class SunshineBot
    {
        public static void NotifyMessage(string text)
        {
            string postAddress = "https://onesignal.com/api/v1/notifications/";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(postAddress);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "MDEyODE2NTUtNDIzNy00MmZlLWJkNWQtNDk0Yjg1ZWFiYWQ0");

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
            request.Content = new StringContent(
                "{\"app_id\":\"f4c7b4bd-1b2f-452c-aa1a-17427cde9078\"," +
                "\"contents\":{\"en\":\"" + text + "\"}," +
                "\"included_segments\":[\"Active Users\"]}",
                                                Encoding.UTF8,
                                                "application/json");//CONTENT-TYPE header

            var result = client.SendAsync(request).Result;
        }
    }
}