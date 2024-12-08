--PROCEDIMIENTOS USADOS PARA FUNCIONAMIENTO DEL INICIO DE SESION

CREATE OR REPLACE PROCEDURE validar_logina(p_usuario IN NUMBER, p_contraseña IN cuentasede.password_sede%TYPE, p_count OUT NUMBER)
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

CREATE OR REPLACE PROCEDURE validar_loginE(p_usuario IN NUMBER, p_contraseña IN cuentasede.password_sede%TYPE, p_count OUT NUMBER)
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

--PROCEDIMIENTOS USADOS PARA REGISTRO DE PERSONA

CREATE OR REPLACE PROCEDURE insertar_persona(
    p_id_persona IN persona.id_persona%type,
    p_id_sede IN sede.id_sede%type,
    p_nombre IN persona.nombre_persona%type,
    p_genero IN persona.genero_persona%type,
    p_telefono IN persona.telefono_persona%type
)
IS
BEGIN
    INSERT INTO Persona VALUES (p_id_persona, p_nombre, p_telefono, p_genero, p_id_sede);
EXCEPTION 
    WHEN DUP_VAL_ON_INDEX THEN
        RAISE_APPLICATION_ERROR(-20002,'Persona existente');
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20003,'Sede no existente');  
    WHEN VALUE_ERROR THEN
        RAISE_APPLICATION_ERROR(-20004, 'Valores nulos no permitidos');
    WHEN OTHERS THEN
        IF SQLCODE = -1400 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Valores nulos no permitidos en una columna que no acepta nulos');
        ELSIF SQLCODE = -12899 THEN 
            RAISE_APPLICATION_ERROR(-20006, 'Violación de restricción de longitud de campo');
        ELSIF SQLCODE = -1722 THEN 
            RAISE_APPLICATION_ERROR(-20007, 'Error de conversión de tipos de datos');
        ELSE
            RAISE_APPLICATION_ERROR(-20008, 'Ocurrió un error no identificado: ' || SQLERRM);
        END IF;
END;

--PROCEDIMIENTOS USADOS PARA REGISTRO DE ENTRENADOR

CREATE OR REPLACE FUNCTION validar_AE(nombre_AESP IN areaespecialidad.nombre_ae%type)
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
END;


CREATE OR REPLACE PROCEDURE insertar_Entrenador(
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
    WHEN VALUE_ERROR THEN
        RAISE_APPLICATION_ERROR(-20004, 'Valores nulos no permitidos');
    WHEN OTHERS THEN
        IF SQLCODE = -1400 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Valores nulos no permitidos en una columna que no acepta nulos');
        ELSIF SQLCODE = -12899 THEN 
            RAISE_APPLICATION_ERROR(-20006, 'Violación de restricción de longitud de campo');
        ELSIF SQLCODE = -1722 THEN 
            RAISE_APPLICATION_ERROR(-20007, 'Error de conversión de tipos de datos');
        ELSE
            RAISE_APPLICATION_ERROR(-20008, 'Ocurrió un error no identificado: ' || SQLERRM);
        END IF;
END;

--PROCEDIMIENTOS USADOS PARA REGISTRAR CLIENTE CON MEMBRESÍA

CREATE OR REPLACE PROCEDURE insertar_Cliente (
    p_id_cliente IN cliente.id_cliente%type,
    p_id_entrenador IN cliente.id_entrenador%type, -- Entrenador opcional
    p_fecha_nacimiento IN cliente.fecha_nacimiento%type)
IS
BEGIN
    INSERT INTO Cliente VALUES (p_id_cliente, p_id_entrenador, p_fecha_nacimiento);
    COMMIT;
    
EXCEPTION
    WHEN VALUE_ERROR THEN
        RAISE_APPLICATION_ERROR(-20004, 'Valores nulos no permitidos');
    WHEN OTHERS THEN
        IF SQLCODE = -1400 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Valores nulos no permitidos en una columna que no acepta nulos');
        ELSIF SQLCODE = -12899 THEN 
            RAISE_APPLICATION_ERROR(-20006, 'Violación de restricción de longitud de campo');
        ELSIF SQLCODE = -1722 THEN 
            RAISE_APPLICATION_ERROR(-20007, 'Error de conversión de tipos de datos');
        ELSE
            RAISE_APPLICATION_ERROR(-20008, 'Ocurrió un error no identificado: ' || SQLERRM);
        END IF;
END insertar_Cliente;

CREATE OR REPLACE PROCEDURE insertar_membresia (
    p_id_cliente IN cliente.id_cliente%type,
    p_tipo IN membresia.tipo%type,
    p_fecha_suscripcion IN membresia.fecha_suscripcion%type
)
IS
BEGIN
    -- Inserta una nueva membresía en la tabla Membresia
    INSERT INTO Membresia (id_Cliente, tipo, fecha_suscripcion) 
    VALUES (p_id_cliente, p_tipo, p_fecha_suscripcion);
    COMMIT;
    
EXCEPTION
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20001, 'Error al registrar la membresía: ' || SQLERRM);
END insertar_membresia;

CREATE OR REPLACE FUNCTION calcular_dias_restantes(p_id_cliente IN NUMBER)  
RETURN NUMBER IS
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

--PROCEDIMIENTOS USADOS PARA FUNCIONAMIENTO DE INSERTAR PMF

CREATE OR REPLACE PROCEDURE crearPMF (
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
        ELSIF SQLCODE = -1400 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Valores nulos no permitidos en una columna que no acepta nulos');
        ELSIF SQLCODE = -12899 THEN 
            RAISE_APPLICATION_ERROR(-20006, 'Error de restricci?n de longitud de campo');
        ELSIF SQLCODE = -1722 THEN 
            RAISE_APPLICATION_ERROR(-20007, 'Error de conversi?n de tipos de datos');
        ELSE
            RAISE_APPLICATION_ERROR(-20008, 'Error no identificado: ' || SQLERRM);
        END IF;
END;

--PROCEDIMIENTOS USADOS PARA FUNCIONAMIENTO DE INSERTAR EJERCICIO

--Secuencia

DROP SEQUENCE seq_idejercicio;

CREATE SEQUENCE seq_idejercicio
START WITH 1
INCREMENT BY 1
MINVALUE 1
NOCYCLE;

--Trigger secuencia
CREATE OR REPLACE TRIGGER trg_incrementar_id_ejercicio
BEFORE INSERT ON Ejercicio
FOR EACH ROW
BEGIN
    :NEW.id_ejercicio := seq_idejercicio.NEXTVAL;
END; 
---------------------- CREAR EJERCICIO -----------------------------------------
CREATE OR REPLACE PROCEDURE crearEjercicio (     
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
        ELSIF SQLCODE = -1400 THEN
            RAISE_APPLICATION_ERROR(-20005, 'Valores nulos no permitidos en una columna que no acepta nulos');
        ELSIF SQLCODE = -12899 THEN 
            RAISE_APPLICATION_ERROR(-20006, 'Error de restricci?n de longitud de campo');
        ELSIF SQLCODE = -1722 THEN 
            RAISE_APPLICATION_ERROR(-20007, 'Error de conversi?n de tipos de datos');
        ELSE
            RAISE_APPLICATION_ERROR(-20008, 'Error no identificado: ' || SQLERRM);
        END IF;
END;

