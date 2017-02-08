using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using HackathonBEES;

namespace HackathonBEES
{
    public static class TwitterBot
    {
        public static void postLastTweet(string search)
        {
            string baseAddress = "https://api.twitter.com/1.1/search/tweets.json?q="+search;
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
                        SparkBot.PostMessage(tweet.text + " " + tweet.user.screen_name);
                    }
                }
            }
        }
    }
}