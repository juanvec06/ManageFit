
using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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

            if (string.IsNullOrEmpty(identificacion))
            {
                throw new Exception("La identificación no puede estar vacía.");
            }

            if (!int.TryParse(identificacion, out int idPersona))
            {
                throw new Exception("La identificación debe ser un número entero válido.");
            }

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("PERSONA_EXISTE", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada
                        cmd.Parameters.Add("P_ID_PERSONA", OracleDbType.Int32).Value = idPersona;

                        // Parámetro de salida
                        var existeParam = new OracleParameter("P_EXISTE", OracleDbType.Decimal)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(existeParam);

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();

                        // Convertir el valor de salida desde OracleDecimal a int
                        var existeDecimal = (Oracle.ManagedDataAccess.Types.OracleDecimal)existeParam.Value;
                        int existeResultado = existeDecimal.ToInt32();

                        // Si el resultado es 1, la persona existe
                        existe = existeResultado == 1;
                    }
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle: " + oex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return existe;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool eliminarPersona(string identificacion)
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
                    using (OracleCommand cmd = new OracleCommand("DELETE FROM PERSONA WHERE id_persona = :id", conexionBD))
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

        // GRAFICAS DEL INICIO:
        public int ObtenerTotalPersonasPorSede(int idSede)
        {
            int totalPersonas = 0;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = conexionBD;
                        cmd.CommandType = System.Data.CommandType.Text;

                        cmd.CommandText = "SELECT pkg_Estadisticas_Poblacion.fn_Total_Personas_Sede(:p_id_sede) FROM DUAL";
                        // Parámetro de entrada
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = idSede;

                        // Ejecutar
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            totalPersonas = int.Parse(result.ToString());
                        }
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

            return totalPersonas;
        }

        public int ObtenerIdSedePorUsuario(string usuario)
        {
            int idSede = 0;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("SELECT id_sede FROM CuentaSede WHERE id_sede = :usuario", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.Add(new OracleParameter("usuario", usuario));

                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int sedeId))
                        {
                            idSede = sedeId;
                        }
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
            return idSede;
        }

    }
}
