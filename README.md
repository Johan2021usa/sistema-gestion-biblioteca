# Sistema de Gestión de Biblioteca

> **Actividad de Construcción Aplicada (ACA)** — Programación Avanzada
> Aplicación de escritorio en C# / Windows Forms con base de datos SQL Server.

---

## Portada

| | |
|---|---|
| **Institución** | Coporacion Unificada Nacional |
| **Programa** | Ingenieria de Sistemas |
| **Asignatura** | Programación Avanzada |
| **Actividad** | ACA — Sistema de Gestión de Biblioteca |
| **Integrante(s)** | Jairo Johan Lasso Chaucanes — Ficha N.º 53304 |
| **Docente** | Veronica Castro Munar |
| **Fecha de entrega** | 9/13/2026 |

## Contraportada

Este documento presenta el análisis, diseño, construcción y pruebas del **Sistema de Gestión
de Biblioteca**, desarrollado como Actividad de Construcción Aplicada de la asignatura
Programación Avanzada. El sistema es una aplicación de escritorio construida en C# sobre
Windows Forms, con persistencia en SQL Server mediante ADO.NET, y organizada en capas
para separar la presentación del acceso a datos.

Todo el código, el script de base de datos y la documentación son de elaboración propia.

---

## Tabla de contenido

1. [Introducción](#1-introducción)
2. [Objetivos](#2-objetivos)
3. [Planteamiento del problema](#3-planteamiento-del-problema)
4. [Análisis de requerimientos](#4-análisis-de-requerimientos)
5. [Casos de uso](#5-casos-de-uso)
6. [Diagrama de clases](#6-diagrama-de-clases)
7. [Modelo entidad-relación](#7-modelo-entidad-relación)
8. [Diccionario de datos](#8-diccionario-de-datos)
9. [Arquitectura del sistema](#9-arquitectura-del-sistema)
10. [Módulos del sistema](#10-módulos-del-sistema)
11. [Instrucciones para ejecutar el proyecto](#11-instrucciones-para-ejecutar-el-proyecto)
12. [Capturas de pantalla](#12-capturas-de-pantalla)
13. [Pruebas de funcionamiento](#13-pruebas-de-funcionamiento)
14. [Conclusiones](#14-conclusiones)
15. [Recomendaciones](#15-recomendaciones)
16. [Referencias](#16-referencias)
17. [Anexos](#17-anexos)

---

## 1. Introducción

Una biblioteca que administra sus préstamos en papel o en hojas de cálculo termina
enfrentando siempre los mismos problemas: no sabe con certeza cuántos ejemplares de un
título están disponibles, registra dos veces el mismo libro con datos distintos, y no
puede responder rápido a la pregunta más básica de todas, *¿quién tiene este libro?*

Este proyecto construye un sistema de escritorio que resuelve esa gestión sobre una base
de datos relacional. El sistema administra cinco entidades —autores, editoriales, libros,
usuarios y préstamos— y controla automáticamente la disponibilidad de ejemplares: cada
préstamo descuenta una unidad del inventario y cada devolución la reintegra, sin que el
operador tenga que llevar esa cuenta a mano.

El documento recorre el análisis, el diseño de datos, la arquitectura, la implementación
de cada módulo y las pruebas ejecutadas sobre el sistema terminado.

---

## 2. Objetivos

### 2.1 Objetivo general

Desarrollar una aplicación de escritorio en C# con Windows Forms y SQL Server que permita
gestionar los libros, autores, editoriales, usuarios y préstamos de una biblioteca,
aplicando una arquitectura por capas y garantizando la integridad de la información.

### 2.2 Objetivos específicos

1. Diseñar un modelo relacional normalizado de cinco tablas con sus llaves primarias,
   llaves foráneas y restricciones de integridad.
2. Implementar la capa de acceso a datos con ADO.NET, centralizando la cadena de conexión
   en una única clase y utilizando exclusivamente consultas parametrizadas.
3. Construir un módulo CRUD completo para cada entidad del catálogo (autores, editoriales,
   usuarios y libros), con validación de los datos antes de escribir en la base.
4. Implementar la lógica de préstamos con control automático de existencias, de modo que
   registrar, devolver o eliminar un préstamo mantenga siempre coherente el inventario.
5. Generar reportes de consulta mediante combinaciones (JOIN) entre varias tablas.
6. Verificar el funcionamiento del sistema mediante casos de prueba documentados con
   evidencia gráfica.

---

## 3. Planteamiento del problema

La gestión manual de una biblioteca presenta tres fallas que se refuerzan entre sí:

**Información duplicada e inconsistente.** Sin una llave que identifique cada libro de
forma única, el mismo título termina registrado varias veces con datos distintos. Lo mismo
ocurre con los usuarios cuando no se controla el documento de identidad.

**Inventario poco confiable.** Cuando el descuento de ejemplares depende de que alguien
recuerde anotarlo, la cifra de disponibles deja de coincidir con la realidad del estante.
El resultado es prometer un libro que ya está prestado.

**Imposibilidad de consultar.** Responder «¿qué préstamos están activos?» o «¿cuánto
inventario hay por categoría?» exige revisar registro por registro, algo inviable a medida
que la colección crece.

El sistema propuesto ataca las tres: la base de datos impone la unicidad mediante llaves
primarias y restricciones `UNIQUE`; la aplicación descuenta y reintegra existencias de
forma automática dentro de transacciones; y el módulo de reportes resuelve las consultas
con combinaciones entre tablas.

---

## 4. Análisis de requerimientos

### 4.1 Requerimientos funcionales

| Código | Requerimiento |
|---|---|
| RF01 | Registrar, consultar, modificar y eliminar autores |
| RF02 | Registrar, consultar, modificar y eliminar editoriales |
| RF03 | Registrar, consultar, modificar y eliminar usuarios |
| RF04 | Registrar, consultar, modificar y eliminar libros |
| RF05 | Buscar un libro por su ISBN y cargarlo en el formulario |
| RF06 | Asociar cada libro a un autor y a una editorial existentes mediante listas desplegables |
| RF07 | Registrar un préstamo asociando un usuario y un libro disponible |
| RF08 | Descontar un ejemplar del inventario al registrar un préstamo |
| RF09 | Registrar la devolución de un préstamo activo y reintegrar el ejemplar |
| RF10 | Eliminar un préstamo, reintegrando el ejemplar si el préstamo estaba activo |
| RF11 | Generar el reporte de préstamos activos |
| RF12 | Generar el reporte de préstamos devueltos |
| RF13 | Generar el reporte de inventario de libros |
| RF14 | Navegar entre los módulos desde un menú principal |

### 4.2 Requerimientos no funcionales

| Código | Requerimiento |
|---|---|
| RNF01 | La aplicación se ejecuta en Windows como aplicación de escritorio |
| RNF02 | Toda consulta a la base de datos usa parámetros; no se concatenan datos del usuario en el texto SQL |
| RNF03 | Toda operación contra la base de datos se ejecuta dentro de un bloque `try/catch` y comunica el resultado con un mensaje comprensible |
| RNF04 | La cadena de conexión está centralizada en una sola clase, para poder cambiar de servidor modificando un único archivo |
| RNF05 | Las operaciones que afectan a más de una tabla se ejecutan dentro de una transacción |
| RNF06 | La interfaz mantiene el mismo tamaño, disposición y paleta de colores en todos los formularios |
| RNF07 | Las conexiones se abren y se liberan mediante bloques `using`, sin dejar conexiones abiertas entre operaciones |

### 4.3 Reglas de negocio

| Código | Regla | Dónde se aplica |
|---|---|---|
| RN01 | El ISBN es único: no pueden existir dos libros con el mismo ISBN | `FrmLibros` + `PK_Libros` |
| RN02 | Los campos obligatorios no pueden quedar vacíos | Todos los módulos |
| RN03 | No se registra un préstamo sin un usuario existente | `FrmPrestamos` + `FK_Prestamos_Usuarios` |
| RN04 | No se registra un préstamo sin un libro existente | `FrmPrestamos` + `FK_Prestamos_Libros` |
| RN05 | No se presta un libro sin ejemplares disponibles | `FrmPrestamos` |
| RN06 | Al registrar un préstamo, las existencias bajan en 1 y el estado queda «Activo» | `FrmPrestamos` |
| RN07 | Al devolver o eliminar un préstamo activo, las existencias suben en 1 | `FrmPrestamos` |
| RN08 | Las llaves primarias no se modifican | Todos los módulos |
| RN09 | El documento del usuario es único | `FrmUsuarios` + `UQ_Usuarios_Documento` |
| RN10 | El año debe ser entero; las existencias, un entero mayor o igual a cero | `FrmLibros` + `CK_Libros_Existencias` |

> Varias reglas se aplican en dos niveles: la aplicación valida antes de enviar los datos,
> para dar un mensaje claro al usuario, y la base de datos los garantiza mediante
> restricciones, para que ningún camino alternativo pueda violarlos.

---

## 5. Casos de uso

El sistema tiene un único actor: el **bibliotecario**, la persona que opera la aplicación.

```mermaid
flowchart LR
    B(["Bibliotecario"])

    subgraph Catalogo["Gestión del catálogo"]
        CU01["CU01 · Gestionar autores"]
        CU02["CU02 · Gestionar editoriales"]
        CU03["CU03 · Gestionar libros"]
        CU04["CU04 · Buscar libro por ISBN"]
    end

    subgraph Personas["Gestión de usuarios"]
        CU05["CU05 · Gestionar usuarios"]
    end

    subgraph Circulacion["Circulación"]
        CU06["CU06 · Registrar préstamo"]
        CU07["CU07 · Registrar devolución"]
        CU08["CU08 · Eliminar préstamo"]
    end

    subgraph Consulta["Consulta"]
        CU09["CU09 · Generar reportes"]
    end

    B --- CU01
    B --- CU02
    B --- CU03
    B --- CU04
    B --- CU05
    B --- CU06
    B --- CU07
    B --- CU08
    B --- CU09
```

### 5.1 Casos de uso detallados

**CU06 — Registrar préstamo**

| | |
|---|---|
| **Actor** | Bibliotecario |
| **Precondición** | Existen usuarios y libros registrados; el libro tiene al menos un ejemplar disponible |
| **Flujo principal** | 1. El bibliotecario abre el módulo Préstamos.<br>2. Selecciona el usuario de la lista desplegable.<br>3. Selecciona el libro, que muestra entre paréntesis los ejemplares disponibles.<br>4. Confirma o cambia la fecha del préstamo (por defecto, hoy).<br>5. Pulsa **Guardar**.<br>6. El sistema verifica las existencias, registra el préstamo con estado «Activo» y descuenta un ejemplar. |
| **Flujo alternativo A** | Si no hay usuario o libro seleccionado, el sistema lo advierte y no continúa (RN03, RN04). |
| **Flujo alternativo B** | Si el libro tiene cero ejemplares, el sistema lo advierte y no registra nada (RN05). |
| **Postcondición** | Existe un préstamo activo y el inventario del libro disminuyó en una unidad. |

**CU07 — Registrar devolución**

| | |
|---|---|
| **Actor** | Bibliotecario |
| **Precondición** | Existe un préstamo en estado «Activo» |
| **Flujo principal** | 1. El bibliotecario hace doble clic sobre el préstamo en la lista.<br>2. Pulsa **Devolver** y confirma.<br>3. El sistema marca el préstamo como «Devuelto», registra la fecha de devolución y reintegra el ejemplar. |
| **Flujo alternativo** | Si el préstamo ya figura como devuelto, el sistema lo informa y no modifica nada. |
| **Postcondición** | El préstamo queda cerrado y el inventario del libro aumentó en una unidad. |

---

## 6. Diagrama de clases

El diagrama refleja la estructura real del código: una clase estática de acceso a datos y
los formularios de la capa de presentación que la consumen.

```mermaid
classDiagram
    class Program {
        <<static>>
        +Main() void
    }

    class Conexion {
        <<static>>
        -CadenaConexion string
        +ObtenerConexion() SqlConnection
    }

    class FrmPrincipal {
        -formularioActivo Form
        -botonActivo IconButton
        -AbrirFormulario(Form, IconButton, string) void
        -ResaltarBoton(IconButton) void
    }

    class FrmInicio
    class FrmAutores {
        -idAutorSeleccionado int
        -CargarAutores() void
        -DatosValidos() bool
    }
    class FrmEditoriales {
        -idEditorialSeleccionada int
        -CargarEditoriales() void
        -DatosValidos() bool
    }
    class FrmUsuarios {
        -idUsuarioSeleccionado int
        -CargarUsuarios() void
        -DocumentoYaRegistrado(string, int) bool
        -DatosValidos() bool
    }
    class FrmLibros {
        -isbnSeleccionado string
        -CargarLibros() void
        -CargarAutores() void
        -CargarEditoriales() void
        -IsbnYaRegistrado(string) bool
        -DatosValidos(int, int) bool
    }
    class FrmPrestamos {
        -idPrestamoSeleccionado int
        -isbnDelPrestamo string
        -estadoDelPrestamo string
        -CargarPrestamos() void
        -RecargarTodo() void
    }
    class FrmReportes {
        -DarNombreAColumnas() void
    }

    Program --> FrmPrincipal : inicia
    FrmPrincipal --> FrmInicio : contiene
    FrmPrincipal --> FrmAutores : contiene
    FrmPrincipal --> FrmEditoriales : contiene
    FrmPrincipal --> FrmUsuarios : contiene
    FrmPrincipal --> FrmLibros : contiene
    FrmPrincipal --> FrmPrestamos : contiene
    FrmPrincipal --> FrmReportes : contiene

    FrmAutores ..> Conexion : usa
    FrmEditoriales ..> Conexion : usa
    FrmUsuarios ..> Conexion : usa
    FrmLibros ..> Conexion : usa
    FrmPrestamos ..> Conexion : usa
    FrmReportes ..> Conexion : usa
```

`FrmPrincipal` no accede a la base de datos: su única responsabilidad es la navegación.
`FrmInicio` tampoco, porque solo presenta información estática.

---

## 7. Modelo entidad-relación

```mermaid
erDiagram
    AUTORES ||--o{ LIBROS : "escribe"
    EDITORIALES ||--o{ LIBROS : "publica"
    USUARIOS ||--o{ PRESTAMOS : "solicita"
    LIBROS ||--o{ PRESTAMOS : "se presta en"

    AUTORES {
        int IdAutor PK
        varchar Nombre
        varchar Apellido
    }
    EDITORIALES {
        int IdEditorial PK
        varchar Nombre
    }
    LIBROS {
        varchar ISBN PK
        varchar Titulo
        int IdAutor FK
        int IdEditorial FK
        varchar Categoria
        int Anio
        int Existencias
    }
    USUARIOS {
        int IdUsuario PK
        varchar Nombre
        varchar Apellido
        varchar Documento UK
        varchar Telefono
        varchar Correo
    }
    PRESTAMOS {
        int IdPrestamo PK
        int IdUsuario FK
        varchar ISBN FK
        date FechaPrestamo
        date FechaDevolucion
        varchar Estado
    }
```

**Relaciones:** las cuatro son de uno a muchos.

| Relación | Cardinalidad | Significado |
|---|---|---|
| Autores → Libros | 1:N | Un autor puede tener varios libros; cada libro tiene un autor |
| Editoriales → Libros | 1:N | Una editorial publica varios libros; cada libro tiene una editorial |
| Usuarios → Préstamos | 1:N | Un usuario puede tener varios préstamos; cada préstamo es de un usuario |
| Libros → Préstamos | 1:N | Un libro puede prestarse varias veces; cada préstamo es de un libro |

> **Decisión de diseño.** La categoría del libro es un campo de texto dentro de `Libros`,
> no una tabla aparte. Con un catálogo de categorías corto y sin atributos propios, una
> sexta tabla añadiría una combinación en cada consulta sin aportar información nueva.

---

## 8. Diccionario de datos

### 8.1 Tabla `Autores`

| Campo | Tipo | Llave | Nulo | Descripción |
|---|---|---|---|---|
| `IdAutor` | INT IDENTITY(1,1) | PK | No | Identificador generado por el motor |
| `Nombre` | VARCHAR(100) | | No | Nombre del autor |
| `Apellido` | VARCHAR(100) | | No | Apellido del autor |

### 8.2 Tabla `Editoriales`

| Campo | Tipo | Llave | Nulo | Descripción |
|---|---|---|---|---|
| `IdEditorial` | INT IDENTITY(1,1) | PK | No | Identificador generado por el motor |
| `Nombre` | VARCHAR(100) | | No | Nombre de la editorial |

### 8.3 Tabla `Libros`

| Campo | Tipo | Llave | Nulo | Descripción |
|---|---|---|---|---|
| `ISBN` | VARCHAR(20) | PK | No | Llave primaria natural; la escribe el usuario |
| `Titulo` | VARCHAR(200) | | No | Título del libro |
| `IdAutor` | INT | FK → `Autores.IdAutor` | No | Autor del libro |
| `IdEditorial` | INT | FK → `Editoriales.IdEditorial` | No | Editorial del libro |
| `Categoria` | VARCHAR(100) | | Sí | Clasificación temática, texto libre |
| `Anio` | INT | | Sí | Año de publicación |
| `Existencias` | INT | | No | Ejemplares disponibles; restricción `CHECK (Existencias >= 0)` |

### 8.4 Tabla `Usuarios`

| Campo | Tipo | Llave | Nulo | Descripción |
|---|---|---|---|---|
| `IdUsuario` | INT IDENTITY(1,1) | PK | No | Identificador generado por el motor |
| `Nombre` | VARCHAR(100) | | No | Nombre del usuario |
| `Apellido` | VARCHAR(100) | | No | Apellido del usuario |
| `Documento` | VARCHAR(20) | UNIQUE | No | Documento de identidad; restricción `UQ_Usuarios_Documento` |
| `Telefono` | VARCHAR(20) | | Sí | Teléfono de contacto |
| `Correo` | VARCHAR(100) | | Sí | Correo electrónico; se valida el formato en la aplicación |

### 8.5 Tabla `Prestamos`

| Campo | Tipo | Llave | Nulo | Descripción |
|---|---|---|---|---|
| `IdPrestamo` | INT IDENTITY(1,1) | PK | No | Identificador generado por el motor |
| `IdUsuario` | INT | FK → `Usuarios.IdUsuario` | No | Usuario que solicita el préstamo |
| `ISBN` | VARCHAR(20) | FK → `Libros.ISBN` | No | Libro prestado |
| `FechaPrestamo` | DATE | | No | Fecha en que se entregó el ejemplar |
| `FechaDevolucion` | DATE | | Sí | Nula mientras el préstamo está activo |
| `Estado` | VARCHAR(20) | | No | «Activo» o «Devuelto» |

### 8.6 Restricciones declaradas

| Nombre | Tabla | Tipo | Propósito |
|---|---|---|---|
| `PK_Autores` | Autores | PRIMARY KEY | Identidad del autor |
| `PK_Editoriales` | Editoriales | PRIMARY KEY | Identidad de la editorial |
| `PK_Libros` | Libros | PRIMARY KEY | Unicidad del ISBN (RN01) |
| `PK_Usuarios` | Usuarios | PRIMARY KEY | Identidad del usuario |
| `PK_Prestamos` | Prestamos | PRIMARY KEY | Identidad del préstamo |
| `FK_Libros_Autores` | Libros | FOREIGN KEY | Todo libro tiene un autor real |
| `FK_Libros_Editoriales` | Libros | FOREIGN KEY | Todo libro tiene una editorial real |
| `FK_Prestamos_Usuarios` | Prestamos | FOREIGN KEY | Todo préstamo tiene un usuario real (RN03) |
| `FK_Prestamos_Libros` | Prestamos | FOREIGN KEY | Todo préstamo tiene un libro real (RN04) |
| `UQ_Usuarios_Documento` | Usuarios | UNIQUE | Documento no repetido (RN09) |
| `CK_Libros_Existencias` | Libros | CHECK | Existencias nunca negativas (RN10) |

---

## 9. Arquitectura del sistema

El proyecto es una única solución de Visual Studio organizada en capas mediante carpetas,
de modo que cada responsabilidad viva en un sitio identificable.

```mermaid
flowchart TD
    A["<b>Capa de presentación</b><br/>FrmPrincipal · FrmInicio · FrmLibros · FrmAutores<br/>FrmEditoriales · FrmUsuarios · FrmPrestamos · FrmReportes"]
    B["<b>Capa de lógica</b><br/>Validaciones y reglas de negocio<br/>en los eventos de cada formulario"]
    C["<b>Capa de acceso a datos</b><br/>Datos/Conexion.cs<br/>única cadena de conexión del sistema"]
    D[("<b>Base de datos</b><br/>SQL Server · Biblioteca<br/>5 tablas")]

    A --> B
    B --> C
    C --> D
```

### 9.1 Capa de acceso a datos

`Datos/Conexion.cs` es una clase estática con dos elementos: una constante privada con la
cadena de conexión y un método `ObtenerConexion()` que devuelve un `SqlConnection` nuevo y
cerrado.

Es la **única** clase del proyecto que conoce la cadena. Los formularios nunca la escriben,
solo la piden. Así, cambiar de servidor implica modificar un archivo en lugar de siete.

Cada conexión se entrega cerrada a propósito: el formulario que la pide la abre dentro de
un bloque `using`, la usa y deja que el `using` la cierre y la libere. Compartir una única
conexión abierta entre formularios provocaría errores en cuanto dos operaciones intentaran
usarla a la vez.

### 9.2 Patrón de acceso a datos

El proyecto aplica el mismo patrón en todos los módulos:

| Operación | Herramienta | Motivo |
|---|---|---|
| Consultas que llenan un `DataGridView` | `SqlDataAdapter` + `DataTable` | El adaptador gestiona la conexión por su cuenta y deja los datos en memoria listos para enlazar |
| `INSERT`, `UPDATE`, `DELETE` | `SqlCommand` + `ExecuteNonQuery()` | Ejecuta la instrucción y devuelve el número de filas afectadas |
| Consultas de un solo valor | `SqlCommand` + `ExecuteScalar()` | Devuelve el primer valor de la primera fila, ideal para un `COUNT(*)` |
| Operaciones sobre dos tablas | `SqlTransaction` | Garantiza que los dos cambios se apliquen juntos o no se aplique ninguno |

**Todas las consultas van parametrizadas.** El texto SQL es fijo y los datos del usuario
viajan aparte como parámetros `@nombre`, de modo que el motor los trata siempre como
valores y nunca como instrucciones:

```csharp
string sql = "INSERT INTO dbo.Autores (Nombre, Apellido) VALUES (@Nombre, @Apellido);";

using (SqlCommand comando = new SqlCommand(sql, conexion))
{
    comando.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
    comando.Parameters.AddWithValue("@Apellido", txtApellido.Text.Trim());

    conexion.Open();
    comando.ExecuteNonQuery();
}
```

### 9.3 Estructura de carpetas

```
/
├── README.md                    Este documento
├── Script_Biblioteca.sql        Script de creación de la base de datos
├── SistemaBiblioteca.sln        Solución de Visual Studio
├── docs/
│   ├── capturas/                Capturas del sistema y evidencias de pruebas
│   └── diagramas/               Diagramas exportados (los del README son Mermaid)
└── SistemaBiblioteca/
    ├── SistemaBiblioteca.csproj
    ├── Program.cs               Punto de entrada
    ├── Datos/
    │   └── Conexion.cs          Capa de acceso a datos
    ├── FrmPrincipal.cs          Ventana contenedora y menú
    ├── FrmInicio.cs
    ├── FrmLibros.cs
    ├── FrmAutores.cs
    ├── FrmEditoriales.cs
    ├── FrmUsuarios.cs
    ├── FrmPrestamos.cs
    └── FrmReportes.cs
```

### 9.4 Tecnologías

| Componente | Versión / herramienta |
|---|---|
| Lenguaje | C# |
| Interfaz | Windows Forms |
| Framework | .NET 10 (`net10.0-windows`) |
| Acceso a datos | ADO.NET con `Microsoft.Data.SqlClient` 7.0.3 |
| Iconos | `FontAwesome.Sharp` 6.6.0 |
| Base de datos | Microsoft SQL Server 2022 |
| IDE | Visual Studio |
| Control de versiones | Git y GitHub |

---

## 10. Módulos del sistema

### 10.1 Ventana principal (`FrmPrincipal`)

Ventana contenedora de la aplicación. Presenta un menú lateral con ocho opciones —Inicio,
Libros, Usuarios, Autores, Editoriales, Préstamos, Reportes y Salida— y aloja los
formularios de cada módulo dentro de un panel, de modo que el sistema trabaja siempre en
una sola ventana.

La opción activa queda resaltada para que el operador sepa en todo momento dónde está, y
la opción **Salida** pide confirmación antes de cerrar la aplicación.

### 10.2 Inicio (`FrmInicio`)

Pantalla de bienvenida. Presenta el sistema y describe brevemente cada módulo del menú.
No accede a la base de datos.

### 10.3 Autores (`FrmAutores`)

CRUD completo sobre la tabla `Autores`. Campos: nombre y apellido; el identificador lo
genera el motor y solo se muestra en la lista.

Botones: **Nuevo, Guardar, Editar, Eliminar, Cancelar**. El doble clic sobre una fila carga
el registro en los campos del formulario.

Si se intenta eliminar un autor que tiene libros registrados, la llave foránea
`FK_Libros_Autores` impide el borrado y el sistema lo explica en lenguaje llano en lugar de
mostrar el error técnico del motor.

### 10.4 Editoriales (`FrmEditoriales`)

CRUD completo sobre la tabla `Editoriales`, con un único campo editable: el nombre. Mismo
conjunto de botones y mismo control de integridad referencial que el módulo de autores,
esta vez a través de `FK_Libros_Editoriales`.

### 10.5 Usuarios (`FrmUsuarios`)

CRUD completo sobre la tabla `Usuarios`. Campos: nombre, apellido, documento, teléfono y
correo. Los tres primeros son obligatorios; teléfono y correo son opcionales y se guardan
como `NULL` cuando se dejan vacíos, para que la base distinguir entre «no tiene teléfono» y
«tiene un teléfono en blanco».

Validaciones propias del módulo:

- **Documento único (RN09).** Antes de guardar, el sistema consulta si el documento ya
  pertenece a otro usuario. Al editar, la consulta excluye la fila del propio usuario; de
  lo contrario, guardar sin cambiar el documento se rechazaría a sí mismo.
- **Formato de correo.** Solo se valida si el usuario escribió algo, porque el campo es
  opcional.

Un usuario con préstamos registrados no puede eliminarse (`FK_Prestamos_Usuarios`).

### 10.6 Libros (`FrmLibros`)

CRUD completo sobre la tabla `Libros`, más búsqueda por ISBN. Campos: ISBN, título, autor,
editorial, categoría, año y existencias.

- **Autor y editorial se eligen en listas desplegables** enlazadas a sus tablas. El control
  muestra el nombre y guarda el identificador, de modo que el operador nunca tiene que
  conocer los códigos internos y las llaves foráneas se respetan siempre.
- **Búsqueda por ISBN.** El botón **Buscar** consulta el ISBN escrito y, si existe, carga
  el libro en el formulario; si no existe, lo informa y conserva el ISBN escrito para poder
  registrarlo enseguida.
- **El ISBN no se puede modificar (RN08).** Al cargar un libro, el campo queda bloqueado y
  se muestra en gris. Para registrar otro libro hay que pulsar **Nuevo**.
- **Validaciones numéricas (RN10).** El año es opcional pero, si se escribe, debe ser un
  número entero. Las existencias son obligatorias, enteras y nunca negativas.

> **Alcance.** La búsqueda implementada es únicamente por ISBN. No se implementó búsqueda
> por título, autor ni categoría.

### 10.7 Préstamos (`FrmPrestamos`)

Módulo donde reside la lógica de negocio del sistema. Cada operación afecta a dos tablas,
por lo que las tres se ejecutan dentro de una **transacción**: o se aplican ambos cambios,
o no se aplica ninguno.

| Acción | Efecto en `Prestamos` | Efecto en `Libros` | Regla |
|---|---|---|---|
| **Guardar** | Inserta el préstamo con estado «Activo» y fecha de devolución nula | Existencias − 1 | RN05, RN06 |
| **Devolver** | Estado → «Devuelto», fecha de devolución = hoy | Existencias + 1 | RN07 |
| **Eliminar** | Borra el préstamo | Existencias + 1 **solo si estaba activo** | RN07 |

Detalles de implementación relevantes:

- La verificación de existencias se hace **dentro** de la transacción, para que el valor
  leído siga siendo válido en el momento de descontarlo.
- La instrucción de devolución incluye `AND Estado = 'Activo'` en su condición: si el
  préstamo ya estuviera devuelto, no se modifica ninguna fila y la transacción se deshace,
  de modo que el ejemplar nunca se reintegra dos veces.
- Eliminar un préstamo ya devuelto no toca las existencias, porque el ejemplar volvió al
  estante en su momento.
- La lista desplegable de libros muestra los ejemplares disponibles entre paréntesis y se
  recarga tras cada operación.

> **Alcance.** Este módulo no incluye la opción de editar un préstamo. Modificar el libro
> de un préstamo activo exigiría ajustar las existencias de dos libros distintos, una regla
> que no forma parte de los requerimientos del sistema.

### 10.8 Reportes (`FrmReportes`)

Consultas de solo lectura. El operador elige un reporte en la lista desplegable y el botón
**Generar** ejecuta la consulta correspondiente; el botón **Limpiar** vacía el resultado.

| Reporte | Tablas combinadas | Columnas |
|---|---|---|
| Préstamos activos | Prestamos + Usuarios + Libros | N.º, usuario, ISBN, título, fecha de préstamo, estado |
| Préstamos devueltos | Prestamos + Usuarios + Libros | Las anteriores más la fecha de devolución |
| Inventario de libros | Libros + Autores + Editoriales | ISBN, título, autor, editorial, categoría, año, existencias |

Los tres usan `INNER JOIN`, porque las llaves foráneas son obligatorias y por tanto ninguna
fila queda fuera de la combinación. Bajo la lista se indica cuántos registros arrojó el
reporte, y si no hay resultados el sistema lo informa en lugar de dejar una tabla vacía sin
explicación.

---

## 11. Instrucciones para ejecutar el proyecto

Esta sección permite clonar, montar y ejecutar el proyecto en una máquina distinta a la de
desarrollo. Al tratarse de una aplicación de escritorio, no hay despliegue en servidor: el
montaje consiste en recrear la base de datos y compilar.

### Paso 1 — Requisitos previos

- **Visual Studio** con la carga de trabajo **«Desarrollo de escritorio de .NET»**.
- **SQL Server** (cualquier edición, incluida Express).
- **SQL Server Management Studio (SSMS)**.

Los paquetes NuGet (`Microsoft.Data.SqlClient` y `FontAwesome.Sharp`) **se restauran solos**
al compilar; no hay que instalarlos a mano.

### Paso 2 — Clonar el repositorio

```bash
git clone [COMPLETAR: URL del repositorio]
```

También puede descargarse el ZIP desde GitHub con **Code → Download ZIP** y descomprimirlo.

### Paso 3 — Crear la base de datos

1. Abrir **SSMS** y conectarse al servidor SQL local.
2. Abrir el archivo `Script_Biblioteca.sql` (**Archivo → Abrir → Archivo**).
3. Ejecutarlo completo con **F5**.

El script crea la base de datos `Biblioteca`, las cinco tablas con sus llaves y
restricciones, y los datos de prueba. Al terminar muestra una tabla con el conteo de
registros por tabla, que debe ser: Autores 8, Editoriales 5, Usuarios 6, Libros 10,
Prestamos 6.

> El script es reejecutable, pero **elimina y recrea las tablas**. Volver a ejecutarlo borra
> todo lo que se haya capturado desde la aplicación.

### Paso 4 — Ajustar la cadena de conexión ⚠️

**Este es el paso que más suele fallar al ejecutar en otra máquina.**

Abrir el archivo `SistemaBiblioteca/Datos/Conexion.cs` y localizar estas líneas:

```csharp
private const string CadenaConexion =
    "Server=LAPTOP-HLBM7AC6;" +      // ← CAMBIAR ESTE VALOR
    "Database=Biblioteca;" +
    "Integrated Security=True;" +
    "TrustServerCertificate=True;";
```

Reemplazar `LAPTOP-HLBM7AC6` por el nombre del servidor SQL propio. Ese nombre es el que
aparece en el campo **«Server name»** al conectarse desde SSMS.

Ejemplos según el tipo de instalación:

| Situación | Valor a escribir |
|---|---|
| Instancia predeterminada | `Server=NOMBRE-DEL-PC;` |
| Instancia con nombre (habitual en SQL Server Express) | `Server=NOMBRE-DEL-PC\SQLEXPRESS;` |
| SQL Server en la misma máquina | `Server=localhost;` |

Las otras tres partes no se modifican: `Database=Biblioteca` es el nombre que crea el
script, `Integrated Security=True` usa la sesión de Windows (no pide usuario ni contraseña)
y `TrustServerCertificate=True` es necesario porque `Microsoft.Data.SqlClient` cifra la
conexión de forma predeterminada y rechazaría el certificado autofirmado de un SQL Server
local.

### Paso 5 — Abrir y compilar

1. Abrir `SistemaBiblioteca.sln` en Visual Studio.
2. Si se solicita, aceptar la restauración de paquetes NuGet.
3. Compilar con **Compilar → Compilar solución** (`Ctrl + Shift + B`).

### Paso 6 — Ejecutar

Pulsar **F5**. La aplicación arranca en `FrmPrincipal`, mostrando el menú lateral y la
pantalla de inicio.

### Paso 7 — Solución de problemas

| Síntoma | Causa probable y solución |
|---|---|
| «No fue posible conectar con la base de datos» al abrir un módulo | El valor de `Server=` no corresponde al servidor local. Revisar el paso 4. |
| «Cannot open database "Biblioteca"» | El script no se ejecutó o falló. Repetir el paso 3 y verificar la tabla de conteos. |
| Error de certificado SSL | Falta `TrustServerCertificate=True` en la cadena de conexión. |
| Los módulos abren vacíos | La base existe pero sin datos. Reejecutar `Script_Biblioteca.sql`. |
| «El archivo se ha bloqueado por SistemaBiblioteca» al compilar | La aplicación sigue en ejecución. Detener la depuración con `Shift + F5`. |

---

## 12. Capturas de pantalla

### 12.1 Menú principal

![Menú principal](docs/capturas/menu.png)

### 12.2 Gestión de libros

![Gestión de libros](docs/capturas/libros.png)

### 12.3 Gestión de autores

![Gestión de autores](docs/capturas/autores.png)

### 12.4 Gestión de editoriales

![Gestión de editoriales](docs/capturas/editoriales.png)

### 12.5 Gestión de usuarios

![Gestión de usuarios](docs/capturas/usuarios.png)

### 12.6 Registro de préstamos

![Registro de préstamos](docs/capturas/prestamos.png)

### 12.7 Reportes

![Reportes](docs/capturas/reportes.png)

---

## 13. Pruebas de funcionamiento

Se ejecutaron nueve casos de prueba sobre el sistema terminado. Cada uno se ejecutó
manualmente y su resultado se verificó contra la base de datos antes de registrar la
evidencia.

### CP01 — Registrar libro válido

**Pasos:** Libros → Nuevo → llenar ISBN, título, autor, editorial, categoría, año y
existencias → Guardar.
**Resultado esperado:** mensaje de éxito y el libro aparece en la lista.
**Resultado obtenido:** correcto.

![CP01 mensaje de éxito](docs/capturas/cp01_libro_ok.png)
![CP01 libro en la lista](docs/capturas/cp01_libro_en_grid.png)

### CP02 — ISBN duplicado

**Pasos:** Libros → Nuevo → escribir un ISBN que ya existe → Guardar.
**Resultado esperado:** mensaje de validación; el libro no se guarda.
**Resultado obtenido:** correcto. Se aplicó RN01.

![CP02 ISBN duplicado](docs/capturas/cp02_isbn_dup.png)

### CP03 — Año no numérico

**Pasos:** Libros → Nuevo → escribir texto en el campo Año → Guardar.
**Resultado esperado:** mensaje de validación; el libro no se guarda.
**Resultado obtenido:** correcto. Se aplicó RN10.

![CP03 validación de año](docs/capturas/cp03_validacion_libro.png)

### CP04 — Registrar usuario válido

**Pasos:** Usuarios → Nuevo → llenar los campos con un correo válido → Guardar.
**Resultado esperado:** mensaje de éxito y el usuario aparece en la lista.
**Resultado obtenido:** correcto.

![CP04 mensaje de éxito](docs/capturas/cp04_usuario_ok.png)
![CP04 usuario en la lista](docs/capturas/cp04_usuario_en_grid.png)

### CP05 — Documento duplicado

**Pasos:** Usuarios → Nuevo → escribir un documento que ya pertenece a otro usuario →
Guardar.
**Resultado esperado:** mensaje de validación; el usuario no se guarda.
**Resultado obtenido:** correcto. Se aplicó RN09.

![CP05 documento duplicado](docs/capturas/cp05_validacion_usuario.png)

### CP06 — Préstamo válido y descuento de existencias

**Pasos:** anotar las existencias del libro → Préstamos → seleccionar usuario y ese libro →
Guardar → volver a Libros y verificar las existencias.
**Resultado esperado:** el préstamo se registra y las existencias bajan en una unidad.
**Resultado obtenido:** correcto. *Cien años de soledad* pasó de 4 a 3 ejemplares. Se
aplicó RN06.

![CP06 existencias antes](docs/capturas/cp06_existencias_antes.png)
![CP06 préstamo registrado](docs/capturas/cp06_prestamo_ok.png)
![CP06 existencias después](docs/capturas/cp06_existencias_despues.png)

### CP07 — Préstamo sin existencias

**Pasos:** Préstamos → seleccionar un libro con cero ejemplares → Guardar.
**Resultado esperado:** el sistema no permite el préstamo e informa al usuario.
**Resultado obtenido:** correcto. Se aplicó RN05.

![CP07 sin existencias](docs/capturas/cp07_sin_existencias.png)

### CP08 — Eliminar préstamo activo devuelve la existencia

**Pasos:** seleccionar un préstamo activo → Eliminar → confirmar → verificar las
existencias del libro.
**Resultado esperado:** el préstamo se elimina y las existencias suben en una unidad.
**Resultado obtenido:** correcto. Se aplicó RN07.

![CP08 confirmación de eliminación](docs/capturas/cp08_devolucion.png)
![CP08 resultado](docs/capturas/cp08_resultado.png)

### CP09 — Reporte generado

**Pasos:** Reportes → seleccionar «Préstamos activos» → Generar.
**Resultado esperado:** la lista muestra el resultado de la combinación entre tablas.
**Resultado obtenido:** correcto.

![CP09 reporte generado](docs/capturas/cp09_reporte.png)

### 13.1 Resumen de resultados

| Caso | Regla verificada | Resultado |
|---|---|---|
| CP01 | RN02 | Aprobado |
| CP02 | RN01 | Aprobado |
| CP03 | RN10 | Aprobado |
| CP04 | RN02 | Aprobado |
| CP05 | RN09 | Aprobado |
| CP06 | RN06 | Aprobado |
| CP07 | RN05 | Aprobado |
| CP08 | RN07 | Aprobado |
| CP09 | RF11 | Aprobado |

---

## 14. Conclusiones

1. **Separar el acceso a datos en una sola clase demostró su valor durante el propio
   desarrollo.** Al centralizar la cadena de conexión en `Conexion.cs`, adaptar el sistema
   a otra máquina se reduce a modificar una línea, en lugar de buscar la cadena repetida en
   siete formularios.

2. **La integridad no puede depender únicamente de la aplicación.** Las reglas críticas se
   implementaron en dos niveles: la aplicación valida antes de enviar los datos para dar un
   mensaje comprensible, y la base de datos los garantiza mediante llaves y restricciones.
   Si algún camino del programa fallara, el motor seguiría rechazando el dato incorrecto.

3. **Las transacciones son indispensables cuando una operación afecta a más de una tabla.**
   Registrar un préstamo implica insertar una fila y descontar un ejemplar. Sin transacción,
   un fallo entre ambas instrucciones dejaría el inventario mintiendo sobre la realidad.

4. **Las consultas parametrizadas son una decisión de diseño, no un detalle.** Mantener el
   texto SQL fijo y enviar los datos del usuario como parámetros impide que un valor escrito
   en un cuadro de texto altere la instrucción, y de paso evita errores con apóstrofos y
   caracteres especiales.

5. **Las listas desplegables enlazadas resuelven las llaves foráneas del lado del usuario.**
   Mostrar el nombre y guardar el identificador permite que el operador trabaje con
   información legible mientras la base recibe exactamente el valor que la relación exige.

---

## 15. Recomendaciones

1. **Agregar un histórico de préstamos por usuario**, que permita consultar el
   comportamiento de un lector concreto sin recorrer el reporte general.
2. **Incorporar fecha de vencimiento y alerta de retraso**, calculando los días
   transcurridos desde la fecha de préstamo para identificar devoluciones fuera de plazo.
3. **Ampliar la búsqueda de libros a título, autor y categoría**, además del ISBN
   actualmente implementado.
4. **Exportar los reportes a PDF o Excel**, de modo que puedan archivarse o entregarse
   fuera de la aplicación.
5. **Incorporar autenticación de operadores** con distintos niveles de permiso, para
   distinguir entre consulta y modificación del catálogo.
6. **Trasladar la cadena de conexión a un archivo de configuración externo**, para no tener
   que recompilar la aplicación al cambiar de servidor.

---

## 16. Referencias

Microsoft. (s. f.). *Desktop Guide (Windows Forms .NET)*. Microsoft Learn.
https://learn.microsoft.com/dotnet/desktop/winforms/

Microsoft. (s. f.). *Microsoft.Data.SqlClient namespace*. Microsoft Learn.
https://learn.microsoft.com/dotnet/api/microsoft.data.sqlclient

Microsoft. (s. f.). *ADO.NET overview*. Microsoft Learn.
https://learn.microsoft.com/dotnet/framework/data/adonet/ado-net-overview

Microsoft. (s. f.). *Local transactions in ADO.NET*. Microsoft Learn.
https://learn.microsoft.com/dotnet/framework/data/adonet/local-transactions

Microsoft. (s. f.). *SQL Server technical documentation*. Microsoft Learn.
https://learn.microsoft.com/sql/sql-server/

Microsoft. (s. f.). *CREATE TABLE (Transact-SQL)*. Microsoft Learn.
https://learn.microsoft.com/sql/t-sql/statements/create-table-transact-sql

Microsoft. (s. f.). *DataGridView Class*. Microsoft Learn.
https://learn.microsoft.com/dotnet/api/system.windows.forms.datagridview

---

## 17. Anexos

### Anexo A — Repositorio

**URL del repositorio público:** `[COMPLETAR: URL del repositorio]`

![Repositorio en GitHub](docs/capturas/git_repo.png)

### Anexo B — Script de base de datos

El script completo se encuentra en la raíz del repositorio:
[`Script_Biblioteca.sql`](Script_Biblioteca.sql)

Contiene, en orden: creación de la base de datos, eliminación previa de tablas para
permitir la reejecución, creación de las cinco tablas con sus llaves y restricciones,
inserción de los datos de prueba y una consulta de verificación.

### Anexo C — Evidencia de commits

El historial de commits documenta la construcción del sistema por bloques: base de datos,
capa de acceso a datos, ventana principal, un commit por cada módulo, capturas y
documentación.

![Historial de commits](docs/capturas/git_commits.png)

### Anexo D — README renderizado

![README en GitHub](docs/capturas/git_readme.png)
