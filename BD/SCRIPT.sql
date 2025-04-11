-- PARA INSERTAR EN LA TABLA SEDE: (3)
INSERT INTO Sede (id_Sede, nombre_sede, ciudad_sede, telefono_sede)
VALUES (1, 'Sede Central', 'Popayan', 3124567890);

INSERT INTO Sede (id_Sede, nombre_sede, ciudad_sede, telefono_sede)
VALUES (2, 'Sede Norte', 'Popayan', 3041234567);

INSERT INTO Sede (id_Sede, nombre_sede, ciudad_sede, telefono_sede)
VALUES (3, 'Sede Sur', 'Popayan', 3119876543);


-- PARA INSERTAR EN LA TABLA CUENTA SEDE: (3)
INSERT INTO CuentaSede (id_Sede, password_sede)
VALUES (1, 'passCentral123');

INSERT INTO CuentaSede (id_Sede, password_sede)
VALUES (2, 'passNorte456');

INSERT INTO CuentaSede (id_Sede, password_sede)
VALUES (3, 'passSur789');


-- PARA INSERTAR EN LA TABLA AREA ESPECIALIDAD: (4)
INSERT INTO AreaEspecialidad (id_AE, nombre_AE)
VALUES (1, 'Crossfit');

INSERT INTO AreaEspecialidad (id_AE, nombre_AE)
VALUES (2, 'Fuerza');

INSERT INTO AreaEspecialidad (id_AE, nombre_AE)
VALUES (3, 'Reduccion de peso');

INSERT INTO AreaEspecialidad (id_AE, nombre_AE)
VALUES (4, 'Culturismo');


-- PARA INSERTAR EN LA TABLA PERSONA
-- Primero debemos insertar los datos de las personas (tanto entrenadores como clientes)
-- Entrenadores Sede 1
INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (1, 'Carlos Gomez', '3001234567', 'M', 1);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (2, 'Maria Perez', '3012345678', 'F', 1);

-- Entrenadores Sede 2
INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (6, 'Ana Rodriguez', '3056789012', 'F', 2);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (9, 'Fernando Garcia', '3089012345', 'M', 2);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (10, 'Camila Rojas', '3090123456', 'F', 2);

-- Entrenadores Sede 3
INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (11, 'Miguel Castillo', '3101234567', 'M', 3);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (12, 'Andrea Moreno', '3112345678', 'F', 3);

-- Clientes de la sede 1
INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (101, 'Juan Garcia', '3111234567', 'M', 1);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (102, 'Lucia Fernandez', '312234567', 'F', 1);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (103, 'Miguel Perez', '3133456789', 'M', 1);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (104, 'Sofia Ramirez', '3144567890', 'F', 1);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (105, 'Carlos Rojas', '3155678901', 'M', 1);

-- Clientes de la sede 2
INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (106, 'Ana Lopez', '3111234568', 'F', 2);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (107, 'Andres Ortega', '3177890123', 'M', 2);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (108, 'Laura Moreno', '3188901234', 'F', 2);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (109, 'Diego Torres', '3199012345', 'M', 2);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (110, 'Isabel Gutierrez', '3200123456', 'F', 2);

-- Clientes de la sede 3
INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (111, 'Roberto Vargas', '3211234567', 'M', 3);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (112, 'Gabriela Reyes', '3222345678', 'F', 3);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (113, 'Jose Mendoza', '3233456789', 'M', 3);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (114, 'Valeria Salazar', '3244567890', 'F', 3);

INSERT INTO Persona (id_Persona, nombre_Persona, telefono_Persona, genero_Persona, id_Sede)
VALUES (115, 'Mauricio Gomez', '3255678901', 'M', 3);


-- PARA INSERTAR EN LA TABLA ENTRENADOR: (7)
-- Insertar entrenadores para la Sede 1 (Sede Central)
INSERT INTO Entrenador (id_Entrenador, id_AE) VALUES (1, 1);

INSERT INTO Entrenador (id_Entrenador, id_AE) VALUES (2, 2);

-- Insertar entrenadores para la Sede 2 (Sede Norte)
INSERT INTO Entrenador (id_Entrenador, id_AE) VALUES (6, 1);

INSERT INTO Entrenador (id_Entrenador, id_AE) VALUES (9, 2);

INSERT INTO Entrenador (id_Entrenador, id_AE) VALUES (10, 1);

-- Insertar entrenadores para la Sede 3 (Sede Sur)
INSERT INTO Entrenador (id_Entrenador, id_AE) VALUES (11, 3);

INSERT INTO Entrenador (id_Entrenador, id_AE) VALUES (12, 4);


-- PARA INSERTAR EN LA TABLA CUENTA ENTRENADOR: (7)
-- Insertar cuentas para los entrenadores de la Sede 1
INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (1, 'password123');

INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (2, 'password456');

-- Insertar cuentas para los entrenadores de la Sede 2
INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (6, 'password678');

INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (9, 'password567');

INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (10, 'password890');

