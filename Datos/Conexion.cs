
using Oracle.ManagedDataAccess.Client;

namespace NET_MVC.Datos
{
    public class Conexion
    {
            public OracleConnection conexion = new OracleConnection("User Id=C##BdManageFit;Password=oracle123;Data Source=localhost:1521/orcl");

            public bool abrirConexion()
            {
                try
                {
                    conexion.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    // Puedes registrar el error aquí si lo deseas
                    Console.WriteLine($"Error al abrir la conexión: {ex.Message}");
                    return false;
                }
            }

            public bool cerrarConexion()
            {
                try
                {
                    conexion.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            public OracleConnection GetConnection() => conexion;
            
        }
    }
