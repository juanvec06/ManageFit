using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;

namespace NET_MVC.Datos
{
    public class AdmPMF
    {
        public OracleConnection conexionBD = Conexion.GetConnection();

        public bool AgregarPMF(PMFModel pmf)
        {
            bool rpta = false;
            try
            {
                if (Conexion.abrirConexion())
                {

                    // llamada a procedimiento almacenado en base de datos 
                    using (OracleCommand cmd = new OracleCommand("crearPMF", conexionBD))
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = pmf.IdCliente;
                        cmd.Parameters.Add("p_fecha_valoracion", OracleDbType.Date).Value = pmf.FechaValoracion;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                    }
                    rpta = true;
                }
            }
            catch (OracleException oex)
            {
                throw oex;
            }
            finally
            {
                Conexion.cerrarConexion(); //Cerrar la conexión
            }
            return rpta;
        }

        public bool AgregarEjercicio(EjercicioModel ejercicio)
        {
            bool rpta = false;
            try
            {
                if (Conexion.abrirConexion())
                {

                    // llamada a procedimiento almacenado en base de datos 
                    using (OracleCommand cmd = new OracleCommand("crearEjercicio", conexionBD))
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = ejercicio.IdCliente;
                        cmd.Parameters.Add("p_id_fecha_valoracion", OracleDbType.Date).Value = ejercicio.FechaValoracion;
                        cmd.Parameters.Add("p_id_nombre_ejercicio", OracleDbType.Int32).Value = ejercicio.Nombre;
                        cmd.Parameters.Add("p_numero_repeticiones", OracleDbType.Int32).Value = ejercicio.Repeticiones;
                        cmd.Parameters.Add("p_numero_series", OracleDbType.Int32).Value = ejercicio.Series;
                        cmd.Parameters.Add("p_dia_semana", OracleDbType.Varchar2).Value = ejercicio.Dia;

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

        public List<NombreEjercicio> ObtenerOpciones()
        {
            List<NombreEjercicio> opciones = new List<NombreEjercicio>();

            try
            {
                if (Conexion.abrirConexion())
                {
                    // llamada a procedimiento almacenado en base de datos 
                    using (OracleCommand cmd = new OracleCommand("SELECT ID_NOMBRE_EJERCICIO,NOMBRE_EJERCICIO FROM NOMBREEJERCICIO", conexionBD))
                    {

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                opciones.Add(new NombreEjercicio
                                {
                                    IdEjercicio = reader.GetInt32(0),    // Primer columna (Id)
                                    NameEjercicio = reader.GetString(1) // Segunda columna (Nombre)
                                });
                            }
                        }

                    }
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
            return opciones;
        }

        public List<EjercicioModel> ListarEjercicios(int idCliente, DateTime FechaValoracion)
        {
            string fecha = FechaValoracion.ToString("yyyy-MM-dd");
            List<EjercicioModel> ejercicios = new List<EjercicioModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    string sql = "SELECT NOMBREEJERCICIO.NOMBRE_EJERCICIO as nombre, " +
                                 "EJERCICIO.NUMERO_REPETICIONES as repeticiones, " +
                                 "EJERCICIO.NUMERO_SERIES as series, " +
                                 "EJERCICIO.DIA_SEMANA dia " +
                                 "FROM EJERCICIO " +
                                 "INNER JOIN NOMBREEJERCICIO ON EJERCICIO.ID_NOMBRE_EJERCICIO = NOMBREEJERCICIO.ID_NOMBRE_EJERCICIO " +
                                 "WHERE EJERCICIO.ID_CLIENTE = " + idCliente + " AND EJERCICIO.ID_FECHA_VALORACION = TO_DATE('" + fecha + "', 'yyyy-MM-dd')";

                    using (OracleCommand cmd = new OracleCommand(sql, conexionBD))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            EjercicioModel objejercicio = new EjercicioModel
                            {
                                NombreString = reader["nombre"].ToString(),
                                Repeticiones = Convert.ToInt32(reader["repeticiones"]),
                                Series = Convert.ToInt32(reader["series"]),
                                Dia = reader["dia"].ToString(),
                            };
                            ejercicios.Add(objejercicio);
                        }
                    }
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
            return ejercicios;
        }

        public bool PmfExistente(PMFModel pmf)
        {
            bool existe = false;
            DateTime fecha = Convert.ToDateTime(pmf.FechaValoracion);
            string fechaString = fecha.ToString("yyyy-MM-dd");

            try
            {
                string sql = "SELECT COUNT(*) FROM pmf WHERE id_cliente = " + pmf.IdCliente +
                    " AND FECHA_VALORACION = TO_DATE('" + fechaString + "', 'yyyy-MM-dd')";
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(sql, conexionBD))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // Si el contador es mayor a 0, significa que el cliente existe
                        existe = count > 0;
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
    }
}
