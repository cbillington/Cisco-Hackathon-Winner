using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace APIwebService.Controllers
{
    public class MessageAlertController : ApiController
    {

        public IHttpActionResult GetMessageAlert()
        {
            return Ok("200");
        }

       [HttpPost]
        public IHttpActionResult PostMessageAlert( [FromBody] MessageAlert alert)
        {
            

            //https://qcwtgexljw.localtunnel.me
            HttpClient client = new HttpClient();
            string baseAddress = "https://api.ciscospark.com/v1/messages/";
            client.BaseAddress = new Uri(baseAddress);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            //adding authentication
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", "Yzk0ZTNjZTktYzk5Mi00NzlkLThjZjEtOTQ1OTM4ODk1ZDA4NjRhMWNmNGQtNDJj");

            // List data response.
            HttpResponseMessage response = client.GetAsync(alert.data.id).Result;  // Blocking call!

            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!

                try
                {
                    //one object in that whole list of pokemon...
                    //therefore we grab the whole information
                    var message = response.Content.ReadAsAsync<Message>().Result;

                    string postAddress = "https://api.ciscospark.com/v1/messages/";
                    HttpClient postClient = new HttpClient();
                    postClient.BaseAddress = new Uri(postAddress);
                    postClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    //adding authentication
                    postClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Yzk0ZTNjZTktYzk5Mi00NzlkLThjZjEtOTQ1OTM4ODk1ZDA4NjRhMWNmNGQtNDJj");

                    var postContent = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("toPersonId", "Y2lzY29zcGFyazovL3VzL1BFT1BMRS9hYzE5OTI2Mi1hNTg2LTQ3ZmUtYjU2OS05NDE5NDk3ZGRiN2Y"),
                new KeyValuePair<string, string>("text", message.text)
            });

                    HttpResponseMessage postResponse = postClient.PostAsync("", postContent).Result;

                    return Ok(message.text);
                  
                }
                catch (AggregateException e)
                {
                    throw e;
                }
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, response.ReasonPhrase);
            
            }










        }


    }
}
