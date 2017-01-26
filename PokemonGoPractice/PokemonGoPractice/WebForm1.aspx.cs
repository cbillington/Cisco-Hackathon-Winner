using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PokemonGoPractice
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //configure the web request object
            string url = "http://pokeapi.co/api/v2/pokemon-form/";

            HttpRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            request.ContentType = "text/jSON; encoding='utf-8'";

            //send request, get json and convert to stream
            WebResponse response = request.GetResponseStream();
            Stream stream = response.GetResponseStream();

            //read stream into a dataset
            DataSet ds = new Dataset();
            ds.ReadJSON(stream);

            //bind to the dataset to gridview
            GvPokemon.Datasource = ds.Tables[0];
            GvPokemon.DataBind();
        }
    }
}