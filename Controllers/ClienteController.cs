using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using NET_MVC.Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using System.Security.Claims;

namespace NET_MVC.Controllers
{
    public class ClienteController : Controller
    {
        public static ClienteModel auxCliente;
        OracleConnection conexionBD = Conexion.GetConnection();
        AdmPersona consulta = new AdmPersona();
        AdmCliente consultaCliente = new AdmCliente();
        AdmEntrenador consultaEntrenador = new AdmEntrenador();

        [Authorize(Roles = "Administrador")]
        public IActionResult Registrar()
        {
            ClienteModel cliente = new ClienteModel();
            AdmCliente actualCliente = new AdmCliente();
            AdmPersona actualPersona = new AdmPersona();
            if (TempData["ClienteDatos"] != null)
            {
                cliente = JsonConvert.DeserializeObject<ClienteModel>((string)TempData["ClienteDatos"]);
                //Se lleva los datos del cliente para cargarlos por si los quiere modificar
                return View("RegistrarCliente", cliente);

            }
            return View("RegistrarCliente");
        }

        [Authorize(Roles = "Entrenador, Administrador")]
        public IActionResult Listar()
        {
            if (HttpContext.Session.GetString("ClienteIdEjercicio") != null)
                HttpContext.Session.Remove("ClienteIdEjercicio");
            if (User.IsInRole("Administrador"))
            {
                return View("ListarCliente", new List<ClienteModel> { });
            }
            else if (User.IsInRole("Entrenador"))
            {
                var ClientesAsignados = ObtenerClientesAsignados();
                return View("ListarClientesAsignados", ClientesAsignados);
            }

            // Si el usuario no tiene ninguno de los roles, podrías redirigir a una vista de error o acceso denegado
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrador,Entrenador")]
        public IActionResult InformacionCliente()
        {
            if (User.IsInRole("Administrador"))
            {
                return View("InformacionCliente");
            }
            else if (User.IsInRole("Entrenador"))
            {
                return RedirectToAction("Listar");
            }
            return View();
        }

        [Authorize(Roles = "Entrenador")]
        public IActionResult ClienteAsignado()
        {
            return View("ClienteAsignado");
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult InformacionClienteEspecifico()
        {
            return View("InformacionClienteEspecifico");
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult AsignarEntrenadorCliente(string identificacionEntrenador)
        {
            var a = auxCliente;
            if (auxCliente.IdEntrenador != 0)
            {
                TempData["ErrorMessage"] = "El cliente ya tiene un entrenador asignado";
                return RedirectToAction("AsignarEntrenadorCliente", "Cliente");
            }
            if (string.IsNullOrWhiteSpace(identificacionEntrenador))
            {
                TempData["ErrorMessage"] = "La identificación no puede estar vacía.";
                return RedirectToAction("AsignarEntrenadorCliente", "Cliente");
            }

            // Verificar que la identificación no tenga más de 10 dígitos
            if (identificacionEntrenador.Length > 10)
            {
                TempData["ErrorMessage"] = "La identificación no puede tener más de 10 dígitos.";
                return RedirectToAction("AsignarEntrenadorCliente", "Cliente");
            }

            // Verificar si la identificación es numérica y convertirla a entero
            if (!int.TryParse(identificacionEntrenador, out int identificacionNum))
            {
                TempData["ErrorMessage"] = "La identificación debe ser un número válido.";
                return RedirectToAction("AsignarEntrenadorCliente", "Cliente");
            }

            // Verificar que la identificación no sea un número negativo
            if (identificacionNum < 0)
            {
                TempData["ErrorMessage"] = "La identificación debe ser un número positivo.";
                return RedirectToAction("AsignarEntrenadorCliente", "Cliente");
            }

            bool entrenadorExistente = consultaEntrenador.EntrenadorExistente(identificacionEntrenador);

            if (!entrenadorExistente)
            {
                TempData["ErrorMessage"] = "Entrenador no existente";
                return RedirectToAction("AsignarEntrenadorCliente", "Cliente");
            }

            List<EntrenadorModel> entrenadoresDisponibles = ObtenerEntrenadoresDisponibles();
            // Verifica si el entrenador está en la lista de disponibles
            for (int varIdx = 0; varIdx < entrenadoresDisponibles.Count; varIdx++)
            {
                if (identificacionEntrenador == entrenadoresDisponibles[varIdx].Identificacion)
                {
                    auxCliente.IdEntrenador = int.Parse(identificacionEntrenador);


                    auxCliente.IdEntrenador = int.Parse(identificacionEntrenador);
                    var auxRegistrarPersona = consulta.RegistrarPersona(auxCliente);
                    if (auxRegistrarPersona == true)
                    {
                        var auxRegistrarCliente = consultaCliente.RegistrarCliente(auxCliente);
                    }
                    TempData["ClienteDatos"] = null;
                    TempData["SuccessMessage"] = "Cliente registrado correctamente";
                    return RedirectToAction("DashboardAdministrador", "Admin");
                }
            }

            if(identificacionEntrenador != null)
            {
                TempData["ErrorMessage"] = "Entrenador no disponible";
                return RedirectToAction("AsignarEntrenadorCliente", "Cliente");
            }

            return Json(new { success = false, errors = ModelState.ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()) });
        }


        [Authorize(Roles = "Administrador")]
        public IActionResult AsignarEntrenadorCliente()
        {
            List<EntrenadorModel> entrenadores = ObtenerEntrenadoresDisponibles();
            return View("AsignarEntrenadorCliente", entrenadores);

        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public JsonResult RegistrarCliente(ClienteModel Cliente)
        {
            Cliente.IdSede = User.FindFirst(ClaimTypes.Name)?.Value;
            HttpContext.Session.SetString("ClienteId", Cliente.Identificacion.ToString());
            if (ModelState.IsValid)
            {
                try
                {
                    if (Cliente.refMembresia == "Premium")
                    {
                        //se almacena en TempData los datos del cliente por si se redirecciona a la pagina anterior mediante el boton de la pagina
                        TempData["ClienteDatos"] = JsonConvert.SerializeObject(Cliente);
                        auxCliente = Cliente;
                        return Json(new { success = true, redirectUrl = Url.Action("AsignarEntrenadorCliente", "Cliente") });
                    }
                    else if (Cliente.refMembresia == "General")
                    {
                        var respuesta = consulta.RegistrarPersona(Cliente);
                        if (respuesta)
                        {
                            var respuesta2 = consultaCliente.RegistrarCliente(Cliente);
                        }
                        if (respuesta)
                        {
                            TempData["SuccessMessage"] = "Cliente registrado correctamente";
                            return Json(new { success = true, redirectUrl = Url.Action("DashboardAdministrador", "Admin") });
                        }
                        else
                        {
                            return Json(new { success = false, errors = new { MensajeError = "Error al registrar cliente" } });
                        }
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

        [HttpGet]
        [HttpPost]
        [Authorize(Roles = "Administrador, Entrenador")]
        public IActionResult BuscarCliente(string identificacion)
        {
            // Si la identificación está almacenada en la sesión y el usuario es un entrenador
            if (HttpContext.Session.GetString("ClienteIdEjercicio") != null && User.IsInRole("Entrenador"))
            {
                identificacion = HttpContext.Session.GetString("ClienteIdEjercicio"); // Para manejar el botón "Atrás"
            }

            string idSede = User.FindFirst(ClaimTypes.Name)?.Value;

            // Validaciones de la identificación

            // 1. Verificar que la identificación no esté vacía
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                TempData["ErrorMessage"] = User.IsInRole("Administrador")
                    ? "Por favor diligenciar los campos marcados como obligatorios."
                    : "La identificación no puede estar vacía.";
                return RedirectToAction("InformacionCliente");
            }

            // 2. Verificar que la longitud no supere los 10 caracteres
            if (identificacion.Length > 10)
            {
                TempData["ErrorMessage"] = "La identificación no puede tener más de 10 dígitos.";
                return RedirectToAction("InformacionCliente");
            }

            // 3. Verificar que sea numérica
            if (!int.TryParse(identificacion, out _))
            {
                TempData["ErrorMessage"] = "La identificación debe ser un número válido.";
                return RedirectToAction("InformacionCliente");
            }

            // 4. Verificar que sea un número positivo
            if (int.Parse(identificacion) < 0)
            {
                TempData["ErrorMessage"] = "La identificación debe ser un número positivo.";
                return RedirectToAction("InformacionCliente");
            }

            // Verificar si el cliente existe
            bool clienteExiste = consultaCliente.ClienteExiste(identificacion);

            if (!clienteExiste)
            {
                TempData["ErrorMessage"] = User.IsInRole("Administrador")
                    ? "Cliente no encontrado."
                    : "El cliente no existe en el sistema.";
                return RedirectToAction("InformacionCliente");
            }

            // Obtener información del cliente
            ClienteModel cliente = consultaCliente.ObtenerClientePorIdentificacion(identificacion, idSede);

            if (cliente == null)
            {
                TempData["ErrorMessage"] = "El cliente no existe en esta sede. Por favor intente nuevamente.";
                return RedirectToAction("InformacionCliente");
            }

            // Guardar identificación en la sesión
            HttpContext.Session.SetString("ClienteIdEjercicio", identificacion);

            // Verificar si el cliente está asignado al entrenador
            List<ClienteModel> clientesAsignados = ObtenerClientesAsignados();
            bool clienteAsignadoResult = clientesAsignados != null && clientesAsignados.Any(c => c.Identificacion == cliente.Identificacion);

            if (User.IsInRole("Administrador"))
            {
                return View("InformacionClienteEspecifico", cliente); // Mostrar la información del cliente
            }
            else if (User.IsInRole("Entrenador"))
            {
                if (clienteAsignadoResult)
                {
                    TempData["ClienteId"] = identificacion;
                    return View("InformacionClienteAsignado", cliente); // Mostrar la información del cliente
                }
                else
                {
                    TempData["ErrorMessage"] = "El cliente no está asignado a este entrenador.";
                }
            }

            return RedirectToAction("InformacionCliente");
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public JsonResult VerificarClienteExistente(string identificacion)
        {
            // Verifica que la identificación sea un número
            if (!int.TryParse(identificacion, out _))
            {
                return Json(new { existe = false, mensaje = "La identificación debe ser un número entero." });
            }

            bool clienteExiste = consultaCliente.ClienteExiste(identificacion);
            return Json(new { existe = clienteExiste });
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public JsonResult VerificarPersonaExistente(string identificacion)
        {
            // Verifica que la identificación sea un número
            if (!int.TryParse(identificacion, out _))
            {
                return Json(new { existe = false, mensaje = "La identificación debe ser un número entero." });
            }

            bool entrenadorExiste = consulta.PersonaExiste(identificacion);
            return Json(new { existe = entrenadorExiste });
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public JsonResult Filtrar(string filter)
        {
            List<ClienteModel> clientesFiltrados = new List<ClienteModel>();
            String IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            //string sql = consultaCliente.cadenaListarClientes(filter, IdSede);

            // Ejecuta la consulta y obtiene la lista filtrada de clientes
            clientesFiltrados = consultaCliente.ListarClientes(filter, IdSede);

            return Json(new
            {
                clientes = clientesFiltrados,
                totalClientes = clientesFiltrados.Count
            });
        }

        [Authorize(Roles = "Administrador")]
        private List<ClienteModel> ListarClientesFiltros(List<ClienteModel> clientesFiltrados)
        {
            try
            {
                if (clientesFiltrados.Count() > 0)
                {
                    return clientesFiltrados;
                }
                else
                {
                    // Mensaje no hay clientes
                    return new List<ClienteModel> { };
                }
            }
            catch (Exception ex)
            {
                // Mensaje error
                return new List<ClienteModel> { };
            }
        }

        [Authorize(Roles = "Administrador")]
        private List<EntrenadorModel> ObtenerEntrenadoresDisponibles()
        {
            String IdSede = User.FindFirst(ClaimTypes.Name)?.Value;
            var entrenadores = consultaEntrenador.ListarEntrenadoresDisponibles(IdSede);

            try
            {
                if (entrenadores.Count() > 0)
                {
                    return entrenadores;
                }
                else
                {
                    // Mensaje no hay entrenadores disponibles
                    return new List<EntrenadorModel> { };
                }
            }
            catch (Exception ex)
            {
                // Mensaje error
                return new List<EntrenadorModel> { };
            }
        }

        [Authorize(Roles = "Entrenador")]
        private List<ClienteModel> ObtenerClientesAsignados()
        {
            String IdEntrenador = User.FindFirst(ClaimTypes.Name)?.Value;
            List<ClienteModel> clientes = consultaCliente.ListarClientesAsignados(IdEntrenador);
            if (clientes.Count() > 0)
            {
                return clientes;
            }
            else
            {
                // Mensaje no hay entrenadores disponibles
                return new List<ClienteModel> { };
            }
        }

    }
}



