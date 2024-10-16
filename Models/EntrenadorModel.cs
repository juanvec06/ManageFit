using System.ComponentModel.DataAnnotations;

namespace NET_MVC.Models
{
    public class EntrenadorModel : Persona
    {
        public required string Especialidad { get; set; }
        public required string Salario { get; set; }
        public required DateTime fechaInicioContrato { get; set; }
        public required string Contraseña { get; set; }
        public required int ClientesAsignados { get; set; }

    }
}
