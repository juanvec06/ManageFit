using Microsoft.AspNetCore.Mvc;

namespace NET_MVC.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult DashboardAdministrador()
        {
            return View(); 
        }


    }
}
