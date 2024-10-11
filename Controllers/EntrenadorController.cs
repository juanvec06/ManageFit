using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System.Security.Claims;

namespace NET_MVC.Controllers
{
    [Authorize]
    public class EntrenadorController : Controller
    {
        public IActionResult DashboardEntrenador()
        {
            return View(); // Devuelve la vista Login.cshtml
        }
        AdmEntrenador consulta = new AdmEntrenador();
        public IActionResult RegistrarEntrenador()
        {
            return View("RegistrarEntrenador");
        }

        public IActionResult Listar()
        {
            return View("ListarEntrenador");
        }

        [HttpPost]
        public IActionResult RegistrarEntrenador(EntrenadorModel Entrenador)
        {
            Entrenador.IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.RegistrarEntrenador(Entrenador);
                    if (respuesta)
                    {
                        TempData["MensajeValidacion"] = "Entrenador registrado";
                        return RedirectToAction("DashboardAdministrador", "Admin");
                    }
                    else
                        return View(Entrenador);
                }
                catch(OracleException oex)
                {

                    if (oex.Number == -20002)
                    {
                        ViewBag.MensajeError = "Entrenador existente";
                        return View(Entrenador);
                    }
                    else
                    {
                        ViewBag.MensajeError = "Error inesperado: " + oex.Message;
                        return View(Entrenador);
                    }
                }
                /*catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(Entrenador);
                }*/
            }
            return View(Entrenador);
        }
    }
}