-- Insertar cuentas para los entrenadores de la Sede 3
INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (11, 'password1234');

INSERT INTO CuentaEntrenador (id_Entrenador, password_entrenador)
VALUES (12, 'password5678');


-- PARA INSERTAR EN LA TABLA CONTRATO: (7)
-- Insertar contratos para los entrenadores de la Sede 1
INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (1, 2000, TO_DATE('2024-01-01', 'YYYY-MM-DD'), TO_DATE('2025-01-01', 'YYYY-MM-DD'));

INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (2, 2500, TO_DATE('2024-02-01', 'YYYY-MM-DD'), TO_DATE('2025-02-01', 'YYYY-MM-DD'));

-- Insertar contratos para los entrenadores de la Sede 2
INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (6, 2300, TO_DATE('2024-06-01', 'YYYY-MM-DD'), TO_DATE('2025-06-01', 'YYYY-MM-DD'));

INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (9, 2200, TO_DATE('2024-09-01', 'YYYY-MM-DD'), NULL);

INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (10, 2400, TO_DATE('2024-10-01', 'YYYY-MM-DD'), NULL);

-- Insertar contratos para los entrenadores de la Sede 3
INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (11, 2000, TO_DATE('2024-01-15', 'YYYY-MM-DD'), TO_DATE('2025-01-15', 'YYYY-MM-DD'));

INSERT INTO Contrato (id_entrenador, salario, fecha_inicio_contrato, fecha_fin_contrato)
VALUES (12, 2100, TO_DATE('2024-02-15', 'YYYY-MM-DD'), TO_DATE('2025-02-15', 'YYYY-MM-DD'));


