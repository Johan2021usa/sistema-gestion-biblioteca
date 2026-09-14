using Microsoft.Data.SqlClient;

namespace SistemaBiblioteca.Datos
{
    /// <summary>
    /// Capa de acceso a datos del sistema.
    ///
    /// Esta es la UNICA clase del proyecto que conoce la cadena de conexion a SQL Server.
    /// Todos los formularios piden aqui su conexion en lugar de escribir la cadena por su
    /// cuenta; de ese modo, si cambia el servidor o el nombre de la base de datos, solo
    /// hay que tocar un archivo.
    ///
    /// La clase es estatica porque no guarda estado: su unica responsabilidad es fabricar
    /// objetos SqlConnection ya configurados.
    /// </summary>
    public static class Conexion
    {
        // =====================================================================
        // AJUSTAR: cambiar 'Server=' por el nombre del servidor SQL local.
        //
        // Es el valor que aparece en el campo "Server name" al conectarse desde
        // SQL Server Management Studio. Si la instancia no es la predeterminada,
        // se escribe con barra invertida, por ejemplo: Server=MI-PC\SQLEXPRESS
        //
        // Integrated Security=True  -> autenticacion de Windows (no lleva usuario ni clave).
        // TrustServerCertificate=True -> acepta el certificado local de SQL Server;
        //                                sin esto, Microsoft.Data.SqlClient rechaza la
        //                                conexion porque cifra el canal de forma obligatoria.
        // =====================================================================
        private const string CadenaConexion =
            "Server=LAPTOP-HLBM7AC6;" +
            "Database=Biblioteca;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True;";

        /// <summary>
        /// Devuelve una conexion NUEVA y cerrada hacia la base de datos Biblioteca.
        ///
        /// Se entrega cerrada a proposito: cada formulario la abre dentro de un bloque
        /// 'using', la usa y deja que el 'using' la cierre y la libere. Compartir una
        /// sola conexion abierta entre formularios provocaria errores cuando dos
        /// operaciones intentaran usarla al mismo tiempo.
        /// </summary>
        /// <returns>Un SqlConnection listo para abrir.</returns>
        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(CadenaConexion);
        }
    }
}
