using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.UI;

namespace APIwebService
{
    public static class Bot
    {
        public static string _roomId = "Y2lzY29zcGFyazovL3VzL1JPT00vOTM4MGJhZDAtZTQwNy0xMWU2LThmZDItMTVhNGZmMjdhZTBh";

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
                case "help":
                    ListCommands();
                    break;

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

                case "teamInvite":
                    TeamInvite(commandParameters);
                    break;

                case "teamRemove":
                    TeamRemove(commandParameters);
                    break;

                case "prank":
                    Prank(commandParameters);
                    break;

                default:
                    PostMessage("/" + command + " is not a valid command!");
                    break;
            }
        }

        public static void ListCommands()
        {
            PostMessage("Commands: \n" +
                        "time \n" +
                        "invite \n" +
                        "remove \n" +
                        "teamInvite \n" +
                        "teamRemove \n" +
                        "emergancy");
        }

        public static void PostMessage(string message)
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
                PostMessage("User added to room!");
            }
        }

        private static void Remove(string email)
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
                    PostMessage("User removed from room!");
                }
                else
                {
                    PostMessage("Cannot remove user from room!");
                }
            }
        }

        private static void TeamInvite(string email)
        {
            string teamId = GetTeamId();
            string postAddress = "https://api.ciscospark.com/v1/team/memberships/";
            HttpClient postClient = Helper.GetClient(postAddress);
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("teamId", teamId),
                new KeyValuePair<string, string>("personEmail", email)
            });
            HttpResponseMessage response = postClient.PostAsync("", postContent).Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                PostMessage("Cannot find user with email: " + email);
            }
            else
            {
                PostMessage("User added to team!");
            }
        }

        private static void TeamRemove(string email)
        {
            string teamId = GetTeamId();
            var memberData = GetTeamMembers(teamId);

            var membersFound = (from member in memberData.items
                                where member.personEmail == email
                                select member.id).ToList();

            if (membersFound.Count == 0)
            {
                PostMessage("User not in team!");
            }
            else
            {
                string membershipId = membersFound[0];

                string deleteAddress = "https://api.ciscospark.com/v1/team/memberships/" + membershipId;
                HttpClient deleteClient = Helper.GetClient(deleteAddress);
                HttpResponseMessage response = deleteClient.DeleteAsync("").Result;
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    PostMessage("User removed from team!");
                }
                else
                {
                    PostMessage("Cannot remove user from team!");
                }
            }
        }
        private static string GetTeamId()
        {
            // getting team id from room id
            string getAddress = "https://api.ciscospark.com/v1/rooms/";
            HttpClient getClient = Helper.GetClient(getAddress);
            HttpResponseMessage response = getClient.GetAsync(_roomId).Result;
            var room = response.Content.ReadAsAsync<Room>().Result;
            return room.teamId;
        }

        private static MemberData GetMembers(string roomId)
        {
            string getAddress = "https://api.ciscospark.com/v1/memberships/?roomId=" + roomId;
            HttpClient getClient = Helper.GetClient(getAddress);

            HttpResponseMessage response = getClient.GetAsync("").Result;
            return response.Content.ReadAsAsync<MemberData>().Result;
        }

        private static MemberData GetTeamMembers(string teamId)
        {
            string getAddress = "https://api.ciscospark.com/v1/team/memberships?teamId=" + teamId;
            HttpClient getClient = Helper.GetClient(getAddress);

            HttpResponseMessage response = getClient.GetAsync("").Result;
            return response.Content.ReadAsAsync<MemberData>().Result;
        }

        private static void Prank(string message)
        {
           // prank Justin using tropo
            string tropoAddress = "https://api.tropo.com/1.0/sessions";
            HttpClient callClient = Helper.GetClient(tropoAddress);

            var callClientContent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("token","6d6d6f4a755472624e6964466f4872497246555a6850437369716f41666e4d537741777a734a765971526743"),
                    //new KeyValuePair<string, string>("number", "12047211238"),
                    new KeyValuePair<string, string>("number", "14038078277"),
                    //new KeyValuePair<string, string>("number", "17789988063"),
                    new KeyValuePair<string, string>("msg", "New Message: " + message),
                    new KeyValuePair<string, string>("networkToUse", "INUM"),
                    new KeyValuePair<string, string>("callerNumber", "14038078277"),
                    //new KeyValuePair<string, string>("callerNumber", "14038078277"),
                });
            callClient.PostAsync("", callClientContent);
        }
    }
}