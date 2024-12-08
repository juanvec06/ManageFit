DROP TABLE Ejercicio;
DROP TABLE PMF;
DROP TABLE ProgresoFisico;
DROP TABLE Membresia;
DROP TABLE Cliente;
DROP TABLE Contrato;
DROP TABLE CuentaEntrenador;
DROP TABLE Entrenador;
DROP TABLE AreaEspecialidad;
DROP TABLE CuentaSede;
DROP TABLE Sede;
DROP TABLE NombreEjercicio;


-- BASES DE DATOS


CREATE TABLE Sede
(
    id_Sede NUMBER NOT NULL,
    nombre_sede VARCHAR2(20) NOT NULL,
    ciudad_sede VARCHAR2(20) NOT NULL,
    telefono_sede NUMBER NOT NULL,
    CONSTRAINT pk_Sede PRIMARY KEY(id_Sede)
);


CREATE TABLE CuentaSede
(
    id_Sede NUMBER NOT NULL,  
    password_sede VARCHAR2(20) NOT NULL,
    CONSTRAINT pk_CuentaSede PRIMARY KEY(id_Sede),
    CONSTRAINT fk_SedeCuenta FOREIGN KEY (id_Sede) REFERENCES Sede(id_Sede)
);


CREATE TABLE AreaEspecialidad
(
    id_Area_Especialidad NUMBER NOT NULL,
    nombre_especialidad VARCHAR2(20) NOT NULL,
    CONSTRAINT pk_AreaEspecialidad PRIMARY KEY(id_Area_Especialidad),
    CONSTRAINT ck_nombre_especialidad CHECK (nombre_especialidad IN ('Crossfit', 'Fuerza', 'Reduccion de peso', 'Culturismo'))
);


CREATE TABLE Entrenador
(
    id_Entrenador NUMBER NOT NULL,
    id_Sede NUMBER NOT NULL,
    id_Area_Especialidad NUMBER NOT NULL,
    nombre_entrenador VARCHAR2(50) NOT NULL,  
    telefono_entrenador VARCHAR2(20) NOT NULL,
    genero_entrenador CHAR(2) NOT NULL,  
    CONSTRAINT pk_Entrenador PRIMARY KEY(id_Entrenador),
    CONSTRAINT fk_SedeEntrenador FOREIGN KEY (id_Sede) REFERENCES Sede(id_Sede),
    CONSTRAINT fk_AreaEspeEntrenador FOREIGN KEY (id_Area_Especialidad) REFERENCES AreaEspecialidad(id_Area_Especialidad),
    CONSTRAINT ck_genero_entrenador CHECK (genero_entrenador IN ('M', 'F', 'NE'))
);


CREATE TABLE CuentaEntrenador
(
    id_Entrenador NUMBER NOT NULL,  
    password_entrenador VARCHAR2(20) NOT NULL,
    CONSTRAINT pk_CuentaEntrenador PRIMARY KEY(id_Entrenador),
    CONSTRAINT fk_EntrenadorCuenta FOREIGN KEY (id_Entrenador) REFERENCES Entrenador(id_Entrenador)
);


CREATE TABLE Contrato
(
    id_entrenador NUMBER NOT NULL,
    salario NUMBER NOT NULL,
    fecha_inicio_contrato DATE NOT NULL,
    fecha_fin_contrato DATE,
    CONSTRAINT pk_Contrato PRIMARY KEY(id_entrenador),
    CONSTRAINT fk_EntrenadorContrato FOREIGN KEY (id_entrenador) REFERENCES Entrenador(id_entrenador)
);


CREATE TABLE Cliente
(
    id_Cliente NUMBER NOT NULL,
    id_Sede NUMBER NOT NULL,
    id_Entrenador NUMBER, -- acepta nulos para los clientes no premium
    nombre_cliente VARCHAR2(50) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    telefono_cliente VARCHAR2(20) NOT NULL,
    genero_cliente CHAR(2) NOT NULL,  
    CONSTRAINT pk_Cliente PRIMARY KEY(id_Cliente),
    CONSTRAINT fk_SedeCliente FOREIGN KEY (id_Sede) REFERENCES Sede(id_Sede),
    CONSTRAINT fk_EntrenadorCliente FOREIGN KEY (id_Entrenador) REFERENCES Entrenador(id_Entrenador),
    CONSTRAINT ck_genero_cliente CHECK (genero_cliente IN ('M', 'F', 'NE'))
);


CREATE TABLE Membresia
(
    id_Cliente NUMBER NOT NULL,
    fecha_suscripcion DATE NOT NULL,
    tipo VARCHAR2(10) NOT NULL,  -- Tipo de membresia
    CONSTRAINT ck_tipo_membresia CHECK (tipo IN ('General', 'Premium', 'Inactivo')), -- CHECK en l?nea separada
    CONSTRAINT pk_Membresia PRIMARY KEY(id_Cliente),
    CONSTRAINT fk_ClienteMembresia FOREIGN KEY (id_Cliente) REFERENCES Cliente(id_Cliente)
);




CREATE TABLE ProgresoFisico
(
    id_Cliente NUMBER NOT NULL,          
    fecha_toma DATE NOT NULL,          
    peso NUMBER NOT NULL,                
    estatura NUMBER NOT NULL,
    presion_arterial_reposo VARCHAR2(7) NOT NULL,
    CONSTRAINT pk_ProgresoFisico PRIMARY KEY(id_Cliente, fecha_toma),  
    CONSTRAINT fk_ClienteProgreso FOREIGN KEY (id_Cliente) REFERENCES Cliente(id_Cliente)
);


CREATE TABLE PMF
(
    id_Cliente NUMBER NOT NULL,        
    fecha_valoracion DATE NOT NULL,    
    objetivo VARCHAR2(100) NOT NULL,    
    CONSTRAINT pk_PMF PRIMARY KEY(id_Cliente, fecha_valoracion),
    CONSTRAINT fk_ClientePMF FOREIGN KEY (id_Cliente) REFERENCES Cliente(id_Cliente)
);


CREATE TABLE NombreEjercicio
(
    id_nombre_ejercicio NUMBER NOT NULL,
    nombre_ejercicio VARCHAR2(30) NOT NULL, -- mayor tamaño para el nombre
    CONSTRAINT pk_NombreEjercicio PRIMARY KEY(id_nombre_ejercicio),
    CONSTRAINT ck_NombreEjercicio CHECK (nombre_ejercicio IN ('Squad', 'Leg press', 'Bench press', 'Pull-ups','Bicep curls', 'Hip thrust', 'Deadlifts','Press banca', 'Planks', 'Bulgarian split squats'))
);


CREATE TABLE Ejercicio
(
    id_ejercicio NUMBER NOT NULL,      
   
    id_cliente NUMBER NOT NULL,                      
    id_fecha_valoracion DATE NOT NULL,  
   
    id_nombre_ejercicio NUMBER NOT NULL,  
   
    numero_repeticiones NUMBER NOT NULL,              
   
    numero_series NUMBER NOT NULL,                    
    dia_semana VARCHAR2(10) NOT NULL,  
    CONSTRAINT pk_Ejercicio PRIMARY KEY(id_ejercicio),  
    CONSTRAINT fk_PMF_Ejercicio FOREIGN KEY (id_cliente, id_fecha_valoracion) REFERENCES PMF(id_cliente, fecha_valoracion),
    CONSTRAINT fk_NombreEjercicio FOREIGN KEY (id_nombre_ejercicio) REFERENCES NombreEjercicio(id_nombre_ejercicio),
    CONSTRAINT ck_dia_semana CHECK (dia_semana IN ('Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'))
);

