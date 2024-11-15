using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using System.Security.Claims;


namespace NET_MVC.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {

        [Authorize(Roles = "Administrador")]
        public IActionResult DashboardAdministrador()
        {
            AdmCliente consultaCliente = new AdmCliente();
            AdmEntrenador consultaEntrenador = new AdmEntrenador();
            AdmPersona consultaPersona = new AdmPersona();

            // Obtener el nombre del usuario actual
            string usuarioActual = User.Identity.Name;

            // Obtener el idSede llamando a la nueva función
            int idSede = consultaPersona.ObtenerIdSedePorUsuario(usuarioActual);

            // Consultar totales y cargar en el ViewBag
            ViewBag.TotalClientes = consultaCliente.ObtenerTotalClientesPorSede(idSede);
            ViewBag.TotalEntrenadores = consultaEntrenador.ObtenerTotalEntrenadoresPorSede(idSede);
            ViewBag.TotalPersonas = consultaPersona.ObtenerTotalPersonasPorSede(idSede);

            return View();
        }


    }
}
