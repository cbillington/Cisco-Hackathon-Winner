using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace HackathonBEES
{
    public static class SparkBot
    {
        public static string _roomId = Config.roomId;
        
        public static void ExecuteCommand(string roomId, string command, Data personData)
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

                case "text":
                    TextUser(commandParameters);
                    break;

                case "emergency":
                    NotifyText(commandParameters);
                    //NotifyCall(commandParameters);
                    PostMessage("Users notified!");

                    var emergCall = new EmergencyCall();
                    emergCall.transcription = commandParameters;
                    emergCall.identifier = personData.personEmail;
                    DBAccess.InsertEmergencyCall(emergCall);
                    break;

                case "checkTemperature":
                    CheckTemperature();
                    break;

                case "checkSensor":
                    CheckSensor();
                    break;

                case "conference":
                    StartConference(commandParameters);
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

                case "searchTweet":
                    TwitterBot.postLastTweet(commandParameters);
                    break;

                case "streamTweet":
                    TwitterBot.UserStream();
                    break;

                case "searchStream":
                    TwitterBot.StreamTwitter(commandParameters);
                    break;

                default:
                    PostMessage(command + " is not a valid command!");
                    break;
            }
        }

        private static void TextUser(string commandParameters)
        {
            HttpClient textClient = Config.GetClient(Config.tropoBase);

            string[] command = commandParameters.Split(',');

            string number = command[0];
            string message = command[1];

            string phoneList = "[\"" + number + "\"]";

            var textClientContent = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("token","4c6176697958676e6f66746448455548685350554b455a456a4166756e644c75486b5649724e6e776a4b756a"),
                    new KeyValuePair<string, string>("msg", "Private Message: " + message),
                    new KeyValuePair<string, string>("networkToUse", "SMS"),
                    new KeyValuePair<string, string>("numbersToDial", phoneList),
                });
            textClient.PostAsync("", textClientContent);
            PostMessage("Message sent to " + number);
        }

        private static void CheckSensor()
        {
            HttpClient getClient = new HttpClient();
            string getAddress = "https://api.relayr.io/devices/e84e2eb1-80bf-48e8-a5c1-c710c5310281/readings";
            getClient.BaseAddress = new Uri(getAddress);
            getClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));

            getClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "RNClQ25HqmxEtFquKuGRAv4b8OAnFbzvh5MpFgNtyqpJOOcngIQEGwf0Xw0vgF4n");

            HttpResponseMessage response = getClient.GetAsync("").Result;
            var readingWrapper = response.Content.ReadAsAsync<ReadingWrapper>().Result;

            decimal temperature = Convert.ToDecimal(readingWrapper.readings[0].value);

            Sensor sensor = new Sensor();
            string status = "Normal";

            string message = "";
            if (temperature > 30)
            {
                message = "Critical readings at: " + "\n" +
                          "LSD: 66-27-75-05W4 \n" +
                          "Temperature: " + temperature;

                SparkBot.NotifyText(message);
                status = "Alert";
            }
            else
            {
                message = "LSD: 66-27-75-05W4 \n" +
                          "Temperature: " + temperature;
            }
            SparkBot.PostMessage(message);

            sensor.LSD = "66-27-75-05W4";
            sensor.temperature = temperature.ToString();
            sensor.status = status;

            DBAccess.InsertSensor(sensor);
        }

        private static void StartConference(string team)
        {
            foreach (var member in DBAccess.GetMembersByTeam(team))
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
                        "invite (User Email) \n" +
                        "remove (User Email)\n" +
                        "teamInvite (User Email)\n" +
                        "teamRemove (User Email)\n" +
                        "emergency (Message)\n" +
                        "checkTemperature \n" +
                        "checkSensor \n" +
                        "conference (A or B)");
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
                    new KeyValuePair<string, string>("msg", "Emergency Message: " + message),
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

        public static void NotifyText(string message)
        {
            var members = DBAccess.GetMembers();
            TextMembers(members, message);
        }

        public static void NotifyCall(string message)
        {
            var members = DBAccess.GetMembers();
            CallMembers(members, message);
        }

        public static async Task CheckTemperature()
        {
            decimal latitude = 56.7264m;
            decimal longitude = -111.3803m;

            string getAddress =
                "http://api.openweathermap.org/data/2.5/weather/?lat=56.7264&lon=-111.3803&APPID=47422c59303274ad97cce155373ede7c";

            HttpClient getClient = new HttpClient();
            getClient.BaseAddress = new Uri(getAddress);

            decimal temp = await Task.FromResult<decimal>(GetTemp(getClient));

            if (temp < 373)
            {
                string mapsLink = "https://www.google.ca/maps?q=" + latitude + "," + longitude;
                NotifyText("Weather warning, temperature: " + temp + "\n Location: " + mapsLink);
                PostMessage("Weather warning, temperature: " + temp + "\n Location: " + mapsLink);
            }
        }

        private static decimal GetTemp(HttpClient getClient)
        {
            decimal temp;
            using (var response = getClient.GetAsync("").Result)
            {
                TemperatureObject tempObj = response.Content.ReadAsAsync<TemperatureObject>().Result;
                temp = Convert.ToDecimal(tempObj.main.temp);
            }
            return temp;
        }
    }
}