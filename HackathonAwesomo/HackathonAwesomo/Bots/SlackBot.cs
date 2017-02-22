using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;

namespace HackathonAwesomo
{
    public static class SlackBot
    {
        private const string BaseUri = "https://slack.com/api/";

        public static void Execute(SlackMessage message)
        {
            string[] command = message.text.Split(' ');
            switch (command[0])
            {
                case "-e":
                    GetEmails();
                    break;
                case "-s":
                    SendText(command);
                    break;
                case "-p":
                    SendCall(command);
                    break;
                case "-m":
                    SendMessage("test");
                    break;
                case "-a":
                    MessageAll("Message");
                    break;
                case "-se":
                    SendEmail(message);
                    break;
                default:
                    break;
            }
        }

        private static void SendEmail(SlackMessage message)
        {
            
        }

        private static void GetEmails()
        {
            HttpClient getClient = new HttpClient();
            string uri = BaseUri + "users.list?token=" + Config.SlackToken2;
            getClient.BaseAddress = new Uri(uri);

            HttpResponseMessage response = getClient.GetAsync("").Result;
            var memberList = response.Content.ReadAsAsync<MemberList>().Result;
            foreach (Member member in memberList.members)
            {
                member.InsertDB();
            }
        }

        private static void MessageAll(string message)
        {
            HttpClient getClient = new HttpClient();
            string uri = BaseUri + "users.list?token=" + Config.SlackToken;
            getClient.BaseAddress = new Uri(uri);
            HttpResponseMessage response = getClient.GetAsync("").Result;
            var memberList = response.Content.ReadAsAsync<MemberList>().Result;

            foreach (Member member in memberList.members)
            {
                HttpClient msgClient = new HttpClient();
                uri = BaseUri + "im.open?token=" + Config.SlackToken +
                      "&user=" + member.id;
                msgClient.BaseAddress = new Uri(uri);
                HttpResponseMessage msgResponse = msgClient.GetAsync("").Result;
                var channelWrapper = msgResponse.Content.ReadAsAsync<ChannelWrapper>().Result;
                if (channelWrapper.channel != null)
                {
                    SendMessage(message, channelWrapper.channel.id);
                }
            }
        }

        private static void SendText(string[] command)
        {
            if (command[1] == null)
            {
                return;
            }
            TropoBot.NotifyUser(command[1], Config.TextMessage,"SMS");
        }

        private static void SendCall(string[] command)
        {
            if (command[1] == null)
            {
                return;
            }
            TropoBot.NotifyUser(command[1], Config.CallMessage, "INUM");
        }

        public static void SendMessage(string message, string channel = "")
        {
            if (channel == "")
            {
                channel = Config.SlackChannel;
            }
            HttpClient getClient = new HttpClient();
            string uri = BaseUri + "chat.postMessage?" +
                         "token=" + Config.SlackToken +
                         "&channel=" + channel +
                         "&text=" + message;
            getClient.BaseAddress = new Uri(uri);
            var response = getClient.GetAsync("");
        }
    }
}