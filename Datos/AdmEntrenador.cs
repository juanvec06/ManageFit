using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;

namespace NET_MVC.Datos
{
    public class AdmEntrenador
    {
        public OracleConnection conexionBD = Conexion.GetConnection();

        public bool RegistrarEntrenador(EntrenadorModel entrenador)
        {
            bool rpta = false;
            try
            {
                if (Conexion.abrirConexion())
                {
                    //llamada a procedimiento almacenado en base de datos 
                    using (OracleCommand cmd = new OracleCommand("insertar_Entrenador", conexionBD)) //VERIFICAR TENER CREADO EL METODO EN PLSQL
                    {
                        // Especifica que es un procedimiento
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        //Castear
                        int id = int.Parse(entrenador.Identificacion);
                        // Agrega los parámetros de entrada
                        cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = id;
                        cmd.Parameters.Add("p_nombreAE", OracleDbType.Varchar2).Value = entrenador.Especialidad;
                        cmd.Parameters.Add("p_salario", OracleDbType.Int32).Value = entrenador.Salario;
                        cmd.Parameters.Add("p_fechaContrato", OracleDbType.Date).Value = entrenador.fechaInicioContrato;
                        cmd.Parameters.Add("p_contraseña", OracleDbType.Varchar2).Value = entrenador.Contraseña;

                        cmd.ExecuteNonQuery();

                    }
                    rpta = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }
            return rpta;
        }
    }
}
