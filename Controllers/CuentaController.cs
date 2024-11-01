﻿using Microsoft.AspNetCore.Mvc;
using NET_MVC.Models;
using NET_MVC.Datos;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
//using AspNetCore;
namespace NET_MVC.Controllers
{
    public class CuentaController : Controller
    {
        public OracleConnection conexionBD = Conexion.GetConnection();
        [HttpGet]
        public IActionResult Login()
        {
            var rolClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            if (User.Identity.IsAuthenticated && rolClaim == "Administrador")
            {
                return RedirectToAction("DashboardAdministrador", "Admin"); // redirige según el rol
            }
            if (User.Identity.IsAuthenticated && rolClaim == "Entrenador")
            {
                return RedirectToAction("DashboardEntrenador", "Entrenador"); // redirige según el rol
            }
            return View(); // Devuelve la vista Login.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                if (!EsUsuarioValido(model.Usuario, model.Contraseña, out string rol))
                {
                    ViewBag.MensajeError = "Identificación o contraseña erróneos";
                    return View(model);
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Usuario), // Nombre del usuario
                    new Claim(ClaimTypes.Role, rol),            // Rol del usuario (Administrador, Entrenador)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Iniciar la sesión con cookies
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity)
                    
                );
                // Redirigir según el rol (usando replace para no agregar la página de login al historial)
                if (rol == "Administrador")
                {
                    Response.Redirect(Url.Action("DashboardAdministrador", "Admin"), true);
                    return new EmptyResult();
                }
                else if (rol == "Entrenador")
                {
                    return RedirectToAction("DashboardEntrenador", "Entrenador", new { replace = true });
                }
            }
            ViewBag.MensajeError = "Identificación y contraseña requeridos";
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Cuenta");
        }
        private bool EsUsuarioValido(string usuario, string contraseña, out string rol)
        {
            rol = null; // Inicializa rol
            try
            {
                int idUsuario;
                bool esNumero = int.TryParse(usuario, out idUsuario);
                if (!esNumero)
                {
                    return false;
                }
                if (Conexion.abrirConexion())
                {
                    //llamada a procedimiento almacenado en base de datos para validar Administrador
                    using (OracleCommand cmd = new OracleCommand("validar_logina", conexionBD)) //VERIFICAR TENER CREADO EL METODO EN PLSQL
                    {
                        // Especifica que es una función
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agrega los parámetros de entrada
                        cmd.Parameters.Add("p_usuario", OracleDbType.Int32).Value = idUsuario;
                        cmd.Parameters.Add("p_contraseña", OracleDbType.Varchar2).Value = contraseña;

                        // Parámetro de retorno (el valor que retorna la función)
                        OracleParameter vCountParam = new OracleParameter("p_count", OracleDbType.Int32);
                        vCountParam.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(vCountParam);
                        // Ejecuta la función

                        cmd.ExecuteNonQuery();

                        // Obtiene el valor de retorno
                        OracleDecimal resultadoOracle = (OracleDecimal)cmd.Parameters["p_count"].Value;
                        int resultado = resultadoOracle.ToInt32();
                        if (resultado > 0)
                        {
                            rol = "Administrador";
                            return true;
                        }
                    }
                    //llamada a procedimiento almacenado en base de datos para validar entrenador
                    using (OracleCommand cmd = new OracleCommand("validar_logine", conexionBD)) //VERIFICAR TENER CREADO EL METODO EN PLSQL
                    {
                        // Especifica que es una función
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agrega los parámetros de entrada
                        cmd.Parameters.Add("p_usuario", OracleDbType.Int32).Value = idUsuario;
                        cmd.Parameters.Add("p_contraseña", OracleDbType.Varchar2).Value = contraseña;

                        // Parámetro de retorno (el valor que retorna la función)
                        OracleParameter vCountParam = new OracleParameter("p_count", OracleDbType.Int32);
                        vCountParam.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(vCountParam);
                        // Ejecuta la función

                        cmd.ExecuteNonQuery();

                        // Obtiene el valor de retorno
                        OracleDecimal resultadoOracle = (OracleDecimal)cmd.Parameters["p_count"].Value;
                        int resultado = resultadoOracle.ToInt32();
                        if (resultado > 0)
                        {
                            rol = "Entrenador";
                            return true;
                        }
                    }
                }
            }
            catch (OracleException Ex)
            {
                Console.WriteLine("Error de Oracle: " + Ex.Message);
                return false;
            }
            finally
            {
                Conexion.cerrarConexion();
            }
            return false; // Credenciales no válidas
        }

        public IActionResult Home()
        {
            return RedirectToAction("DashboardAdministrador", "Admin");
        }
        /**
        *<summary>
        *   el metodo se encarga de redirigir a la pagina anterior verificando dependiendo de donde venga
        *   la llamada a la funcion para que no se guarden los datos en la base de datos
        *</summary>>
        */
        
        public IActionResult PaginaAnterior()
        {
            // Obtenemos la URL de la que viene la solicitud
            var referer = Request.Headers["Referer"].ToString();
            AdmCliente actualCliente = new AdmCliente();
            AdmPersona actualPersona = new AdmPersona();

            // Validamos si la URL contiene el nombre de la vista AsignarEntrenadorCliente
            if (referer.Contains("AsignarEntrenadorCliente"))
            {
                return RedirectToAction("Registrar", "Cliente");
            }


            // Si no proviene de la vista AsignarEntrenadorCliente, redirige a la página actual
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            // Si no se puede obtener la URL anterior, redirige a una página por defecto
            return RedirectToAction("DashboardAdministrador", "Admin");
        }

        public IActionResult PaginaAnteriorInfoCliente()
        {
            return RedirectToAction("InformacionCliente", "Cliente");
        }

    }
}