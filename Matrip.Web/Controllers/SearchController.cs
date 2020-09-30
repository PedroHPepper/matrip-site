using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Matrip.Web.Controllers
{
    public class SearchController : Controller
    {
        private IConfiguration _configuration;
        private HttpClient client;
        public SearchController(IConfiguration configuration)
        {
            _configuration = configuration;
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public async Task<IActionResult> SearchCities([FromQuery] string cityText, [FromQuery] string UF)
        {
            HttpResponseMessage response = await client.GetAsync("City/SearchCities?cityText=" + cityText + "&UF=" + UF);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                List<ma09city> cities = JsonConvert.DeserializeObject<List<ma09city>>(result);
                List<string> citesStringList = new List<string>();
                cities.ForEach(e =>
                {
                    citesStringList.Add(e.ma09name);
                });

                string[] citiesString = citesStringList.ToArray();
                
                return Ok(citiesString);
            }
            return Ok();
        }

        public async Task<IActionResult> SearchCitiesWithUF([FromQuery] string cityText)
        {
            HttpResponseMessage response = await client.GetAsync("City/GetHomeCityList?cityText=" + cityText);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                List<ma09city> cities = JsonConvert.DeserializeObject<List<ma09city>>(result);
                if (cities.Any())
                {
                    List<string> citesStringList = new List<string>();
                    cities.ForEach(e =>
                    {
                        citesStringList.Add(e.ma09name + " - " + e.ma08uf.ma08UFInitials);
                    });

                    string[] citiesString = citesStringList.ToArray();
                    return Ok(citiesString);
                }

                return Ok(null);
                
            }
            return Ok();
        }

        public async Task<IActionResult> SearchPartners([FromQuery] string PartnerText)
        {
            HttpResponseMessage response = await client.GetAsync("Partner/SearchPartners?PartnerText=" + PartnerText);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                List<ma25partner> partners = JsonConvert.DeserializeObject<List<ma25partner>>(result);
                List<string> partnersStringList = new List<string>();
                partners.ForEach(e =>
                {
                    partnersStringList.Add(e.ma25name);
                });

                string[] partnersString = partnersStringList.ToArray();

                return Ok(partnersString);
            }
            return Ok();
        }


        public async Task<IActionResult> SearchTrip([FromQuery] string TripNameText)
        {
            TripNameText = HttpUtility.UrlEncode(TripNameText);
            HttpResponseMessage response = await client.GetAsync("Trip/SearchTrips?TripNameText=" + TripNameText);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                List<string> triplistName = JsonConvert.DeserializeObject<List<string>>(result);
                string[] triplistNameString = triplistName.ToArray();

                return Ok(triplistNameString);
            }
            return Ok(null);
        }
    }
}