/* Eliminar todo
BEGIN
    FOR p IN (SELECT object_name FROM user_procedures WHERE object_type = 'PROCEDURE') LOOP
        EXECUTE IMMEDIATE 'DROP PROCEDURE ' || p.object_name;
    END LOOP;
END;

BEGIN
    FOR f IN (SELECT object_name FROM user_procedures WHERE object_type = 'FUNCTION') LOOP
        EXECUTE IMMEDIATE 'DROP FUNCTION ' || f.object_name;
    END LOOP;
END;

*/

--  Definicion secuencias antes de la insercion
CREATE SEQUENCE seq_idejercicio
START WITH 1
INCREMENT BY 1
MINVALUE 1
NOCYCLE;

-- Definicion trigger antes de la insercion
CREATE OR REPLACE TRIGGER trg_incrementar_id_ejercicio
BEFORE INSERT ON Ejercicio
FOR EACH ROW
BEGIN
    :NEW.id_ejercicio := seq_idejercicio.NEXTVAL;
END; 

CREATE OR REPLACE PACKAGE pkg_inicio_sesion AS
    -- Inicio de sesion adiminstrador
    PROCEDURE validar_logina(
        p_usuario IN NUMBER, 
        p_contraseña IN cuentasede.password_sede%TYPE, 
        p_count OUT NUMBER);
    -- Inicio de sesion entrenador
    PROCEDURE validar_loginE(
        p_usuario IN NUMBER, 
        p_contraseña IN cuentasede.password_sede%TYPE, 
        p_count OUT NUMBER);
END pkg_inicio_sesion;

CREATE OR REPLACE PACKAGE BODY pkg_inicio_sesion AS
    -- Inicio de sesion adiminstrador
    PROCEDURE validar_logina(
        p_usuario IN NUMBER, 
        p_contraseña IN cuentasede.password_sede%TYPE, 
        p_count OUT NUMBER)
    IS
        v_count number;
    BEGIN
        SELECT COUNT(*)
        INTO v_count
        FROM cuentasede
        WHERE id_sede = p_usuario AND password_sede = p_contraseña;
    
        IF v_count > 0 THEN
            p_count := 1;
        ELSE
            p_count := 0;
        END IF;
    END validar_logina;
    
    -- Inicio de sesion entrenador
    PROCEDURE validar_loginE(
        p_usuario IN NUMBER, 
        p_contraseña IN cuentasede.password_sede%TYPE, 
        p_count OUT NUMBER)
    IS
        v_count number;
    BEGIN
        SELECT COUNT(*)
        INTO v_count
        FROM cuentaentrenador
        WHERE id_entrenador = p_usuario AND password_entrenador = p_contraseña;
    
        IF v_count > 0 THEN
            p_count := 1;
        ELSE
            p_count := 0;
        END IF;
    END validar_logine;
    
END pkg_inicio_sesion;

CREATE OR REPLACE PACKAGE pkg_Inserciones AS
    -- Insertar Persona
    PROCEDURE insertar_persona(
        p_id_persona IN persona.id_persona%type,
        p_id_sede IN sede.id_sede%type,
        p_nombre IN persona.nombre_persona%type,
        p_genero IN persona.genero_persona%type,
        p_telefono IN persona.telefono_persona%type);
    
    -- Funcion complementaria insertar entrenador
    FUNCTION validar_AE(nombre_AESP IN areaespecialidad.nombre_ae%type)
    RETURN NUMBER;
    
    -- Insertar entrenador
    PROCEDURE insertar_Entrenador(
        p_id_entrenador IN entrenador.id_entrenador%type,
        p_nombreAE IN areaespecialidad.nombre_ae%type,
        p_salario IN contrato.salario%type,
        p_fechaContrato IN contrato.fecha_inicio_contrato%type,
        p_contraseña IN cuentaentrenador.password_entrenador%type);
    
    -- Insertar cliente
    PROCEDURE insertar_Cliente(
        p_id_cliente IN cliente.id_cliente%type,
        p_id_entrenador IN cliente.id_entrenador%type,
        p_fecha_nacimiento IN cliente.fecha_nacimiento%type);    
    
    -- Insertar membresia
    PROCEDURE insertar_membresia(
        p_id_cliente IN cliente.id_cliente%type,
        p_tipo IN membresia.tipo%type,
        p_fecha_suscripcion IN membresia.fecha_suscripcion%type);
    
    -- Insercion PMF
    PROCEDURE crearPMF(
        p_id_cliente IN pmf.id_Cliente%type, 
        p_fecha_valoracion IN pmf.fecha_valoracion%type);   
    
    -- Insertar ejercicio
    PROCEDURE crearEjercicio(     
        p_id_cliente IN ejercicio.id_cliente%type,                      
        p_id_fecha_valoracion IN ejercicio.id_fecha_valoracion%type,  
        p_id_nombre_ejercicio IN ejercicio.id_nombre_ejercicio%type,  
        p_numero_repeticiones IN ejercicio.numero_repeticiones%type,              
        p_numero_series IN ejercicio.numero_series%type,                    
        p_dia_semana IN ejercicio.dia_semana%type) ;     
    
END pkg_Inserciones;

