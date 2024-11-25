using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NET_MVC.Models;
using NuGet.Protocol.Plugins;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                if (oex.Number == 20008)
                {
                    throw new Exception("Error: es menor de 18 años");
                }
                else
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
                Conexion.cerrarConexion(); //Cerrar la conexión
            }
            return rpta;
        }
        /// <summary>
        /// se encarga de insertar un nuevo cliente haciendo uso de un trigger instead of que esta en la base de datos
        /// </summary>
        /// <param name="cliente">El modelo del cliente que se quiere insertar, aqui estan los datos del cliente</param>
        /// <returns>true si la operacion de insercion se hizo correctamente, false si no se realizó correctamente</returns>
        public bool RegistrarClienteInsteadOf(ClienteModel cliente)
        {
            if(cliente == null)
            {
                return false;
            }
            try
            {
                if (Conexion.abrirConexion())
                {
                    string query = @"
                                    INSERT INTO view_clientes
                                    VALUES (:id_cliente, :nombre, :telefono, TO_DATE(:fecha_nacimiento, 'DD-MM-YYYY'), :genero, :membresia, :id_entrenador, :idSede)";
                    using (OracleCommand command = new OracleCommand(query, conexionBD))
                    {
                        //Ejecutamos la inserción
                        command.Parameters.Add(":id_cliente", OracleDbType.Int64).Value = int.Parse(cliente.Identificacion);
                        command.Parameters.Add(":nombre", OracleDbType.Varchar2).Value = cliente.Nombre;
                        command.Parameters.Add(":telefono", OracleDbType.Int64).Value = long.Parse(cliente.Telefono);
                        command.Parameters.Add(":fecha_nacimiento", OracleDbType.Date).Value = cliente.FechaNacimiento;
                        command.Parameters.Add(":genero", OracleDbType.Varchar2).Value = cliente.Genero;
                        command.Parameters.Add(":membresia", OracleDbType.Varchar2).Value = cliente.refMembresia;
                        if(cliente.IdEntrenador != 0 && int.TryParse(cliente.IdEntrenador.ToString(),out int result))
                        {
                            command.Parameters.Add(":id_entrenador", OracleDbType.Int32).Value = result;
                        }
                        else
                        {
                            command.Parameters.Add(":id_entrenador", OracleDbType.Int32).Value = DBNull.Value ;
                        }
                        command.Parameters.Add(":idSede", OracleDbType.Int32).Value = cliente.IdSede;
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error al abrir la conexion o hacer insercion del nuevo cliente: " + oex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }
            return true;
        }
        public bool ActualizarMembresiaCliente(string identificacion, string nuevaMembresia)
        {
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("SP_ACTUALIZAR_MEMBRESIA", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Varchar2).Value = identificacion;
                        cmd.Parameters.Add("p_nueva_membresia", OracleDbType.Varchar2).Value = nuevaMembresia;

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error al actualizar la membresía: " + oex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }
            return false;
        }


        public List<ClienteModel> ListarClientes(string filter, string IdSede)
        {
            var clientes = new List<ClienteModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("LISTAR_CLIENTES", conexionBD))
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
        public List<ClienteModel> ListarClientesVista(string idSede)
        {
            //si es nulo, no se mostrara nada
            if (idSede == null)
            {
                return new List<ClienteModel> { };
            }
            List<ClienteModel> ListaClientesVista = new List<ClienteModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("LISTAR_CLIENTES_VISTA", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro de salida (cursor)
                        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("P_IDSEDE", OracleDbType.Int32).Value = idSede;
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if(reader["id_entrenador"] != DBNull.Value && int.TryParse(reader["id_entrenador"].ToString(), out int idEntrenador))
                                {
                                    ClienteModel objcliente = new ClienteModel
                                    {
                                        Identificacion = reader["id_cliente"].ToString(),
                                        Nombre = reader["nombre"].ToString(),
                                        Telefono = reader["telefono"].ToString(),
                                        FechaNacimiento = DateTime.Parse(reader["fecha_nacimiento"].ToString()),
                                        Genero = reader["genero"].ToString(),
                                        refMembresia = reader["membresia"].ToString(),
                                        IdEntrenador = idEntrenador,
                                    };
                                    ListaClientesVista.Add(objcliente);
                                }
                                else
                                {
                                    ClienteModel objcliente = new ClienteModel
                                    {
                                        Identificacion = reader["id_cliente"].ToString(),
                                        Nombre = reader["nombre"].ToString(),
                                        Telefono = reader["telefono"].ToString(),
                                        FechaNacimiento = DateTime.Parse(reader["fecha_nacimiento"].ToString()),
                                        Genero = reader["genero"].ToString(),
                                        refMembresia = reader["membresia"].ToString(),
                                    };
                                    ListaClientesVista.Add(objcliente);
                                }
                            }
                        }
                    }
                }
                return ListaClientesVista;
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle: " + oex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general: " + ex.Message);
            }
        }
        public List<ClienteModel> ListarClientesAsignados(string idEntrenador)
        {
            var clientes = new List<ClienteModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("LISTAR_CLIENTES_ASIGNADOS", conexionBD))
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
                    using (OracleCommand cmd = new OracleCommand("CLIENTE_EXISTE", conexionBD))
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
                    using (OracleCommand cmd = new OracleCommand("sp_obtener_cliente", conexionBD))
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
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = conexionBD;
                        cmd.CommandType = System.Data.CommandType.Text;

                        cmd.CommandText = "SELECT pkg_Estadisticas_Poblacion.fn_Total_Clientes_Sede(:p_id_sede) FROM DUAL";
                        // Parámetro de entrada
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = idSede;

                        // Ejecutar
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            totalClientes = int.Parse(result.ToString());
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

        public bool VerificarClienteEnSede(int idCliente, int idSede)
        {
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("VerificarClienteEnSede", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros de entrada
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = idCliente;
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = idSede;

                        // Parámetro de salida
                        var existeParam = new OracleParameter("p_existe", OracleDbType.Decimal)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(existeParam);

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        // Convertir el valor del parámetro de salida a un entero
                        if (existeParam.Value != DBNull.Value)
                        {
                            var result = ((Oracle.ManagedDataAccess.Types.OracleDecimal)existeParam.Value).ToInt32();
                            return result == 1;
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception($"Error al verificar cliente en sede: {ex.Message}");
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return false;
        }



        public int ObtenerTotalClientesPorTipo(int idSede, string tipoMembresia)
        {
            int total = 0;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = conexionBD;
                        cmd.CommandType = System.Data.CommandType.Text;

                        cmd.CommandText = "SELECT pkg_Estadisticas_Poblacion.fn_Total_Clientes_Tipo_Sede(:p_id_sede, :p_tipo_membresia) FROM DUAL";
                        // Parámetro de entrada
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = idSede;
                        cmd.Parameters.Add("p_tipo_membresia", OracleDbType.Varchar2).Value = tipoMembresia;


                        // Ejecutar
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            total = int.Parse(result.ToString());
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

        public string VerificarMensajeTrigger(string idCliente)
        {
            string mensaje = string.Empty;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("SP_VERIFICAR_MENSAJE_TRIGGER", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada: ID del cliente
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = idCliente;

                        // Parámetro de salida para el mensaje
                        cmd.Parameters.Add("p_mensaje", OracleDbType.Varchar2, 500).Direction = ParameterDirection.Output;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();

                        // Obtener el mensaje de salida
                        if (cmd.Parameters["p_mensaje"].Value != DBNull.Value)
                        {
                            mensaje = cmd.Parameters["p_mensaje"].Value.ToString();
                        }
                    }
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle al verificar mensaje del trigger: " + oex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return mensaje;
        }

        public void TransferirClienteSede(string idCliente, string idSedeDestino, string sedeActual)
        {
            try
            {
                // Validar que los valores puedan convertirse a números
                if (!int.TryParse(idCliente, out int idClienteInt))
                {
                    throw new ArgumentException("El ID del cliente no es un número válido.");
                }
                if (!int.TryParse(idSedeDestino, out int idSedeDestinoInt))
                {
                    throw new ArgumentException("El ID de la sede destino no es un número válido.");
                }

                if (!int.TryParse(sedeActual, out int sedeActualInt))
                {
                    throw new ArgumentException("El ID de la sede destino no es un número válido.");
                }

                // Abrir conexión y ejecutar procedimiento
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("GestionSedes.TransferirCliente", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetros del procedimiento
                        cmd.Parameters.Add("v_id_sede_origen", OracleDbType.Int32).Value = sedeActualInt; // Sede origen
                        cmd.Parameters.Add("v_id_sede_destino", OracleDbType.Int32).Value = idSedeDestinoInt; // Sede destino
                        cmd.Parameters.Add("v_id_cliente", OracleDbType.Int32).Value = idClienteInt; // ID del cliente

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception($"Error en Oracle al transferir cliente: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al procesar la transferencia: {ex.Message}");
            }
            finally
            {
                Conexion.cerrarConexion();
            }
        }

        public List<SedeModel> ObtenerSedesDisponibles(string idSedeActual)
        {
            List<SedeModel> sedes = new List<SedeModel>();

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("ObtenerSedesDisponibles", conexionBD))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parámetro de entrada para la sede actual
                        cmd.Parameters.Add("p_sede_actual", OracleDbType.Int32).Value = idSedeActual;

                        // Parámetro de salida para el cursor
                        var cursorParam = new OracleParameter("p_cursor", OracleDbType.RefCursor)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(cursorParam);

                        // Ejecutar el procedimiento
                        cmd.ExecuteNonQuery();

                        // Leer el cursor
                        using (OracleDataReader reader = ((OracleRefCursor)cursorParam.Value).GetDataReader())
                        {
                            while (reader.Read())
                            {
                                sedes.Add(new SedeModel
                                {
                                    IdSede = reader.GetInt32(0),
                                    Nombre = reader.GetString(1)
                                });
                            }
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception($"Error al obtener sedes disponibles: {ex.Message}");
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return sedes;
        }



    }

}
