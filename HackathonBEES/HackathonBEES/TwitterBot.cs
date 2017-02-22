using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using HackathonBEES;
using Tweetinvi;
using Tweetinvi.Models;

namespace HackathonBEES
{
    public static class TwitterBot
    {
        public static string TestHeader =
           "oauth_consumer_key=\"phPtjk73YAMrGAzguY7fVGmxc\",oauth_token=\"829034637712187392-ISxT1p3UEzsC6qZqzbFpxf8jyTgGAtS\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"1486937540\",oauth_nonce=\"0bCDTN\",oauth_version=\"1.0\",oauth_signature=\"179Kbw3U%2FBnA9FXMmUwn8ichPKc%3D\"";
        public static async void UserStream()
        {
            string requestUri = "https://stream.twitter.com/1.1/statuses/filter.json?track=trump";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(requestUri);

                TestHeader = TwitterConfig.GetAuthHeader(requestUri);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", TestHeader);
                httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

                var stream = await httpClient.GetStreamAsync(requestUri).ConfigureAwait(false);

                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        //We are ready to read the stream
                        var currentLine = reader.ReadLine();
                        SparkBot.PostMessage(currentLine);
                        Thread.Sleep(500);
                    }
                }
            }
        }

        public static void StreamTwitter(string search)
        {
            TwitterConfig.setCredentials();
            var stream = Tweetinvi.Stream.CreateFilteredStream();
            stream.AddTweetLanguageFilter(LanguageFilter.English);
            stream.AddTrack(search);
            stream.MatchingTweetReceived += (sender, args) =>
            {
                SparkBot.PostMessage(args.Tweet.Text);
            };
            stream.StartStreamMatchingAllConditionsAsync();
        }

        public static void postLastTweet(string search)
        {
            string baseAddress = "https://api.twitter.com/1.1/search/tweets.json?q=" + search;
            HttpClient getClient = TwitterConfig.GetClient(baseAddress);

            HttpResponseMessage response = getClient.GetAsync("").Result;

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                var tweets = response.Content.ReadAsAsync<Tweets>().Result;

                foreach (Tweet tweet in tweets.statuses)
                {
                    if (tweet.user.followers_count > 1000)
                    {
                        //SparkBot.PostMessage(tweet.text + " " + tweet.user.screen_name);
                        SunshineBot.NotifyMessage(tweet.text);
                    }
                }
            }
        }
    }
}