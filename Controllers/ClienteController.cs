using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using NET_MVC.Models;
using NET_MVC.Services;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Security.Claims;

namespace NET_MVC.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        #region Dependencias
        public ClienteServices _ServicioCliente = new ClienteServices();
        public EntrenadorServices _ServicioEntrenador = new EntrenadorServices();
        #endregion

        #region IActionResult
        public IActionResult Registrar() => View("RegistrarCliente");
        public IActionResult Listar() => View("ListarCliente", new List<ClienteModel> { });
        public IActionResult InformacionCliente() => View("InformacionCliente");
        public IActionResult InformacionClienteEspecifico() => View("InformacionClienteEspecifico");
        public IActionResult AsignarEntrenadorCliente()
        {
            List<EntrenadorModel> entrenadores = ObtenerEntrenadoresDisponibles();
            return View("AsignarEntrenadorCliente", entrenadores);
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public JsonResult RegistrarCliente(ClienteModel Cliente)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, errors = ModelState.ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });

            Cliente.IdSede = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);

            // Si es General, lo registramos directamente
            if (Cliente.refMembresia == "General")
            {
                var registrarCliente = _ServicioCliente.registrarCliente(Cliente);
                if (registrarCliente.success)
                {
                    TempData["SuccessMessage"] = registrarCliente.mensaje;
                    return Json(new { success = true, redirectUrl = Url.Action("DashboardAdministrador", "Admin") });
                }
            }

            // Si es Premium, guardamos en TempData para luego completar el proceso
            TempData["ClienteDatos"] = JsonConvert.SerializeObject(Cliente);
            return Json(new { success = true, redirectUrl = Url.Action("AsignarEntrenadorCliente", "Cliente") });
        }

        [HttpPost]
        public IActionResult BuscarCliente(string identificacion)
        {
            var ObtenerClienteResult = _ServicioCliente.ObtenerClientePorIdentificacion(identificacion);
            if (!ObtenerClienteResult.access)
            {
                TempData["ErrorMessage"] = ObtenerClienteResult.mensaje;
                return RedirectToAction("InformacionCliente");
            }
            return View("InformacionClienteEspecifico", ObtenerClienteResult.objetoCliente);
        }

        [HttpPost]
        public JsonResult VerificarClienteExistente(string identificacion)
        {
            var resultadoConsulta = _ServicioCliente.existeTupla(identificacion);
            return Json(new { existe = resultadoConsulta.success });
        }

        [HttpPost]
        public ActionResult Filtrar(string filter)
        {
            string IdSede = User.FindFirst(ClaimTypes.Name)?.Value;
            var ListClientesFiltrados = _ServicioCliente.listarClienteFiltro(filter, IdSede);
            ViewBag.TotalClientes = ListClientesFiltrados.Count;
            return View("ListarCliente", ListClientesFiltrados);
        }
        #endregion

        #region Utilidades
        protected List<EntrenadorModel> ObtenerEntrenadoresDisponibles()
        {
            string IdSede = User.FindFirst(ClaimTypes.Name)?.Value;
            return _ServicioEntrenador.obternerEntrenadoresDisponibles(IdSede);
        }
        #endregion
    }
}