CREATE OR REPLACE PACKAGE BODY pkg_Inserciones AS
    -- Insertar Persona
    PROCEDURE insertar_persona(
        p_id_persona IN persona.id_persona%type,
        p_id_sede IN sede.id_sede%type,
        p_nombre IN persona.nombre_persona%type,
        p_genero IN persona.genero_persona%type,
        p_telefono IN persona.telefono_persona%type)
    IS
    BEGIN
        INSERT INTO Persona VALUES (p_id_persona, p_nombre, p_telefono, p_genero, p_id_sede);
        COMMIT;
    EXCEPTION 
        WHEN DUP_VAL_ON_INDEX THEN
            RAISE_APPLICATION_ERROR(-20002,'Persona existente');
        WHEN NO_DATA_FOUND THEN
            RAISE_APPLICATION_ERROR(-20003,'Sede no existente');  
        WHEN OTHERS THEN
            RAISE_APPLICATION_ERROR(-20008, 'Ocurrió un error no identificado: ' || SQLERRM);
    END insertar_persona;
    
    -- Funcion complementaria insertar entrenador
    FUNCTION validar_AE(nombre_AESP IN areaespecialidad.nombre_ae%type)
    RETURN NUMBER
    IS
        v_id areaespecialidad.id_ae%type;
    BEGIN
        SELECT id_ae
        INTO v_id
        FROM areaespecialidad
        WHERE nombre_ae = nombre_AESP;
        RETURN v_id;
    EXCEPTION 
        WHEN NO_DATA_FOUND THEN
            RAISE_APPLICATION_ERROR(-20001,'AE no existente'); 
    END validar_AE;
        
    -- Insertar entrenador
    PROCEDURE insertar_Entrenador(
        p_id_entrenador IN entrenador.id_entrenador%type,
        p_nombreAE IN areaespecialidad.nombre_ae%type,
        p_salario IN contrato.salario%type,
        p_fechaContrato IN contrato.fecha_inicio_contrato%type,
        p_contraseña IN cuentaentrenador.password_entrenador%type)
    IS
        v_idAE areaespecialidad.id_ae%type :=  validar_AE(p_nombreAE);
    BEGIN
        INSERT INTO Entrenador VALUES (p_id_entrenador, v_idAE);
        INSERT INTO Contrato VALUES (p_id_entrenador, p_salario, p_fechaContrato,null);
        INSERT INTO CuentaEntrenador VALUES (p_id_entrenador,p_contraseña);
        COMMIT;
    EXCEPTION 
        WHEN OTHERS THEN
            RAISE_APPLICATION_ERROR(-20008, 'Ocurrió un error no identificado: ' || SQLERRM);
    END insertar_Entrenador;

    -- Insertar cliente
    PROCEDURE insertar_Cliente(
        p_id_cliente IN cliente.id_cliente%type,
        p_id_entrenador IN cliente.id_entrenador%type,
        p_fecha_nacimiento IN cliente.fecha_nacimiento%type)  
    IS
    BEGIN
        INSERT INTO Cliente VALUES (p_id_cliente, p_id_entrenador, p_fecha_nacimiento);
        COMMIT; 
    EXCEPTION
        WHEN OTHERS THEN
            RAISE_APPLICATION_ERROR(-20008, 'Ocurrió un error no identificado: ' || SQLERRM);
    END insertar_Cliente;
    
    -- Insertar membresia
    PROCEDURE insertar_membresia (
        p_id_cliente IN cliente.id_cliente%type,
        p_tipo IN membresia.tipo%type,
        p_fecha_suscripcion IN membresia.fecha_suscripcion%type)
    IS
    BEGIN
        INSERT INTO Membresia (id_Cliente, tipo, fecha_suscripcion) 
        VALUES (p_id_cliente, p_tipo, p_fecha_suscripcion);
        COMMIT;
    EXCEPTION
        WHEN OTHERS THEN
            RAISE_APPLICATION_ERROR(-20001, 'Error al registrar la membresía: ' || SQLERRM);
    END insertar_membresia;
    
    -- Insercion PMF
    PROCEDURE crearPMF(
        p_id_cliente IN pmf.id_Cliente%type, 
        p_fecha_valoracion IN pmf.fecha_valoracion%type)  
    IS
    BEGIN
        INSERT INTO pmf VALUES (p_id_cliente, p_fecha_valoracion);
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE = -1 THEN
                RAISE_APPLICATION_ERROR(-20010, 'PMF existente para el cliente con la fecha de valoracion diligenciada');
            ELSIF SQLCODE = -2291 THEN
                RAISE_APPLICATION_ERROR(-20011, 'Cliente no existente');
            ELSE
                RAISE_APPLICATION_ERROR(-20008, 'Error no identificado: ' || SQLERRM);
            END IF;
    END crearPMF;
    
    -- Insertar ejercicio
    PROCEDURE crearEjercicio(     
        p_id_cliente IN ejercicio.id_cliente%type,                      
        p_id_fecha_valoracion IN ejercicio.id_fecha_valoracion%type,  
        p_id_nombre_ejercicio IN ejercicio.id_nombre_ejercicio%type,  
        p_numero_repeticiones IN ejercicio.numero_repeticiones%type,              
        p_numero_series IN ejercicio.numero_series%type,                    
        p_dia_semana IN ejercicio.dia_semana%type)   
    IS
        id_ejercicio NUMBER;
    BEGIN
        INSERT INTO ejercicio VALUES (id_ejercicio,p_id_cliente,p_id_fecha_valoracion,p_id_nombre_ejercicio,p_numero_repeticiones,p_numero_series,p_dia_semana);
    EXCEPTION
        WHEN OTHERS THEN
            IF SQLCODE = -1 THEN
                RAISE_APPLICATION_ERROR(-20013, 'Ejercicio existente');
            ELSIF SQLCODE = -2291 THEN
                RAISE_APPLICATION_ERROR(-20014, 'Nombre del ejercicio no existente');
            ELSIF SQLCODE = -290 THEN
                RAISE_APPLICATION_ERROR(-20015, 'Dia de la semana no existente');
            ELSE
                RAISE_APPLICATION_ERROR(-20008, 'Error no identificado: ' || SQLERRM);
            END IF;
    END crearEjercicio;
