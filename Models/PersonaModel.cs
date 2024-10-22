
namespace NET_MVC.Models
{
    public class PersonaModel
    {
        #region Atributos
        public int IdSede { get; set; }
        public string Nombre { get; set; }
        public int Identificacion { get; set; }
        public string Telefono { get; set; }
        public string Genero { get; set; }
        #endregion

        #region Métodos
        public virtual Dictionary<string, object> obtenerParametros()
        {
            var Diccionario = new Dictionary<string, object>();
            Diccionario.Add("p_id_sede", IdSede);
            Diccionario.Add("p_nombre_persona", Nombre);
            Diccionario.Add("p_id_persona", Identificacion);
            Diccionario.Add("p_telefono_persona", Telefono);
            Diccionario.Add("p_genero_persona", Genero);

            return Diccionario;
        } 
        #endregion
    }
}
