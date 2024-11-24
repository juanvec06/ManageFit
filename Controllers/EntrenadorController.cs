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
        AdmCliente consultaCliente = new AdmCliente();
        AdmEntrenador consultaEntrenador = new AdmEntrenador();

        [Authorize(Roles = "Entrenador")]
        public IActionResult DashboardEntrenador()
        {
            if (HttpContext.Session.GetString("ClienteIdEjercicio") != null)
                HttpContext.Session.Remove("ClienteIdEjercicio");
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
        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult BuscarEntrenador(string identificacion)
        {
            String IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            // Verificar que la identificación no esté vacía
            if (string.IsNullOrWhiteSpace(identificacion))
            {
                TempData["ErrorMessage"] = "La identificación no puede estar vacía.";
                return RedirectToAction("InformacionEntrenador"); // Redirigir a la página donde se muestra el formulario
            }

            // Verificar si la identificación es numérica
            if (!int.TryParse(identificacion, out int identificacionNumero))
            {
                TempData["ErrorMessage"] = "La identificación debe ser un número válido.";
                return RedirectToAction("InformacionEntrenador");
            }

            // Verificar que la identificación no tenga más de 10 dígitos
            if (identificacion.Length > 10)
            {
                TempData["ErrorMessage"] = "La identificación no puede tener más de 10 dígitos.";
                return RedirectToAction("InformacionEntrenador"); // Redirigir a la página donde se muestra el formulario
            }


            // Verificar que la identificación no sea menor a 0
            if (int.Parse(identificacion) < 0)
            {
                TempData["ErrorMessage"] = "La identificación debe ser un número positivo.";
                return RedirectToAction("InformacionEntrenador");
            }


            // Verificar si la persona existe en la base de datos
            bool EntrenadorExistente = consultaEntrenador.EntrenadorExistente(identificacion);

            if (EntrenadorExistente)
            {
                var entrenador = consultaEntrenador.ObtenerEntrenadorPorIdentificacion(identificacion, IdSede);

                // Verificar si se pudo obtener la información del cliente
                if (entrenador != null)
                {
                    return View("InformacionEntrenadorEspecifico", entrenador); // Mostrar la información del cliente
                }
                else
                {
                    TempData["ErrorMessage"] = "Entrenador no encontrado.";
                    return RedirectToAction("InformacionEntrenador");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Entrenador no encontrado.";
                return RedirectToAction("InformacionEntrenador");
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public JsonResult Filtrar(string filter)
        {
            List<EntrenadorModel> entrenadoresFiltrados = new List<EntrenadorModel>();
            String IdSede = User.FindFirst(ClaimTypes.Name)?.Value;

            // Ejecuta la consulta y obtiene la lista filtrada de clientes
            entrenadoresFiltrados = consultaEntrenador.ListarEntrenadores(IdSede, filter);

            return Json(new
            {
                entrenadores = entrenadoresFiltrados,
                totalEntrenadores = entrenadoresFiltrados.Count
            });
        }


        [Authorize(Roles = "Administrador")]
        public IActionResult BuscarActualizarSalario()
        {
            return View("BuscarActualizarSalario");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult ActualizarSalario(string idEntrenador)
        {
            try
            {
                string idSede = User.FindFirst(ClaimTypes.Name)?.Value;

                if (string.IsNullOrWhiteSpace(idEntrenador))
                {
                    TempData["ErrorMessage"] = "Debe ingresar una identificación válida.";
                    ViewBag.idEntrenador = idEntrenador;
                    return View("BuscarActualizarSalario");
                }

                // Validar que el ID sea numérico
                if (!int.TryParse(idEntrenador, out int idEntrenadorNum))
                {
                    TempData["ErrorMessage"] = "La identificación debe ser un número válido.";
                    ViewBag.idEntrenador = idEntrenador;
                    return View("BuscarActualizarSalario");
                }

                // Obtener datos del entrenador
                var entrenador = consultaEntrenador.ObtenerEntrenadorPorIdentificacion(idEntrenador, idSede);

                if (entrenador == null)
                {
                    TempData["ErrorMessage"] = "El entrenador con esa identificación no existe.";
                    ViewBag.idEntrenador = idEntrenador;
                    return View("BuscarActualizarSalario");
                }

                // Configurar el ViewBag para pasar el ID
                ViewBag.idEntrenador = idEntrenador;

                return View("ActualizarSalario", entrenador);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al buscar el entrenador: {ex.Message}";
                ViewBag.idEntrenador = idEntrenador;
                return View("BuscarActualizarSalario");
            }
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public IActionResult GuardarActualizacionSalario(string Identificacion, decimal Salario)
        {
            try
            {
                // Convertir identificación a entero y validar
                if (!int.TryParse(Identificacion, out int idEntrenador))
                {
                    TempData["ErrorMessage"] = "Debe proporcionar una identificación numérica válida.";
                    ViewBag.idEntrenador = Identificacion;
                    return View("ActualizarSalario");
                }

                // Validar salario y actualizar
                consultaEntrenador.ActualizarSalarioEntrenador(idEntrenador, Salario);

                TempData["SuccessMessage"] = "Transacción Exitosa. El salario del entrenador se actualizó exitosamente.";
                return RedirectToAction("BuscarActualizarSalario");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ViewBag.idEntrenador = Identificacion;
                return View("ActualizarSalario");
            }
        }

    }
}
