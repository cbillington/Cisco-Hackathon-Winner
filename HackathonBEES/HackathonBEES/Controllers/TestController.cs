using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HackathonBEES.Controllers
{
    public class TestController : ApiController
    {
        public void PostTest([FromBody] Test message)
        {
            SparkBot.PostMessage("New message from " + message.Name + ": " + message.Message);
        }
    }
}