-- PARA INSERTAR EN LA TABLA CLIENTE: (15)
-- Insertar clientes para entrenadores en la Sede 1
INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (101, NULL, TO_DATE('1990-01-01', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (102, 2, TO_DATE('1995-02-15', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (103, 1, TO_DATE('1988-03-30', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (104, 1, TO_DATE('1993-07-20', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (105, NULL, TO_DATE('1998-11-05', 'YYYY-MM-DD'));

-- Insertar clientes para entrenadores en la Sede 2
INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (106, 6, TO_DATE('1992-06-18', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (107, 9, TO_DATE('1985-09-10', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (108, 10, TO_DATE('1994-03-22', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (109, 9, TO_DATE('1989-12-01', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (110, 10, TO_DATE('1991-05-25', 'YYYY-MM-DD'));

-- Insertar clientes para entrenadores en la Sede 3
INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (111, NULL, TO_DATE('1987-08-14', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (112, 12, TO_DATE('1996-04-11', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (113, 11, TO_DATE('1983-10-09', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (114, NULL, TO_DATE('1997-12-30', 'YYYY-MM-DD'));

INSERT INTO Cliente (id_Cliente, id_Entrenador, fecha_nacimiento)
VALUES (115, 11, TO_DATE('1997-12-30', 'YYYY-MM-DD'));


-- PARA INSERTAR EN LA TABLA MEMBRESIA: (15)
-- Insertar membresías para los clientes de la Sede 1
INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (101, TO_DATE('2024-01-01', 'YYYY-MM-DD'), 'General');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (102, TO_DATE('2024-02-15', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (103, TO_DATE('2024-03-30', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (104, TO_DATE('2024-07-20', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (105, TO_DATE('2024-11-05', 'YYYY-MM-DD'), 'Inactivo');

-- Insertar membres�as para los clientes de la Sede 2
INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (106, TO_DATE('2024-06-18', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (107, TO_DATE('2024-09-10', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (108, TO_DATE('2024-03-22', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (109, TO_DATE('2024-12-01', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (110, TO_DATE('2024-05-25', 'YYYY-MM-DD'), 'Premium');

-- Insertar membres�as para los clientes de la Sede 3
INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (111, TO_DATE('2024-08-14', 'YYYY-MM-DD'), 'General');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (112, TO_DATE('2024-04-11', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (113, TO_DATE('2024-10-09', 'YYYY-MM-DD'), 'Premium');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (114, TO_DATE('2024-12-30', 'YYYY-MM-DD'), 'Inactivo');

INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (115, TO_DATE('2024-11-27', 'YYYY-MM-DD'), 'Premium');


-- PARA INSERTAR EN LA TABLA PROGRESO F�SICO: (11)
-- Insertar progreso f�sico para los clientes premium
INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (102, TO_DATE('2024-09-01', 'YYYY-MM-DD'), 70, 1.65, '120/80');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (103, TO_DATE('2024-09-02', 'YYYY-MM-DD'), 60, 1.70, '115/75');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (104, TO_DATE('2024-09-03', 'YYYY-MM-DD'), 80, 1.75, '125/80');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (106, TO_DATE('2024-09-04', 'YYYY-MM-DD'), 68, 1.60, '118/76');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (107, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 72, 1.80, '122/78');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (108, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 74, 1.82, '116/74');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (109, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 68, 1.65, '122/78');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (110, TO_DATE('2024-09-03', 'YYYY-MM-DD'), 67, 1.57, '125/80');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (112, TO_DATE('2024-09-01', 'YYYY-MM-DD'), 70, 1.70, '121/79');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (113, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 56, 1.55, '123/80');

INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (115, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 72, 1.60, '120/80');


-- PARA INSERTAR EN LA TABLA PMF: (11)
-- Insertar PMF para los clientes premium
INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (102, TO_DATE('2024-09-10', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (103, TO_DATE('2024-09-11', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (104, TO_DATE('2024-09-12', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (106, TO_DATE('2024-09-13', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (107, TO_DATE('2024-09-14', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (108, TO_DATE('2024-09-06', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (109, TO_DATE('2024-09-05', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (110, TO_DATE('2024-09-03', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (112, TO_DATE('2024-09-01', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (113, TO_DATE('2024-09-05', 'YYYY-MM-DD'));

INSERT INTO PMF (id_Cliente, fecha_valoracion)
VALUES (115, TO_DATE('2024-09-06', 'YYYY-MM-DD'));


-- PARA INSERTAR EN LA TABLA NOMBRE EJERCICIO: (10)
-- Insertar ejercicios en la tabla NombreEjercicio
INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (1, 'Squad');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (2, 'Leg press');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (3, 'Bench press');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (4, 'Pull-ups');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (5, 'Bicep curls');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (6, 'Hip thrust');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (7, 'Deadlifts');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (8, 'Press banca');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (9, 'Planks');

INSERT INTO NombreEjercicio (id_nombre_ejercicio, nombre_ejercicio)
VALUES (10, 'Bulgarian split squats');


-- PARA INSERTAR EN LA TABLA EJERCICIO: (22)
-- Insertar ejercicios para el cliente 2 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (1, 102, TO_DATE('2024-09-10', 'YYYY-MM-DD'), 1, 12, 4, 'Lunes');  -- Squad

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (2, 102, TO_DATE('2024-09-10', 'YYYY-MM-DD'), 3, 10, 3, 'Miercoles');  -- Bench press

-- Insertar ejercicios para el cliente 3 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (3, 103, TO_DATE('2024-09-11', 'YYYY-MM-DD'), 2, 15, 3, 'Martes');  -- Leg press

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (4, 103, TO_DATE('2024-09-11', 'YYYY-MM-DD'), 4, 8, 4, 'Jueves');  -- Pull-ups

-- Insertar ejercicios para el cliente 4 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (5, 104, TO_DATE('2024-09-12', 'YYYY-MM-DD'), 5, 12, 3, 'Lunes');  -- Bicep curls

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (6, 104, TO_DATE('2024-09-12', 'YYYY-MM-DD'), 7, 10, 4, 'Martes');  -- Deadlifts

-- Insertar ejercicios para el cliente 6 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (7, 106, TO_DATE('2024-09-13', 'YYYY-MM-DD'), 6, 15, 3, 'Miercoles');  -- Hip thrust

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (8, 106, TO_DATE('2024-09-13', 'YYYY-MM-DD'), 8, 10, 4, 'Jueves');  -- Press banca

-- Insertar ejercicios para el cliente 7 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (9, 107, TO_DATE('2024-09-14', 'YYYY-MM-DD'), 9, 30, 3, 'Viernes');  -- Planks

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (10, 107, TO_DATE('2024-09-14', 'YYYY-MM-DD'), 10, 12, 4, 'Sabado');  -- Bulgarian split squats

-- Insertar ejercicios para el cliente 8 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (11, 108, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 1, 12, 3, 'Lunes');  -- Squat

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (12, 108, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 3, 10, 3, 'Miercoles');  -- Bench press

-- Insertar ejercicios para el cliente 9 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (13, 109, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 2, 15, 3, 'Martes');  -- Leg press

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (14, 109, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 4, 8, 4, 'Jueves');  -- Pull-ups

-- Insertar ejercicios para el cliente 10 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (15, 110, TO_DATE('2024-09-03', 'YYYY-MM-DD'), 5, 12, 3, 'Lunes');  -- Bicep curls

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (16, 110, TO_DATE('2024-09-03', 'YYYY-MM-DD'), 7, 10, 4, 'Martes');  -- Deadlifts

-- Insertar ejercicios para el cliente 12 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (17, 112, TO_DATE('2024-09-01', 'YYYY-MM-DD'), 6, 15, 3, 'Miercoles');  -- Hip thrust

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (18, 112, TO_DATE('2024-09-01', 'YYYY-MM-DD'), 8, 10, 4, 'Jueves');  -- Press banca

-- Insertar ejercicios para el cliente 13 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (19, 113, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 9, 30, 3, 'Viernes');  -- Planks

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (20, 113, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 10, 12, 4, 'Sabado');  -- Bulgarian split squats

-- Insertar ejercicios para el cliente 15 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (21, 115, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 1, 12, 3, 'Lunes');  -- Squat

INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (22, 115, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 3, 10, 3, 'Miercoles');  -- Bench press
