

namespace NET_MVC.Models
{
    public class ClienteModel : PersonaModel
    {
        #region Atributos
        public int? IdEntrenador { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaSuscripcion { get; set; }
        public string refMembresia { get; set; }


        public int DiasRestantes { get; set; }
        #endregion

        #region Métodos
        public override Dictionary<string, object> obtenerParametros()
        {
            var Diccionario = base.obtenerParametros(); // Obtener parámetros de PersonaModel

            Diccionario.Add("p_id_entrenador", IdEntrenador.HasValue ? (object)IdEntrenador.Value : DBNull.Value);
            Diccionario.Add("p_fecha_nacimiento", FechaNacimiento);
            Diccionario.Add("p_fecha_suscripcion", FechaSuscripcion = DateTime.Now);
            Diccionario.Add("p_tipo_membresia", refMembresia);

            return Diccionario;
        } 
        #endregion
    }
}
