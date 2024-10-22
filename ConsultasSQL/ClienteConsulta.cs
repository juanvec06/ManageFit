namespace NET_MVC.ConsultasSQL
{
    public class ClienteConsulta
    {
        public string consultaTodosFiltro(string IdSede)
        {
            return "SELECT c.id_cliente, MAX(p.nombre_persona) AS nombre, MAX(p.telefono_persona) AS telefono, " +
                         "MAX(m.tipo) AS membresia, TO_CHAR(MAX(m.fecha_suscripcion), 'DD-MM-YYYY') AS fecha " +
                         "FROM cliente c INNER JOIN persona p ON c.id_cliente = p.id_persona " +
                         "INNER JOIN membresia m ON c.id_cliente = m.id_cliente " +
                         "WHERE p.id_sede = " + IdSede + " GROUP BY c.id_cliente";
        }
        public string consultaMembresiaFiltro(int opcion, string IdSede)
        {
            string nomMembresia;
            if (opcion == 1) nomMembresia = "Premium";
            else nomMembresia = "General";

            string consultaSql = "SELECT c.id_cliente, MAX(p.nombre_persona) AS nombre, MAX(p.telefono_persona) AS telefono,MAX(m.tipo) AS membresia, TO_CHAR(MAX(m.fecha_suscripcion), 'DD-MM-YYYY') AS fecha " +
                                "FROM cliente c INNER JOIN persona p ON c.id_cliente = p.id_persona " +
                                "INNER JOIN membresia m ON c.id_cliente = m.id_cliente " +
                                "WHERE p.id_sede = " + IdSede + " AND m.tipo = '" + nomMembresia + "' " +
                                "GROUP BY c.id_cliente";

            return consultaSql;
        }
        public string consultaGeneroFiltro(int opcion, string IdSede)
        {
            string genero;
            if (opcion == 1) genero = "M";
            else if (opcion == 2) genero = "F";
            else genero = "NE";

            string consultaSql = "SELECT c.id_cliente, MAX(p.nombre_persona) AS nombre, MAX(p.telefono_persona) AS telefono,MAX(m.tipo) AS membresia, TO_CHAR(MAX(m.fecha_suscripcion), 'DD-MM-YYYY') AS fecha " +
                                "FROM cliente c INNER JOIN persona p ON c.id_cliente = p.id_persona " +
                                "INNER JOIN membresia m ON c.id_cliente = m.id_cliente " +
                                "WHERE p.id_sede = " + IdSede + " AND p.genero_persona = '" + genero + "' " +
                                "GROUP BY c.id_cliente";
            return consultaSql;
        }
        public string consultarInformacion()
        {
            return "SELECT c.id_cliente, p.nombre_persona AS nombre, p.telefono_persona AS telefono, p.genero_persona AS genero, m.tipo, " +
                  "calcular_dias_restantes(c.id_cliente) AS dias_restantes " +
                  "FROM Cliente c " +
                  "JOIN Persona p ON c.id_cliente = p.id_persona " +
                  "JOIN Membresia m ON c.id_cliente = m.id_cliente " +
                  "WHERE c.id_cliente = :p_id_cliente";
        }
        public string consultarClientePorId(string prmIdentificacion)
        {
            return "SELECT c.id_cliente, p.nombre_persona AS nombre, p.telefono_persona AS telefono, p.genero_persona AS genero, m.tipo, " +
                              "calcular_dias_restantes(c.id_cliente) AS dias_restantes " +
                              "FROM Cliente c " +
                              "JOIN Persona p ON c.id_cliente = p.id_persona " +
                              "JOIN Membresia m ON c.id_cliente = m.id_cliente " +
                              $"WHERE c.id_cliente = {prmIdentificacion}";
        }
    }
}
