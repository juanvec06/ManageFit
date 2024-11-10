﻿using Microsoft.AspNetCore.Authorization;
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
        public JsonResult AsignarEntrenadorCliente(string identificacionEntrenador)
        {

            List<EntrenadorModel> entrenadoresDisponibles = ObtenerEntrenadoresDisponibles();
            // Verifica si el entrenador está en la lista de disponibles
            for (int varIdx = 0; varIdx < entrenadoresDisponibles.Count; varIdx++)
            {
                if(identificacionEntrenador == entrenadoresDisponibles[varIdx].Identificacion)
                {
                    auxCliente.IdEntrenador = int.Parse(identificacionEntrenador);


                    //BOTON DE CANCELAR O CONFIRMAR - luego insertarlo en BD.
                    return Json(new { success = true, mensaje = "Entrenador asignado correctamente." });
                }
            }
                return Json(new { success = false, mensaje = "El entrenador no está disponible." });
        }

        /*
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public JsonResult AsignarEntrenadorCliente(string identificacionEntrenador)
        {
            // Verificar que la identificación no esté vacía
            if (string.IsNullOrWhiteSpace(identificacionEntrenador))
            {
                return Json(new { success = false, mensaje = "La identificación no puede estar vacía." });
            }

            // Verificar que la identificación no tenga más de 10 dígitos
            if (identificacionEntrenador.Length > 10)
            {
                return Json(new { success = false, mensaje = "La identificación no puede tener más de 10 dígitos." });
            }

            // Verificar si la identificación es numérica y convertirla a entero
            if (!int.TryParse(identificacionEntrenador, out int identificacionNum))
            {
                return Json(new { success = false, mensaje = "La identificación debe ser un número válido." });
            }

            // Verificar que la identificación no sea un número negativo
            if (identificacionNum < 0)
            {
                return Json(new { success = false, mensaje = "La identificación debe ser un número positivo." });
            }

            // Obtener la lista de entrenadores disponibles
            List<EntrenadorModel> entrenadoresDisponibles = ObtenerEntrenadoresDisponibles();

            // Verifica si el entrenador está en la lista de disponibles
            for (int varIdx = 0; varIdx < entrenadoresDisponibles.Count; varIdx++)
            {
                if (identificacionEntrenador == entrenadoresDisponibles[varIdx].Identificacion)
                {
                    auxCliente.IdEntrenador = identificacionNum;

                    // Retornar mensaje de éxito como JSON
                    return Json(new { success = true, mensaje = "Entrenador asignado correctamente." });
                }
            }

            // Retornar mensaje de error si el entrenador no está disponible
            return Json(new { success = false, mensaje = "El entrenador no está disponible." });
        }
 
        */


        /*
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult AsignarEntrenadorCliente(string identificacionEntrenador)
        {
            // Verificar que la identificación no esté vacía
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

            // Obtener la lista de entrenadores disponibles
            List<EntrenadorModel> entrenadoresDisponibles = ObtenerEntrenadoresDisponibles();

            // Verifica si el entrenador está en la lista de disponibles
            for (int varIdx = 0; varIdx < entrenadoresDisponibles.Count; varIdx++)
            {
                if (identificacionEntrenador == entrenadoresDisponibles[varIdx].Identificacion)
                {
                    auxCliente.IdEntrenador = identificacionNum;

                    // Mensaje de éxito con TempData
                    TempData["SuccessMessage"] = "Entrenador asignado correctamente.";
                    return RedirectToAction("AsignarEntrenadorCliente", "Cliente");
                }
            }

            // Mensaje de error si no está disponible
            TempData["ErrorMessage"] = "El entrenador no está disponible.";
            return RedirectToAction("AsignarEntrenadorCliente", "Cliente");
        }
        */


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
                        var respuesta2 = consultaCliente.RegistrarCliente(Cliente);
                        if (respuesta && respuesta2)
                        {
                            //mensaje de exito, este por ahora solo se muestra cuando es general
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

        [HttpPost]
        [Authorize(Roles = "Administrador, Entrenador")]
        public IActionResult BuscarCliente(string identificacion)
        {
            String IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            // Verificar que la identificación no esté vacía
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                if (User.IsInRole("Administrador"))
                {
                    TempData["ErrorMessage"] = "La identificación no puede estar vacía.";
                }
                else if (User.IsInRole("Entrenador"))
                {
                    TempData["ErrorMessage"] = "Por favor diligenciar los campos marcados como obligatorios.";
                }
                return RedirectToAction("InformacionCliente"); // Redirigir a la página donde se muestra el formulario
            }
           
            // Verificar que la identificación no tenga más de 10 dígitos
            if (identificacion.Length > 10)
            {
                TempData["ErrorMessage"] = "La identificación no puede tener más de 10 dígitos.";
                return RedirectToAction("InformacionCliente"); // Redirigir a la página donde se muestra el formulario
            }

            // Verificar si la identificación es numérica
            if (!int.TryParse(identificacion, out _))
            {
                TempData["ErrorMessage"] = "La identificación debe ser un número válido.";
                return RedirectToAction("InformacionCliente");
            }

            // Verificar que la identificación no sea menor a 0
            if (int.Parse(identificacion) < 0)
            {
                TempData["ErrorMessage"] = "La identificación debe ser un número positivo.";
                return RedirectToAction("InformacionEntrenador");
            }

            // Verificar si la persona existe en la base de datos
            bool clienteExiste = consultaCliente.ClienteExiste(identificacion);

            if (clienteExiste)
            {
                var cliente = consultaCliente.ObtenerClientePorIdentificacion(identificacion,IdSede);

                // Verificar si se pudo obtener la información del cliente
                if (cliente != null && User.IsInRole("Administrador"))
                {
                    return View("InformacionClienteEspecifico", cliente); // Mostrar la información del cliente
                }
                else if (cliente != null && User.IsInRole("Entrenador"))
                {
                    TempData["ClienteId"] = identificacion;
                    return View("InformacionClienteAsignado", cliente); // Mostrar la información del cliente
                }
                else
                {
                    TempData["ErrorMessage"] = "Cliente no encontrado.";
                    return RedirectToAction("InformacionCliente");
                }
            }
            else
            {
                if (User.IsInRole("Administrador"))
                {
                    TempData["ErrorMessage"] = "Cliente no encontrado.";
                }
                else if (User.IsInRole("Entrenador"))
                {
                    TempData["ErrorMessage"] = "Cliente no existente.";
                }
                return RedirectToAction("InformacionCliente");
            }
        }
        [Authorize(Roles ="Entrenador")]
        public string ObtenerObjetivoDe(string id)
        {
            return consultaCliente.GetObjetivo(id);
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

            string sql = consultaCliente.cadenaListarClientes(filter,IdSede);

            // Ejecuta la consulta y obtiene la lista filtrada de clientes
            clientesFiltrados = consultaCliente.ListarClientes(sql);

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
        [Authorize(Roles = "Administrador")]
        private string cadenasql2(int opcion, String IdSede)
        {
            string nomMembresia;
            if (opcion == 1) nomMembresia = "Premium";
            else nomMembresia = "General";

            string sql2 = "SELECT c.id_cliente, MAX(p.nombre_persona) AS nombre, MAX(p.telefono_persona) AS telefono,MAX(m.tipo) AS membresia, TO_CHAR(MAX(m.fecha_suscripcion), 'DD-MM-YYYY') AS fecha " +
                                "FROM cliente c INNER JOIN persona p ON c.id_cliente = p.id_persona " +
                                "INNER JOIN membresia m ON c.id_cliente = m.id_cliente " +
                                "WHERE p.id_sede = " + IdSede + " AND m.tipo = '" + nomMembresia + "' " +
                                "GROUP BY c.id_cliente";

            return sql2;
        }
        [Authorize(Roles = "Administrador")]
        private string cadenasql3(int opcion, String IdSede)
        {
            string genero;
            if (opcion == 1) genero = "M";
            else if (opcion == 2) genero = "F";
            else genero = "NE";

            string sql3 = "SELECT c.id_cliente, MAX(p.nombre_persona) AS nombre, MAX(p.telefono_persona) AS telefono,MAX(m.tipo) AS membresia, TO_CHAR(MAX(m.fecha_suscripcion), 'DD-MM-YYYY') AS fecha " +
                                "FROM cliente c INNER JOIN persona p ON c.id_cliente = p.id_persona " +
                                "INNER JOIN membresia m ON c.id_cliente = m.id_cliente " +
                                "WHERE p.id_sede = " + IdSede + " AND p.genero_persona = '" + genero + "' " +
                                "GROUP BY c.id_cliente";
            return sql3;
        }

    }
}



