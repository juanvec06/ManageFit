using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System.Security.Claims;

namespace NET_MVC.Controllers
{
    [Authorize]
    public class EntrenadorController : Controller
    {
        AdmPersona consulta = new AdmPersona();
        AdmEntrenador consultaEntrenador = new AdmEntrenador();

        [Authorize(Roles = "Entrenador")]
        public IActionResult DashboardEntrenador()
        {
            return View(); // Devuelve la vista Login.cshtml
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult RegistrarEntrenador()
        {
            return View("RegistrarEntrenador");
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Listar()
        {
            return View("ListarEntrenador");
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult InformacionEntrenador()
        {
            return View("InformacionEntrenador");
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult InformacionEntrenadorEspecifico()
        {
            return View("InformacionEntrenadorEspecifico");
        }

        [HttpPost]
        [Authorize(Roles ="Administrador")]
        public JsonResult RegistrarEntrenador(EntrenadorModel Entrenador)
        {
            // Obtener el usuario actual
            string idsede = User.FindFirst(ClaimTypes.Name)?.Value;
            Entrenador.IdSede = idsede;

            if (ModelState.IsValid)
            {
                try
                {
                    var respuesta = consulta.RegistrarPersona(Entrenador);
                    var respuesta2 = consultaEntrenador.RegistrarEntrenador(Entrenador);
                    if (respuesta && respuesta2)
                    {
                        TempData["SuccessMessage"] = "Entrenador registrado correctamente";

                        // Si se necesita redirigir a otra vista, como en el caso de membresía
                        return Json(new { success = true, redirectUrl = Url.Action("DashboardAdministrador", "Admin") });
                    }
                    else
                    {
                        return Json(new { success = false, errors = new { MensajeError = "Error al registrar entrenador" } });
                    }
                }
                catch (OracleException oex)
                {
                    if (oex.Number == -20002)
                    {
                        return Json(new { success = false, errors = new { MensajeError = "Entrenador existente" } });
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
        [Authorize(Roles = "Administrador")]
        public JsonResult VerificarEntrenadorExistente(string identificacion)
        {
            // Verifica que la identificación sea un número
            if (!int.TryParse(identificacion, out _))
            {
                return Json(new { existe = false, mensaje = "La identificación debe ser un número entero." });
            }

            bool entrenadorExiste = consultaEntrenador.EntrenadorExistente(identificacion);
            return Json(new { existe = entrenadorExiste });
        }

        [HttpPost]
        public JsonResult VerificarPersonaExistente(string identificacion)
        {
            // Verifica que la identificación sea un número
            if (!int.TryParse(identificacion, out _))
            {
                return Json(new { existe = false, mensaje = "La identificación debe ser un número entero." });
            }

            bool personaExiste = consulta.PersonaExiste(identificacion);
            return Json(new { existe = personaExiste });
        }

        [HttpPost]
        public JsonResult Filtrar(string filter)
        {
            List<EntrenadorModel> clientesFiltrados = new List<EntrenadorModel>();
            String IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            // La consulta SQL se basa en el filtro seleccionado
            string sql = "SELECT E.id_Entrenador, " +
             "MAX(P.nombre_Persona) AS nombre_entrenador, " +
             "MAX(P.telefono_Persona) AS telefono_entrenador, " +
             "MAX(AE.nombre_AE) AS area_especialidad, " +
             "COUNT(C.id_Cliente) AS numero_clientes, " +
             "MAX(CT.fecha_inicio_contrato) AS fecha_contratacion, " + // Usar fecha_inicio_contrato de la tabla Contrato
             "MAX(CT.salario) AS salario " + // Agregar el salario del entrenador
             "FROM Entrenador E " +
             "INNER JOIN Persona P ON E.id_Entrenador = P.id_Persona " +
             "INNER JOIN AreaEspecialidad AE ON E.id_AE = AE.id_AE " +
             "LEFT JOIN Cliente C ON E.id_Entrenador = C.id_Entrenador " +
             "LEFT JOIN Contrato CT ON E.id_Entrenador = CT.id_entrenador " + // Unir con la tabla Contrato
             "WHERE P.id_Sede = " + IdSede + " " + // Asegúrate de que IdSede sea un valor seguro para concatenar
             "GROUP BY E.id_Entrenador;";




            switch (filter)
            {
                case "all":
                    break;
                case "option1":
                    sql = cadenaSqlAreaEspecialidad(1, IdSede); // Crossfit 
                    break;
                case "option2":
                    sql = cadenaSqlAreaEspecialidad(2, IdSede); // Fuerza
                    break;
                case "option3":
                    sql = cadenaSqlAreaEspecialidad(3, IdSede); // Reducción de peso
                    break;
                case "option4":
                    sql = cadenaSqlGenero(1, IdSede); // Género Masculino
                    break;
                case "option5":
                    sql = cadenaSqlGenero(2, IdSede); // Género Femenino
                    break;
                case "option6":
                    sql = cadenaSqlGenero(3, IdSede); // Género No especificado
                    break;
                default:
                    break;
            }

            // Ejecuta la consulta y obtiene la lista filtrada de clientes
            clientesFiltrados = consultaEntrenador.ListarEntrenadores(sql);

            return Json(new
            {
                clientes = clientesFiltrados,
                totalClientes = clientesFiltrados.Count
            });
        }
        private string cadenaSqlAreaEspecialidad(int opcion, String IdSede)
        {
            string areaEspecialidad = "Crossfit";
            if (opcion == 2) areaEspecialidad = "Fuerza";
            if (opcion == 3) areaEspecialidad = "Reducción de peso";

            string sql2 = "SELECT E.id_Entrenador, " +
                          "MAX(P.nombre_Persona) AS nombre_entrenador, " +
                          "MAX(P.telefono_Persona) AS telefono_entrenador, " +
                          "MAX(AE.nombre_AE) AS area_especialidad, " +
                          "COUNT(C.id_Cliente) AS numero_clientes, " +
                          "MAX(CT.fecha_inicio_contrato) AS fecha_contratacion, " + // Usar fecha_inicio_contrato de la tabla Contrato
                          "MAX(CT.salario) AS salario " + // Agregar el salario del entrenador
                          "FROM Entrenador E " +
                          "INNER JOIN Persona P ON E.id_Entrenador = P.id_Persona " +
                          "INNER JOIN AreaEspecialidad AE ON E.id_AE = AE.id_AE " +
                          "LEFT JOIN Cliente C ON E.id_Entrenador = C.id_Entrenador " +
                          "LEFT JOIN Contrato CT ON E.id_Entrenador = CT.id_entrenador " + // Unir con la tabla Contrato
                          "WHERE P.id_Sede = " + IdSede + " " +
                          "AND AE.nombre_AE = '" + areaEspecialidad + "' " + // Filtrar por área de especialidad
                          "GROUP BY E.id_Entrenador;";




            return sql2;
        }
        [Authorize(Roles = "Administrador")]
        private string cadenaSqlGenero(int opcion, string IdSede)
        {
            string genero;
            if (opcion == 1) genero = "M";
            else if (opcion == 2) genero = "F";
            else genero = "NE";

            string sql = "SELECT E.id_Entrenador, " +
                         "MAX(P.nombre_Persona) AS nombre_entrenador, " +
                         "MAX(P.telefono_Persona) AS telefono_entrenador, " +
                         "MAX(AE.nombre_AE) AS area_especialidad, " +
                         "COUNT(C.id_Cliente) AS numero_clientes, " +
                         "MAX(CT.fecha_inicio_contrato) AS fecha_contratacion, " + // Agregado para obtener la fecha de inicio del contrato
                         "MAX(CT.salario) AS salario " + // Agregar el salario del entrenador
                         "FROM Entrenador E " +
                         "INNER JOIN Persona P ON E.id_Entrenador = P.id_Persona " +
                         "INNER JOIN AreaEspecialidad AE ON E.id_AE = AE.id_AE " +
                         "LEFT JOIN Cliente C ON E.id_Entrenador = C.id_Entrenador " +
                         "LEFT JOIN Contrato CT ON E.id_Entrenador = CT.id_entrenador " + // Unir con la tabla Contrato
                         "WHERE P.id_Sede = " + IdSede + " AND P.genero_Persona = '" + genero + "' " + // Filtrar por sede y género
                         "GROUP BY E.id_Entrenador;";


            return sql;
        }




    }
}
