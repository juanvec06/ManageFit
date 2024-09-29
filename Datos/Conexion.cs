
using Oracle.ManagedDataAccess.Client;

namespace NET_MVC.Datos
{
    public class Conexion
    {
            // Cadena de conexión adaptada con tus datos
            private static string ruta = "User Id=Prueba1;Password=oracle;Data Source=localhost;Persist Security Info=True;Unicode=True;";
            private static OracleConnection conexion = new OracleConnection(ruta);

            public static bool abrirConexion()
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

            public static bool cerrarConexion()
            {
                try
                {
                    conexion.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    // Puedes registrar el error aquí si lo deseas
                    Console.WriteLine($"Error al cerrar la conexión: {ex.Message}");
                    return false;
                }
            }

            public static OracleConnection GetCadenaOracle()
            {
                return conexion;
            }
        }
    }
