using Microsoft.AspNetCore.Mvc;

namespace NET_MVC.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Registrar()
        {
            return View("RegistrarCliente");
        }
    }
}
