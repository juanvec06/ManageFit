using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NET_MVC.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {

        public IActionResult DashboardAdministrador()
        {
            //<summary>
            //  para el control del almacenamiento en cache de las vistas,
            //  con esto tiene que consultar al servidor antes de cargar una vista
            //</summary>
            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            //esto para conpatibilidad con versiones viejas
            Response.Headers.Add("Pragma", "no-cache");
            //El valor 0 significa que la página expira inmediatamente y no puede ser almacenada o reutilizada
            Response.Headers.Add("Expires", "0");
            return View(); 
        }


    }
}
