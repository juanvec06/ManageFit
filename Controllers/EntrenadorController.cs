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
            // Verifica que los datos del usuario estén disponibles.
            Entrenador.IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.RegistrarEntrenador(Entrenador);
                    if (respuesta)
                    {
                        TempData["MensajeValidacion"] = "Entrenador registrado correctamente";
                        return RedirectToAction("DashboardAdministrador", "Admin");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error al registrar el entrenador.";
                        return View(Entrenador);
                    }
                }
                catch (OracleException oex)
                {
                    if (oex.Number == -20002) // Ejemplo de error para entrenador existente
                    {
                        ViewBag.ErrorMessage = "Entrenador existente";
                        return View(Entrenador);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error inesperado: " + oex.Message;
                        return View(Entrenador);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                    return View(Entrenador);
                }
            }
            return View(Entrenador); // En caso de que la validación falle
        }

        [HttpPost]
        public JsonResult VerificarEntrenadorExistente(string identificacion)
        {
            // Verifica que la identificación sea un número
            if (!int.TryParse(identificacion, out _))
            {
                return Json(new { existe = false, mensaje = "La identificación debe ser un número entero." });
            }

            bool entrenadorExiste = consulta.EntrenadorExiste(identificacion);
            return Json(new { existe = entrenadorExiste });
        }


    }
}
