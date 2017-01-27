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
        public IHttpActionResult GetMessageAlert()
        {
            return Ok("200");
        }

        [HttpPost]
        public void PostMessageAlert([FromBody] MessageAlert alert)
        {
            // ignore own messages
            if (alert.data.personId == Helper.myId)
            {
                return;
            }

            // create http client
            string baseAddress = "https://api.ciscospark.com/v1/messages/";
            HttpClient client = Helper.GetClient(baseAddress);

            // List data response.
            HttpResponseMessage response = client.GetAsync(alert.data.id).Result;

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var message = response.Content.ReadAsAsync<Message>().Result;

                // pass message text on to bot
                if (message.text.StartsWith(@"AWESOME_BOT"))
                {
                    int index = 11;
                    string command = message.text.Substring(index);
                    command = command.Trim(' ');
                    Bot.ExecuteCommand(message.roomId, command.Trim('/'));
                }
                else
                {
                    // in case we want to annoy Justin
                    //Notify(message.roomId, message.text);
                }
            }
        }

        private void Notify(string roomId, string message)
        {
            // getting team id from room id
            string getAddress = "https://api.ciscospark.com/v1/rooms/";
            HttpClient getClient = Helper.GetClient(getAddress);
            HttpResponseMessage response = getClient.GetAsync(roomId).Result;
            var room = response.Content.ReadAsAsync<Room>().Result;

            // getting team member data with team id
            string baseAddress = "https://api.ciscospark.com/v1/team/memberships?teamId=" + room.teamId;
            HttpClient getMemberDataClient = Helper.GetClient(baseAddress);
            response = getMemberDataClient.GetAsync("").Result;
            var memberData = response.Content.ReadAsAsync<MemberData>().Result;

            // send a message or call each member in team
            foreach (var member in memberData.items)
            {
                // message member on spark
                string messageAddress = "https://api.ciscospark.com/v1/messages/";
                HttpClient messageClient = Helper.GetClient(messageAddress);

                var messageClientContent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("toPersonId", member.personId),
                        new KeyValuePair<string, string>("text", message)
                    });
                messageClient.PostAsync("", messageClientContent);


                // call member using tropo
                string tropoAddress = "https://api.tropo.com/1.0/sessions";
                HttpClient callClient = Helper.GetClient(tropoAddress);

                var callClientContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("token","6d6d6f4a755472624e6964466f4872497246555a6850437369716f41666e4d537741777a734a765971526743"),
                    //new KeyValuePair<string, string>("number", "12047211238"),
                    new KeyValuePair<string, string>("number", "17789988063"),
                    new KeyValuePair<string, string>("msg", "New Message: " + message),
                    new KeyValuePair<string, string>("networkToUse", "INUM"),
                    new KeyValuePair<string, string>("callerNumber", "14038078277"),
                });
                callClient.PostAsync("", callClientContent);
            }
        }
    }
}
