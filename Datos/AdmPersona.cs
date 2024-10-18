
using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;

namespace NET_MVC.Datos
{
    public class AdmPersona
    {
        public OracleConnection conexionBD = Conexion.GetConnection();

        public bool RegistrarPersona(PersonaModel persona)
        {
            bool rpta = false;
            try
            {
                if (Conexion.abrirConexion())
                {
                    // llamada a procedimiento almacenado en base de datos 
                    using (OracleCommand cmd = new OracleCommand("insertar_persona", conexionBD))
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Castear
                        int id = int.Parse(persona.Identificacion);
                        int idSede = int.Parse(persona.IdSede);

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_persona", OracleDbType.Int32).Value = persona.Identificacion;
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = persona.IdSede;
                        cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2).Value = persona.Nombre;
                        cmd.Parameters.Add("p_genero", OracleDbType.Char).Value = persona.Genero;
                        cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2).Value = persona.Telefono;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                    }
                    rpta = true;
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle: " + oex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general: " + ex.Message);
            }
            finally
            {
                Conexion.cerrarConexion(); //Cerrar la conexión
            }
            return rpta;
        }

        public bool PersonaExiste(string identificacion)
        {
            bool existe = false;

            if (!int.TryParse(identificacion, out int idPersona))
            {
                throw new Exception("La identificación debe ser un número entero válido.");
            }

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM PERSONA WHERE id_persona = :id", conexionBD))
                    {
                        cmd.Parameters.Add(new OracleParameter("id", idPersona));
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
