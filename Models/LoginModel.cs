namespace NET_MVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string? Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string? Contraseña { get; set; }

        public bool EsAdministrador { get; set; }
        public bool EsEntrenador { get; set; }
    }
}
