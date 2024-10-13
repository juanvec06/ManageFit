using System.ComponentModel.DataAnnotations;

namespace NET_MVC.Models
{
    public class EntrenadorModel
    {
        public required string Id { get; set; }
        public string? IdSede { get; set; }
        public required string Nombre { get; set; }
        public required string Genero { get; set; }
        public required string Telefono { get; set; }
        public required string Especialidad { get; set; }
        public required string Salario { get; set; }
        public required DateTime fechaInicioContrato { get; set; }
        public required string Contraseña { get; set; }
        public required int ClientesAsignados { get; set; }

    }
}