END pkg_Inserciones;

CREATE OR REPLACE PACKAGE pkg_Procedimientos AS
    -- Funcion calcular dias restantes de membresia
    FUNCTION calcular_dias_restantes(p_id_cliente IN NUMBER)  
    RETURN NUMBER ;
    
    -- Listar clientes
    PROCEDURE LISTAR_CLIENTES(
        P_FILTER      IN  VARCHAR2,
        P_ID_SEDE     IN  NUMBER,
        P_CURSOR      OUT SYS_REFCURSOR);
    
    -- Listar clientes asignados con membresia vigente
    PROCEDURE LISTAR_CLIENTES_ASIGNADOS(
        P_ID_ENTRENADOR IN NUMBER,
        P_CURSOR OUT SYS_REFCURSOR);
        
    -- Validacion existencia cliente
    PROCEDURE CLIENTE_EXISTE(
        P_ID_CLIENTE IN VARCHAR2,
        P_EXISTE OUT NUMBER);
    
    -- Obtener cliente por id y sede
    PROCEDURE sp_obtener_cliente(
        p_id_cliente IN NUMBER,
        p_id_sede IN NUMBER,
        p_id OUT NUMBER,
        p_nombre OUT VARCHAR2,
        p_telefono OUT VARCHAR2,
        p_genero OUT VARCHAR2,
        p_tipo OUT VARCHAR2,
        p_dias_restantes OUT NUMBER);
    
    -- Obtener total de clientes por sede
    PROCEDURE sp_obtener_total_clientes(
        p_id_sede IN NUMBER,
        p_total_clientes OUT NUMBER);
    
    -- Obtener total de clientes por tipo de membresia
    PROCEDURE sp_obtener_total_clientes_por_tipo(
        p_id_sede IN NUMBER,
        p_tipo_membresia IN VARCHAR2,
        p_total OUT NUMBER);
        
    -- Listar entrenadores disponibles
    PROCEDURE sp_listar_entrenadores_disponibles(
        p_id_sede IN NUMBER,
        p_resultado OUT SYS_REFCURSOR);
      
    -- Validacion existencia entrenadores
    PROCEDURE ENTRENADOR_EXISTE(
        P_ID_ENTRENADOR IN VARCHAR2,
        P_EXISTE OUT NUMBER);      
    
    -- Listar entrenadores
    PROCEDURE LISTAR_ENTRENADORES(
        P_ID_SEDE IN VARCHAR2,
        P_FILTER IN VARCHAR2,
        P_ENTRENADORES OUT SYS_REFCURSOR);
        
        -- Obtener entrenador por id y sede
    PROCEDURE ObtenerEntrenadorPorIdentificacion (
        p_id_entrenador IN INT,
        p_id_sede IN INT,
        p_identificacion OUT VARCHAR2,
        p_nombre OUT VARCHAR2,
        p_telefono OUT VARCHAR2,
        p_genero OUT VARCHAR2,
        p_especialidad OUT VARCHAR2,
        p_salario OUT VARCHAR2,
        p_fecha_contratacion OUT DATE); 
    
    -- Obtener total de entrenadores por sede
    PROCEDURE ObtenerTotalEntrenadoresPorSede(
        p_id_sede IN NUMBER,
        p_total_entrenadores OUT NUMBER);
        
    -- Validacion existencia persona
    PROCEDURE PERSONA_EXISTE(
        P_ID_PERSONA IN NUMBER,
        P_EXISTE OUT NUMBER);
    
    -- Obtener total de persona por sede
    PROCEDURE OBTENER_TOTAL_PERSONAS_POR_SEDE(
        P_ID_SEDE IN NUMBER,
        P_TOTAL_PERSONAS OUT NUMBER);
    
    -- Listar ejercicios por PMF
    PROCEDURE ListarEjercicios(
        p_idCliente IN NUMBER,
        p_fechaValoracion IN DATE,
        p_resultado OUT SYS_REFCURSOR);
    
    -- Validacion existencia PMF
    PROCEDURE PmfExistente(
        p_idCliente IN NUMBER,
        p_fechaValoracion IN DATE,
        p_existe OUT NUMBER);
    
    -- Validacion existencia Ejercicio
    PROCEDURE SP_EJERCICIO_EXISTENTE(
        p_idEjercicio IN NUMBER,
        p_idCliente IN NUMBER,
        p_fechaValoracion IN DATE,
        p_existe OUT NUMBER);
    
    -- Obtener datos de un ejercicio por id
    PROCEDURE SP_OBTENER_DATOS_EJERCICIO(
        p_idEjercicio IN VARCHAR2,
        p_idCliente OUT NUMBER,
        p_fecha OUT DATE,
        p_nombre OUT VARCHAR2,
        p_repeticiones OUT NUMBER,
        p_series OUT NUMBER,
        p_dia OUT VARCHAR2);
    
    -- Obtener el total de clientes asignados
    PROCEDURE NumeroClientesAsignados(
        p_id_entrenador IN NUMBER,
        p_total_clientes OUT NUMBER); 
    
    -- Obtener dias restantes de contrato
    PROCEDURE DiasRestantesContrato(
        p_id_entrenador IN NUMBER,
        p_dias_restantes OUT NUMBER); 
        
    -- Obtener el preso promedio de los clientes asignados del entrenador
    PROCEDURE PesoPromedioClientesEntrenador(
        p_id_entrenador IN NUMBER,
        p_peso_promedio OUT NUMBER); 
        
    -- Modificar la informacion general del ejercicio
    PROCEDURE modificarGeneralEjercicio(
        p_id_ejercicio IN ejercicio.id_ejercicio%type, 
        p_numero_repeticiones IN ejercicio.numero_repeticiones%type,              
        p_numero_series IN ejercicio.numero_series%type,                    
        p_dia_semana IN ejercicio.dia_semana%type);
    
    -- Eliminar el ejercicio
    PROCEDURE eliminarEjercicio(p_id_ejercicio IN ejercicio.id_ejercicio%type);
    
    PROCEDURE modificarNombreEjercicio(
    p_id_ejercicio IN nombreEjercicio.id_nombre_ejercicio%type, 
    p_id_nombre_ejercicio IN nombreEjercicio.nombre_ejercicio%type);
    
