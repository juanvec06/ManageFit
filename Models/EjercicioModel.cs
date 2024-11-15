namespace NET_MVC.Models
{
    public class EjercicioModel
    {
        public int? IdCliente { get; set; }
        public DateTime? FechaValoracion { get; set; }
        public int? Nombre { get; set; }
        public string? NombreString { get; set; }
        public int? Repeticiones { get; set; }
        public int? Series { get; set; }
        public string? Dia { get; set; }
        public List<NombreEjercicio>? Opciones { get; set; }
    }
}
