

namespace NET_MVC.Models
{
    public class EntrenadorModel : PersonaModel
    {
        #region Atributos
        public int IdAreaEspecialidad { get; set; }
        public string Contraseña { get; set; }
        public int Salario { get; set; }
        public DateTime FechaInicioContrato { get; set; }
        public DateTime FechaFinContrato { get; set; }

        public int NumeroClientes { get; set; }
        public string Especialidad { get; set; }
        #endregion

        #region Métodos
        public override Dictionary<string, object> obtenerParametros()
        {
            var Diccionario = base.obtenerParametros();
            Diccionario.Add("p_id_ae", IdAreaEspecialidad);
            Diccionario.Add("p_password_entrenador", Contraseña);
            Diccionario.Add("p_salario", Salario);
            Diccionario.Add("p_fecha_inicio_contrato", FechaInicioContrato);
            Diccionario.Add("p_fecha_fin_contrato", FechaFinContrato == default(DateTime) ? (object)DBNull.Value : FechaFinContrato); // Manejo de valores nulos

            return Diccionario;
        }

        public int obtenerIdEspecialidad(string prmEspecialidad)
        {
            switch (prmEspecialidad)  // Convertir a minúsculas para evitar problemas de mayúsculas
            {
                case "Fuerza":
                    return 1;

                case "Reducción de peso":
                    return 2;

                case "Crossfit":
                    return 3;

                case "Culturismo":
                    return 4;

                default:
                    return 0;  // Devuelve 0 si no se encuentra la especialidad
            }
        } 
        #endregion
    }
}
