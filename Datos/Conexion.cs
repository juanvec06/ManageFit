
using Oracle.ManagedDataAccess.Client;

namespace NET_MVC.Datos
{
    public class Conexion
    {
            // Cadena de conexión adaptada con tus datos
            private static string ruta = "User Id=prueba1;Password=oracle;Data Source=localhost:1521/xe";
            private static OracleConnection conexion = new OracleConnection(ruta);

            public static bool abrirConexion()
            {
                try
                {
                    if (conexion.State == System.Data.ConnectionState.Open)
                    {
                        conexion.Close();
                    }

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

            public static bool cerrarConexion()
            {
                try
                {
                    if (conexion.State == System.Data.ConnectionState.Open)
                    {
                        conexion.Close();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    // Puedes registrar el error aquí si lo deseas
                    Console.WriteLine($"Error al cerrar la conexión: {ex.Message}");
                    return false;
                }
            }

            public static OracleConnection GetConnection()
            {
                return conexion;
            }
        }
    }
