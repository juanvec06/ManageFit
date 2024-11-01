using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NET_MVC.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {

        public IActionResult DashboardAdministrador()
        {
            return View(); 
        }


    }
}
