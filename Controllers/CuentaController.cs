using Microsoft.AspNetCore.Mvc;
using NET_MVC.Models;

namespace NET_MVC.Controllers
{
    public class CuentaController : Controller
    {
        public IActionResult Login()
        {
            return View(); // Devuelve la vista Login.cshtml
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (!EsUsuarioValido(model.Usuario, model.Contraseña, out string rol))
                {
                    ViewBag.MensajeError = "Identificación o contraseña erróneos";
                    return View(model);
                }

                // Redirigir según el rol
                if (rol == "Administrador")
                {
                    return RedirectToAction("DashboardAdministrador", "Admin");
                }
                else if (rol == "Entrenador")
                {
                    return RedirectToAction("DashboardEntrenador", "Entrenador");
                }
            }

            ViewBag.MensajeError = "Identificación o contraseña requeridos";
            return View(model);
        }


        private bool EsUsuarioValido(string usuario, string contraseña, out string rol)
        {
            rol = null; // Inicializa rol

            if (usuario == "admin" && contraseña == "12345")
            {
                rol = "Administrador";
                return true;
            }
            else if (usuario == "entrenador" && contraseña == "67890")
            {
                rol = "Entrenador";
                return true;
            }

            return false; // Credenciales no válidas
        }

    }
}
