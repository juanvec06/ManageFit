--  PARA INSERTAR EN LA TABLA SEDE: (3)
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
INSERT INTO AreaEspecialidad (id_Area_Especialidad, nombre_especialidad)
VALUES (1, 'Crossfit');


INSERT INTO AreaEspecialidad (id_Area_Especialidad, nombre_especialidad)
VALUES (2, 'Fuerza');


INSERT INTO AreaEspecialidad (id_Area_Especialidad, nombre_especialidad)
VALUES (3, 'Reduccion de peso');


INSERT INTO AreaEspecialidad (id_Area_Especialidad, nombre_especialidad)
VALUES (4, 'Culturismo');


-- PARA INSERTAR EN LA TABLA ENTRENADOR: (7)


-- Insertar entrenadores para la Sede 1 (Sede Central)
INSERT INTO Entrenador (id_Entrenador, id_Sede, id_Area_Especialidad, nombre_entrenador, telefono_entrenador, genero_entrenador)
VALUES (1, 1, 1, 'Carlos Gomez', '3001234567', 'M');


INSERT INTO Entrenador (id_Entrenador, id_Sede, id_Area_Especialidad, nombre_entrenador, telefono_entrenador, genero_entrenador)
VALUES (2, 1, 2, 'Mar?a Perez', '3012345678', 'F');




-- Insertar entrenadores para la Sede 2 (Sede Norte)
INSERT INTO Entrenador (id_Entrenador, id_Sede, id_Area_Especialidad, nombre_entrenador, telefono_entrenador, genero_entrenador)
VALUES (6, 2, 1, 'Ana Rodriguez', '3056789012', 'F');




INSERT INTO Entrenador (id_Entrenador, id_Sede, id_Area_Especialidad, nombre_entrenador, telefono_entrenador, genero_entrenador)
VALUES (9, 2, 2, 'Fernando Garcia', '3089012345', 'M');


INSERT INTO Entrenador (id_Entrenador, id_Sede, id_Area_Especialidad, nombre_entrenador, telefono_entrenador, genero_entrenador)
VALUES (10, 2, 1, 'Camila Rojas', '3090123456', 'F');


-- Insertar entrenadores para la Sede 3 (Sede Sur)
INSERT INTO Entrenador (id_Entrenador, id_Sede, id_Area_Especialidad, nombre_entrenador, telefono_entrenador, genero_entrenador)
VALUES (11, 3, 3, 'Miguel Castillo', '3101234567', 'M');


INSERT INTO Entrenador (id_Entrenador, id_Sede, id_Area_Especialidad, nombre_entrenador, telefono_entrenador, genero_entrenador)
VALUES (12, 3, 4, 'Andrea Moreno', '3112345678', 'F');




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
INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (1, 1, NULL, 'Juan Garcia', TO_DATE('1990-01-01', 'YYYY-MM-DD'), '3111234567', 'M');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (2, 1, 2, 'Lucia Fernandez', TO_DATE('1995-02-15', 'YYYY-MM-DD'), '3122345678', 'F');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (3, 1, 1, 'Miguel Perez', TO_DATE('1988-03-30', 'YYYY-MM-DD'), '3133456789', 'M');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (4, 1, 1, 'Sofia Ramirez', TO_DATE('1993-07-20', 'YYYY-MM-DD'), '3144567890', 'F');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (5, 1, NULL, 'Carlos Rojas', TO_DATE('1998-11-05', 'YYYY-MM-DD'), '3155678901', 'M');


-- Insertar clientes para entrenadores en la Sede 2
INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (6, 2, 6, 'Ana Lopez', TO_DATE('1992-06-18', 'YYYY-MM-DD'), '3166789012', 'F');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (7, 2, 9, 'Andres Ortega', TO_DATE('1985-09-10', 'YYYY-MM-DD'), '3177890123', 'M');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (8, 2, 10, 'Laura Moreno', TO_DATE('1994-03-22', 'YYYY-MM-DD'), '3188901234', 'F');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (9, 2, 9, 'Diego Torres', TO_DATE('1989-12-01', 'YYYY-MM-DD'), '3199012345', 'M');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (10, 2, 10, 'Isabel Gutierrez', TO_DATE('1991-05-25', 'YYYY-MM-DD'), '3200123456', 'F');


