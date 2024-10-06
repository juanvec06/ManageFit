namespace NET_MVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class LoginModel
    {
        [Required(ErrorMessage = "   !")]
        public string? Usuario { get; set; }
        [Required(ErrorMessage = "   !")]
        public string? Contraseña { get; set; }

        public bool EsAdministrador { get; set; }
        public bool EsEntrenador { get; set; }
    }
}
