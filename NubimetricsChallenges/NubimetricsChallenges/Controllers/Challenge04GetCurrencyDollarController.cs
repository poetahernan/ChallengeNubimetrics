using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace NubimetricsChallenge01Countries.Controllers
{
  
    [Route("api/GetCurrencyDollar")]
    [ApiController]
    public class Challenge04GetCurrencyDollarController : ControllerBase
    {
        static readonly HttpClient client = new HttpClient();


        //Cambiar la direccion donde se guarda los archivos

        public string DIR_JSON = @"D:\Challenge04StructureCurrencies.json";
        public string DIR_CSV  = @"D:\Challenge04RatiosCurrencies.csv";

        //api obtiene las currencies
        [HttpGet("Conversion")]
        public async Task<ActionResult> GetCurrenciesAsync(string search)
        {

            var response = await GetDataFromMercadoLibre("https://api.mercadolibre.com/currencies");
            var ratiosFromCurrencies = new List<object>();
            var res = JsonConvert.DeserializeObject<JArray>(response);
            JArray schema = JsonConvert.DeserializeObject<JArray>(response);

                                  
            //Guardar schema
            SaveSchemaJson(schema);


            string responseConvert = "";

            res.Select(obj => (JObject)obj).ToList()
                .ForEach(async node =>
                {

                    node.Properties().ToList()
                        .ForEach(async d =>
                        {
                            if (d.Name == "id")
                            {
                                responseConvert = GetDataFromMercadoLibreSynchronous("https://api.mercadolibre.com/currency_conversions/search?from=" + d.Value.ToString() + "&to=USD");
                            }
                        });
                    if (responseConvert == "")
                    {
                        node.Add("todolar", "");
                    }
                    else {

                        node.Add("todolar", JObject.Parse(responseConvert));
                        ratiosFromCurrencies.Add(new { Ratio = JObject.Parse(responseConvert)["ratio"].ToString() });
                    }

                });

            //Guardar ratios
            SaveLocalCsv(DIR_CSV, ratiosFromCurrencies);

            return Ok(res);

        }


        public void SaveSchemaJson(JArray schema)
        {
            schema.Select(obj => (JObject)obj).ToList()
             .ForEach(node =>
             {
                 node.Properties().ToList()
                     .ForEach(d =>
                     {
                         d.Value = d.Value.Type.ToString();
                     });
             });

            var schemaJson = schema.Select(obj => (JObject)obj).ToList()[0];
            SaveToLocal(schemaJson.ToString(), DIR_JSON);
        }

        /*
        * 
        * Metodo que obtiene información desde mercado libre asyncrono
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


        /*
        * 
        * Metodo que obtiene información desde mercado libre syncrono
        * 
        */

        static string GetDataFromMercadoLibreSynchronous(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content;

                        return responseContent.ReadAsStringAsync().Result;
                    }
                }
                return "";
            }
            catch (HttpRequestException ex)
            {
                return ex.ToString();
            }
        }

                     
        /*
         * https://github.com/JoshClose/CsvHelper
        */
        /*
         * Metodo que escribe .csv
         */
        public void SaveLocalCsv(string contentFile, List<object> ratios)
        {
            using (var writer = new StreamWriter(contentFile))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(ratios);
            }
        }


        /*
         * Metodo que escribe json
         */
        public void SaveToLocal(string contentFile, string nameFile)
        {
            TextWriter writer;
            using (writer = new StreamWriter(nameFile, append: false))
            {
                writer.WriteLine(contentFile);
            }
        }

    }
}