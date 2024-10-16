namespace NET_MVC.Models
{
    public class Persona
    {
        public required string Nombre { get; set; }
        public required string Id { get; set; }
        public required string Telefono { get; set; }
        public int? IdSede { get; set; }
        public required string Genero { get; set; }
    }
}
