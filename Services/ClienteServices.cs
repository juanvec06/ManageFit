using NET_MVC.ConsultasSQL;
using NET_MVC.Datos;
using NET_MVC.Models;
using System.Data;
using System.Reflection.Metadata;

namespace NET_MVC.Services
{
    public class ClienteServices : UserService
    {
        #region Dependencias
        public ClienteConsulta _ClienteConsultas = new ClienteConsulta();
        #endregion

        #region Cruds
        public (ClienteModel objetoCliente, bool access, string mensaje) ObtenerClientePorIdentificacion(string identificacion)
        {

            #region Validadores
            var IdentificacionValidaResult = validarIdentificacion(identificacion);
            if (!IdentificacionValidaResult.success) return (null, false, IdentificacionValidaResult.mensaje);
            var ClienteExistenteResult = existeTupla(identificacion);
            if (!ClienteExistenteResult.success) return (null, false, ClienteExistenteResult.mensaje);
            #endregion

            #region Proceso
            ClienteModel clienteEncontrado = null;
            string consulta = _ClienteConsultas.consultarClientePorId(identificacion);
            DataTable dataTable = _DataBaseController.EjecutarConsulta(consulta);

            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                clienteEncontrado = new ClienteModel
                {
                    Identificacion = Convert.ToInt32(row["id_cliente"]),
                    Nombre = row["nombre"].ToString(),
                    Telefono = row["telefono"].ToString(),
                    Genero = row["genero"].ToString(),
                    refMembresia = row["tipo"].ToString(),
                    DiasRestantes = Convert.ToInt32(row["dias_restantes"])
                };
            }
            #endregion

            return (clienteEncontrado, true, "");
        }
        public List<ClienteModel> listarClienteFiltro(string filtre, string IdSede)
        {
            #region Proceso
            string consultaSql = _ClienteConsultas.consultaTodosFiltro(IdSede);
            switch (filtre)
            {
                case "all":
                    break;
                case "option1": // Clientes premium
                    consultaSql = _ClienteConsultas.consultaMembresiaFiltro(1, IdSede);
                    break;
                case "option2": // Clientes generales
                    consultaSql = _ClienteConsultas.consultaMembresiaFiltro(2, IdSede);
                    break;
                case "option3": // Género Masculino
                    consultaSql = _ClienteConsultas.consultaGeneroFiltro(1, IdSede);
                    break;
                case "option4": // Género Femenino
                    consultaSql = _ClienteConsultas.consultaGeneroFiltro(2, IdSede);
                    break;
                case "option5": // Género No especificado
                    consultaSql = _ClienteConsultas.consultaGeneroFiltro(3, IdSede);
                    break;
                default:
                    break;
            }

            List<ClienteModel> listaClientes = _DataBaseController.ListarObjetos<ClienteModel>(consultaSql, reader => new ClienteModel
            {
                Identificacion = Convert.ToInt32(reader["id_cliente"]),
                Nombre = reader["nombre"].ToString(),
                Telefono = reader["telefono"].ToString(),
                refMembresia = reader["membresia"].ToString(),
                FechaSuscripcion = DateTime.Parse(reader["fecha"].ToString())
            });
            #endregion

            return listaClientes;
        }
        public (bool success, string mensaje) registrarCliente(ClienteModel prmCliente)
        {
            #region Validadores
            var IdentificacionValidaResult = validarIdentificacion(prmCliente.Identificacion.ToString());
            if (!IdentificacionValidaResult.success) return IdentificacionValidaResult;
            var ClienteExistenteResult = existeTupla(prmCliente.Identificacion.ToString());
            if (ClienteExistenteResult.success) return ClienteExistenteResult;
            #endregion

            #region Proceso
            var InsertarClienteResult = _DataBaseController.InsertarObjeto("sp_InsertarCliente", prmCliente.obtenerParametros());
            if (InsertarClienteResult.success) return (true, "Cliente registrado correctamente."); 
            #endregion

            return (false, InsertarClienteResult.mensaje);
        }
        #endregion

        #region Utilidades
        public override (bool success, string mensaje) existeTupla(string prmIdentificacion)
        {
            bool result = _DataBaseController.ExisteTupla("Cliente", "id_cliente", prmIdentificacion);
            if (result) return (true, "Cliente ya existe.");
            return (false, "Cliente no encontrado.");
        } 
        #endregion
    }
}
