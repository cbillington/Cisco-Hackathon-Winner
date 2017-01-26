using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace APIwebService.Controllers
{
    public class MessageAlertController : ApiController
    {
        public static AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer",
            "NzFlN2U5YWYtODRhMC00MzgxLTliZDQtMmRjNDk5MDk1NzdjYjZjZDE3YzItNmQ4");

        public IHttpActionResult GetMessageAlert()
        {
            return Ok("200");
        }

        [HttpPost]
        public IHttpActionResult PostMessageAlert([FromBody] MessageAlert alert)
        {
            string myId = "Y2lzY29zcGFyazovL3VzL1BFT1BMRS8xMmQzODU0My01NWE2LTRiMWEtOWQ3Ni1mNDk4ODhmMmM0YzQ";
            if (alert.data.personId == myId)
            {
                return Ok();
            }
            
            HttpClient client = new HttpClient();
            string baseAddress = "https://api.ciscospark.com/v1/messages/";
            client.BaseAddress = new Uri(baseAddress);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            //adding authentication
            client.DefaultRequestHeaders.Authorization = authHeader;

            // List data response.
            HttpResponseMessage response = client.GetAsync(alert.data.id).Result;  // Blocking call!

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var message = response.Content.ReadAsAsync<Message>().Result;

                SendMessage(alert.data.roomId, message.text);

                string replyText = "";
                string reply = CommandReply(message.text);

                if (reply == "null")
                {
                    return Ok();
                }
                else
                {
                    if (reply == "")
                    {
                        replyText += "Command " + message.text + " not understood!";
                    }
                    else
                    {
                        replyText += "The time is: " + reply;
                    }

                    string postAddress = "https://api.ciscospark.com/v1/messages/";
                    HttpClient postClient = new HttpClient();
                    postClient.BaseAddress = new Uri(postAddress);
                    postClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    //adding authentication
                    postClient.DefaultRequestHeaders.Authorization = authHeader;
                    var postContent = new FormUrlEncodedContent(new[]
                    {
                        //new KeyValuePair<string, string>("toPersonId", "Y2lzY29zcGFyazovL3VzL1BFT1BMRS9hYzE5OTI2Mi1hNTg2LTQ3ZmUtYjU2OS05NDE5NDk3ZGRiN2Y"),

                        new KeyValuePair<string, string>("roomId", message.roomId),
                        new KeyValuePair<string, string>("text", replyText)
                    });
                    HttpResponseMessage postResponse = postClient.PostAsync("", postContent).Result;
                    return Ok(message.text);
                }
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, response.ReasonPhrase);
            }
        }

        private void SendMessage(string roomId, string message)
        {
            // getting team id from room id
            string getAddress = "https://api.ciscospark.com/v1/rooms/";
            HttpClient getClient = new HttpClient();
            getClient.BaseAddress = new Uri(getAddress);
            getClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            getClient.DefaultRequestHeaders.Authorization = authHeader;
            HttpResponseMessage response = getClient.GetAsync(roomId).Result;

            var room = response.Content.ReadAsAsync<Room>().Result;

            // getting team member data with team id
            HttpClient getClient2 = new HttpClient();
            getClient2.BaseAddress = new Uri("https://api.ciscospark.com/v1/team/memberships?teamId="+room.teamId);
            getClient2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            getClient2.DefaultRequestHeaders.Authorization = authHeader;
            response = getClient2.GetAsync("").Result;

            var memberData = response.Content.ReadAsAsync<MemberData>().Result;

            foreach (var member in memberData.items)
            {
                //string postAddress = "https://api.ciscospark.com/v1/messages/";
                //HttpClient postClient = new HttpClient();
                //postClient.BaseAddress = new Uri(postAddress);
                //postClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //postClient.DefaultRequestHeaders.Authorization = authHeader;
                //var postContent = new FormUrlEncodedContent(new[]
                //{
                //        new KeyValuePair<string, string>("toPersonId", member.personId),
                //        new KeyValuePair<string, string>("text", "Emergancy!")
                //    });
                //postClient.PostAsync("", postContent);

                string postAddress2 = "https://api.tropo.com/1.0/sessions";
                HttpClient postClient2 = new HttpClient();
                postClient2.BaseAddress = new Uri(postAddress2);
                postClient2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var postContent2 = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("token","6d6d6f4a755472624e6964466f4872497246555a6850437369716f41666e4d537741777a734a765971526743"),
                    new KeyValuePair<string, string>("number", "12047211238"),
                    new KeyValuePair<string, string>("msg", "New Message: " + message),
                    new KeyValuePair<string, string>("networkToUse", "INUM"),
                    new KeyValuePair<string, string>("callerNumber", "14038078277"), 
                });
                postClient2.PostAsync("", postContent2);
            }
        }


        private string CommandReply(string command)
        {
            string reply = "";
            if (command.StartsWith(@"/"))
            {
                if (command.Trim('/') == "time")
                {
                    reply += DateTime.Now.ToString("h:mm:ss tt zz");
                }
            }
            else
            {
                reply = "null";
            }

            return reply;
        }

    }
}
