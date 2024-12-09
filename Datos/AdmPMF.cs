using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Collections.Generic;
using System.Data;

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
                    using (OracleCommand cmd = new OracleCommand("pkg_Inserciones.crearPMF", conexionBD))
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
                    using (OracleCommand cmd = new OracleCommand("pkg_Inserciones.crearEjercicio", conexionBD))
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

        public bool ModificarEjercicio(EjercicioModel ejercicio)
        {
            bool rpta = false;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.modificarGeneralEjercicio", conexionBD))
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_ejercicio", OracleDbType.Int32).Value = ejercicio.IdEjercicio;
                        cmd.Parameters.Add("p_numero_repeticiones", OracleDbType.Int32).Value = ejercicio.Repeticiones;
                        cmd.Parameters.Add("p_numero_series", OracleDbType.Int32).Value = ejercicio.Series;
                        cmd.Parameters.Add("p_dia_semana", OracleDbType.Varchar2).Value = ejercicio.Dia;
                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                    }
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.modificarNombreEjercicio", conexionBD))
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_ejercicio", OracleDbType.Int32).Value = ejercicio.IdEjercicio;
                        cmd.Parameters.Add("p_id_nombre_ejercicio", OracleDbType.Int32).Value = ejercicio.Nombre;
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

        public bool EliminarEjercicio(EjercicioModel ejercicio)
        {
            bool rpta = false;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.eliminarEjercicio", conexionBD))
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_ejercicio", OracleDbType.Int32).Value = ejercicio.IdEjercicio;
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
            List<EjercicioModel> ejercicios = new List<EjercicioModel>();

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.ListarEjercicios", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("p_idCliente", OracleDbType.Int32).Value = idCliente;
                        cmd.Parameters.Add("p_fechaValoracion", OracleDbType.Date).Value = FechaValoracion;

                        // Parámetro de salida
                        OracleParameter resultadoParam = new OracleParameter("p_resultado", OracleDbType.RefCursor);
                        resultadoParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(resultadoParam);

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();

                        // Leer los resultados del cursor
                        using (OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["p_resultado"].Value).GetDataReader())
                        {
                            while (reader.Read())
                            {
                                EjercicioModel objejercicio = new EjercicioModel
                                {
                                    IdEjercicio = reader["id"].ToString(),
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

            return ejercicios;
        }

        public bool PmfExistente(PMFModel pmf)
        {
            bool existe = false;

            try
            {
                // Convertir la fecha
                DateTime fecha = Convert.ToDateTime(pmf.FechaValoracion);

                if (Conexion.abrirConexion())
                {
                    // Llamar al procedimiento almacenado
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.PmfExistente", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("p_idCliente", OracleDbType.Int32).Value = pmf.IdCliente;
                        cmd.Parameters.Add("p_fechaValoracion", OracleDbType.Date).Value = fecha;

                        // Parámetro de salida
                        OracleParameter existeParam = new OracleParameter("p_existe", OracleDbType.Int32);
                        existeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(existeParam);

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        // Obtener el valor de la salida y convertirlo
                        var oracleDecimal = (Oracle.ManagedDataAccess.Types.OracleDecimal)existeParam.Value;
                        existe = oracleDecimal.ToInt32() > 0; // Convertir y verificar si es mayor que 0
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
                Conexion.cerrarConexion(); // Cerrar la conexión
            }

            return existe;
        }

        public bool EjercicioExistente(EjercicioModel ejercicio)
        {
            bool existe = false;

            try
            {
                // Convertir la fecha
                DateTime fecha = Convert.ToDateTime(ejercicio.FechaValoracion);

                if (Conexion.abrirConexion())
                {
                    // Llamar al procedimiento almacenado
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.SP_EJERCICIO_EXISTENTE", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("p_idEjercicio", OracleDbType.Int32).Value = Convert.ToInt32(ejercicio.IdEjercicio);
                        cmd.Parameters.Add("p_idCliente", OracleDbType.Int32).Value = Convert.ToInt32(ejercicio.IdCliente);
                        cmd.Parameters.Add("p_fechaValoracion", OracleDbType.Date).Value = fecha;

                        // Parámetro de salida
                        OracleParameter existeParam = new OracleParameter("p_existe", OracleDbType.Decimal);
                        existeParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(existeParam);

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida y convertirlo a int
                        var result = (Oracle.ManagedDataAccess.Types.OracleDecimal)existeParam.Value;
                        existe = result.Value > 0;
                    }
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle: " + oex.Message);
            }
            catch (Exception ex)
            {
                return existe;
            }
            finally
            {
                Conexion.cerrarConexion(); // Cerrar la conexión
            }

            return existe;
        }

        public EjercicioModel ObtenerDatosEjercicio(string IdEjercicio)
        {
            EjercicioModel objejercicio = null;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.SP_OBTENER_DATOS_EJERCICIO", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada
                        cmd.Parameters.Add("p_idEjercicio", OracleDbType.Varchar2).Value = IdEjercicio;

                        // Parámetros de salida
                        cmd.Parameters.Add("p_idCliente", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_fecha", OracleDbType.Date).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2, 100).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_repeticiones", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_series", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("p_dia", OracleDbType.Varchar2, 20).Direction = ParameterDirection.Output;

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        // Asignar los valores devueltos al modelo
                        objejercicio = new EjercicioModel
                        {
                            IdEjercicio = IdEjercicio,
                            IdCliente = ((Oracle.ManagedDataAccess.Types.OracleDecimal)cmd.Parameters["p_idCliente"].Value).ToInt32(),
                            FechaValoracion = ((Oracle.ManagedDataAccess.Types.OracleDate)cmd.Parameters["p_fecha"].Value).Value,
                            NombreString = cmd.Parameters["p_nombre"].Value.ToString(),
                            Repeticiones = ((Oracle.ManagedDataAccess.Types.OracleDecimal)cmd.Parameters["p_repeticiones"].Value).ToInt32(),
                            Series = ((Oracle.ManagedDataAccess.Types.OracleDecimal)cmd.Parameters["p_series"].Value).ToInt32(),
                            Dia = cmd.Parameters["p_dia"].Value.ToString()
                        };

                        // Opciones adicionales
                        objejercicio.Opciones = ObtenerOpciones();
                        objejercicio.Nombre = objejercicio.Opciones
                            .Where(p => p.NameEjercicio == objejercicio.NombreString)
                            .Select(p => p.IdEjercicio)
                            .FirstOrDefault();
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
                Conexion.cerrarConexion(); // Cerrar la conexión
            }

            return objejercicio ?? throw new Exception("Ejercicio no encontrado.");
        }

        public DateTime ObtenerDateUltimoPMD(string idCliente)
        {
            bool isIdValid = int.TryParse(idCliente, out int newIdCliente);
            if (!isIdValid)
            {
                throw new Exception("El id del cliente no es valido");
            }
            DateTime result = default;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_procedimientos.consultarDateUltimoPMF", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //parametro de entrada
                        cmd.Parameters.Add("p_id_cliente",OracleDbType.Int32).Value = newIdCliente;
                        //parametro de salida 
                        cmd.Parameters.Add("p_date", OracleDbType.Date).Direction = ParameterDirection.Output;
                        //ejecucion
                        cmd.ExecuteNonQuery();
                        result = ((Oracle.ManagedDataAccess.Types.OracleDate)cmd.Parameters["p_date"].Value).Value;
                    }
                }
            }
            catch (OracleException oex)
            {
                if (oex.Number != 20006)
                {
                    throw new Exception("Error en Oracle: " + oex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error general: " + ex.Message);
            }
            finally
            {
                Conexion.cerrarConexion(); // Cerrar la conexión
            }
            return result;
        }
    }
}
