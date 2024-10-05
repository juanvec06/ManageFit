using System.ComponentModel.DataAnnotations;

namespace NET_MVC.Models
{
    public class EntrenadorModel
    {
        [RegularExpression("^[0-9]\\d*$|^0$", ErrorMessage = "Identificación invalida")]
        public required string Id { get; set; }
        public string? IdSede { get; set; }
        public required string Nombre { get; set; }
        public required string Genero { get; set; }
        [RegularExpression("^3\\d{9}$", ErrorMessage = "El telefono no es válido")] 
        public required string Telefono { get; set; }
        public required string Especialidad { get; set; }
        [RegularExpression("^[0-9]\\d*$|^0$", ErrorMessage = "Salario invalido")]
        public required string Salario { get; set; }
        public required DateTime fechaInicioContrato { get; set; }
        public required string Contraseña { get; set; }
        public required int ClientesAsignados { get; set; }

    }
}
