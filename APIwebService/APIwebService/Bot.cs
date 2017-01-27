using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace APIwebService
{
    public static class Bot
    {
        public static string _roomId;

        public static void ExecuteCommand(string roomId, string command)
        {
            _roomId = roomId;
            string commandHeader, commandParameters;

            int index = command.IndexOf(' ');
            if (index != -1)
            {
                commandHeader = command.Substring(0, index);
                commandParameters = command.Substring(index).Trim(' ');
            }
            else
            {
                commandHeader = command;
                commandParameters = "";
            }

            switch (commandHeader)
            {
                case "time":
                    PostMessage(DateTime.Now.ToString("h:mm:ss tt zz"));
                    break;

                case "emergency":
                    PostMessage("Emergency");
                    break;

                case "invite":
                    Invite(commandParameters);
                    break;

                case "remove":
                    Remove(commandParameters);
                    break;
                default:
                    PostMessage("/" + command + " is not a valid command!");
                    break;
            }
        }

        private static void PostMessage(string message)
        {
            string postAddress = "https://api.ciscospark.com/v1/messages/";
            HttpClient postClient = Helper.GetClient(postAddress);
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("roomId", _roomId),
                new KeyValuePair<string, string>("text", message)
            });
            postClient.PostAsync("", postContent);
        }

        private static void Invite(string email)
        {
            string postAddress = "https://api.ciscospark.com/v1/memberships/";
            HttpClient postClient = Helper.GetClient(postAddress);
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("roomId", _roomId),
                new KeyValuePair<string, string>("personEmail", email) 
            });
            HttpResponseMessage response =  postClient.PostAsync("", postContent).Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                PostMessage("Cannot find user with email: " + email);
            }
            else
            {
                PostMessage("User added!");
            }
        }

        public static void Remove(string email)
        {
            var memberData = GetMembers(_roomId);

            var membersFound = (from member in memberData.items
                where member.personEmail == email
                select member.id).ToList();

            if (membersFound.Count == 0)
            {
                PostMessage("User not in room!");
            }
            else
            {
                string membershipId = membersFound[0];

                string deleteAddress = "https://api.ciscospark.com/v1/memberships/" + membershipId;
                HttpClient deleteClient = Helper.GetClient(deleteAddress);
                HttpResponseMessage response = deleteClient.DeleteAsync("").Result;
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    PostMessage("User Deleted!");
                }
                else
                {
                    PostMessage("Cannot remove user!");
                }
            }
        }

        private static MemberData GetMembers(string roomId)
        {
            string getAddress = "https://api.ciscospark.com/v1/memberships/?roomId=" + roomId;
            HttpClient getClient = Helper.GetClient(getAddress);

            HttpResponseMessage response = getClient.GetAsync("").Result;
            return response.Content.ReadAsAsync<MemberData>().Result;
        }
    }
}