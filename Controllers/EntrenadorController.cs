using Microsoft.AspNetCore.Mvc;

namespace NET_MVC.Controllers
{
    public class EntrenadorController : Controller
    {

        public IActionResult RegistrarEntrenador()
        {
            return View("RegistrarEntrenador");
        }
    }
}
