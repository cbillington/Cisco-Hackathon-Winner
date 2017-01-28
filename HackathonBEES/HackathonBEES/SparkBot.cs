using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace HackathonBEES
{
    public static class SparkBot
    {
        public static string _roomId = Config.roomId;
        
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
                    NotifyAll(commandParameters);
                    break;

                case "checkTemperature":
                    CheckTemperature();
                    break;

                case "conference":
                    StartConference();
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

                default:
                    PostMessage(command + " is not a valid command!");
                    break;
            }
        }

        private static void StartConference()
        {
            foreach (var member in DBAccess.GetMembers())
            {
                HttpClient conClient = Config.GetClient(Config.tropoBase);
                var conClientContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("token", "4b6569594547477173474864445555445453516157446b656e6d587074417670746b61724c445a646d4a644a"),
                    new KeyValuePair<string, string>("numberToDial", member.phone),
                    new KeyValuePair<string, string>("conferenceID", "1111"),
                });
                conClient.PostAsync("", conClientContent);
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
            HttpClient postClient = Config.GetClient(postAddress);
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("roomId", _roomId),
                new KeyValuePair<string, string>("text", message)
            });
            postClient.PostAsync("", postContent);
        }

        public static void MessageMember(string personEmail, string message)
        {
            string postAddress = "https://api.ciscospark.com/v1/messages/";
            HttpClient postClient = Config.GetClient(postAddress);
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("toPersonEmail", personEmail),
                new KeyValuePair<string, string>("text", message)
            });
            postClient.PostAsync("", postContent);
        }

        private static void Invite(string email)
        {
            string postAddress = "https://api.ciscospark.com/v1/memberships/";
            HttpClient postClient = Config.GetClient(postAddress);
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("roomId", _roomId),
                new KeyValuePair<string, string>("personEmail", email)
            });
            HttpResponseMessage response = postClient.PostAsync("", postContent).Result;
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
                HttpClient deleteClient = Config.GetClient(deleteAddress);
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

        private static Member TeamInvite(string email)
        {
            Member member = null;
            string teamId = GetTeamId();
            string postAddress = "https://api.ciscospark.com/v1/team/memberships/";
            HttpClient postClient = Config.GetClient(postAddress);
            var postContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("teamId", teamId),
                new KeyValuePair<string, string>("personEmail", email)
            });
            HttpResponseMessage response = postClient.PostAsync("", postContent).Result;

            member = response.Content.ReadAsAsync<Member>().Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                PostMessage("Cannot find user with email: " + email);
            }
            else
            {
                PostMessage(member.personEmail +" added to team!");
            }
            return member;
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
                HttpClient deleteClient = Config.GetClient(deleteAddress);
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
            HttpClient getClient = Config.GetClient(getAddress);
            HttpResponseMessage response = getClient.GetAsync(_roomId).Result;
            var room = response.Content.ReadAsAsync<Room>().Result;
            return room.teamId;
        }

        private static MemberData GetMembers(string roomId)
        {
            string getAddress = "https://api.ciscospark.com/v1/memberships/?roomId=" + roomId;
            HttpClient getClient = Config.GetClient(getAddress);

            HttpResponseMessage response = getClient.GetAsync("").Result;
            return response.Content.ReadAsAsync<MemberData>().Result;
        }

        private static MemberData GetTeamMembers(string teamId)
        {
            string getAddress = "https://api.ciscospark.com/v1/team/memberships?teamId=" + teamId;
            HttpClient getClient = Config.GetClient(getAddress);

            HttpResponseMessage response = getClient.GetAsync("").Result;
            return response.Content.ReadAsAsync<MemberData>().Result;
        }

        private static void TextMembers(List<Member> members, string message)
        {
            HttpClient textClient = Config.GetClient(Config.tropoBase);

            string phoneList = "[";
            foreach (var member in members)
            {
                phoneList += "\"" + member.phone + "\",";
            }

            phoneList = phoneList.Trim(',');
            phoneList += "]";

            var textClientContent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("token","4c6176697958676e6f66746448455548685350554b455a456a4166756e644c75486b5649724e6e776a4b756a"),
                    new KeyValuePair<string, string>("msg", "Emergancy Message: " + message),
                    new KeyValuePair<string, string>("networkToUse", "SMS"),
                    new KeyValuePair<string, string>("numbersToDial", phoneList),
                });
            textClient.PostAsync("", textClientContent);
        }

        private static void CallMembers(List<Member> members, string message)
        {
            HttpClient callClient = Config.GetClient(Config.tropoBase);

            string phoneList = "[";
            foreach (var member in members)
            {
                phoneList += "\"" + member.phone + "\",";
            }

            phoneList = phoneList.Trim(',');
            phoneList += "]";

            var callClientContent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("token","4c6176697958676e6f66746448455548685350554b455a456a4166756e644c75486b5649724e6e776a4b756a"),
                    new KeyValuePair<string, string>("msg", "Emergancy Message: " + message),
                    new KeyValuePair<string, string>("networkToUse", "INUM"),
                    new KeyValuePair<string, string>("numbersToDial", phoneList),
                });
            callClient.PostAsync("", callClientContent);
        }
        public static void AddTeamMember(Member member)
        {
            Member addedMember = TeamInvite(member.personEmail);
            member.personId = addedMember.personId;
            member.id = addedMember.id;

            MessageMember(member.personEmail,Config.welcomeMessage);

            DBAccess.InsertMember(member);
        }

        public static void NotifyAll(string message)
        {
            var members = DBAccess.GetMembers();
            TextMembers(members, message);
            //CallMembers(members, message);
        }

        
        public static async void CheckTemperature()
        {
            decimal latitude = 56.7264m;
            decimal longitude = 111.3803m;

            string getAddress = "api.openweathermap.org/data/2.5/weather/?lat="+latitude+"&lon="+longitude+"&APPID=47422c59303274ad97cce155373ede7c";

            HttpClient getClient = new HttpClient();
            getClient.BaseAddress = new Uri(getAddress);

            decimal temp = await GetTemp(getClient);

            if (temp < 373)
            {
                string mapsLink = "https://www.google.ca/maps?q=" + longitude + "," + latitude;
                NotifyAll("Weather warning, temperature: " + temp + "\n Location: " + mapsLink);
            }
        }

        private static async Task<decimal> GetTemp(HttpClient getClient)
        {
            decimal temp;
            using (var response = await getClient.GetAsync(""))
            {
                TemperatureObject tempObj = response.Content.ReadAsAsync<TemperatureObject>().Result;
                temp = Convert.ToDecimal(tempObj.main.temp);
            }
            return temp;
        }
    }
}