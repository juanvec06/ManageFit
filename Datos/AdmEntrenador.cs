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
                if (Conexion.abrirConexion())  // Asegúrate de que el método abrirConexion() funcione correctamente y devuelva una conexión válida
                {
                    using (OracleCommand cmd = new OracleCommand("insertar_Entrenador", conexionBD)) // Asegúrate de que 'conexionBD' esté bien inicializada
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Transformar los datos que vienen del modelo
                        int salario = int.Parse(entrenador.Salario);

                        // Agregar los parámetros de entrada al procedimiento almacenado
                        cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = entrenador.Id;
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = entrenador.IdSede;
                        cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = entrenador.Nombre;
                        cmd.Parameters.Add("p_genero", OracleDbType.Varchar2).Value = entrenador.Genero;
                        cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = entrenador.Telefono;
                        cmd.Parameters.Add("p_nombreAE", OracleDbType.Varchar2).Value = entrenador.Especialidad;
                        cmd.Parameters.Add("p_salario", OracleDbType.Int32).Value = salario;
                        cmd.Parameters.Add("p_fechaContrato", OracleDbType.Date).Value = entrenador.fechaInicioContrato ;
                        cmd.Parameters.Add("p_contraseña", OracleDbType.Varchar2).Value = entrenador.Contraseña;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();

                        // Si la ejecución llega aquí sin errores, marcamos como éxito
                        rpta = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                throw new Exception("Error al registrar entrenador: " + ex.Message);
            }
            finally
            {
                // Asegurarse de cerrar la conexión
                Conexion.cerrarConexion();
            }
            return rpta;
        }
        public bool EntrenadorExiste(string identificacion)
        {
            bool existe = false;

            if (!int.TryParse(identificacion, out int idEntrenador))
            {
                throw new Exception("La identificación debe ser un número entero válido.");
            }

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM ENTRENADOR WHERE id_entrenador = :id", conexionBD))
                    {
                        cmd.Parameters.Add(new OracleParameter("id", idEntrenador));
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        existe = count > 0;
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Error en Oracle: " + ex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }
            return existe;
        }

    }
}
