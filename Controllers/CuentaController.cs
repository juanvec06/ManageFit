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
                if (!EsUsuarioValido(model.Usuario, model.Contraseña))
                {
                    ViewBag.MensajeError = "Identificación o contraseña erróneos";
                    return View(model);
                }

                // Redirigir según el rol
                if (model.EsAdministrador)
                {
                    return RedirectToAction("DashboardAdministrador", "Admin");
                }
                else
                {
                    return RedirectToAction("DashboardEntrenador", "Entrenador");
                }
            }

            ViewBag.MensajeError = "Identificación o contraseña requeridos";
            return View(model);
        }

        private bool EsUsuarioValido(string usuario, string contraseña)
        {
            return usuario == "admin" && contraseña == "12345"; // ejemplo
        }
    }

}
