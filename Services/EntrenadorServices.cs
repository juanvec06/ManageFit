using NET_MVC.ConsultasSQL;
using NET_MVC.Datos;
using NET_MVC.Models;

namespace NET_MVC.Services
{
    public class EntrenadorServices : UserService
    {
        #region Dependencias
        public EntrenadorConsulta _EntrenadorConsulta = new EntrenadorConsulta();
        #endregion

        #region Cruds
        public List<EntrenadorModel> obternerEntrenadoresDisponibles(string IdSede)
        {
            string consultaSql = _EntrenadorConsulta.consultaEntrenadoresDisponibles(IdSede);
            return _DataBaseController.ListarObjetos<EntrenadorModel>(consultaSql, reader => new EntrenadorModel
            {
                Identificacion = Convert.ToInt32(reader["ID_ENTRENADOR"]),
                Nombre = reader["NOMBRE_ENTRENADOR"].ToString(),
                Especialidad = reader["AREA_ESPECIALIDAD"].ToString(),
                NumeroClientes = Convert.ToInt32(reader["NUM_CLIENTES"])
            });
        }
        public (bool success, string mensaje) registrarEntrenador(EntrenadorModel prmEntrenador)
        {
            #region Validadores
            var IdentificacionValidaResult = validarIdentificacion(prmEntrenador.Identificacion.ToString());
            if (!IdentificacionValidaResult.success) return IdentificacionValidaResult;
            var EntrenadorExistenteResult = existeTupla(prmEntrenador.Identificacion.ToString());
            if (EntrenadorExistenteResult.success) return IdentificacionValidaResult;
            #endregion

            #region Proceso
            prmEntrenador.Nombre = AjustarNombre(prmEntrenador.Nombre);
            var InsertarEntrenadorResult = _DataBaseController.InsertarObjeto("sp_InsertarEntrenador", prmEntrenador.obtenerParametros());
            if (InsertarEntrenadorResult.success) return (true, "Entrenador registrado correctamente.");
            #endregion

            return (false, InsertarEntrenadorResult.mensaje);
        } 
        #endregion

        #region Utilidades
        public override (bool success, string mensaje) existeTupla(string prmIdentificacion)
        {
            bool result = _DataBaseController.ExisteTupla("Entrenador", "id_Entrenador", prmIdentificacion);
            if (result) return (true, "Entrenador ya existe.");
            return (false, "Entrenador no encontrado.");
        }
        #endregion
    }
}
