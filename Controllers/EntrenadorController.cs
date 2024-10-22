using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using NET_MVC.Models;
using NET_MVC.Services;
using Oracle.ManagedDataAccess.Client;
using System.Security.Claims;

namespace NET_MVC.Controllers
{
    [Authorize]
    public class EntrenadorController : Controller
    {

        #region Dependencias
        EntrenadorServices _ServicioEntrenador = new EntrenadorServices();
        #endregion

        #region IActrionResult
        public IActionResult DashboardEntrenador() => View();
        public IActionResult RegistrarEntrenador() => View("RegistrarEntrenador");
        public IActionResult Listar() => View("ListarEntrenador");
        #endregion

        #region HttpPost
        [HttpPost]
        public JsonResult RegistrarEntrenador(EntrenadorModel Entrenador)
        {
            Entrenador.IdAreaEspecialidad = Entrenador.obtenerIdEspecialidad(Entrenador.Especialidad);
            Entrenador.IdSede = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
            var RegistrarEntrenadorResult = _ServicioEntrenador.registrarEntrenador(Entrenador);
            if (RegistrarEntrenadorResult.success)
            {
                TempData["SuccessMessage"] = "Entrenador registrado correctamente";
                return Json(new { success = true, redirectUrl = Url.Action("DashboardAdministrador", "Admin") });
            }
            return Json(new { success = false, errors = new { MensajeError = RegistrarEntrenadorResult.mensaje } });
        }

        [HttpPost]
        public JsonResult VerificarEntrenadorExistente(string identificacion)
        {
            #region Validadores
            var validarIdentificacionResult = _ServicioEntrenador.validarIdentificacion(identificacion);
            if (!validarIdentificacionResult.success) return Json(new { existe = false, mensaje = validarIdentificacionResult.mensaje });
            #endregion

            var existeTuplaResult = _ServicioEntrenador.existeTupla(identificacion);

            return Json(new { existe = existeTuplaResult.success });
        } 
        #endregion

    }
}
