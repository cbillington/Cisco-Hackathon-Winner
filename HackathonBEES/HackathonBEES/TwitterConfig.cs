using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Tweetinvi;

namespace HackathonBEES
{
    public static class TwitterConfig
    {
        // Application only auth request header
        public static AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer",
            "AAAAAAAAAAAAAAAAAAAAAPxnzAAAAAAAvl9uhUNMSY4txqtLh4H02a2TXD8%3DvMjSCgmrSONxIl0iUo3rnyOrE6PPkC0rYB0YTTtyQYOoyho52T");

        // Tokens for generating OAuth1 header
        private static readonly string consumerKey = "phPtjk73YAMrGAzguY7fVGmxc";
        private static readonly string consumerKeySecret = "UeSvB6GMTghSCo3EiVWn7cmLGdjIrTiBoUkQkYi8Wawj4aNdKi";
        private static readonly string accessToken = "829034637712187392-ISxT1p3UEzsC6qZqzbFpxf8jyTgGAtS";
        private static readonly string accessTokenSecret = "YvrA5HMxaMyf5pyZQUHMtdmTetDED7MAATZb1VmRVW9dO";

        // Application only httpclient
        public static HttpClient GetClient(string address)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(address);

            //adding authentication
            client.DefaultRequestHeaders.Authorization = authHeader;

            return client;
        }

        public static void setCredentials()
        {
            Auth.SetUserCredentials(consumerKey, consumerKeySecret, accessToken, accessTokenSecret);
        }


        public static string GetAuthHeader(string url)
        {
            var oauth_token = accessToken;
            var oauth_token_secret = accessTokenSecret;
            var oauth_consumer_key = consumerKey;
            var oauth_consumer_secret = consumerKeySecret;

            // oauth implementation details
            var oauth_version = "1.0";
            var oauth_signature_method = "HMAC-SHA1";

            // unique request details
            var oauth_nonce = "G7F5H6";
            var timeSpan = DateTime.UtcNow
                           - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            // message api details
            var resource_url = url;
            // create oauth signature
            var baseFormat = "oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method={2}" +
                             "&oauth_timestamp={3}&oauth_token={4}&oauth_version={5}";

            var baseString = string.Format(baseFormat,
                oauth_consumer_key,
                oauth_nonce,
                oauth_signature_method,
                oauth_timestamp,
                oauth_token,
                oauth_version
            );

            baseString = string.Concat("GET&", Uri.EscapeDataString(resource_url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(oauth_consumer_secret),
                "&", Uri.EscapeDataString(oauth_token_secret));

            string oauth_signature;
            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            // create the request header
            var headerFormat = "oauth_consumer_key=\"{3}\",oauth_token=\"{4}\"," +
                               "oauth_signature_method=\"{1}\",oauth_timestamp=\"{2}\"," +
                               "oauth_nonce=\"{0}\",oauth_version=\"{6}\",oauth_signature=\"{5}\"";

            var authHeader = string.Format(headerFormat,
                Uri.EscapeDataString(oauth_nonce),
                Uri.EscapeDataString(oauth_signature_method),
                Uri.EscapeDataString(oauth_timestamp),
                Uri.EscapeDataString(oauth_consumer_key),
                Uri.EscapeDataString(oauth_token),
                Uri.EscapeDataString(oauth_signature),
                Uri.EscapeDataString(oauth_version)
            );
            return authHeader;
        }
    }
}