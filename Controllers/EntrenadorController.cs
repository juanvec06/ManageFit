using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NET_MVC.Datos;
using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
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
        [Authorize(Roles = "Administrador")]
        public JsonResult Filtrar(string filter)
        {
            try
            {

                List<EntrenadorModel> entrenadoresFiltrados = new List<EntrenadorModel>();
                string IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

                string sql = "SELECT E.id_Entrenador, " +
                 "MAX(P.nombre_Persona) AS nombre_entrenador, " +
                 "MAX(P.telefono_Persona) AS telefono_entrenador, " +
                 "MAX(P.genero_persona) AS genero_entrenador, " +
                 "MAX(AE.nombre_AE) AS area_especialidad, " +
                 "COUNT(C.id_Cliente) AS numero_clientes, " +
                 "MAX(CT.fecha_inicio_contrato) AS fecha_contratacion, " +
                 "MAX(CT.salario) AS salario " +
                 "FROM Entrenador E " +
                 "INNER JOIN Persona P ON E.id_Entrenador = P.id_Persona " +
                 "INNER JOIN AreaEspecialidad AE ON E.id_AE = AE.id_AE " +
                 "LEFT JOIN Cliente C ON E.id_Entrenador = C.id_Entrenador " +
                 "LEFT JOIN Contrato CT ON E.id_Entrenador = CT.id_entrenador " +
                 "WHERE P.id_Sede = " + IdSede +
                 " GROUP BY E.id_Entrenador";


                // Aplica el filtro según la selección
                switch (filter)
                {
                    case "all":
                        break;
                    case "crossfit":
                        sql = cadenaSqlAreaEspecialidad(1, IdSede); // Crossfit
                        break;
                    case "fuerza":
                        sql = cadenaSqlAreaEspecialidad(2, IdSede); // Fuerza
                        break;
                    case "reduccion":
                        sql = cadenaSqlAreaEspecialidad(3, IdSede); // Reducción de peso
                        break;
                    case "masculino":
                        sql = cadenaSqlGenero(1, IdSede); // Género Masculino
                        break;
                    case "femenino":
                        sql = cadenaSqlGenero(2, IdSede); // Género Femenino
                        break;
                    case "no-especificado":
                        sql = cadenaSqlGenero(3, IdSede); // Género No especificado
                        break;
                    default:
                        break;
                }

                // Ejecutar la consulta y obtener los entrenadores filtrados
                entrenadoresFiltrados = consultaEntrenador.ListarEntrenadores(sql);

                return Json(new
                {
                    entrenadores = entrenadoresFiltrados,
                    totalEntrenadores = entrenadoresFiltrados.Count
                });

                }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [Authorize(Roles = "Administrador")]
        private string cadenaSqlAreaEspecialidad(int opcion, string IdSede)
        {
            string areaEspecialidad = opcion == 1 ? "Crossfit" :
                                      opcion == 2 ? "Fuerza" :
                                      opcion == 3 ? "Reducción de peso" : "Culturismo";

            return "SELECT E.id_Entrenador, " +
          "MAX(P.nombre_Persona) AS nombre_entrenador, " +
          "MAX(P.telefono_Persona) AS telefono_entrenador, " +
          "MAX(P.genero_persona) AS genero_entrenador, " +
          "MAX(AE.nombre_AE) AS area_especialidad, " +
          "COUNT(C.id_Cliente) AS numero_clientes, " +
          "MAX(CT.fecha_inicio_contrato) AS fecha_contratacion, " +
          "MAX(CT.salario) AS salario " +
          "FROM Entrenador E " +
          "INNER JOIN Persona P ON E.id_Entrenador = P.id_Persona " +
          "INNER JOIN AreaEspecialidad AE ON E.id_AE = AE.id_AE " +
          "LEFT JOIN Cliente C ON E.id_Entrenador = C.id_Entrenador " +
          "LEFT JOIN Contrato CT ON E.id_Entrenador = CT.id_entrenador " +
          "WHERE P.id_Sede = " + IdSede + " AND AE.nombre_AE = '" + areaEspecialidad + "' " +
          "GROUP BY E.id_Entrenador";
        }


        [Authorize(Roles = "Administrador")]
        private string cadenaSqlGenero(int opcion, string IdSede)
        {
            string genero = opcion == 1 ? "M" : opcion == 2 ? "F" : "NE";

            return "SELECT E.id_Entrenador, " +
          "MAX(P.nombre_Persona) AS nombre_entrenador, " +
          "MAX(P.telefono_Persona) AS telefono_entrenador, " +
          "MAX(P.genero_persona) AS genero_entrenador, " +
          "MAX(AE.nombre_AE) AS area_especialidad, " +
          "COUNT(C.id_Cliente) AS numero_clientes, " +
          "MAX(CT.fecha_inicio_contrato) AS fecha_contratacion, " +
          "MAX(CT.salario) AS salario " +
          "FROM Entrenador E " +
          "INNER JOIN Persona P ON E.id_Entrenador = P.id_Persona " +
          "INNER JOIN AreaEspecialidad AE ON E.id_AE = AE.id_AE " +
          "LEFT JOIN Cliente C ON E.id_Entrenador = C.id_Entrenador " +
          "LEFT JOIN Contrato CT ON E.id_Entrenador = CT.id_entrenador " +
          "WHERE P.id_Sede = " + IdSede + " AND P.genero_Persona = '" + genero + "' " +
          "GROUP BY E.id_Entrenador";
        }
    }
}
