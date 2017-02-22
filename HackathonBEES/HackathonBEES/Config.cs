using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace HackathonBEES
{
    public static class Config
    {
        //ngrok http 80 -host-header="localhost:80"
        public static string botName = "BEEZBOT";

        public static string botId = "Y2lzY29zcGFyazovL3VzL1BFT1BMRS8zMDE5M2ZjMC0zYzQxLTQxZmYtYjY1OS00YTJhNmUwMjU4MGY";

        public static AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Bearer",
            "M2Y2MzM4ZDEtZmMzOC00YjcyLWI2ZDUtZTZkZWJmNzEyNzI1ZjZhOWMxMmItZTIw");

        public static string teamId = "Y2lzY29zcGFyazovL3VzL1RFQU0vNmM4OGY5YjAtZTRmNy0xMWU2LTlmNTctODVkNDI5YmNiOWI4";

        public static string roomId = "Y2lzY29zcGFyazovL3VzL1JPT00vNmM4OGY5YjAtZTRmNy0xMWU2LTlmNTctODVkNDI5YmNiOWI4";

        public static string tropoBase = @"https://api.tropo.com/1.0/sessions";

        public static string connectionString =
            @"Data Source=ICTVM-FQQ06UJG2\SQLEXPRESS;Initial Catalog=Beez;Integrated Security=True";

        public static string insertMemberQuery = @"INSERT into members values (@id,@personId,@personEmail,@name,@phone)";

        public static string getMembersQuery = @"SELECT * FROM Members";

        internal static string getMembersByTeamQuery = @"SELECT * FROM Members WHERE role = @role";

        public static string welcomeMessage = "Welcome to the BEEZTEAM! \n" +
                                              "If you need any assistance, please talk to me in the general team space. \n" + "Have a great day!";

        public static string getEmergQuery = @"SELECT * FROM EmergencyCalls";
        public static string getSensorQuery = @"SELECT * FROM Sensors";

        public static HttpClient GetClient(string address)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(address);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //adding authentication
            client.DefaultRequestHeaders.Authorization = authHeader;

            return client;
        }
    }
}