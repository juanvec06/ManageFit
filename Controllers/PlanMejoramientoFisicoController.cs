using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using NET_MVC.Datos;
using NET_MVC.Models;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Security.Claims;

namespace NET_MVC.Controllers
{
    public class PlanMejoramientoFisicoController : Controller
    {
        AdmPMF consulta = new AdmPMF();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult modificarPMF(string clienteID)
        {
            TempData["ClienteId"] = clienteID;
            return View("ModificarPMF");
        }

        [HttpPost]
        public JsonResult AgregarPMF(PMFModel pmf, string clienteID)
        {
            HttpContext.Session.SetString("FechaValoracion", pmf.FechaValoracion.ToString());
            int IdCliente = int.Parse(clienteID);
            pmf.IdCliente = IdCliente;
            //pmf.IdCliente = int.Parse(HttpContext.Session.GetString("4"));
            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.AgregarPMF(pmf);
                    if (respuesta)
                    {
                        TempData["ClienteId"] = clienteID;
                        return Json(new { success = true, redirectUrl = Url.Action("modificarEjercicios") });
                    }
                    else
                    {
                        return Json(new { existe = respuesta });
                    }
                }
                catch (OracleException oex)
                {
                    
                    return Json(new { success = false, errors = new { MensajeError = "Error inesperado: " + oex.Message } });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errors = new { MensajeError = ex.Message } });
                }
            }
            return Json(new { success = false, errors = ModelState.ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
        }
        public IActionResult modificarEjercicios()
        {
            return View("modificarEjercicios");
        }
        public IActionResult AgregarEjercicio(int clienteID)
        {
            List<NombreEjercicio> opciones = consulta.ObtenerOpciones();
            var modelo = new EjercicioModel
            {
                Opciones = opciones
            };
            TempData["ClienteId"] = clienteID;
            return View("AgregarEjercicio", modelo);
        }

        public IActionResult PMF()
        {
            return View("PMF");
        }

        [HttpPost]
        public JsonResult AgregarEjercicioProc(EjercicioModel ejercicio, string clienteID)
        {
            int IdCliente = int.Parse(clienteID);
            ejercicio.IdCliente = IdCliente;
            ejercicio.FechaValoracion = DateTime.Parse(HttpContext.Session.GetString("FechaValoracion"));
            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.AgregarEjercicio(ejercicio);
                    if (respuesta)
                    {
                        //mensaje de exito
                        TempData["SuccessMessage"] = "Ejercicio registrado";
                        return Json(new { success = true, redirectUrl = Url.Action("ModificarEjercicios") });
                    }
                    else
                    {
                        return Json(new { success = false, errors = new { MensajeError = "Error al registrar ejercicio" } });
                    }
                }
                catch (OracleException oex)
                {
                    return Json(new { success = false, errors = new { MensajeError = "Error inesperado: " + oex.Message } });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errors = new { MensajeError = ex.Message } });
                }
            }
            return Json(new { success = false, errors = ModelState.ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
        }

    }
}
