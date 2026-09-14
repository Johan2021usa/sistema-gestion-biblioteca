/* =============================================================================
   Script_Biblioteca.sql
   Sistema de Gestión de Biblioteca — ACA Programación Avanzada
   Motor: SQL Server (ejecutar desde SQL Server Management Studio)

   Contenido, en orden:
     1) Creación de la base de datos "Biblioteca"
     2) Creación de las 5 tablas (Autores, Editoriales, Libros, Usuarios, Prestamos)
     3) Relaciones (llaves foráneas) e integridad referencial
     4) Datos de prueba

   El script es REEJECUTABLE: elimina las tablas antes de volver a crearlas, de modo
   que se puede correr las veces que haga falta sin errores de objeto duplicado.
   OJO: al reejecutarlo se pierden los datos capturados desde la aplicación.
   ============================================================================= */


/* -----------------------------------------------------------------------------
   1) BASE DE DATOS
   Se crea solo si no existe todavía, para no destruir la base en cada ejecución.
   ----------------------------------------------------------------------------- */
IF DB_ID('Biblioteca') IS NULL
BEGIN
    CREATE DATABASE Biblioteca;
END
GO

USE Biblioteca;
GO


/* -----------------------------------------------------------------------------
   2) LIMPIEZA PREVIA
   Se eliminan las tablas en orden inverso a sus dependencias: primero las que
   contienen llaves foráneas (Prestamos, Libros) y después las tablas padre.
   Al revés, SQL Server bloquearía el DROP por integridad referencial.
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.Prestamos', 'U')   IS NOT NULL DROP TABLE dbo.Prestamos;
IF OBJECT_ID('dbo.Libros', 'U')      IS NOT NULL DROP TABLE dbo.Libros;
IF OBJECT_ID('dbo.Usuarios', 'U')    IS NOT NULL DROP TABLE dbo.Usuarios;
IF OBJECT_ID('dbo.Editoriales', 'U') IS NOT NULL DROP TABLE dbo.Editoriales;
IF OBJECT_ID('dbo.Autores', 'U')     IS NOT NULL DROP TABLE dbo.Autores;
GO


/* -----------------------------------------------------------------------------
   3) TABLAS
   ----------------------------------------------------------------------------- */

/* Autores: catálogo de autores. El Id lo genera SQL Server (IDENTITY). */
CREATE TABLE dbo.Autores
(
    IdAutor  INT          IDENTITY(1,1) NOT NULL,
    Nombre   VARCHAR(100)               NOT NULL,
    Apellido VARCHAR(100)               NOT NULL,
    CONSTRAINT PK_Autores PRIMARY KEY (IdAutor)
);
GO

/* Editoriales: catálogo de editoriales. Solo guarda el nombre. */
CREATE TABLE dbo.Editoriales
(
    IdEditorial INT          IDENTITY(1,1) NOT NULL,
    Nombre      VARCHAR(100)               NOT NULL,
    CONSTRAINT PK_Editoriales PRIMARY KEY (IdEditorial)
);
GO

/* Libros: el ISBN es la llave primaria natural (lo escribe el usuario, no es IDENTITY).
   Categoria es texto libre: por decisión de diseño NO existe una tabla Categorias.
   Existencias lleva un CHECK que garantiza la regla RN10 (entero >= 0). */
CREATE TABLE dbo.Libros
(
    ISBN        VARCHAR(20)  NOT NULL,
    Titulo      VARCHAR(200) NOT NULL,
    IdAutor     INT          NOT NULL,
    IdEditorial INT          NOT NULL,
    Categoria   VARCHAR(100)     NULL,
    Anio        INT              NULL,
    Existencias INT          NOT NULL,
    CONSTRAINT PK_Libros             PRIMARY KEY (ISBN),
    CONSTRAINT FK_Libros_Autores     FOREIGN KEY (IdAutor)     REFERENCES dbo.Autores(IdAutor),
    CONSTRAINT FK_Libros_Editoriales FOREIGN KEY (IdEditorial) REFERENCES dbo.Editoriales(IdEditorial),
    CONSTRAINT CK_Libros_Existencias CHECK (Existencias >= 0)
);
GO

/* Usuarios: el Documento es único (regla RN09). Se refuerza con UNIQUE para que la
   base también lo garantice, además de la validación que hará la aplicación. */
CREATE TABLE dbo.Usuarios
(
    IdUsuario INT          IDENTITY(1,1) NOT NULL,
    Nombre    VARCHAR(100)               NOT NULL,
    Apellido  VARCHAR(100)               NOT NULL,
    Documento VARCHAR(20)                NOT NULL,
    Telefono  VARCHAR(20)                    NULL,
    Correo    VARCHAR(100)                   NULL,
    CONSTRAINT PK_Usuarios           PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Usuarios_Documento UNIQUE (Documento)
);
GO

/* Prestamos: relaciona un usuario con un libro.
   FechaDevolucion queda NULL mientras el préstamo está activo.
   Estado guarda "Activo" o "Devuelto". */
CREATE TABLE dbo.Prestamos
(
    IdPrestamo      INT         IDENTITY(1,1) NOT NULL,
    IdUsuario       INT                       NOT NULL,
    ISBN            VARCHAR(20)               NOT NULL,
    FechaPrestamo   DATE                      NOT NULL,
    FechaDevolucion DATE                          NULL,
    Estado          VARCHAR(20)               NOT NULL,
    CONSTRAINT PK_Prestamos          PRIMARY KEY (IdPrestamo),
    CONSTRAINT FK_Prestamos_Usuarios FOREIGN KEY (IdUsuario) REFERENCES dbo.Usuarios(IdUsuario),
    CONSTRAINT FK_Prestamos_Libros   FOREIGN KEY (ISBN)      REFERENCES dbo.Libros(ISBN)
);
GO


