using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebEF.Models;
using Newtonsoft.Json;
using WebEF.Models.API;
using System;

namespace WebEF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient httpClient = new();

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Persona> jsonPersona = new List<Persona>();
            var url = _configuration["UrlAPI"];
            HttpResponseMessage httpResponse = await httpClient.GetAsync(url + "/get/persona");
            if (httpResponse.IsSuccessStatusCode)
            {
                string responseBody = await httpResponse.Content.ReadAsStringAsync();
                jsonPersona = JsonConvert.DeserializeObject<List<Persona>>(responseBody);
            }

            return View(jsonPersona);   
        }

        public async Task<IActionResult> DeleteUser(string tipo, string documento)
        {
            var url = _configuration["UrlAPI"];
            int tipoDoc = await Documentos(tipo);
            string delete = string.Format("/delete/persona?tipo={0}&documento={1}", tipoDoc, documento);
            HttpResponseMessage httpResponse = await httpClient.DeleteAsync(url + delete);
            if (httpResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Error", "Home");
        }
        
        private async Task<int> Documentos(string tipo)
        {
            var url = _configuration["UrlAPI"];
            HttpResponseMessage httpResponse = await httpClient.GetAsync(url + "/get/documentos?documento=" + tipo);
            if (httpResponse.IsSuccessStatusCode)
            {
                string responseBody = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(responseBody);
            }
            return 0;

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
