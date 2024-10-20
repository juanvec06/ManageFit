using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;
using System.Security.Claims;

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
                        if (entrenador.Especialidad == "Reducción de peso") entrenador.Especialidad = "Reduccion de peso";
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

        public List<EntrenadorModel> ListarEntrenadoresDisponibles(string IdSede)
        {
            var entrenadores = new List<EntrenadorModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    string sql = "SELECT e.id_entrenador, MAX(p.nombre_persona) AS nombre_entrenador, MAX(nombre_ae) area_especialidad, COUNT(c.id_cliente) AS num_clientes " +
                        "FROM ENTRENADOR e INNER JOIN PERSONA p ON e.id_entrenador = p.id_persona " +
                        "INNER JOIN AREAESPECIALIDAD ae ON e.id_ae = ae.id_ae\r\nLEFT JOIN CLIENTE c ON c.id_entrenador = e.id_entrenador " +
                        "WHERE p.id_sede = " + IdSede +
                        "GROUP BY e.id_entrenador " +
                        "HAVING COUNT(c.id_cliente) < 5";  
                    using (OracleCommand cmd = new OracleCommand(sql, conexionBD))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            EntrenadorModel entrenador = new EntrenadorModel
                            {
                                Identificacion = reader["ID_ENTRENADOR"].ToString(),
                                Nombre = reader["NOMBRE_ENTRENADOR"].ToString(),
                                Especialidad = reader["AREA_ESPECIALIDAD"].ToString(),
                                ClientesAsignados = Convert.ToInt32(reader["NUM_CLIENTES"])
                            };
                            entrenadores.Add(entrenador);
                        }
                    }
                }
                return entrenadores;
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

        }
        
    }
}
