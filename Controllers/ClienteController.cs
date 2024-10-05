using Microsoft.AspNetCore.Mvc;
using NET_MVC.Models;

namespace NET_MVC.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Registrar()
        {
            return View("RegistrarCliente");
        }

        public IActionResult AsignarEntrenador()
        {
           
            var entrenadores = new List<EntrenadorModel>
            {
                /*new EntrenadorModel { Id = 1, Nombre = "Juan Pérez", Especialidad = "Marketing Digital", ClientesAsignados = 5 },
                new EntrenadorModel { Id = 2, Nombre = "Ana Gómez", Especialidad = "Desarrollo Web", ClientesAsignados = 3 },
                new EntrenadorModel { Id = 2, Nombre = "Ana Gómez", Especialidad = "Desarrollo Web", ClientesAsignados = 3 },
                new EntrenadorModel { Id = 2, Nombre = "Ana Gómez", Especialidad = "Desarrollo Web", ClientesAsignados = 3 },
                new EntrenadorModel { Id = 2, Nombre = "Ana Gómez", Especialidad = "Desarrollo Web", ClientesAsignados = 3 },
                new EntrenadorModel { Id = 2, Nombre = "Ana Gómez", Especialidad = "Desarrollo Web", ClientesAsignados = 3 },
                new EntrenadorModel { Id = 2, Nombre = "Ana Gómez", Especialidad = "Desarrollo Web", ClientesAsignados = 3 },
                new EntrenadorModel { Id = 2, Nombre = "Ana Gómez", Especialidad = "Desarrollo Web", ClientesAsignados = 3 },
                new EntrenadorModel { Id = 2, Nombre = "Ana Gómez", Especialidad = "Desarrollo Web", ClientesAsignados = 3 },
                new EntrenadorModel { Id = 2, Nombre = "Ana Gómez", Especialidad = "Desarrollo Web", ClientesAsignados = 3 },
                new EntrenadorModel { Id = 3, Nombre = "Carlos López", Especialidad = "Diseño Gráfico", ClientesAsignados = 7 },
                new EntrenadorModel { Id = 4, Nombre = "María Rodríguez", Especialidad = "Desarrollo de Software", ClientesAsignados = 4 },
                new EntrenadorModel { Id = 5, Nombre = "Luis Fernández", Especialidad = "Gestión de Proyectos", ClientesAsignados = 6 },
                */
            };

            return View("AsignarEntrenadorCliente", entrenadores);
        }
    }
}
