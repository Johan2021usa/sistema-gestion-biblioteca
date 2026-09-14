using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SistemaBiblioteca.Datos;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Modulo de Reportes: consultas de solo lectura sobre la informacion ya registrada.
    ///
    /// El usuario elige uno de tres reportes en el ComboBox y el boton Generar ejecuta
    /// la consulta que corresponda. Los tres se apoyan en JOIN, porque la informacion
    /// util esta repartida entre varias tablas: un prestamo guarda IdUsuario e ISBN, no
    /// el nombre de la persona ni el titulo del libro.
    ///
    /// Este formulario nunca modifica datos: solo consulta.
    /// </summary>
    public partial class FrmReportes : Form
    {
        // Los indices del ComboBox se nombran como constantes para que el switch se lea
        // solo, en lugar de comparar contra 0, 1 y 2 sueltos.
        private const int ReportePrestamosActivos = 0;
        private const int ReportePrestamosDevueltos = 1;
        private const int ReporteInventario = 2;

        private const string EstadoActivo = "Activo";
        private const string EstadoDevuelto = "Devuelto";

        public FrmReportes()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Carga las tres opciones de reporte. La lista es fija: no sale de la base de
        /// datos, porque no son datos sino funciones del programa.
        /// </summary>
        private void FrmReportes_Load(object sender, EventArgs e)
        {
            AplicarEstiloLista();

            cboReporte.Items.Add("1. Préstamos activos");
            cboReporte.Items.Add("2. Préstamos devueltos");
            cboReporte.Items.Add("3. Inventario de libros");
            cboReporte.SelectedIndex = -1;

            LimpiarReporte();
        }

        // ====================================================================
        // GENERAR EL REPORTE
        // ====================================================================

        /// <summary>
        /// Ejecuta el reporte elegido y vuelca el resultado en el DataGridView.
        ///
        /// La estructura es siempre la misma: se decide que consulta toca, se ejecuta
        /// con SqlDataAdapter y el DataTable resultante se enlaza al grid. Lo unico que
        /// cambia entre reportes es el texto SQL y sus parametros.
        /// </summary>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (cboReporte.SelectedIndex < 0)
            {
                MessageBox.Show("Seleccione el tipo de reporte que desea generar.",
                                "Dato faltante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboReporte.Focus();
                return;
            }

            string sql;
            string estado = null;

            switch (cboReporte.SelectedIndex)
            {
                case ReportePrestamosActivos:
                    // Reporte 1: quien tiene ahora mismo un libro prestado.
                    sql = "SELECT P.IdPrestamo, " +
                          "       U.Nombre + ' ' + U.Apellido AS Usuario, " +
                          "       P.ISBN, L.Titulo, P.FechaPrestamo, P.Estado " +
                          "FROM dbo.Prestamos P " +
                          "INNER JOIN dbo.Usuarios U ON U.IdUsuario = P.IdUsuario " +
                          "INNER JOIN dbo.Libros L ON L.ISBN = P.ISBN " +
                          "WHERE P.Estado = @Estado " +
                          "ORDER BY P.FechaPrestamo DESC;";
                    estado = EstadoActivo;
                    break;

                case ReportePrestamosDevueltos:
                    // Reporte 2: igual que el anterior, pero anadiendo la fecha en que
                    // se devolvio el ejemplar.
                    sql = "SELECT P.IdPrestamo, " +
                          "       U.Nombre + ' ' + U.Apellido AS Usuario, " +
                          "       P.ISBN, L.Titulo, P.FechaPrestamo, P.FechaDevolucion, P.Estado " +
                          "FROM dbo.Prestamos P " +
                          "INNER JOIN dbo.Usuarios U ON U.IdUsuario = P.IdUsuario " +
                          "INNER JOIN dbo.Libros L ON L.ISBN = P.ISBN " +
                          "WHERE P.Estado = @Estado " +
                          "ORDER BY P.FechaDevolucion DESC;";
                    estado = EstadoDevuelto;
                    break;

                case ReporteInventario:
                    // Reporte 3: el catalogo completo con autor y editorial resueltos.
                    sql = "SELECT L.ISBN, L.Titulo, " +
                          "       A.Nombre + ' ' + A.Apellido AS Autor, " +
                          "       E.Nombre AS Editorial, " +
                          "       L.Categoria, L.Anio, L.Existencias " +
                          "FROM dbo.Libros L " +
                          "INNER JOIN dbo.Autores A ON A.IdAutor = L.IdAutor " +
                          "INNER JOIN dbo.Editoriales E ON E.IdEditorial = L.IdEditorial " +
                          "ORDER BY L.Titulo;";
                    break;

                default:
                    return;
            }

            try
            {
                DataTable tabla = new DataTable();

                using (SqlConnection conexion = Conexion.ObtenerConexion())
                using (SqlCommand comando = new SqlCommand(sql, conexion))
                {
                    // Los dos primeros reportes filtran por estado; el inventario no
                    // lleva parametros porque no filtra nada.
                    if (estado != null)
                    {
                        comando.Parameters.AddWithValue("@Estado", estado);
                    }

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                    {
                        adaptador.Fill(tabla);
                    }
                }

                dgvReporte.DataSource = tabla;
                DarNombreAColumnas();
                QuitarResaltadoDeLista();

                lblLista.Text = "Resultado: " + cboReporte.SelectedItem.ToString();

                if (tabla.Rows.Count == 0)
                {
                    lblResumen.Text = "El reporte no arrojó ningún registro.";
                    MessageBox.Show("El reporte se ejecutó correctamente, pero no hay " +
                                    "registros que cumplan esa condición.",
                                    "Reporte sin resultados",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (tabla.Rows.Count == 1)
                {
                    lblResumen.Text = "Se encontró 1 registro.";
                }
                else
                {
                    lblResumen.Text = "Se encontraron " + tabla.Rows.Count + " registros.";
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible generar el reporte." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Limpia el resultado para dejar la pantalla lista para otra consulta.
        /// </summary>
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarReporte();
        }

        /// <summary>
        /// Vacia el grid, el resumen y la seleccion del ComboBox.
        ///
        /// Se pone DataSource en null y ademas se limpian las columnas: si solo se
        /// quitara el origen de datos, los encabezados del reporte anterior seguirian
        /// visibles y darian la impresion de que aun hay un resultado en pantalla.
        /// </summary>
        private void LimpiarReporte()
        {
            dgvReporte.DataSource = null;
            dgvReporte.Columns.Clear();

            cboReporte.SelectedIndex = -1;
            lblResumen.Text = string.Empty;
            lblLista.Text = "Resultado del reporte";

            cboReporte.Focus();
        }

        /// <summary>
        /// Quita el resaltado automatico de la primera fila.
        /// </summary>
        private void QuitarResaltadoDeLista()
        {
            if (dgvReporte.Rows.Count > 0)
            {
                dgvReporte.CurrentCell = null;
            }

            dgvReporte.ClearSelection();
        }

        /// <summary>
        /// Da formato visual a la lista. Se ejecuta una sola vez, al cargar el formulario.
        /// </summary>
        private void AplicarEstiloLista()
        {
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReporte.GridColor = Color.FromArgb(222, 228, 232);

            dgvReporte.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 76, 92);
            dgvReporte.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvReporte.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dgvReporte.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgvReporte.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvReporte.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvReporte.DefaultCellStyle.SelectionBackColor = Color.FromArgb(19, 111, 99);
            dgvReporte.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvReporte.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 245);
        }

        /// <summary>
        /// Pone titulos legibles a las columnas del reporte que se acaba de generar.
        ///
        /// Los tres reportes devuelven conjuntos de columnas distintos, asi que en lugar
        /// de escribir un metodo por reporte se recorre lo que haya llegado y se traduce
        /// cada nombre conocido. Las columnas que no aparecen en un reporte concreto
        /// simplemente no entran en el recorrido.
        /// </summary>
        private void DarNombreAColumnas()
        {
            foreach (DataGridViewColumn columna in dgvReporte.Columns)
            {
                switch (columna.Name)
                {
                    case "IdPrestamo":
                        columna.HeaderText = "N°";
                        columna.FillWeight = 7;
                        columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;

                    case "Usuario":
                        columna.HeaderText = "Usuario";
                        columna.FillWeight = 20;
                        break;

                    case "ISBN":
                        columna.HeaderText = "ISBN";
                        columna.FillWeight = 18;
                        break;

                    case "Titulo":
                        columna.HeaderText = "Título";
                        columna.FillWeight = 25;
                        break;

                    case "FechaPrestamo":
                        columna.HeaderText = "Préstamo";
                        columna.FillWeight = 13;
                        columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                        columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;

                    case "FechaDevolucion":
                        columna.HeaderText = "Devolución";
                        columna.FillWeight = 13;
                        columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                        columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;

                    case "Estado":
                        columna.HeaderText = "Estado";
                        columna.FillWeight = 12;
                        break;

                    case "Autor":
                        columna.HeaderText = "Autor";
                        columna.FillWeight = 18;
                        break;

                    case "Editorial":
                        columna.HeaderText = "Editorial";
                        columna.FillWeight = 16;
                        break;

                    case "Categoria":
                        columna.HeaderText = "Categoría";
                        columna.FillWeight = 12;
                        break;

                    case "Anio":
                        columna.HeaderText = "Año";
                        columna.FillWeight = 7;
                        columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;

                    case "Existencias":
                        columna.HeaderText = "Existencias";
                        columna.FillWeight = 14;
                        columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;
                }
            }
        }
    }
}
