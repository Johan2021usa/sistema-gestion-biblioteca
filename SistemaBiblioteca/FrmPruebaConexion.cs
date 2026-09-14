using System;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SistemaBiblioteca.Datos;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Formulario TEMPORAL de verificacion.
    ///
    /// Su unico objetivo es comprobar, antes de construir los modulos, que la capa de
    /// acceso a datos funciona: que la clase Conexion abre la base Biblioteca y que las
    /// tablas creadas por Script_Biblioteca.sql responden.
    ///
    /// Se elimina del proyecto cuando entre FrmPrincipal.
    /// </summary>
    public partial class FrmPruebaConexion : Form
    {
        public FrmPruebaConexion()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Abre la conexion, consulta datos del servidor y cuenta los registros de las
        /// cinco tablas. Usa el mismo patron que se aplicara en todo el proyecto:
        /// SqlConnection dentro de un 'using', SqlCommand para la consulta y try/catch
        /// con MessageBox para informar al usuario.
        /// </summary>
        private void btnProbar_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder informe = new StringBuilder();

                // El 'using' garantiza que la conexion se cierre y se libere aunque
                // ocurra un error a mitad de la operacion.
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    conexion.Open();

                    informe.AppendLine("Conexion abierta correctamente.");
                    informe.AppendLine("Servidor      : " + conexion.DataSource);
                    informe.AppendLine("Base de datos : " + conexion.Database);
                    informe.AppendLine("Version SQL   : " + conexion.ServerVersion);
                    informe.AppendLine();
                    informe.AppendLine("Registros por tabla:");

                    // Consulta de solo lectura sobre las cinco tablas. No recibe ningun
                    // dato escrito por el usuario, por eso no lleva parametros; en los
                    // modulos siguientes, toda consulta con datos del usuario ira
                    // parametrizada con @parametro.
                    string sql =
                        "SELECT 'Autores' AS Tabla, COUNT(*) AS Registros FROM dbo.Autores " +
                        "UNION ALL SELECT 'Editoriales', COUNT(*) FROM dbo.Editoriales " +
                        "UNION ALL SELECT 'Usuarios',    COUNT(*) FROM dbo.Usuarios " +
                        "UNION ALL SELECT 'Libros',      COUNT(*) FROM dbo.Libros " +
                        "UNION ALL SELECT 'Prestamos',   COUNT(*) FROM dbo.Prestamos;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            informe.AppendLine("  - " + lector["Tabla"].ToString().PadRight(14) +
                                               lector["Registros"].ToString());
                        }
                    }
                }

                txtResultado.Text = informe.ToString();
                MessageBox.Show("La conexion con la base de datos Biblioteca funciona correctamente.",
                                "Conexion exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                // Errores propios de SQL Server: servidor inalcanzable, base inexistente,
                // permisos, tablas que no existen, etc.
                txtResultado.Text = "Error de SQL Server (numero " + ex.Number + "):" +
                                    Environment.NewLine + ex.Message;
                MessageBox.Show("No fue posible conectar con la base de datos." + Environment.NewLine +
                                Environment.NewLine + ex.Message + Environment.NewLine +
                                Environment.NewLine + "Revise el valor de 'Server=' en Datos/Conexion.cs.",
                                "Error de conexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Cualquier otro error no previsto.
                txtResultado.Text = "Error inesperado:" + Environment.NewLine + ex.Message;
                MessageBox.Show("Ocurrio un error inesperado: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
