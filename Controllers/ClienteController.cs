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
        OracleConnection conexionBD = Conexion.GetConnection();
        AdmPersona consulta = new AdmPersona();
        AdmCliente consultaCliente = new AdmCliente();
        AdmEntrenador consultaEntrenador = new AdmEntrenador();

        public IActionResult Registrar()
        {
            return View("RegistrarCliente");
        }

        public IActionResult Listar()
        {
            return View("ListarCliente", new List<ClienteModel> { });
        }

        public IActionResult AsignarEntrenadorCliente()
        {
            List<EntrenadorModel> entrenadores = ObtenerEntrenadoresDisponibles();
            return View("AsignarEntrenadorCliente", entrenadores);
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
        public ActionResult Filtrar(string filter)
        {
            // Lógica para filtrar clientes según el valor seleccionado
            var clientesFiltrados = new List<ClienteModel>();
            String IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            //Todos
            string sql = "SELECT c.id_cliente, MAX(p.nombre_persona) AS nombre, MAX(p.telefono_persona) AS telefono,MAX(m.tipo) AS membresia, TO_CHAR(MAX(m.fecha_suscripcion), 'DD-MM-YYYY') AS fecha " +
                                "FROM cliente c INNER JOIN persona p ON c.id_cliente = p.id_persona " +
                                "INNER JOIN membresia m ON c.id_cliente = m.id_cliente " +
                                "WHERE p.id_sede = " + IdSede +
                                "GROUP BY c.id_cliente";

            switch (filter)
            {
                case "all":
                    break;
                case "option1": // Clientes premium
                    sql = cadenasql2(1, IdSede);
                    break;
                case "option2": // Clientes generales
                    sql = cadenasql2(2, IdSede);
                    break;
                case "option3": // Género Masculino
                    sql = cadenasql3(1, IdSede);
                    break;
                case "option4": // Género Femenino
                    sql = cadenasql3(2, IdSede);
                    break;
                case "option5": // Género No especificado
                    sql = cadenasql3(3, IdSede);
                    break;
                default:
                    break;
            }
            // Listar clientes con filtros
            clientesFiltrados = ListarClientesFiltros(consultaCliente.ListarClientes(sql));
            // Guardar el total de clientes filtrados en ViewBag
            ViewBag.TotalClientes = clientesFiltrados.Count;
            return View("ListarCliente", clientesFiltrados);
        }

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



