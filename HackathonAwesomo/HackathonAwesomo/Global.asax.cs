using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace HackathonAwesomo
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static Timer timer;
        protected void Application_Start()
        {
            var howLongTillTimerFirstGoesInMilliseconds = 10000;
            var intervalBetweenTimerEventsInMilliseconds = 30000;
            var timerCallback = new TimerCallback(CheckPatients);
            timer = new Timer(timerCallback,
                null, // if you need to provide state to the function specify it here
                howLongTillTimerFirstGoesInMilliseconds,
                intervalBetweenTimerEventsInMilliseconds
            );
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void CheckPatients(object state)
        {
            RelayrBot.CheckPatients();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (timer != null)
            {
                timer.Dispose();
            }
        }
    }
}
