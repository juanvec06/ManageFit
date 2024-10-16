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
        public JsonResult RegistrarEntrenador(EntrenadorModel Entrenador)
        {
            // Obtener el usuario actual

            string usuario = User.FindFirst(ClaimTypes.Name)?.Value;
            Entrenador.IdSede = int.Parse(usuario);

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.RegistrarEntrenador(Entrenador);
                    if (respuesta)
                    {
                        TempData["SuccessMessage"] = "Entrenador registrado correctamente";

                        // Si se necesita redirigir a otra vista, como en el caso de membresía
                        return Json(new { success = true, redirectUrl = Url.Action("DashboardAdministrador", "Admin") });
                    }
                    else
                    {
                        return Json(new { success = false, errors = new { MensajeError = "Error al registrar entrenador" } });
                    }
                }
                catch (OracleException oex)
                {
                    if (oex.Number == -20002) // Error de entrenador existente
                    {
                        return Json(new { success = false, errors = new { MensajeError = "Entrenador existente" } });
                    }
                    else
                    {
                        return Json(new { success = false, errors = new { MensajeError = "Error inesperado: " + oex.Message } });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, errors = new { MensajeError = ex.Message } });
                }
            }

            return Json(new { success = false, errors = ModelState.ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
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
