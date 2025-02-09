using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebEF.Models;
using WebEF.Models.API;
using WebEF.Repository.IRequest;

namespace WebEF.Controllers
{
    public class HomeController(IPetition petition) : Controller
    {
        private readonly IPetition _petition = petition;
        
        public async Task<IActionResult> Index(Buscar buscar)
        {
            string response = string.Empty;
            if ((!buscar.Busqueda && string.IsNullOrEmpty(buscar.Documento)) || (buscar.Busqueda && string.IsNullOrEmpty(buscar.Documento)))
            {
                response = await _petition.ClientAsync("Get", "/get/person");
                if (!string.IsNullOrEmpty(response))
                {
                    return View(JsonConvert.DeserializeObject<List<Persona>>(response));
                }
                return RedirectToAction("Error", "Home");
            }
            else
            {
                response = await _petition.ClientAsync("Get", "/get/personbyid?documento=" + buscar.Documento);
                if (!string.IsNullOrEmpty(response))
                {
                    return View(JsonConvert.DeserializeObject<List<Persona>>(response));
                }
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
