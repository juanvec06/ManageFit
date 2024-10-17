using System.ComponentModel.DataAnnotations;

namespace NET_MVC.Models
{
    public class EntrenadorModel
    {
        public required string Id { get; set; }
        public string? IdSede { get; set; }
        public required string Nombre { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        public required string Especialidad { get; set; }
        public string Salario { get; set; }
        public DateTime fechaInicioContrato { get; set; }
        public string Contraseña { get; set; }
        public required int ClientesAsignados { get; set; }

    }
}
