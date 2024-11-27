using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NET_MVC.Models;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace NET_MVC.Datos
{
    public class AdmCliente
    {
        public OracleConnection conexionBD = Conexion.GetConnection();

        public bool RegistrarCliente(ClienteModel cliente)
        {
            bool rpta = false; // Asegurarse de inicializar rpta
            try
            {
                if (Conexion.abrirConexion())
                {
                    // llamada a procedimiento almacenado en base de datos 
                    using (OracleCommand cmd = new OracleCommand("pkg_Inserciones.insertar_cliente", conexionBD))
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Castear
                        int id = int.Parse(cliente.Identificacion);
                        int idSede = int.Parse(cliente.IdSede);

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = cliente.Identificacion;
                        if(cliente.IdEntrenador == 0 || cliente.IdEntrenador == null){
                            cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = null;
                        }
                        else{
                            cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = cliente.IdEntrenador;
                        }
                        cmd.Parameters.Add("p_fecha_nacimiento", OracleDbType.Date).Value = cliente.FechaNacimiento;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                    }

                    using (OracleCommand cmdMembresia = new OracleCommand("pkg_Inserciones.insertar_membresia", conexionBD))
                    {
                        cmdMembresia.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdMembresia.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = cliente.Identificacion;
                        cmdMembresia.Parameters.Add("p_tipo", OracleDbType.Varchar2).Value = cliente.refMembresia;
                        cmdMembresia.Parameters.Add("p_fecha_suscripcion", OracleDbType.Date).Value = DateTime.Now;

                        cmdMembresia.ExecuteNonQuery();

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

        public List<ClienteModel> ListarClientes(string filter, string IdSede)
        {
            var clientes = new List<ClienteModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.LISTAR_CLIENTES", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("P_FILTER", OracleDbType.Varchar2).Value = filter;
                        cmd.Parameters.Add("P_ID_SEDE", OracleDbType.Int32).Value = Convert.ToInt32(IdSede);

                        // Parámetro de salida (cursor)
                        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClienteModel objcliente = new ClienteModel
                                {
                                    Identificacion = reader["id_cliente"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Telefono = reader["telefono"].ToString(),
                                    refMembresia = reader["membresia"].ToString(),
                                    fechaMembresia = reader["fecha"].ToString(),
                                };
                                clientes.Add(objcliente);
                            }
                        }
                    }
                }
                return clientes;
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

        public List<ClienteModel> ListarClientesAsignados(string idEntrenador)
        {
            var clientes = new List<ClienteModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.LISTAR_CLIENTES_ASIGNADOS", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada
                        cmd.Parameters.Add("P_ID_ENTRENADOR", OracleDbType.Int32).Value = Convert.ToInt32(idEntrenador);

                        // Parámetro de salida (cursor)
                        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClienteModel objcliente = new ClienteModel
                                {
                                    Identificacion = reader["id_cliente"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Telefono = reader["telefono"].ToString(),
                                    refMembresia = reader["membresia"].ToString(),
                                    DiasRestantes = Convert.ToInt32(reader["dias_restantes"])
                                };
                                clientes.Add(objcliente);
                            }
                        }
                    }
                }
                return clientes;
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
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool eliminarCliente(string identificacion)
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
                    using (OracleCommand cmd = new OracleCommand("DELETE FROM MEMBRESIA WHERE id_cliente = :id", conexionBD))
                    {
                        cmd.Parameters.Add(new OracleParameter("id", idPersona)); //eliminacion de membresia
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        existe = count > 0;
                    }
                    using (OracleCommand cmd = new OracleCommand("DELETE FROM CLIENTE WHERE id_cliente = :id", conexionBD))
                    {
                        cmd.Parameters.Add(new OracleParameter("id", idPersona)); //eliminacion de registro de cliente
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

        public bool ClienteExiste(string identificacion)
        {
            if (string.IsNullOrEmpty(identificacion))
            {
                throw new ArgumentException("La identificación no puede estar vacía.");
            }

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.CLIENTE_EXISTE", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada
                        cmd.Parameters.Add("P_ID_CLIENTE", OracleDbType.Varchar2).Value = identificacion;

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

        public ClienteModel ObtenerClientePorIdentificacion(string identificacion, string idSede)
        {
            ClienteModel clienteEncontrado = null;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.sp_obtener_cliente", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = int.Parse(identificacion);
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = int.Parse(idSede);

                        // Parámetros de salida
                        cmd.Parameters.Add("p_id", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add("p_nombre", OracleDbType.Varchar2, 100).Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add("p_telefono", OracleDbType.Varchar2, 20).Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add("p_genero", OracleDbType.Varchar2, 10).Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add("p_tipo", OracleDbType.Varchar2, 50).Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add("p_dias_restantes", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        clienteEncontrado = new ClienteModel
                        {
                            Identificacion = cmd.Parameters["p_id"].Value.ToString(),
                            Nombre = cmd.Parameters["p_nombre"].Value.ToString(),
                            Telefono = cmd.Parameters["p_telefono"].Value.ToString(),
                            Genero = cmd.Parameters["p_genero"].Value.ToString(),
                            refMembresia = cmd.Parameters["p_tipo"].Value.ToString(),
                            DiasRestantes = cmd.Parameters["p_dias_restantes"].Value != DBNull.Value
                                ? (cmd.Parameters["p_dias_restantes"].Value is OracleDecimal ?
                                    ((OracleDecimal)cmd.Parameters["p_dias_restantes"].Value).ToInt32() :
                                    Convert.ToInt32(cmd.Parameters["p_dias_restantes"].Value))
                                : 0 // Valor predeterminado si es nulo
                        };
                    }
                }
            }
            catch (OracleException oex)
            {
                return clienteEncontrado;
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return clienteEncontrado;
        }

        

        public int ObtenerTotalClientesPorSede(int idSede)
        {
            int totalClientes = 0;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.sp_obtener_total_clientes", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Parámetro de entrada
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = idSede;

                        // Parámetro de salida
                        cmd.Parameters.Add("p_total_clientes", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida
                        if (cmd.Parameters["p_total_clientes"].Value != DBNull.Value)
                        {
                            totalClientes = Convert.ToInt32(((OracleDecimal)cmd.Parameters["p_total_clientes"].Value).Value);
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

            return totalClientes;
        }

        public int ObtenerTotalClientesPorTipo(int idSede, string tipoMembresia)
        {
            int total = 0;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("pkg_Procedimientos.sp_obtener_total_clientes_por_tipo", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = idSede;
                        cmd.Parameters.Add("p_tipo_membresia", OracleDbType.Varchar2).Value = tipoMembresia;

                        // Parámetro de salida
                        cmd.Parameters.Add("p_total", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida
                        if (cmd.Parameters["p_total"].Value != DBNull.Value)
                        {
                            total = Convert.ToInt32(((OracleDecimal)cmd.Parameters["p_total"].Value).Value);
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

            return total;
        }

    }
}
