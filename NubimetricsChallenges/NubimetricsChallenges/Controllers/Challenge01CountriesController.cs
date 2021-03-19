using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NubimetricsChallenge01Countries.Controllers
{
    
    [Route("api/Countries")]
    [ApiController]
    public class Challenge01CountriesController : ControllerBase
    {


        static readonly HttpClient client = new HttpClient();



        /*
         * 
         * Metodo asyncrono que obtiene el pais 
         * 
         */
        [HttpGet("{nameCountry}")]
        public async Task<ActionResult> GetDataAsync(string nameCountry)
        {
            if (nameCountry == "BR" || nameCountry == "CO")
            {
                return Unauthorized(new { success = false, message = "Acceso no permitido." });
            }
            else
            {
                if (nameCountry == "AR")
                {
                    string response = await GetDataFromMercadoLibre("https://api.mercadolibre.com/classified_locations/countries/AR");
                    return Ok(JsonConvert.DeserializeObject<JObject>(response));
                }
                else {

                    return Ok(new { success = false, message = "Parámetro no valido." });
                }

            }
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
