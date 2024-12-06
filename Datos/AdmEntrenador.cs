using Microsoft.AspNetCore.Authorization;
using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
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
                    using (OracleCommand cmd = new OracleCommand("pkg_Inserciones.insertar_Entrenador", conexionBD)) //VERIFICAR TENER CREADO EL METODO EN PLSQL
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
        
        public List<EntrenadorModel> ListarEntrenadoresDisponibles(string idSede)
        {
            var entrenadores = new List<EntrenadorModel>();

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.sp_listar_entrenadores_disponibles", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Parámetro de entrada
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = int.Parse(idSede);

                        // Parámetro de salida para el cursor
                        OracleParameter refCursorParam = new OracleParameter("p_resultado", OracleDbType.RefCursor);
                        refCursorParam.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(refCursorParam);

                        // Ejecutar el comando
                        cmd.ExecuteNonQuery();

                        // Verificar si el cursor tiene datos antes de intentar leer
                        if (refCursorParam.Value != null)
                        {
                            using (OracleDataReader reader = ((OracleRefCursor)refCursorParam.Value).GetDataReader())
                            {
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
                        else
                        {
                            throw new Exception("El cursor no contiene datos.");
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
                Conexion.cerrarConexion(); // Cerrar la conexión
            }
        }

        public bool EntrenadorExistente(string idEntrenador)
        {
            if (string.IsNullOrEmpty(idEntrenador))
            {
                throw new ArgumentException("El ID del entrenador no puede estar vacío.");
            }

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.ENTRENADOR_EXISTE", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada
                        cmd.Parameters.Add("P_ID_ENTRENADOR", OracleDbType.Varchar2).Value = idEntrenador;

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
                        int existe = existeDecimal.ToInt32();

                        // Si el valor es 1, el entrenador existe
                        return existe == 1;
                    }
                }
                return false;
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle: " + oex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }
        }

        public List<EntrenadorModel> ListarEntrenadores(string idSede, string filtro)
        {
            var entrenadores = new List<EntrenadorModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.LISTAR_ENTRENADORES", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("P_ID_SEDE", OracleDbType.Varchar2).Value = idSede;
                        cmd.Parameters.Add("P_FILTER", OracleDbType.Varchar2).Value = filtro;

                        // Parámetro de salida (cursor)
                        var refCursor = new OracleParameter("P_ENTRENADORES", OracleDbType.RefCursor)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(refCursor);

                        // Ejecutar el procedimiento almacenado
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var objEntrenador = new EntrenadorModel
                                {
                                    Identificacion = reader["id_Entrenador"]?.ToString() ?? "N/A",
                                    Genero = reader["genero_entrenador"]?.ToString() ?? "Desconocido",
                                    Nombre = reader["nombre_entrenador"]?.ToString() ?? "Desconocido",
                                    Telefono = reader["telefono_entrenador"]?.ToString() ?? "No disponible",
                                    Especialidad = reader["area_especialidad"]?.ToString() ?? "No especificada",
                                    fechaInicioContrato = reader["fecha_contratacion"] != DBNull.Value
                                        ? Convert.ToDateTime(reader["fecha_contratacion"])
                                        : DateTime.MinValue,
                                    ClientesAsignados = reader["numero_clientes"] != DBNull.Value
                                        ? Convert.ToInt32(reader["numero_clientes"])
                                        : 0
                                };
                                entrenadores.Add(objEntrenador);
                            }
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

        public EntrenadorModel ObtenerEntrenadorPorIdentificacion(string identificacion, string idSede)
        {
            EntrenadorModel EntrenadorEncontrado = null;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.ObtenerEntrenadorPorIdentificacion", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Pasamos los parámetros de entrada
                        cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = int.Parse(identificacion);
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = int.Parse(idSede);

                        // Parámetros de salida
                        cmd.Parameters.Add("p_identificacion", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_genero", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_especialidad", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_salario", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_fecha_contratacion", OracleDbType.Date).Direction = ParameterDirection.Output;

                        // Ejecutamos el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                            // Crear objeto de modelo con los valores de salida
                            EntrenadorEncontrado = new EntrenadorModel
                            {
                                Identificacion = cmd.Parameters["p_identificacion"].Value != DBNull.Value ? cmd.Parameters["p_identificacion"].Value.ToString() : "N/A",
                                Nombre = cmd.Parameters["p_nombre"].Value != DBNull.Value ? cmd.Parameters["p_nombre"].Value.ToString() : "Desconocido",
                                Telefono = cmd.Parameters["p_telefono"].Value != DBNull.Value ? cmd.Parameters["p_telefono"].Value.ToString() : "No disponible",
                                Genero = cmd.Parameters["p_genero"].Value != DBNull.Value ? cmd.Parameters["p_genero"].Value.ToString() : "Desconocido",
                                Especialidad = cmd.Parameters["p_especialidad"].Value != DBNull.Value ? cmd.Parameters["p_especialidad"].Value.ToString() : "No especificada",
                                Salario = cmd.Parameters["p_salario"].Value != DBNull.Value ? cmd.Parameters["p_salario"].Value.ToString() : "No especificado",
                                fechaInicioContrato = cmd.Parameters["p_fecha_contratacion"].Value != DBNull.Value
                                ? ((OracleDate)cmd.Parameters["p_fecha_contratacion"].Value).Value // Uso de .Value para obtener DateTime
                                : DateTime.MinValue
                            };
                    }
                }
            }
            catch (OracleException oex)
            {
                return EntrenadorEncontrado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error general: " + ex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return EntrenadorEncontrado;
        }

        // GRAFICAS DEL INICIO:
        public int ObtenerTotalEntrenadoresPorSede(int idSede)
        {
            int totalEntrenadores = 0;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.ObtenerTotalEntrenadoresPorSede", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = idSede;

                        // Parámetro de salida
                        cmd.Parameters.Add("p_total_entrenadores", OracleDbType.Int32).Direction = ParameterDirection.Output;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida y convertirlo a entero
                        OracleDecimal oracleDecimalValue = (OracleDecimal)cmd.Parameters["p_total_entrenadores"].Value;
                        totalEntrenadores = oracleDecimalValue.ToInt32();  // Convertir de OracleDecimal a int
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Error en Oracle: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general: " + ex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return totalEntrenadores;
        }

        public int NumeroClientesAsignados(int idEntrenador)
        {
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.NumeroClientesAsignados", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada y salida
                        cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = idEntrenador;
                        var totalClientes = new OracleParameter("p_total_clientes", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(totalClientes);

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        // Accede al valor de OracleDecimal correctamente
                        if (totalClientes.Value != DBNull.Value)
                        {
                            OracleDecimal oracleDecimal = (OracleDecimal)totalClientes.Value;
                            return oracleDecimal.IsNull ? 0 : oracleDecimal.ToInt32(); // Asegúrate de que no sea nulo antes de convertir
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                return 0;
            }
            finally
            {
                Conexion.cerrarConexion();
            }
        }

        public int DiasRestantesContrato(int idEntrenador)
        {
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.DiasRestantesContrato", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada y salida
                        cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = idEntrenador;
                        var diasRestantes = new OracleParameter("p_dias_restantes", OracleDbType.Int32)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(diasRestantes);

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        // Accede al valor de OracleDecimal correctamente
                        if (diasRestantes.Value != DBNull.Value)
                        {
                            OracleDecimal oracleDecimal = (OracleDecimal)diasRestantes.Value;
                            return oracleDecimal.IsNull ? 0 : oracleDecimal.ToInt32(); // Asegúrate de que no sea nulo antes de convertir
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                return 0;
            }
            finally
            {
                Conexion.cerrarConexion();
            }
        }

        public decimal PesoPromedioClientesEntrenador(int idEntrenador)
        {
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.PesoPromedioClientesEntrenador", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada y salida
                        cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = idEntrenador;
                        var pesoPromedio = new OracleParameter("p_peso_promedio", OracleDbType.Decimal)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(pesoPromedio);

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        // Accede al valor de OracleDecimal correctamente
                        if (pesoPromedio.Value != DBNull.Value)
                        {
                            OracleDecimal oracleDecimal = (OracleDecimal)pesoPromedio.Value;
                            return oracleDecimal.IsNull ? 0 : oracleDecimal.ToInt32(); // Asegúrate de que no sea nulo antes de convertir
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                return 0;
            }
            finally
            {
                Conexion.cerrarConexion();
            }
        }

    }
}
