using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIwebService.Controllers
{
    public class TropoController : ApiController
    {
        public void PostTropo([FromBody] Message message)
        {
            Bot.PostMessage(message.text);
        }
    }
}
