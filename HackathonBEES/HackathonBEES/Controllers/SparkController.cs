using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HackathonBEES
{
    public class SparkController : ApiController
    {
        public IHttpActionResult GetSpark()
        {
            return Ok();
        }

        public void PostSpark([FromBody] Notification alert)
        {
            // ignore bot's own messages
            if (alert.data.personId == Config.botId)
            {
                return;
            }

            // create http client
            string baseAddress = "https://api.ciscospark.com/v1/messages/";
            HttpClient client = Config.GetClient(baseAddress);

            // List data response.
            HttpResponseMessage response = client.GetAsync(alert.data.id).Result;

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var message = response.Content.ReadAsAsync<Message>().Result;

                // pass message text on to bot
                if (message.text.StartsWith(Config.botName))
                {
                    int index = Config.botName.Length;
                    string command = message.text.Substring(index);
                    command = command.Trim(' ');
                    SparkBot.ExecuteCommand(message.roomId, command);

                    // add command to notify proper users
                }
                else
                {
                    //Notify(message.roomId, message.text);
                }
            }
        }
    }
}


