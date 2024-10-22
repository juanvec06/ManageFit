using NET_MVC.Controllers;
using NET_MVC.Models;

namespace NET_MVC.Services
{
    public class UserService
    {

        #region Dependencias
        public DataBaseController _DataBaseController = new DataBaseController();
        #endregion
        
        #region Validaciones
        public (bool success, string mensaje) validarIdentificacion(string prmIdentificacion)
        {
            if (string.IsNullOrWhiteSpace(prmIdentificacion)) return (false, "La identificación no puede estar vacía.");
            if (prmIdentificacion.Length > 10) return (false, "La identificación no puede tener más de 10 dígitos.");
            if (!int.TryParse(prmIdentificacion, out _)) return (false, "La identificación debe ser un número válido.");

            return (true, null);
        }
        public virtual (bool success, string mensaje) existeTupla(string prmIdentificacion) => (false, null); 
        #endregion

    }
}
