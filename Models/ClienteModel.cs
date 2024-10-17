namespace NET_MVC.Models
{
    public class ClienteModel : PersonaModel
    {
        public DateTime FechaNacimiento { get; set; }
        public int IdEntrenador { get; set; }
        public string refMembresia { get; set; }
        public string? fechaMembresia { get; set; }
    }
}
