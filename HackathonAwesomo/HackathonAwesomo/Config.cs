using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackathonAwesomo
{
    public static class Config
    {
        public static string SlackToken2 = "xoxb-141291209760-YTBVdqISz1b9RIf03ITW0pep";
        public static string SlackChannel2 = "C4600N95Z";

        public static string SlackToken = "xoxb-140950723587-jbcykrtDeA1aDqGQcDqaGATX";
        public static string SlackChannel = "C440JCDPT";

        public static string RelayrID = "597a1471-9c0c-4b7a-aeed-cf5c5061aac2";

        public static string RelayrToken = "lOVfoSu9WRXcmWGBIPbHsrpwZO2z48mw30zuPYFIamQLFl8BKinEsTloMbAV0CgW";

        public static string RelayrModel = "4128fb19-8561-4ddb-883d-f52dc03d7863";

        public static string DeviceId = "5a18ff80-bd0c-464b-8eac-986eecaa5415";

        public static string TropoToken =
            "4c6176697958676e6f66746448455548685350554b455a456a4166756e644c75486b5649724e6e776a4b756a";

        public static string FromNumber = "15874366543";

        public static string TextMessage =
            "Congratulations! You are one of the seven people to qualify for the bonus prize, be the first person to yell \"Calgary Hacks!\" during the presentation to win. Good luck!";

        public static string CallMessage =
            "Welcome to the Calgary hacking Competition, we look forward to seeing you perform at your best! We have many prizes available for the best ideas out there. Be sure to write hashtag calgary hacks in the general chatroom for a chance to win a bonus prize!";

        public static string OneSignalTokenOld = "MDEyODE2NTUtNDIzNy00MmZlLWJkNWQtNDk0Yjg1ZWFiYWQ0";
        public static string OneSignalAppIdOld = "f4c7b4bd-1b2f-452c-aa1a-17427cde9078";

        public static string OneSignalToken = "Y2VhZDc4N2ItYzlhMC00MjEwLTkxYTktOThiOTc2ZTczNjUw";
        public static string OneSignalAppId = "865ae748-d04d-47a0-ac8a-595a25999f5f";

        public static string connectionString =
            @"Data Source=ICTVM-FQQ06UJG2\SQLEXPRESS;Initial Catalog=Awesomo;Integrated Security=True";

        public static string GetReadingsQuery = @" SELECT TOP(10) * FROM readings                                 
                                                where meaning = @meaning
                                                and device = @device
                                                order by sqlid desc";

        public static string EmailUser = "moxue2017@gmail.com";
        public static string EmailPass = "";

        public static string GetLastReading = @"select max(received) as time from Readings
                                                where meaning = @meaning
                                                and device = @device";

        public static string GetPatientsQuery = @"select * from Patients";

        public static string GetPatientsDetailsQuery = @"SELECT * FROM Patients where id = @id";
    }
}