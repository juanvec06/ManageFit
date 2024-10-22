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
            List<EntrenadorModel> ListaEntrenadoresDisponibles = ObtenerEntrenadoresDisponibles();
            return View("AsignarEntrenadorCliente", ListaEntrenadoresDisponibles);
        }
        #endregion

        #region HttpPost
        [HttpPost]
        public JsonResult RegistrarCliente(ClienteModel Cliente)
        {
            Cliente.IdSede = int.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
            if (Cliente.refMembresia == "General")
            {
                var registrarClienteResult = _ServicioCliente.RegistrarCliente(Cliente);
                if (registrarClienteResult.success)
                {
                    TempData["SuccessMessage"] = registrarClienteResult.mensaje;
                    return Json(new { success = true, redirectUrl = Url.Action("DashboardAdministrador", "Admin") });
                }
            }
            TempData["ClienteDatos"] = JsonConvert.SerializeObject(Cliente);
            return Json(new { success = true, redirectUrl = Url.Action("AsignarEntrenadorCliente", "Cliente") });
        }

        [HttpPost]
        public IActionResult BuscarCliente(string identificacion)
        {
            var obtenerClienteResult = _ServicioCliente.ObtenerClientePorIdentificacion(identificacion);
            if (!obtenerClienteResult.access)
            {
                TempData["ErrorMessage"] = obtenerClienteResult.mensaje;
                return RedirectToAction("InformacionCliente");
            }
            return View("InformacionClienteEspecifico", obtenerClienteResult.objetoCliente);
        }

        [HttpPost]
        public JsonResult VerificarClienteExistente(string identificacion)
        {
            var existeClienteResult = _ServicioCliente.existeTupla(identificacion);
            return Json(new { existe = existeClienteResult.success });
        }

        [HttpPost]
        public ActionResult Filtrar(string filter)
        {
            var listarClientesFiltradosResult = _ServicioCliente.ListarClienteFiltro(filter, User.FindFirst(ClaimTypes.Name)?.Value);
            ViewBag.TotalClientes = listarClientesFiltradosResult.Count;
            return View("ListarCliente", listarClientesFiltradosResult);
        }
        #endregion

        #region Utilidades
        protected List<EntrenadorModel> ObtenerEntrenadoresDisponibles()
        {
            return _ServicioEntrenador.obternerEntrenadoresDisponibles(User.FindFirst(ClaimTypes.Name)?.Value);
        }
        #endregion
    }
}




