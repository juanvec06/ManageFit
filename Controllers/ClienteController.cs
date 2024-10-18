using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using NET_MVC.Models;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using System.Security.Claims;

namespace NET_MVC.Controllers
{
    [Authorize]
    public class ClienteController : Controller
    {
        AdmPersona consulta = new AdmPersona();
        AdmCliente consultaCliente = new AdmCliente();

        public IActionResult Registrar()
        {
            return View("RegistrarCliente");
        }

        public IActionResult Listar()
        {
            return View("ListarCliente");
        }

        public IActionResult InformacionCliente()
        {
            return View("InformacionCliente");
        }

        public IActionResult InformacionClienteEspecifico()
        {
            return View("InformacionClienteEspecifico");
        }

        public IActionResult AsignarEntrenadorCliente()
        {
            return View("AsignarEntrenadorCliente");
        }

        [HttpPost]
        public JsonResult RegistrarCliente(ClienteModel Cliente)
        {
            Cliente.IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.RegistrarPersona(Cliente);
                    var respuesta2 = consultaCliente.RegistrarCliente(Cliente);
                    if (respuesta && respuesta2)
                    {
                        TempData["SuccessMessage"] = "Cliente registrado correctamente";

                        if (Cliente.refMembresia == "Premium")
                        {
                            return Json(new { success = true, redirectUrl = Url.Action("AsignarEntrenadorCliente", "Cliente") });
                        }
                        else if (Cliente.refMembresia == "General")
                        {
                            return Json(new { success = true, redirectUrl = Url.Action("DashboardAdministrador", "Admin") });
                        }
                    }
                    else
                    {
                        return Json(new { success = false, errors = new { MensajeError = "Error al registrar cliente" } });
                    }

                }
                catch (OracleException oex)
                {
                    if (oex.Number == -20002)
                    {
                        return Json(new { success = false, errors = new { MensajeError = "Cliente existente" } });
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
        public JsonResult VerificarClienteExistente(string identificacion)
        {
            // Verifica que la identificación sea un número
            if (!int.TryParse(identificacion, out _))
            {
                return Json(new { existe = false, mensaje = "La identificación debe ser un número entero." });
            }

            bool clienteExiste = consulta.PersonaExiste(identificacion);
            return Json(new { existe = clienteExiste });
        }

        [HttpPost]
        public IActionResult BuscarCliente(string identificacion)
        {
            // Verificar que la identificación no esté vacía
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                TempData["ErrorMessage"] = "La identificación no puede estar vacía.";
                return RedirectToAction("InformacionCliente"); // Redirigir a la página donde se muestra el formulario
            }

            // Verificar si la identificación es numérica
            if (!int.TryParse(identificacion, out _))
            {
                TempData["ErrorMessage"] = "La identificación debe ser un número válido.";
                return RedirectToAction("InformacionCliente");
            }

            // Verificar si la persona existe en la base de datos
            bool clienteExiste = consulta.PersonaExiste(identificacion);

            if (clienteExiste)
            {
                var cliente = consultaCliente.ObtenerClientePorIdentificacion(identificacion);

                // Verificar si se pudo obtener la información del cliente
                if (cliente != null)
                {
                    return View("InformacionClienteEspecifico", cliente); // Mostrar la información del cliente
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al obtener la información del cliente.";
                    return RedirectToAction("InformacionCliente");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Cliente no encontrado.";
                return RedirectToAction("InformacionCliente");
            }
        }

    }
}