END pkg_Procedimientos;

CREATE OR REPLACE PACKAGE BODY pkg_Procedimientos AS
    -- Funcion calcular dias restantes de membresia
    FUNCTION calcular_dias_restantes(p_id_cliente IN NUMBER)  
    RETURN NUMBER 
    IS
        v_fecha_suscripcion DATE;
        v_dias_restantes NUMBER;
    BEGIN
        -- Recupera la fecha de suscripción desde la tabla Membresia
        SELECT fecha_suscripcion 
        INTO v_fecha_suscripcion 
        FROM Membresia 
        WHERE id_cliente = p_id_cliente;
    
        -- Si la fecha actual es anterior a la fecha de suscripción, no ha comenzado, se mantienen 31 días
        IF SYSDATE < v_fecha_suscripcion THEN
            v_dias_restantes := 31;
        ELSE
            -- Si ya comenzó, calcular los días restantes desde la fecha de suscripción
            v_dias_restantes := 31 - (TRUNC(SYSDATE) - TRUNC(v_fecha_suscripcion));
        END IF;
    
        -- Si ya ha pasado más de 31 días desde la suscripción, entonces quedan 0 días
        IF v_dias_restantes < 0 THEN
            v_dias_restantes := 0;
        END IF;
    
        RETURN v_dias_restantes;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            RETURN -1;  -- Manejo de errores si no se encuentra el cliente
        WHEN OTHERS THEN
            RAISE;
    END calcular_dias_restantes;
    
    -- Listar clientes
    PROCEDURE LISTAR_CLIENTES(
        P_FILTER      IN  VARCHAR2,
        P_ID_SEDE     IN  NUMBER,
        P_CURSOR      OUT SYS_REFCURSOR)
    AS
        SQL_QUERY VARCHAR2(4000);
    BEGIN
        -- Inicializamos la consulta base
        SQL_QUERY := 'SELECT c.id_cliente, ' ||
                     'MAX(p.nombre_persona) AS nombre, ' ||
                     'MAX(p.telefono_persona) AS telefono, ' ||
                     'MAX(m.tipo) AS membresia, ' ||
                     'TO_CHAR(MAX(m.fecha_suscripcion), ''DD-MM-YYYY'') AS fecha ' ||
                     'FROM cliente c ' ||
                     'INNER JOIN persona p ON c.id_cliente = p.id_persona ' ||
                     'INNER JOIN membresia m ON c.id_cliente = m.id_cliente ' ||
                     'WHERE p.id_sede = :P_ID_SEDE ';
    
        -- Aplicar el filtro
        IF P_FILTER = 'all' THEN
            -- No se aplica ningún filtro adicional
            NULL;
        ELSIF P_FILTER = 'premium' THEN
            SQL_QUERY := SQL_QUERY || ' AND m.tipo = ''Premium''';
        ELSIF P_FILTER = 'general' THEN
            SQL_QUERY := SQL_QUERY || ' AND m.tipo = ''General''';
        ELSIF P_FILTER = 'masculino' THEN
            SQL_QUERY := SQL_QUERY || ' AND p.genero_persona = ''M''';
        ELSIF P_FILTER = 'femenino' THEN
            SQL_QUERY := SQL_QUERY || ' AND p.genero_persona = ''F''';
        ELSIF P_FILTER = 'no-especificado' THEN
            SQL_QUERY := SQL_QUERY || ' AND p.genero_persona = ''NE''';
        ELSE
            RAISE_APPLICATION_ERROR(-20001, 'Filtro no válido.');
        END IF;
    
        -- Añadir agrupamiento
        SQL_QUERY := SQL_QUERY || ' GROUP BY c.id_cliente';
    
        -- Abrir el cursor con la consulta construida
        OPEN P_CURSOR FOR SQL_QUERY USING P_ID_SEDE;
    END LISTAR_CLIENTES;
    
    -- Listar clientes asignados con membresia vigente
    PROCEDURE LISTAR_CLIENTES_ASIGNADOS(
        P_ID_ENTRENADOR IN NUMBER,
        P_CURSOR OUT SYS_REFCURSOR)
    AS
    BEGIN
        OPEN P_CURSOR FOR
        SELECT CLIENTE.id_cliente AS id_cliente,
               PERSONA.nombre_persona AS nombre,
               PERSONA.telefono_persona AS telefono,
               MEMBRESIA.tipo AS membresia,
               calcular_dias_restantes(CLIENTE.id_cliente) AS dias_restantes
        FROM cliente
        INNER JOIN persona ON cliente.id_cliente = persona.id_persona
        INNER JOIN membresia ON cliente.id_cliente = membresia.id_cliente
        WHERE CLIENTE.id_entrenador = P_ID_ENTRENADOR AND calcular_dias_restantes(CLIENTE.id_cliente)>0 AND membresia.tipo NOT IN ('Inactivo');
    END LISTAR_CLIENTES_ASIGNADOS;
    
    -- Validacion existencia cliente
    PROCEDURE CLIENTE_EXISTE(
        P_ID_CLIENTE IN VARCHAR2,
        P_EXISTE OUT NUMBER)
    AS
    BEGIN
        SELECT COUNT(*)
        INTO P_EXISTE
        FROM CLIENTE
        WHERE id_cliente = P_ID_CLIENTE;
    
        -- Convertir el conteo en 1 o 0 para indicar existencia
        IF P_EXISTE > 0 THEN
            P_EXISTE := 1;
        ELSE
            P_EXISTE := 0;
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            -- En caso de error, devolver 0
            P_EXISTE := 0;
            RAISE;
    END CLIENTE_EXISTE;
    
    -- Obtener cliente por id y sede
    PROCEDURE sp_obtener_cliente(
        p_id_cliente IN NUMBER,
        p_id_sede IN NUMBER,
        p_id OUT NUMBER,
        p_nombre OUT VARCHAR2,
        p_telefono OUT VARCHAR2,
        p_genero OUT VARCHAR2,
        p_tipo OUT VARCHAR2,
        p_dias_restantes OUT NUMBER)
    AS
    BEGIN
        SELECT c.id_cliente, 
    p.nombre_persona, 
    p.telefono_persona, 
    p.genero_persona, 
    m.tipo,
               calcular_dias_restantes(c.id_cliente)
        INTO 	p_id, 
    p_nombre, 
    p_telefono, 
    p_genero, 
    p_tipo, 
    p_dias_restantes
        FROM Cliente c
        JOIN Persona p ON c.id_cliente = p.id_persona
        JOIN Membresia m ON c.id_cliente = m.id_cliente
        WHERE c.id_cliente = p_id_cliente AND p.id_sede = p_id_sede;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            raise;
    END sp_obtener_cliente;
    
    -- Obtener total de clientes por sede
    PROCEDURE sp_obtener_total_clientes(
        p_id_sede IN NUMBER,
        p_total_clientes OUT NUMBER) 
    AS
    BEGIN
        SELECT COUNT(*)
        INTO p_total_clientes
        FROM Cliente c
        JOIN Persona p ON c.id_Cliente = p.id_Persona
        WHERE p.id_Sede = p_id_sede;
    END sp_obtener_total_clientes;
    
    -- Obtener total de clientes por tipo de membresia
    PROCEDURE sp_obtener_total_clientes_por_tipo(
        p_id_sede IN NUMBER,
        p_tipo_membresia IN VARCHAR2,
        p_total OUT NUMBER) 
    AS
    BEGIN
        SELECT COUNT(*)
        INTO p_total
        FROM Cliente c
        JOIN Persona p ON c.id_Cliente = p.id_Persona
        JOIN Membresia m ON c.id_Cliente = m.id_Cliente
        WHERE p.id_Sede = p_id_sede AND m.tipo = p_tipo_membresia;
    END sp_obtener_total_clientes_por_tipo;
    
    -- Listar entrenadores disponibles
    PROCEDURE sp_listar_entrenadores_disponibles(
        p_id_sede IN NUMBER,
        p_resultado OUT SYS_REFCURSOR) 
    AS
    BEGIN
        OPEN p_resultado FOR
            SELECT e.id_entrenador, 
                   MAX(p.nombre_persona) AS nombre_entrenador, 
                   MAX(ae.nombre_ae) AS area_especialidad, 
                   COUNT(c.id_cliente) AS num_clientes
            FROM ENTRENADOR e 
            INNER JOIN PERSONA p ON e.id_entrenador = p.id_persona 
            INNER JOIN AREAESPECIALIDAD ae ON e.id_ae = ae.id_ae
            LEFT JOIN CLIENTE c ON c.id_entrenador = e.id_entrenador
            WHERE p.id_sede = p_id_sede
            GROUP BY e.id_entrenador
            HAVING COUNT(c.id_cliente) < 5;
    END sp_listar_entrenadores_disponibles;
    
    -- Validacion existencia entrenadores
    PROCEDURE ENTRENADOR_EXISTE(
        P_ID_ENTRENADOR IN VARCHAR2,
        P_EXISTE OUT NUMBER)
    AS
    BEGIN
        SELECT COUNT(*)
        INTO P_EXISTE
        FROM ENTRENADOR
        WHERE id_entrenador = P_ID_ENTRENADOR;
    
        -- Convertir el conteo en 1 o 0 para indicar existencia
        IF P_EXISTE > 0 THEN
            P_EXISTE := 1;
        ELSE
            P_EXISTE := 0;
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            -- En caso de error, devolver 0
            P_EXISTE := 0;
            RAISE;
    END ENTRENADOR_EXISTE;
    
    -- Listar entrenadores
    PROCEDURE LISTAR_ENTRENADORES(
        P_ID_SEDE IN VARCHAR2,
        P_FILTER IN VARCHAR2,
        P_ENTRENADORES OUT SYS_REFCURSOR)
    AS
        SQL_QUERY VARCHAR2(4000);
    BEGIN
        -- Inicializamos la consulta base
        SQL_QUERY := 'SELECT E.id_Entrenador, ' ||
                     'MAX(P.nombre_Persona) AS nombre_entrenador, ' ||
                     'MAX(P.telefono_Persona) AS telefono_entrenador, ' ||
                     'MAX(P.genero_persona) AS genero_entrenador, ' ||
                     'MAX(AE.nombre_AE) AS area_especialidad, ' ||
                     'COUNT(C.id_Cliente) AS numero_clientes, ' ||
                     'MAX(CT.fecha_inicio_contrato) AS fecha_contratacion, ' ||
                     'MAX(CT.salario) AS salario ' ||
                     'FROM Entrenador E ' ||
                     'INNER JOIN Persona P ON E.id_Entrenador = P.id_Persona ' ||
                     'INNER JOIN AreaEspecialidad AE ON E.id_AE = AE.id_AE ' ||
                     'LEFT JOIN Cliente C ON E.id_Entrenador = C.id_Entrenador ' ||
                     'LEFT JOIN Contrato CT ON E.id_Entrenador = CT.id_entrenador ' ||
                     'WHERE P.id_Sede = :P_ID_SEDE ';
    
        -- Aplicar el filtro de especialidad o género
        IF P_FILTER = 'all' THEN
            -- No se aplica ningún filtro
            NULL;
        ELSIF P_FILTER = 'crossfit' THEN
            SQL_QUERY := SQL_QUERY || ' AND AE.nombre_AE = ''Crossfit''';
        ELSIF P_FILTER = 'fuerza' THEN
            SQL_QUERY := SQL_QUERY || ' AND AE.nombre_AE = ''Fuerza''';
        ELSIF P_FILTER = 'reduccion' THEN
            SQL_QUERY := SQL_QUERY || ' AND AE.nombre_AE = ''Reduccion de peso''';
        ELSIF P_FILTER = 'culturismo' THEN
            SQL_QUERY := SQL_QUERY || ' AND AE.nombre_AE = ''Culturismo''';
        ELSIF P_FILTER = 'masculino' THEN
            SQL_QUERY := SQL_QUERY || ' AND P.genero_persona = ''M''';
        ELSIF P_FILTER = 'femenino' THEN
            SQL_QUERY := SQL_QUERY || ' AND P.genero_persona = ''F''';
        ELSIF P_FILTER = 'no-especificado' THEN
            SQL_QUERY := SQL_QUERY || ' AND P.genero_persona = ''NE''';
        END IF;
    
        SQL_QUERY := SQL_QUERY || ' GROUP BY E.id_Entrenador';
    
        -- Abrir el cursor con la consulta construida
        OPEN P_ENTRENADORES FOR SQL_QUERY USING P_ID_SEDE;
    END LISTAR_ENTRENADORES;
    
    -- Obtener entrenador por id y sede
    PROCEDURE ObtenerEntrenadorPorIdentificacion(
        p_id_entrenador IN INT,
        p_id_sede IN INT,
        p_identificacion OUT VARCHAR2,
        p_nombre OUT VARCHAR2,
        p_telefono OUT VARCHAR2,
        p_genero OUT VARCHAR2,
        p_especialidad OUT VARCHAR2,
        p_salario OUT VARCHAR2,
        p_fecha_contratacion OUT DATE) 
    AS
    BEGIN
        SELECT E.id_Entrenador, 
               P.nombre_Persona,
               P.telefono_Persona,
               P.genero_Persona,
               AE.nombre_AE,
               CT.salario,
               CT.fecha_inicio_contrato
        INTO p_identificacion,
             p_nombre,
             p_telefono,
             p_genero,
             p_especialidad,
             p_salario,
             p_fecha_contratacion
        FROM Entrenador E
        JOIN Persona P ON E.id_Entrenador = P.id_Persona
        JOIN AreaEspecialidad AE ON E.id_AE = AE.id_AE
        LEFT JOIN Contrato CT ON E.id_Entrenador = CT.id_entrenador
        WHERE E.id_Entrenador = p_id_entrenador AND P.id_sede = p_id_sede;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            raise;
    END ObtenerEntrenadorPorIdentificacion;
    
    -- Obtener total de entrenadores por sede
    PROCEDURE ObtenerTotalEntrenadoresPorSede(
        p_id_sede IN NUMBER,
        p_total_entrenadores OUT NUMBER)
    AS
    BEGIN
        SELECT COUNT(*) 
        INTO p_total_entrenadores
        FROM Entrenador e
        JOIN Persona p ON e.id_Entrenador = p.id_Persona
        WHERE p.id_Sede = p_id_sede;
    END ObtenerTotalEntrenadoresPorSede;
    
    -- Validacion existencia persona
    PROCEDURE PERSONA_EXISTE(
        P_ID_PERSONA IN NUMBER,
        P_EXISTE OUT NUMBER)
    AS
    BEGIN
        -- Verificar si la persona existe en la tabla PERSONA
        SELECT COUNT(*)
        INTO P_EXISTE
        FROM PERSONA
        WHERE id_persona = P_ID_PERSONA;
    
        -- Convertir el conteo en 1 (existe) o 0 (no existe)
        IF P_EXISTE > 0 THEN
            P_EXISTE := 1;
        ELSE
            P_EXISTE := 0;
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            -- En caso de error, devolver 0
            P_EXISTE := 0;
            RAISE;
    END PERSONA_EXISTE;
    
    -- Obtener total de persona por sede
    PROCEDURE OBTENER_TOTAL_PERSONAS_POR_SEDE(
        P_ID_SEDE IN NUMBER,
        P_TOTAL_PERSONAS OUT NUMBER)
    AS
    BEGIN
        -- Consultar el total de personas para la sede indicada
        SELECT COUNT(*)
        INTO P_TOTAL_PERSONAS
        FROM PERSONA
        WHERE id_sede = P_ID_SEDE;
    EXCEPTION
        WHEN OTHERS THEN
            -- En caso de error, establecer el total de personas a 0
            P_TOTAL_PERSONAS := 0;
            RAISE;
    END OBTENER_TOTAL_PERSONAS_POR_SEDE;
    
    -- Listar ejercicios por PMF
    PROCEDURE ListarEjercicios(
        p_idCliente IN NUMBER,
        p_fechaValoracion IN DATE,
        p_resultado OUT SYS_REFCURSOR)
    AS
    BEGIN
        OPEN p_resultado FOR
            SELECT 
                E.ID_EJERCICIO AS id,
                NE.NOMBRE_EJERCICIO AS nombre,
                E.NUMERO_REPETICIONES AS repeticiones,
                E.NUMERO_SERIES AS series,
                E.DIA_SEMANA AS dia
            FROM 
                EJERCICIO E
            INNER JOIN 
                NOMBREEJERCICIO NE ON E.ID_NOMBRE_EJERCICIO = NE.ID_NOMBRE_EJERCICIO
            WHERE 
                E.ID_CLIENTE = p_idCliente
                AND E.ID_FECHA_VALORACION = p_fechaValoracion;
    END ListarEjercicios;
    
    -- Validacion existencia PMF
    PROCEDURE PmfExistente(
        p_idCliente IN NUMBER,
        p_fechaValoracion IN DATE,
        p_existe OUT NUMBER)
    AS
    BEGIN
        -- Realizar la consulta con los parámetros
        SELECT COUNT(*)
        INTO p_existe
        FROM pmf
        WHERE id_cliente = p_idCliente
          AND FECHA_VALORACION = p_fechaValoracion;
          
        -- Si no existen resultados, p_existe será 0; de lo contrario, será mayor que 0
    END PmfExistente;
    
    -- Validacion existencia Ejercicio
    PROCEDURE SP_EJERCICIO_EXISTENTE(
        p_idEjercicio IN NUMBER,
        p_idCliente IN NUMBER,
        p_fechaValoracion IN DATE,
        p_existe OUT NUMBER)
    AS
    BEGIN
        SELECT COUNT(*)
        INTO p_existe
        FROM ejercicio
        WHERE ID_EJERCICIO = p_idEjercicio
          AND id_cliente = p_idCliente
          AND ID_FECHA_VALORACION = p_fechaValoracion;
    END SP_EJERCICIO_EXISTENTE;
    
    -- Obtener datos de un ejercicio por id
    PROCEDURE SP_OBTENER_DATOS_EJERCICIO(
        p_idEjercicio IN VARCHAR2,
        p_idCliente OUT NUMBER,
        p_fecha OUT DATE,
        p_nombre OUT VARCHAR2,
        p_repeticiones OUT NUMBER,
        p_series OUT NUMBER,
        p_dia OUT VARCHAR2) 
    AS
    BEGIN
        SELECT EJERCICIO.ID_CLIENTE,
               EJERCICIO.ID_FECHA_VALORACION,
               NOMBREEJERCICIO.NOMBRE_EJERCICIO,
               EJERCICIO.NUMERO_REPETICIONES,
               EJERCICIO.NUMERO_SERIES,
               EJERCICIO.DIA_SEMANA
        INTO   	p_idCliente, 
                p_fecha, 
                p_nombre,
                p_repeticiones, 
                p_series, 
                p_dia
        FROM EJERCICIO
        INNER JOIN NOMBREEJERCICIO
        ON EJERCICIO.ID_NOMBRE_EJERCICIO = NOMBREEJERCICIO.ID_NOMBRE_EJERCICIO
        WHERE EJERCICIO.ID_EJERCICIO = p_idEjercicio;
    END SP_OBTENER_DATOS_EJERCICIO;
    
    -- Obtener el total de clientes asignados
    PROCEDURE NumeroClientesAsignados(
        p_id_entrenador IN NUMBER,
        p_total_clientes OUT NUMBER) 
    AS
    BEGIN
        SELECT COUNT(*)
        INTO p_total_clientes
        FROM Cliente
        WHERE id_Entrenador = p_id_entrenador;
        DBMS_OUTPUT.PUT_LINE('El entrenador con ID ' || p_id_entrenador || ' tiene ' || p_total_clientes || ' clientes asignados.');
    END NumeroClientesAsignados;
    
    -- Obtener dias restantes de contrato
    PROCEDURE DiasRestantesContrato(
        p_id_entrenador IN NUMBER,
        p_dias_restantes OUT NUMBER) 
    AS
    BEGIN
        SELECT GREATEST(0, fecha_fin_contrato - TRUNC(SYSDATE))
        INTO p_dias_restantes
        FROM Contrato
        WHERE id_entrenador = p_id_entrenador;
        DBMS_OUTPUT.PUT_LINE('El entrenador con ID ' || p_id_entrenador || ' tiene ' || p_dias_restantes || ' días restantes de contrato.');
    END DiasRestantesContrato;
    
    -- Obtener el peso promedio de los clientes del entrenador
    PROCEDURE PesoPromedioClientesEntrenador(
        p_id_entrenador IN NUMBER,
        p_peso_promedio OUT NUMBER) 
    AS
    BEGIN
        SELECT AVG(pf.peso)
        INTO p_peso_promedio
        FROM ProgresoFisico pf
        JOIN Cliente c ON pf.id_Cliente = c.id_Cliente
        WHERE c.id_Entrenador = p_id_entrenador;
    
        IF p_peso_promedio IS NULL THEN
            DBMS_OUTPUT.PUT_LINE('El entrenador con ID ' || p_id_entrenador || ' no tiene clientes asignados con progreso físico registrado.');
        ELSE
            DBMS_OUTPUT.PUT_LINE('El peso promedio de los clientes asignados al entrenador con ID ' || p_id_entrenador || ' es ' || p_peso_promedio || ' kg.');
        END IF;
    END PesoPromedioClientesEntrenador;

    -- Modificar la informacion general del ejercicio
    PROCEDURE modificarGeneralEjercicio(
        p_id_ejercicio IN ejercicio.id_ejercicio%type, 
        p_numero_repeticiones IN ejercicio.numero_repeticiones%type,              
        p_numero_series IN ejercicio.numero_series%type,                    
        p_dia_semana IN ejercicio.dia_semana%type)
    IS
        v_count NUMBER;
        e_NotFound_Ejercicio EXCEPTION;
    BEGIN
        SELECT COUNT(*) INTO v_count
        FROM ejercicio
        WHERE id_ejercicio = p_id_ejercicio;
        IF v_count > 0 THEN
            UPDATE ejercicio 
            SET numero_repeticiones = p_numero_repeticiones,
                numero_series = p_numero_series,
                dia_semana = p_dia_semana 
            WHERE id_ejercicio = p_id_ejercicio;
        ELSE
            RAISE e_NotFound_Ejercicio;
        END IF;
    EXCEPTION
        WHEN e_NotFound_Ejercicio THEN
            RAISE_APPLICATION_ERROR(-20010, 'Ejercicio no existente');
        WHEN OTHERS THEN
            IF SQLCODE = -1400 THEN
                RAISE_APPLICATION_ERROR(-20005, 'Valores nulos no permitidos en una columna que no acepta nulos');
            ELSIF SQLCODE = -12899 THEN 
                RAISE_APPLICATION_ERROR(-20006, 'Error de restricci?n de longitud de campo');
            ELSIF SQLCODE = -1722 THEN 
                RAISE_APPLICATION_ERROR(-20007, 'Error de conversi?n de tipos de datos');
            ELSE
                RAISE_APPLICATION_ERROR(-20008, 'Error no identificado: ' || SQLERRM);
            END IF;
    END modificarGeneralEjercicio;
    
    -- Eliminar el ejercicio
    PROCEDURE eliminarEjercicio(p_id_ejercicio IN ejercicio.id_ejercicio%type)
    IS
        v_count NUMBER;
        e_NotFound_ejercicio EXCEPTION; 
    BEGIN
        SELECT COUNT(*) INTO v_count
        FROM ejercicio
        WHERE id_ejercicio = p_id_ejercicio;
        IF v_count > 0 THEN
            DELETE FROM ejercicio WHERE id_ejercicio = p_id_ejercicio;
        ELSE
            RAISE e_NotFound_ejercicio;
        END IF;
    EXCEPTION
        WHEN e_NotFound_ejercicio THEN
            RAISE_APPLICATION_ERROR(-20010, 'Ejercicio no existente');
        WHEN OTHERS THEN
            RAISE_APPLICATION_ERROR(-20008, 'Error no identificado: ' || SQLERRM);
    END eliminarEjercicio;
    
    PROCEDURE modificarNombreEjercicio(
    p_id_ejercicio IN nombreEjercicio.id_nombre_ejercicio%type, 
    p_id_nombre_ejercicio IN nombreEjercicio.nombre_ejercicio%type)
    IS
    BEGIN
        UPDATE ejercicio 
        SET id_nombre_ejercicio = p_id_nombre_ejercicio 
        WHERE id_ejercicio = p_id_ejercicio;
    END modificarNombreEjercicio;
    
END pkg_Procedimientos;