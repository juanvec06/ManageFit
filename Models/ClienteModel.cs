namespace NET_MVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClienteModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        public int Identificacion { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio.")]
        [Phone(ErrorMessage = "Formato de teléfono inválido.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La sede es obligatoria.")]
        public int IdSede { get; set; }
        public int IdEntrenador { get; set; }
        public string refMembresia { get; set; }
    }
}