-- Insertar clientes para entrenadores en la Sede 3
INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (11, 3, NULL, 'Roberto Vargas', TO_DATE('1987-08-14', 'YYYY-MM-DD'), '3211234567', 'M');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (12, 3, 12, 'Gabriela Reyes', TO_DATE('1996-04-11', 'YYYY-MM-DD'), '3222345678', 'F');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (13, 3, 11, 'Jose Mendoza', TO_DATE('1983-10-09', 'YYYY-MM-DD'), '3233456789', 'M');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (14, 3, NULL, 'Valeria Salazar', TO_DATE('1997-12-30', 'YYYY-MM-DD'), '3244567890', 'F');


INSERT INTO Cliente (id_Cliente, id_Sede, id_Entrenador, nombre_cliente, fecha_nacimiento, telefono_cliente, genero_cliente)
VALUES (15, 3, 11, 'Mauricio Gomez', TO_DATE('1990-11-27', 'YYYY-MM-DD'), '3255678901', 'M');




-- PARA INSERTAR EN LA TABLA MEMBRESIA: (15)


-- Insertar membres?as para los clientes de la Sede 1
INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (1, TO_DATE('2024-01-01', 'YYYY-MM-DD'), 'General');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (2, TO_DATE('2024-02-15', 'YYYY-MM-DD'), 'Premium');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (3, TO_DATE('2024-03-30', 'YYYY-MM-DD'), 'Premium');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (4, TO_DATE('2024-07-20', 'YYYY-MM-DD'), 'Premium');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (5, TO_DATE('2024-11-05', 'YYYY-MM-DD'), 'Inactivo');


-- Insertar membres?as para los clientes de la Sede 2
INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (6, TO_DATE('2024-06-18', 'YYYY-MM-DD'), 'Premium');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (7, TO_DATE('2024-09-10', 'YYYY-MM-DD'), 'Premium');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (8, TO_DATE('2024-03-22', 'YYYY-MM-DD'), 'Premium');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (9, TO_DATE('2024-12-01', 'YYYY-MM-DD'), 'Premium');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (10, TO_DATE('2024-05-25', 'YYYY-MM-DD'), 'Premium');


-- Insertar membres?as para los clientes de la Sede 3
INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (11, TO_DATE('2024-08-14', 'YYYY-MM-DD'), 'General');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (12, TO_DATE('2024-04-11', 'YYYY-MM-DD'), 'Premium');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (13, TO_DATE('2024-10-09', 'YYYY-MM-DD'), 'Premium');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (14, TO_DATE('2024-12-30', 'YYYY-MM-DD'), 'Inactivo');


INSERT INTO Membresia (id_Cliente, fecha_suscripcion, tipo)
VALUES (15, TO_DATE('2024-11-27', 'YYYY-MM-DD'), 'Premium');


-- PARA INSERTAR EN LA TABLA PROGRESO F?SICO: (11)
-- Insertar progreso f?sico para los clientes premium
INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (2, TO_DATE('2024-09-01', 'YYYY-MM-DD'), 70, 1.65, '120/80');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (3, TO_DATE('2024-09-02', 'YYYY-MM-DD'), 60, 1.70, '115/75');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (4, TO_DATE('2024-09-03', 'YYYY-MM-DD'), 80, 1.75, '125/80');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (6, TO_DATE('2024-09-04', 'YYYY-MM-DD'), 68, 1.60, '118/76');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (7, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 72, 1.80, '122/78');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (8, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 74, 1.82, '116/74');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (9, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 68, 1.65, '122/78');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (10, TO_DATE('2024-09-03', 'YYYY-MM-DD'), 67, 1.57, '125/80');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (12, TO_DATE('2024-09-01', 'YYYY-MM-DD'), 70, 1.70, '121/79');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (13, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 56, 1.55, '123/80');


INSERT INTO ProgresoFisico (id_Cliente, fecha_toma, peso, estatura, presion_arterial_reposo)
VALUES (15, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 72, 1.60, '120/80');


-- PARA INSERTAR EN LA TABLA PMF: (11)


