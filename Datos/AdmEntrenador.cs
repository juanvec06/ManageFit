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

        public bool EntrenadorExistente(string idEntrenador)
        {
            bool existe = false;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(
                        "SELECT COUNT(*) FROM Entrenador WHERE id_entrenador = :p_id_entrenador", conexionBD))
                    {
                        // Agrega el parámetro id_entrenador para evitar inyecciones SQL
                        cmd.Parameters.Add(":p_id_entrenador", OracleDbType.Varchar2).Value = idEntrenador;

                        // Ejecuta la consulta y obtiene el número de coincidencias
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // Si el contador es mayor que 0, el entrenador existe
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
                // Cierra la conexión a la base de datos
                Conexion.cerrarConexion();
            }

            return existe;
        }


        public List<EntrenadorModel> ListarEntrenadores(string sql)
        {
            var entrenadores = new List<EntrenadorModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(sql, conexionBD))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                EntrenadorModel objEntrenador = new EntrenadorModel
                                {
                                    Identificacion = reader["id_Entrenador"]?.ToString() ?? "N/A",
                                    Genero = reader["genero_entrenador"]?.ToString() ?? "Desconocido",
                                    Nombre = reader["nombre_entrenador"]?.ToString() ?? "Desconocido",
                                    Telefono = reader["telefono_entrenador"]?.ToString() ?? "No disponible",
                                    Especialidad = reader["area_especialidad"]?.ToString() ?? "No especificada",
                                    fechaInicioContrato = reader["fecha_contratacion"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_contratacion"]) : DateTime.MinValue,
                                    ClientesAsignados = reader["numero_clientes"] != DBNull.Value ? Convert.ToInt32(reader["numero_clientes"]) : 0,
                                   
                                };
                                entrenadores.Add(objEntrenador);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron resultados.");
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

        public EntrenadorModel ObtenerEntrenadorPorIdentificacion(string identificacion)
        {
            EntrenadorModel EntrenadorEncontrado = null;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(
                        "SELECT E.id_Entrenador, " +
                        "P.nombre_Persona AS nombre_entrenador, " +
                        "P.telefono_Persona AS telefono_entrenador, " +
                        "P.genero_Persona AS genero_entrenador, " +
                        "AE.nombre_AE AS area_especialidad, " +
                        "CT.salario, " +
                        "CT.fecha_inicio_contrato AS fecha_contratacion " +
                        "FROM Entrenador E " +
                        "JOIN Persona P ON E.id_Entrenador = P.id_Persona " +
                        "JOIN AreaEspecialidad AE ON E.id_AE = AE.id_AE " +
                        "LEFT JOIN Contrato CT ON E.id_Entrenador = CT.id_entrenador " +
                        "WHERE E.id_Entrenador = :p_id_entrenador", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.Add(":p_id_entrenador", OracleDbType.Int32).Value = int.Parse(identificacion);

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                EntrenadorEncontrado = new EntrenadorModel
                                {
                                    Identificacion = reader["id_Entrenador"]?.ToString() ?? "N/A",
                                    Nombre = reader["nombre_entrenador"]?.ToString() ?? "Desconocido",
                                    Telefono = reader["telefono_entrenador"]?.ToString() ?? "No disponible",
                                    Genero = reader["genero_entrenador"]?.ToString() ?? "Desconocido",
                                    Especialidad = reader["area_especialidad"]?.ToString() ?? "No especificada",
                                    Salario = reader["salario"] != DBNull.Value ? reader["salario"].ToString() : "No especificado",
                                    fechaInicioContrato = reader["fecha_contratacion"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_contratacion"]) : DateTime.MinValue,
                                };
                            }
                        }
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

            return EntrenadorEncontrado;
        }

    }
}
