namespace NET_MVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ClienteModel
    {
        public string? Nombre { get; set; }

        public int? Identificacion { get; set; }

        public string? genero { get; set; }
        public string? membresia { get; set; }

        public string fecha { get; set; }
    }
}
