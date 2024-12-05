# **MANAGE FIT**
- **Nombre del negocio:** SPARTAN BOX
  
---

## **Manual de Instalación para el Software**

Este manual describe los pasos necesarios para instalar y configurar el software. Sigue los pasos en el orden indicado para garantizar un funcionamiento correcto.

---

## **Requisitos Previos**
1. **Base de datos:**
   - Oracle Database 21c Express Edition (versión 21.0.0.0).
   - SQL Developer (versión 24.3.0.284).

2. **IDE:**
   - Visual Studio 2019 (versión 16.0.31110) con las librerías necesarias para C#.

3. **Navegador web:**
   - Asegúrate de tener un navegador actualizado (Chrome, Firefox, Edge, etc.).

4. **Sistema operativo:**
   - Windows 10 o superior.

---

## **Pasos de Instalación**

### 1. **Instalar Oracle Database 21c Express Edition**
   - Descarga e instala Oracle Database 21c desde [el sitio oficial de Oracle](https://www.oracle.com/database/technologies/appdev/xe.html).
   - Sigue las instrucciones de instalación predeterminadas.

### 2. **Crear un perfil de base de datos**
   - Abre SQL Developer.
   - Conéctate a la base de datos con las credenciales de administrador.
   - Ejecuta los siguientes comandos para crear el perfil de base de datos:
     ```sql
     CREATE USER C##MANAGEFIT IDENTIFIED BY managefitpassword;
     ALTER USER C##MANAGEFIT ACCOUNT UNLOCK;
     GRANT CONNECT, RESOURCE, DBA TO C##MANAGEFIT;
     ```
   - Cambia `managefitpassword` por una contraseña segura.

### 3. **Cargar el script de las tablas y procedimientos**
   - Desde SQL Developer:
     - Abre una conexión con el usuario `C##MANAGEFIT`.
     - Ejecuta el archivo de script proporcionado (`script.sql`) para crear las tablas, procedimientos almacenados y triggers.

### 4. **Insertar tuplas iniciales**
   - Una vez creada la base de datos con el script, abre SQL Developer.
   - Ejecuta el archivo `tuplasPrincipales.sql` para insertar las tuplas iniciales necesarias para el funcionamiento del sistema.

### 5. **Crear administrador**
   - Inserta un registro en la tabla `Cuentasede` para establecer un administrador del sistema. Este será el encargado de administrar el software y tendrá acceso total. Utiliza una consulta como la siguiente:
     ```sql
     INSERT INTO Cuentasede (id_sede, password_sede) 
     VALUES (1,"password123");
     ```

### 6. **Configurar Visual Studio 2019**
   - Descarga Visual Studio 2019 desde [el sitio oficial de Microsoft](https://visualstudio.microsoft.com/).
   - Durante la instalación, selecciona las librerías necesarias para trabajar con C# (como .NET desktop development).
   - Una vez instalado, abre Visual Studio y selecciona **Importar proyecto**.
   - Carga el proyecto proporcionado desde el repositorio.

### 7. **Configurar la conexión a la base de datos**
   - Dentro del proyecto en Visual Studio, localiza el archivo de configuración para la conexión a la base de datos (`conexion.cs`) dentro de la carpeta `Datos`.
   - Asegúrate de colocar la contraseña en la ruta de conexión, reemplazando `{contraseña_managefit_oracle}`:
     ```csharp
     private static string ruta = "User Id=C##MANAGEFIT;Password={contraseña_managefit_oracle};Data Source=192.168.1.2:1521/xe";
     ```

### 8. **Ejecutar la aplicación**
   - Compila y ejecuta el proyecto desde Visual Studio.


---

## **Notas Finales**
- Asegúrate de que todos los servicios necesarios (como Oracle Database) estén en ejecución antes de iniciar la aplicación.
- Verifica que el firewall permita la conexión entre Visual Studio y Oracle Database si estás ejecutándolos en máquinas distintas.
- Si surge algún error, consulta los logs generados por la aplicación para identificar posibles problemas.

---

¡Gracias por usar nuestro software! Si tienes preguntas, no dudes en contactarnos.
