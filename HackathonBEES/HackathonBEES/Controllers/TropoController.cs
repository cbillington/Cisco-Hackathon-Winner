using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HackathonBEES
{
    public class TropoController : ApiController
    {
        public void PostTropo([FromBody] Emergency emergency)
        {
            string message = emergency.result.transcription;
            string identifier = emergency.result.identifier;
            string[] result = identifier.Split(',');

            EmergencyCall call = new EmergencyCall();
            call.transcription = message;
            call.identifier = result[0];
            call.latitude = Convert.ToDecimal(result[1]);
            call.longitude = Convert.ToDecimal(result[2]);

            string mapsLink = "https://www.google.ca/maps?q=" + call.latitude + "," + call.longitude;

            SparkBot.NotifyText(message + "\n" + mapsLink);
            
            SparkBot.PostMessage("Emergency notification: " + message + "\n" + "Emergency location: " + mapsLink);

            DBAccess.InsertEmergencyCall(call);
        }
    }
}
