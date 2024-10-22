namespace NET_MVC.ConsultasSQL
{
    public class EntrenadorConsulta
    {
        public string consultaEntrenadoresDisponibles(string IdSede)
        {
            return "SELECT e.id_entrenador, " +
            "MAX(p.nombre_persona) AS nombre_entrenador, " +
            "MAX(ae.nombre_ae) AS area_especialidad, " +
            "COUNT(c.id_cliente) AS num_clientes " +
            "FROM ENTRENADOR e " +
            "INNER JOIN PERSONA p ON e.id_entrenador = p.id_persona " +
            "INNER JOIN AREAESPECIALIDAD ae ON e.id_ae = ae.id_ae " +
            "LEFT JOIN CLIENTE c ON c.id_entrenador = e.id_entrenador " +
            "WHERE p.id_sede = " + IdSede +
            "GROUP BY e.id_entrenador " +
            "HAVING COUNT(c.id_cliente) < 5";
        }
    }
}
