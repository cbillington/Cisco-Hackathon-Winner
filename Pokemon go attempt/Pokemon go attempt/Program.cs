using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_go_attempt
{
     
       

        public class Class1
        {
            private const string URL = "http://pokeapi.co/api/v2/pokemon-form/";
            private static string urlParameters = ""; //this needs to be static

            static void Main(string[] args)
            {

                string next20 = null;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL);
           

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

                // List data response.
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!

                try
                {
                    //one object in that whole list of pokemon...
                    //therefore we grab the whole information
                    var PokemonList = response.Content.ReadAsAsync<Pokemon>().Result;

                    next20 = PokemonList.next;

                    foreach (Result r in PokemonList.results)
                    {
                        Console.WriteLine(r.Name);
                    }
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.GetType() + e.Message);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }




            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri(next20);
            //sending a header with the application/json
            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

          
            HttpResponseMessage response2 = client2.GetAsync("").Result;

            if (response2.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!

                try
                {
                    //one object in that whole list of pokemon...
                    //therefore we grab the whole information
                    var PokemonList20 = response2.Content.ReadAsAsync<Pokemon>().Result;

                   

                    foreach (Result r in PokemonList20.results)
                    {
                        Console.WriteLine(r.Name);
                    }
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.GetType() + e.Message);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }




            Console.WriteLine("Press any key to Exit");
            Console.ReadKey();
            }
        }


    }
    