/* -----------------------------------------------------------------------------
   4) DATOS DE PRUEBA
   Se insertan en orden de dependencia: primero los catálogos (Autores, Editoriales,
   Usuarios) y después Libros y Prestamos, que dependen de ellos.
   ----------------------------------------------------------------------------- */

/* --- Autores (8) --- */
INSERT INTO dbo.Autores (Nombre, Apellido) VALUES
    ('Gabriel',    'García Márquez'),
    ('Isabel',     'Allende'),
    ('Jorge Luis', 'Borges'),
    ('Mario',      'Vargas Llosa'),
    ('Laura',      'Restrepo'),
    ('Julio',      'Cortázar'),
    ('Robert',     'Martin'),
    ('Andrew',     'Hunt');
GO

/* --- Editoriales (5) --- */
INSERT INTO dbo.Editoriales (Nombre) VALUES
    ('Editorial Norma'),
    ('Planeta Colombia'),
    ('Penguin Random House'),
    ('Alfaguara'),
    ('Pearson Educación');
GO

/* --- Usuarios (6) --- */
INSERT INTO dbo.Usuarios (Nombre, Apellido, Documento, Telefono, Correo) VALUES
    ('Johan',    'Lasso',    '1012345678', '3001112233', 'johan.lasso@correo.com'),
    ('Camila',   'Ortiz',    '1023456789', '3012223344', 'camila.ortiz@correo.com'),
    ('Andrés',   'Beltrán',  '1034567890', '3023334455', 'andres.beltran@correo.com'),
    ('Valeria',  'Muñoz',    '1045678901', '3034445566', 'valeria.munoz@correo.com'),
    ('Santiago', 'Rojas',    '1056789012', '3045556677', 'santiago.rojas@correo.com'),
    ('Daniela',  'Quintero', '1067890123', NULL,         NULL);
GO

/* --- Libros (10) ---
   Los Ids de autor y editorial corresponden al orden en que se insertaron arriba.
   Las existencias que aparecen aquí YA ESTÁN DESCONTADAS por los préstamos activos
   que se insertan más abajo, para que la base quede coherente con la regla RN06.
   El libro 978-84-663-3777-9 queda en 0 existencias a propósito: sirve para probar
   el caso CP07 (no se puede prestar un libro sin existencias). */
INSERT INTO dbo.Libros (ISBN, Titulo, IdAutor, IdEditorial, Categoria, Anio, Existencias) VALUES
    ('978-03-074-7472-8', 'Cien años de soledad',              1, 3, 'Novela',       1967, 4),
    ('978-84-204-7183-9', 'El amor en los tiempos del cólera', 1, 4, 'Novela',       1985, 3),
    ('978-84-013-5283-6', 'La casa de los espíritus',          2, 2, 'Novela',       1982, 3),
    ('978-95-863-9280-8', 'Ficciones',                         3, 1, 'Cuento',       1944, 2),
    ('978-84-663-3777-9', 'La ciudad y los perros',            4, 3, 'Novela',       1963, 0),
    ('978-95-842-6936-2', 'Delirio',                           5, 1, 'Novela',       2004, 3),
    ('978-84-376-0457-2', 'Rayuela',                           6, 4, 'Novela',       1963, 1),
    ('978-01-323-5088-4', 'Código limpio',                     7, 5, 'Programación', 2008, 6),
    ('978-02-016-1622-4', 'El programador pragmático',         8, 5, 'Programación', 1999, 4),
    ('978-95-863-9745-2', 'El Aleph',                          3, 1, 'Cuento',       1949, 1);
GO

/* --- Prestamos (6) ---
   Cuatro activos (FechaDevolucion en NULL) y dos ya devueltos, para que los tres
   reportes del módulo FrmReportes tengan información que mostrar desde el inicio. */
INSERT INTO dbo.Prestamos (IdUsuario, ISBN, FechaPrestamo, FechaDevolucion, Estado) VALUES
    (1, '978-03-074-7472-8', '2026-08-20', NULL,         'Activo'),
    (2, '978-84-013-5283-6', '2026-08-25', NULL,         'Activo'),
    (3, '978-01-323-5088-4', '2026-07-10', '2026-07-28', 'Devuelto'),
    (4, '978-84-376-0457-2', '2026-09-01', NULL,         'Activo'),
    (5, '978-84-663-3777-9', '2026-09-05', NULL,         'Activo'),
    (1, '978-95-863-9280-8', '2026-06-15', '2026-06-30', 'Devuelto');
GO


/* -----------------------------------------------------------------------------
   5) VERIFICACIÓN RÁPIDA
   Consulta de comprobación para confirmar en SSMS que todo quedó cargado.
   ----------------------------------------------------------------------------- */
SELECT 'Autores' AS Tabla, COUNT(*) AS Registros FROM dbo.Autores
UNION ALL SELECT 'Editoriales', COUNT(*) FROM dbo.Editoriales
UNION ALL SELECT 'Usuarios',    COUNT(*) FROM dbo.Usuarios
UNION ALL SELECT 'Libros',      COUNT(*) FROM dbo.Libros
UNION ALL SELECT 'Prestamos',   COUNT(*) FROM dbo.Prestamos;
GO
