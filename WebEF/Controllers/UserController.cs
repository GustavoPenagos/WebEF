using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using WebEF.Models.API;
using WebEF.Repository.IRequest;

namespace WebEF.Controllers
{
    public class UserController (IPetition petition): Controller
    {
        private readonly IPetition _petition = petition;

        public async Task<IActionResult> SaveUser()
        {
            string? response = await _petition.ClientAsync("Get", "/get/AllDocuments");
            if (!string.IsNullOrEmpty(response))
            {
                ViewBag.Documento = JsonConvert.DeserializeObject<List<Documentos>>(response);
            }
            response = await _petition.ClientAsync("Get", "/get/AllMartalStatus");
            if (!string.IsNullOrEmpty(response))
            {
                ViewBag.Civil = JsonConvert.DeserializeObject<List<EstadoCivil>>(response);
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Add(Persona persona)
        {
            try
            {
                persona.Fecha = DateTime.Now.ToString("dd/MM/yyyy");
                StringContent stringContent = new (JsonConvert.SerializeObject(persona), Encoding.UTF8, "application/json");
                string? response = await _petition.ClientAsync("Post", "/post/person", stringContent);
                if (!string.IsNullOrEmpty(response))
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Error", "Home");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Delete(string tipo, string documento)
        {

            int tipoDoc = await Documentos(tipo);
            string delete = string.Format("/delete/person?tipo={0}&documento={1}", tipoDoc, documento);
            string? response = await _petition.ClientAsync("Delete", delete);
            if (!string.IsNullOrEmpty(response))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Error", "Home");
        }

        public async Task<IActionResult> Update(string documento)
        {
            List<Persona>? personas = new List<Persona>();
            var response = await _petition.ClientAsync("Get", "/get/personbyid?documento=" + documento);
            if (!string.IsNullOrEmpty(response))
            {
                personas = JsonConvert.DeserializeObject<List<Persona>>(response);
                
            }
            response = await _petition.ClientAsync("Get", "/get/AllDocuments");
            if (!string.IsNullOrEmpty(response))
            {
                ViewBag.Documento = JsonConvert.DeserializeObject<List<Documentos>>(response);
            }
            response = await _petition.ClientAsync("Get", "/get/AllMartalStatus");
            if (!string.IsNullOrEmpty(response))
            {
                ViewBag.Civil = JsonConvert.DeserializeObject<List<EstadoCivil>>(response);
                return View(personas);
            }
            return View();
        }

        public async Task<IActionResult> UpdateUser(Persona persona)
        {
            if (ValidData(persona))
            {
                StringContent stringContent = new(JsonConvert.SerializeObject(persona), Encoding.UTF8, "application/json");
                string? response = await _petition.ClientAsync("Put", "/put/person", stringContent);
            }
            return RedirectToAction("Index", "Home");
        }

        private bool ValidData(Persona persona)
        {
            if(!string.IsNullOrEmpty(persona.Nombres) ||
                !string.IsNullOrEmpty(persona.Apellidos) ||
                !string.IsNullOrEmpty(persona.EstadoCivil) ||
                !string.IsNullOrEmpty(persona.ValorGanar.ToString()))
                { return true; } return false;
        }

        private async Task<int> Documentos(string tipo)
        {
            string? response = await _petition.ClientAsync("Get", "/get/document?documento=" + tipo);
            if (!string.IsNullOrEmpty(response))
            {
                return JsonConvert.DeserializeObject<int>(response);
            }
            return 0;

        }
    }
}
