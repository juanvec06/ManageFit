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
        public OracleConnection conexionBD = Conexion.GetConnection();
        AdmCliente consulta = new AdmCliente();
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
            string usuario = User.FindFirst(ClaimTypes.Name)?.Value;
            Cliente.IdSede = int.Parse(usuario);

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.RegistrarCliente(Cliente);
                    if (respuesta)
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

            bool clienteExiste = consulta.ClienteExiste(identificacion);
            return Json(new { existe = clienteExiste });
        }
        private List<EntrenadorModel> ObtenerEntrenadoresDisponibles()
        {
            var entrenadores = new List<EntrenadorModel>();

            if (Conexion.abrirConexion())
            {
                string sql = "SELECT ID_ENTRENADOR, MAX(NOMBRE_ENTRENADOR) NOMBRE_ENTRENADOR ,MAX(nombre_especialidad) AREA_ESPECIALIDAD, COUNT(*) CLIENTES_ASIGNADOS FROM ENTRENADOR NATURAL JOIN areaespecialidad NATURAL JOIN CLIENTE GROUP BY id_entrenador HAVING COUNT(*) < 5";

                using (OracleCommand cmd = new OracleCommand(sql, conexionBD))
                {
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        EntrenadorModel entrenador = new EntrenadorModel
                        {
                            Id = reader["ID_ENTRENADOR"].ToString(),
                            Nombre = reader["NOMBRE"].ToString(),
                            Especialidad = reader["AREA_ESPECIALIDAD"].ToString(),
                            ClientesAsignados = Convert.ToInt32(reader["CLIENTES_ASIGNADOS"])
                        };
                        entrenadores.Add(entrenador);
                    }
                }
            }
            return entrenadores;
        }
    }
}

    

