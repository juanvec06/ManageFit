using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Roles = "Entrenador")]
        public IActionResult modificarPMF()
        {

            return View("ModificarPMF");
        }

        [Authorize(Roles = "Entrenador")]
        [HttpPost]
        public JsonResult AgregarPMF(PMFModel pmf)
        {
            HttpContext.Session.SetString("FechaValoracion", pmf.FechaValoracion.ToString());
            pmf.IdCliente = int.Parse(HttpContext.Session.GetString("ClienteIdEjercicio"));
            if (ModelState.IsValid)
            {
                try
                {
                    var existencia = consulta.PmfExistente(pmf);
                    if (existencia)
                    {
                        return Json(new { success = true, redirectUrl = Url.Action("modificarEjercicios") });
                    }
                    else
                    {
                        var respuesta = consulta.AgregarPMF(pmf);
                        if (respuesta)
                        {
                            return Json(new { success = true, redirectUrl = Url.Action("modificarEjercicios") });
                        }
                        else
                        {
                            return Json(new { existe = respuesta });
                        }
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

        [Authorize(Roles = "Entrenador")]
        private List<EjercicioModel> ObtenerEjercicios()
        {
            int idCliente = int.Parse(HttpContext.Session.GetString("ClienteIdEjercicio")); ;
            DateTime FechaValoracion = DateTime.Parse(HttpContext.Session.GetString("FechaValoracion"));
            List<EjercicioModel> ejercicios = consulta.ListarEjercicios(idCliente, FechaValoracion);
            if (ejercicios.Count() > 0)
            {
                return ejercicios;
            }
            else
            {
                // Mensaje no hay entrenadores disponibles
                return new List<EjercicioModel> { };
            }
        }

        [Authorize(Roles = "Entrenador")]
        public IActionResult modificarEjercicios()
        {
            var ejercicios = ObtenerEjercicios();
            return View("modificarEjercicios", ejercicios);
        }
        [Authorize(Roles = "Entrenador")]
        public IActionResult AgregarEjercicio()
        {
            List<NombreEjercicio> opciones = consulta.ObtenerOpciones();
            var modelo = new EjercicioModel
            {
                Opciones = opciones
            };

            return View("AgregarEjercicio", modelo);
        }


        [Authorize(Roles = "Entrenador")]
        [HttpPost]
        public JsonResult AgregarEjercicioProc(EjercicioModel ejercicio)
        {
            ejercicio.IdCliente = int.Parse(HttpContext.Session.GetString("ClienteIdEjercicio")); ;
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
