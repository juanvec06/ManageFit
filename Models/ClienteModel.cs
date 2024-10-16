namespace NET_MVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClienteModel : Persona
    {
        public DateTime FechaNacimiento { get; set; }
        public int IdEntrenador { get; set; }
        public string refMembresia { get; set; }
    }
}
