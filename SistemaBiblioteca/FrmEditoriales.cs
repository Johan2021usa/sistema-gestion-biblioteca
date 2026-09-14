using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SistemaBiblioteca.Datos;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Modulo de Editoriales: alta, consulta, modificacion y baja sobre dbo.Editoriales.
    ///
    /// Es el CRUD mas sencillo del sistema porque la tabla solo tiene un campo editable
    /// (Nombre); el IdEditorial lo genera SQL Server con IDENTITY y no se modifica (RN08).
    /// </summary>
    public partial class FrmEditoriales : Form
    {
        /// <summary>
        /// Id de la editorial cargada tras hacer doble clic en la lista. Vale 0 cuando
        /// no hay nada seleccionado.
        /// </summary>
        private int idEditorialSeleccionada = 0;

        public FrmEditoriales()
        {
            InitializeComponent();
        }

        private void FrmEditoriales_Load(object sender, EventArgs e)
        {
            AplicarEstiloLista();
            CargarEditoriales();
            LimpiarCampos();
        }

        /// <summary>
        /// El DataGridView reasigna su celda actual al hacerse visible, despues de Load,
        /// por eso el resaltado se limpia tambien aqui.
        /// </summary>
        private void FrmEditoriales_Shown(object sender, EventArgs e)
        {
            QuitarResaltadoDeLista();
        }

        // ====================================================================
        // ACCESO A DATOS
        // ====================================================================

        /// <summary>
        /// Consulta todas las editoriales y las muestra en el DataGridView.
        /// </summary>
        private void CargarEditoriales()
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT IdEditorial, Nombre " +
                                 "FROM dbo.Editoriales " +
                                 "ORDER BY Nombre;";

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);
                        dgvEditoriales.DataSource = tabla;
                    }
                }

                DarNombreAColumnas();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible cargar la lista de editoriales." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Inserta una editorial nueva con la consulta parametrizada.
        /// </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!DatosValidos())
            {
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "INSERT INTO dbo.Editoriales (Nombre) VALUES (@Nombre);";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Editorial registrada correctamente.", "Registro exitoso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarEditoriales();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible guardar la editorial." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Actualiza la editorial seleccionada. El IdEditorial solo aparece en el WHERE,
        /// nunca en el SET: la llave primaria no se edita (RN08).
        /// </summary>
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idEditorialSeleccionada == 0)
            {
                MessageBox.Show("Seleccione primero una editorial de la lista " +
                                "(doble clic sobre la fila).", "Seleccion requerida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DatosValidos())
            {
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "UPDATE dbo.Editoriales " +
                                 "SET Nombre = @Nombre " +
                                 "WHERE IdEditorial = @IdEditorial;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                        comando.Parameters.AddWithValue("@IdEditorial", idEditorialSeleccionada);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Editorial actualizada correctamente.", "Actualizacion exitosa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarEditoriales();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible actualizar la editorial." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Elimina la editorial seleccionada, previa confirmacion.
        ///
        /// Si la editorial tiene libros publicados, la llave foranea
        /// FK_Libros_Editoriales impide el borrado y SQL Server devuelve el error 547,
        /// que se traduce a un mensaje comprensible para el usuario.
        /// </summary>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idEditorialSeleccionada == 0)
            {
                MessageBox.Show("Seleccione primero una editorial de la lista " +
                                "(doble clic sobre la fila).", "Seleccion requerida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Desea eliminar la editorial " + txtNombre.Text.Trim() + "?",
                "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "DELETE FROM dbo.Editoriales WHERE IdEditorial = @IdEditorial;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdEditorial", idEditorialSeleccionada);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Editorial eliminada correctamente.", "Eliminacion exitosa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarEditoriales();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show("No se puede eliminar esta editorial porque tiene libros " +
                                    "registrados." + Environment.NewLine +
                                    "Elimine primero esos libros en el modulo de Libros.",
                                    "Eliminacion no permitida",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("No fue posible eliminar la editorial." + Environment.NewLine +
                                    ex.Message, "Error de base de datos",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ====================================================================
        // VALIDACIONES Y APOYO A LA INTERFAZ
        // ====================================================================

        /// <summary>
        /// Comprueba que el nombre no este vacio (RN02).
        /// </summary>
        private bool DatosValidos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre de la editorial es obligatorio.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        /// <summary>
        /// Vacia el campo, olvida la editorial seleccionada y devuelve el cursor al
        /// cuadro de texto.
        /// </summary>
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            idEditorialSeleccionada = 0;
            QuitarResaltadoDeLista();
            txtNombre.Focus();
        }

        /// <summary>
        /// Quita el resaltado que el DataGridView pone sobre la primera fila al recibir
        /// datos, para que lo que se ve coincida con lo que el formulario considera
        /// seleccionado.
        /// </summary>
        private void QuitarResaltadoDeLista()
        {
            if (dgvEditoriales.Rows.Count > 0)
            {
                dgvEditoriales.CurrentCell = null;
            }

            dgvEditoriales.ClearSelection();
        }

        /// <summary>
        /// Doble clic sobre una fila: carga esa editorial en el formulario y guarda su
        /// Id para que Editar y Eliminar sepan sobre que registro trabajar.
        /// </summary>
        private void dgvEditoriales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvEditoriales.Rows[e.RowIndex];

            idEditorialSeleccionada = Convert.ToInt32(fila.Cells["IdEditorial"].Value);
            txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
        }

        /// <summary>
        /// Da formato visual a la lista. Se ejecuta una sola vez, al cargar el formulario.
        /// </summary>
        private void AplicarEstiloLista()
        {
            dgvEditoriales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEditoriales.GridColor = Color.FromArgb(222, 228, 232);

            dgvEditoriales.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 76, 92);
            dgvEditoriales.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEditoriales.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dgvEditoriales.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgvEditoriales.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvEditoriales.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvEditoriales.DefaultCellStyle.SelectionBackColor = Color.FromArgb(19, 111, 99);
            dgvEditoriales.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvEditoriales.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 245);
        }

        /// <summary>
        /// Pone titulos legibles a las columnas y reparte el ancho. Se llama tras cada
        /// carga porque al asignar un DataSource nuevo el grid regenera sus columnas.
        /// </summary>
        private void DarNombreAColumnas()
        {
            if (dgvEditoriales.Columns.Count == 0)
            {
                return;
            }

            dgvEditoriales.Columns["IdEditorial"].HeaderText = "ID";
            dgvEditoriales.Columns["Nombre"].HeaderText = "Nombre de la editorial";

            dgvEditoriales.Columns["IdEditorial"].FillWeight = 15;
            dgvEditoriales.Columns["Nombre"].FillWeight = 85;
        }
    }
}
