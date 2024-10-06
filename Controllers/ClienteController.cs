using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using NET_MVC.Models;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;

namespace NET_MVC.Controllers
{
    public class ClienteController : Controller
    {
        AdmCliente consulta = new AdmCliente();
        public IActionResult Registrar()
        {
            return View("RegistrarCliente");
        }

        [HttpPost]
        public IActionResult RegistrarCliente(ClienteModel Cliente)
        {
            string auxStringSession = HttpContext.Session.GetString("idUsuario"); 
            Cliente.IdSede = int.Parse(auxStringSession);


            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.RegistrarCliente(Cliente);
                    if (respuesta)
                    {
                        TempData["MensajeValidacion"] = "Cliente registrado";
                        return RedirectToAction("DashboardAdministrador", "Admin");
                    }
                    else
                        return View(Cliente);
                }
                catch (OracleException oex)
                {

                    if (oex.Number == -20002)
                    {
                        ViewBag.MensajeError = "Cliente existente";
                        return View(Cliente);
                    }
                    else
                    {
                        ViewBag.MensajeError = "Error inesperado: " + oex.Message;
                        return View(Cliente);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(Cliente);
                } 
            }
            return View(Cliente);
        }
    }
}

    

