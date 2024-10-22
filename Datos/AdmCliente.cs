﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using NET_MVC.Models;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;

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
                    using (OracleCommand cmd = new OracleCommand("insertar_cliente", conexionBD))
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Castear
                        int id = int.Parse(cliente.Identificacion);
                        int idSede = int.Parse(cliente.IdSede);

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = cliente.Identificacion;
                        cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = null;
                        cmd.Parameters.Add("p_fecha_nacimiento", OracleDbType.Date).Value = cliente.FechaNacimiento;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                    }

                    using (OracleCommand cmdMembresia = new OracleCommand("insertar_membresia", conexionBD))
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

        
        public List<ClienteModel> ListarClientes(string sql)
        {
            var clientes = new List<ClienteModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(sql, conexionBD))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();
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

        // Función para verificar la existencia de un cliente
        public bool ClienteExiste(string identificacion)
        {
            bool existe = false;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(
                        "SELECT COUNT(*) FROM Cliente WHERE id_cliente = :p_id_cliente", conexionBD))
                    {
                        cmd.Parameters.Add(":p_id_cliente", OracleDbType.Varchar2).Value = identificacion;

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

        public ClienteModel ObtenerClientePorIdentificacion(string identificacion)
        {
            ClienteModel clienteEncontrado = null;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(
                      "SELECT c.id_cliente, p.nombre_persona AS nombre, p.telefono_persona AS telefono, p.genero_persona AS genero, m.tipo, " +
                      "calcular_dias_restantes(c.id_cliente) AS dias_restantes " +
                      "FROM Cliente c " +
                      "JOIN Persona p ON c.id_cliente = p.id_persona " +
                      "JOIN Membresia m ON c.id_cliente = m.id_cliente " +
                      "WHERE c.id_cliente = :p_id_cliente", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.Add(":p_id_cliente", OracleDbType.Int32).Value = int.Parse(identificacion);

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clienteEncontrado = new ClienteModel
                                {
                                    Identificacion = reader["id_cliente"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Telefono = reader["telefono"].ToString(),
                                    Genero = reader["genero"].ToString(),
                                    refMembresia = reader["tipo"].ToString(),
                                    DiasRestantes = Convert.ToInt32(reader["dias_restantes"])
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

            return clienteEncontrado;
        }

    }
}