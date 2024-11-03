using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NET_MVC.Controllers
{
    public class PlanMejoramientoFisicoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult modificarPMF()
        {
            return View("ModificarPMF");
        }

        public IActionResult AgregarEjercicio()
        {
            return View("AgregarEjercicio");
        }

        public IActionResult PMF()
        {
            return View("PMF");
        }
    }
}
