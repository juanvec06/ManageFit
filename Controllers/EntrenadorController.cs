using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;

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
        [HttpPost]
        public IActionResult RegistrarEntrenador(EntrenadorModel model)
        {
            model.IdSede = HttpContext.Session.GetString("idUsuario");
            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.RegistrarEntrenador(model);
                    if (respuesta)
                    {
                        TempData["MensajeValidacion"] = "Entrenador registrado";
                        return RedirectToAction("DashboardAdministrador", "Admin");
                    }
                    else
                        return View(model);
                }
                catch(OracleException oex)
                {

                    if (oex.Number == -20002)
                    {
                        ViewBag.MensajeError = "Entrenador existente";
                        return View(model);
                    }
                    else
                    {
                        ViewBag.MensajeError = "Error inesperado: " + oex.Message;
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(model);
                }
            }
            return View(model);
        }
    }
}
