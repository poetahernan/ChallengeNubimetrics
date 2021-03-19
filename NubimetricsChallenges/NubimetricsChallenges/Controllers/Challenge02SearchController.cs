using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NubimetricsChallenge01Countries.Controllers
{

    [Route("api/Search")]
    [ApiController]
    public class Challenge02SearchController : ControllerBase
    {

        static readonly HttpClient client = new HttpClient();

        /*
         * 
         * Metodo asyncrono que obtiene la busqueda
         * 
         */
        [HttpGet("{search}")]
        public async Task<ActionResult> GetSearchAsync(String search)
        {
                

            /*○ id
            ○ site_id
            ○ title
            ○ price
            ○ seller.id
            ○ permalink*/

            var response = await GetDataFromMercadoLibre("https://api.mercadolibre.com/sites/MLA/search?q="+ search);

            var parsing = JsonConvert.DeserializeObject<JObject>(response);

            ((JArray)parsing.Property("results").Value)
                .Select(obj => (JObject)obj).ToList()
                .ForEach(node =>node.Properties().ToList()
                        .ForEach(p =>
                        {
                            //mantenemos los nodos..
                            if (p.Name == "id" || 
                                p.Name == "site_id" || 
                                p.Name == "title" || 
                                p.Name == "price" || 
                                p.Name == "seller" ||
                                p.Name == "permalink"
                                )
                            {

                                if (p.Name == "seller")
                                {
                                    // removemos todos los nodos menos "id"
                                    var childSeller = JsonConvert.DeserializeObject<JObject>(p.Value.ToString());
                                    childSeller.Properties().Where(attr => attr.Name != "id").ToList().ForEach(attr => attr.Remove());
                                    p.Value = childSeller;
                                }
                            }
                            else {
                                p.Remove();
                            }
                               
                        }));

           

            return Ok(parsing);



        }

        /*
         * 
         * Metodo que obtiene información desde mercado libre
         * 
         */
        static async Task<String> GetDataFromMercadoLibre(String url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // string responseBody = await client.GetDataFromMercadoLibre(url);

                return responseBody.ToString();
            }
            catch (HttpRequestException ex)
            {
                return ex.ToString();
            }

        }

    }
}