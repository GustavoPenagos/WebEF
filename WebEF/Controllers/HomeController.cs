using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebEF.Models;
using Newtonsoft.Json;
using WebEF.Models.API;

namespace WebEF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<Persona> jsonPersona = new List<Persona>();
            var url = _configuration["UrlAPI"];
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = await httpClient.GetAsync(url+ "/get/persona");
            if(httpResponse.IsSuccessStatusCode)
            {
                string responseBody = await httpResponse.Content.ReadAsStringAsync();
                jsonPersona = JsonConvert.DeserializeObject<List<Persona>>(responseBody);
            }

            return View(jsonPersona);
        }

        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
