namespace NET_MVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClienteModel
    {
        public string Nombre { get; set; }
        public string Identificacion { get; set; }

        public string Genero { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Telefono { get; set; }

        public int IdSede { get; set; }
        public int IdEntrenador { get; set; }
        public string refMembresia { get; set; }
    }
}
