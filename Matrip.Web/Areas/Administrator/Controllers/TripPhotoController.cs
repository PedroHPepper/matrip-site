using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Matrip.Domain.Models.AccountModels;
using Matrip.Domain.Models.Entities;
using Matrip.Web.Libraries.Archive;
using Matrip.Web.Libraries.Filter;
using Matrip.Web.Libraries.Login;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Matrip.Web.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [AdminGuideAuthentication]
    public class TripPhotoController : Controller
    {
        private IHostingEnvironment _env;
        private IConfiguration _configuration;
        private UserLogin _userLogin;

        private HttpClient client;
        public TripPhotoController(IHostingEnvironment env, IConfiguration configuration, UserLogin UserLogin)
        {
            _env = env;
            _configuration = configuration;
            _userLogin = UserLogin;
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(_configuration.GetValue<string>("Matrip.API:ApiLink"));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddTripPhoto([FromForm] int tripID, [FromForm] string TripName, [FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                TokenModel JWToken = _userLogin.GetToken();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

                HttpResponseMessage response = await client.GetAsync("Photo/AddTripPhoto/" + tripID);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    ma13tripphoto tripphoto = JsonConvert.DeserializeObject<ma13tripphoto>(result);

                    await PhotoManager.AddTripImage(file, tripphoto);
                    TempData["MSG_S"] = "Foto adicionada com sucesso!";
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _userLogin.Logout();
                    return RedirectToAction("Index", "Home", new { Area = "" });
                }
            }
            return RedirectToAction("UpdateTrip", "AdminTrip", new { TripName });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTripPhoto([FromForm] int tripID, [FromForm] string TripName, [FromForm] int photoquantity, [FromForm] int position, [FromForm] string filePath)
        {
            TokenModel JWToken = _userLogin.GetToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + JWToken.token);

            HttpResponseMessage response = await client.GetAsync("Photo/DeleteTripPhoto/" + tripID);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    bool isDeleted = PhotoManager.ExcludeTripImage(filePath, photoquantity, position, tripID);
                    if (isDeleted)
                    {
                        TempData["MSG_S"] = "Foto " + position + " excluída com sucesso!";
                    }
                    else
                    {
                        TempData["MSG_E"] = "Erro ao excluir foto!";
                    }
                }
                catch(Exception e)
                {
                    TempData["MSG_E"] = e.Message;
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _userLogin.Logout();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            return RedirectToAction("UpdateTrip", "AdminTrip", new { TripName });
        }
    }
}