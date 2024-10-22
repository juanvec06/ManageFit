using NET_MVC.Datos;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace NET_MVC.Controllers
{
    public class DataBaseController
    {
        #region Dependencias
        public Conexion _Conexion = new Conexion();
        #endregion

        #region Cruds Parametrizados
        public (bool success, string mensaje) InsertarObjeto(string prmProcedimiento, Dictionary<string, object> prmDiccionario)
        {
            try
            {
                if (_Conexion.abrirConexion())
                {
                    using (OracleCommand command = new OracleCommand(prmProcedimiento, _Conexion.GetConnection()))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var param in prmDiccionario)
                        {
                            Console.WriteLine($"Parámetro: {param.Key}, Valor: {param.Value}, Tipo: {param.Value.GetType()}");
                            command.Parameters.Add(new OracleParameter(param.Key, param.Value ?? DBNull.Value));
                        }
                        command.ExecuteNonQuery();
                    }

                    return (true, null);
                }
                else
                {
                    return (false, "No se pudo abrir la conexión a base de datos");
                }
            }
            catch (OracleException ex)
            {
                return (false, "Error inesperado de Oracle " + ex.Message);
            }
            catch (Exception ex)
            {
                return (false, "Error al ejecutar el procedimiento " + ex.Message);
            }
            finally
            {
                _Conexion.cerrarConexion();
            }
        }
        public DataTable EjecutarConsulta(string consulta)
        {
            DataTable dataTable = new DataTable();

            try
            {
                if (_Conexion.abrirConexion())
                {
                    using (OracleCommand command = new OracleCommand(consulta, _Conexion.GetConnection()))
                    {
                        // Ejecutar la consulta y llenar el DataTable
                        using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No se pudo abrir la conexión.");
                }
            }
            catch (OracleException ex)
            {
                Console.WriteLine($"OracleException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al ejecutar la consulta: {ex.Message}");
            }
            finally
            {
                _Conexion.cerrarConexion();
            }

            return dataTable;
        }
        public List<T> ListarObjetos<T>(string sql, Func<OracleDataReader, T> map)
        {
            var objetos = new List<T>();

            try
            {
                if (_Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(sql, _Conexion.GetConnection()))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            T objeto = map(reader);
                            objetos.Add(objeto);
                        }
                    }
                }
                return objetos;
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
                _Conexion.cerrarConexion();
            }
        }
        public bool ExisteTupla(string prmTabla, string prmAtributo, string prmValor)
        {
            try
            {
                if (_Conexion.abrirConexion())
                {
                    string consultaSql = $"SELECT 1 FROM {prmTabla} WHERE {prmAtributo} = :valor";

                    using (OracleCommand cmd = new OracleCommand(consultaSql, _Conexion.GetConnection()))
                    {
                        cmd.Parameters.Add(new OracleParameter("valor", prmValor));
                        object resultado = cmd.ExecuteScalar();
                        return resultado != null;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (OracleException oex)
            {
                Console.WriteLine($"Error de Oracle: {oex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                return false;
            }
            finally
            {
                _Conexion.cerrarConexion();
            }
        }
        #endregion
    }
}
