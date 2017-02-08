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
        public string GetSpark()
        {
            return "success";
        }

        private bool checkEmail(string email)
        {
            string mo = "mo_xue1989@yahoo.ca";
            string matt = "mattjennings44@gmail.com";
            string cal = "calliasnguyen@gmail.com";
            string chris = "christopher.billington.school@gmail.com";
            string craig = "craig.macritchie@gmail.com";

            if (email == mo || email == matt || email == cal || email == chris || email == craig)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void PostSpark([FromBody] Notification alert)
        {
            if (!checkEmail(alert.data.personEmail))
            {
                return;
            }
            // ignore bot's own messages
            //if (alert.data.personId == Config.botId)
            //{
            //    return;
            //}

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
                    SparkBot.ExecuteCommand(message.roomId, command, alert.data);

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


