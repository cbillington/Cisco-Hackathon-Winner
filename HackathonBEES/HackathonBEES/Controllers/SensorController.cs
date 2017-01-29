using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HackathonBEES
{
    public class SensorController : ApiController
    {
        private const decimal CRIT_TEMP = 40m;
        private const decimal CRIT_PRESSURE = 5000m;

        public void PostSensor([FromBody] Sensor sensor)
        {
            string status = "Normal";
            if (Convert.ToDecimal(sensor.temperature) > CRIT_TEMP || Convert.ToDecimal(sensor.pressure) > CRIT_PRESSURE)
            {
                string message = "Critical readings LSD " + sensor.LSD + "\n" +
                                 "Temperature: " + sensor.temperature + "\n" +
                                 "Pressure: " + sensor.pressure;

                SparkBot.NotifyText(message);
                SparkBot.PostMessage(message);
                status = "Alert";
            }
            sensor.status = status;
            DBAccess.InsertSensor(sensor);
        }
    }
}
