using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HackathonAwesomo
{
    public class SlackController : ApiController
    {
        public IHttpActionResult PostSlack([FromBody] SlackMessage message)
        {
            SlackBot.Execute(message);
            return Ok();
        }
    }
}
