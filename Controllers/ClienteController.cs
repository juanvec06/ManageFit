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

        public IActionResult Registrar()
        {
            return View("RegistrarCliente");
        }

        public IActionResult Listar()
        {
            return View("ListarCliente");
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
        private List<EntrenadorModel> ObtenerEntrenadoresDisponibles()
        {
            var entrenadores = new List<EntrenadorModel>();
            String IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            if (Conexion.abrirConexion())
            {

                string sql = "SELECT e.id_entrenador, MAX(p.nombre_persona) AS nombre_entrenador, MAX(nombre_ae) area_especialidad, COUNT(c.id_cliente) AS num_clientes " +
                    "FROM ENTRENADOR e INNER JOIN PERSONA p ON e.id_entrenador = p.id_persona " +
                    "INNER JOIN AREAESPECIALIDAD ae ON e.id_ae = ae.id_ae\r\nLEFT JOIN CLIENTE c ON c.id_entrenador = e.id_entrenador " +
                    "WHERE p.id_sede = " +IdSede+
                    "GROUP BY e.id_entrenador " +
                    "HAVING COUNT(c.id_cliente) < 5";  //muestra entrenadores disponibles(que no tengan más de 5 clientes asignado) de la sede 
                using (OracleCommand cmd = new OracleCommand(sql, conexionBD))
                {
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EntrenadorModel entrenador = new EntrenadorModel
                        {
                            Identificacion = reader["ID_ENTRENADOR"].ToString(),
                            Nombre = reader["NOMBRE_ENTRENADOR"].ToString(),
                            Especialidad = reader["AREA_ESPECIALIDAD"].ToString(),
                            ClientesAsignados = Convert.ToInt32(reader["NUM_CLIENTES"])
                        };
                        entrenadores.Add(entrenador);
                    }
                }
            }
            return entrenadores;
        }
    }
}



