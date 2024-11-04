
namespace NET_MVC.Models
{
    public class EntrenadorModel : PersonaModel
    {
        public string Especialidad { get; set; }
        public string Salario { get; set; }
        public DateTime fechaInicioContrato { get; set; }
        public string Contraseña { get; set; }
        public required int ClientesAsignados { get; set; }
    }
}