-- Insertar PMF para los clientes premium
INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (2, TO_DATE('2024-09-10', 'YYYY-MM-DD'), 'Aumentar resistencia cardiovascular y tonificar m?sculos.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (3, TO_DATE('2024-09-11', 'YYYY-MM-DD'), 'Reducir grasa corporal y mejorar fuerza muscular.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (4, TO_DATE('2024-09-12', 'YYYY-MM-DD'), 'Ganar masa muscular y mejorar el rendimiento f?sico.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (6, TO_DATE('2024-09-13', 'YYYY-MM-DD'), 'Mantener peso saludable y mejorar flexibilidad.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (7, TO_DATE('2024-09-14', 'YYYY-MM-DD'), 'Mejorar la postura y corregir desequilibrios musculares.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (8, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 'Mantener peso actual y mejorar la condici?n cardiovascular.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (9, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 'Tonificar m?sculos y mejorar la flexibilidad general.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (10, TO_DATE('2024-09-03', 'YYYY-MM-DD'), 'Aumentar masa muscular y mejorar la postura corporal.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (12, TO_DATE('2024-09-01', 'YYYY-MM-DD'), 'Reducir grasa corporal y aumentar la resistencia muscular.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (13, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 'Ganar peso de forma saludable y mejorar la fuerza general.');


INSERT INTO PMF (id_Cliente, fecha_valoracion, objetivo)
VALUES (15, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 'Reducir peso corporal y mejorar la flexibilidad articular.');


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
VALUES (1, 2, TO_DATE('2024-09-10', 'YYYY-MM-DD'), 1, 12, 4, 'Lunes');  -- Squad


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (2, 2, TO_DATE('2024-09-10', 'YYYY-MM-DD'), 3, 10, 3, 'Miercoles');  -- Bench press


-- Insertar ejercicios para el cliente 3 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (3, 3, TO_DATE('2024-09-11', 'YYYY-MM-DD'), 2, 15, 3, 'Martes');  -- Leg press


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (4, 3, TO_DATE('2024-09-11', 'YYYY-MM-DD'), 4, 8, 4, 'Jueves');  -- Pull-ups


-- Insertar ejercicios para el cliente 4 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (5, 4, TO_DATE('2024-09-12', 'YYYY-MM-DD'), 5, 12, 3, 'Lunes');  -- Bicep curls


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (6, 4, TO_DATE('2024-09-12', 'YYYY-MM-DD'), 7, 10, 4, 'Martes');  -- Deadlifts


-- Insertar ejercicios para el cliente 6 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (7, 6, TO_DATE('2024-09-13', 'YYYY-MM-DD'), 6, 15, 3, 'Miercoles');  -- Hip thrust


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (8, 6, TO_DATE('2024-09-13', 'YYYY-MM-DD'), 8, 10, 4, 'Jueves');  -- Press banca


-- Insertar ejercicios para el cliente 7 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (9, 7, TO_DATE('2024-09-14', 'YYYY-MM-DD'), 9, 30, 3, 'Viernes');  -- Planks


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (10, 7, TO_DATE('2024-09-14', 'YYYY-MM-DD'), 10, 12, 4, 'Sabado');  -- Bulgarian split squats


-- Insertar ejercicios para el cliente 8 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (11, 8, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 1, 12, 3, 'Lunes');  -- Squat


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (12, 8, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 3, 10, 3, 'Miercoles');  -- Bench press


-- Insertar ejercicios para el cliente 9 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (13, 9, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 2, 15, 3, 'Martes');  -- Leg press


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (14, 9, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 4, 8, 4, 'Jueves');  -- Pull-ups


-- Insertar ejercicios para el cliente 10 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (15, 10, TO_DATE('2024-09-03', 'YYYY-MM-DD'), 5, 12, 3, 'Lunes');  -- Bicep curls


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (16, 10, TO_DATE('2024-09-03', 'YYYY-MM-DD'), 7, 10, 4, 'Martes');  -- Deadlifts


-- Insertar ejercicios para el cliente 12 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (17, 12, TO_DATE('2024-09-01', 'YYYY-MM-DD'), 6, 15, 3, 'Miercoles');  -- Hip thrust


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (18, 12, TO_DATE('2024-09-01', 'YYYY-MM-DD'), 8, 10, 4, 'Jueves');  -- Press banca


-- Insertar ejercicios para el cliente 13 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (19, 13, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 9, 30, 3, 'Viernes');  -- Planks


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (20, 13, TO_DATE('2024-09-05', 'YYYY-MM-DD'), 10, 12, 4, 'Sabado');  -- Bulgarian split squats


-- Insertar ejercicios para el cliente 15 (Premium)
INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (21, 15, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 1, 12, 3, 'Lunes');  -- Squat


INSERT INTO Ejercicio (id_ejercicio, id_cliente, id_fecha_valoracion, id_nombre_ejercicio, numero_repeticiones, numero_series, dia_semana)
VALUES (22, 15, TO_DATE('2024-09-06', 'YYYY-MM-DD'), 3, 10, 3, 'Miercoles');  -- Bench press