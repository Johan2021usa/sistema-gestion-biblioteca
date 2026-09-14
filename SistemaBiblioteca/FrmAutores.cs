using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SistemaBiblioteca.Datos;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Modulo de Autores: alta, consulta, modificacion y baja sobre la tabla dbo.Autores.
    ///
    /// El IdAutor lo genera SQL Server con IDENTITY, por eso no hay un cuadro de texto
    /// para escribirlo: solo se muestra en la lista y se usa internamente para saber
    /// que fila se esta modificando o eliminando (RN08, no se edita la llave primaria).
    /// </summary>
    public partial class FrmAutores : Form
    {
        /// <summary>
        /// Id del autor cargado en el formulario tras hacer doble clic en la lista.
        /// Vale 0 cuando no hay ningun registro seleccionado, que es la senal que usan
        /// Editar y Eliminar para saber que deben exigir una seleccion.
        /// </summary>
        private int idAutorSeleccionado = 0;

        public FrmAutores()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Al abrir el formulario se da formato a la lista y se cargan los autores.
        /// </summary>
        private void FrmAutores_Load(object sender, EventArgs e)
        {
            AplicarEstiloLista();
            CargarAutores();
            LimpiarCampos();
        }

        // ====================================================================
        // ACCESO A DATOS
        // ====================================================================

        /// <summary>
        /// Consulta todos los autores y los vuelca en el DataGridView.
        ///
        /// Para las consultas de lectura se usa SqlDataAdapter + DataTable: el adaptador
        /// abre y cierra la conexion por su cuenta y deja los datos en una tabla en
        /// memoria que el DataGridView sabe mostrar directamente.
        /// </summary>
        private void CargarAutores()
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT IdAutor, Nombre, Apellido " +
                                 "FROM dbo.Autores " +
                                 "ORDER BY Apellido, Nombre;";

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);
                        dgvAutores.DataSource = tabla;
                    }
                }

                DarNombreAColumnas();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible cargar la lista de autores." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Inserta un autor nuevo. La consulta va parametrizada: los datos que escribe
        /// el usuario viajan como parametros (@Nombre, @Apellido) y nunca se concatenan
        /// dentro del texto SQL, de modo que no pueden alterar la consulta.
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
                    string sql = "INSERT INTO dbo.Autores (Nombre, Apellido) " +
                                 "VALUES (@Nombre, @Apellido);";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                        comando.Parameters.AddWithValue("@Apellido", txtApellido.Text.Trim());

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Autor registrado correctamente.", "Registro exitoso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarAutores();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible guardar el autor." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Actualiza el autor seleccionado. El WHERE usa IdAutor para ubicar la fila,
        /// pero el SET nunca toca esa columna: la llave primaria no se modifica (RN08).
        /// </summary>
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idAutorSeleccionado == 0)
            {
                MessageBox.Show("Seleccione primero un autor de la lista " +
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
                    string sql = "UPDATE dbo.Autores " +
                                 "SET Nombre = @Nombre, Apellido = @Apellido " +
                                 "WHERE IdAutor = @IdAutor;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                        comando.Parameters.AddWithValue("@Apellido", txtApellido.Text.Trim());
                        comando.Parameters.AddWithValue("@IdAutor", idAutorSeleccionado);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Autor actualizado correctamente.", "Actualizacion exitosa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarAutores();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible actualizar el autor." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Elimina el autor seleccionado, previa confirmacion del usuario.
        ///
        /// Si el autor tiene libros registrados, SQL Server rechaza el borrado por la
        /// llave foranea FK_Libros_Autores y devuelve el error 547. Ese caso se atrapa
        /// aparte para explicarselo al usuario en lugar de mostrarle el mensaje tecnico.
        /// </summary>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idAutorSeleccionado == 0)
            {
                MessageBox.Show("Seleccione primero un autor de la lista " +
                                "(doble clic sobre la fila).", "Seleccion requerida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Desea eliminar al autor " + txtNombre.Text.Trim() + " " +
                txtApellido.Text.Trim() + "?",
                "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "DELETE FROM dbo.Autores WHERE IdAutor = @IdAutor;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdAutor", idAutorSeleccionado);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Autor eliminado correctamente.", "Eliminacion exitosa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarAutores();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show("No se puede eliminar este autor porque tiene libros " +
                                    "registrados a su nombre." + Environment.NewLine +
                                    "Elimine primero esos libros en el modulo de Libros.",
                                    "Eliminacion no permitida",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("No fue posible eliminar el autor." + Environment.NewLine +
                                    ex.Message, "Error de base de datos",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ====================================================================
        // VALIDACIONES Y APOYO A LA INTERFAZ
        // ====================================================================

        /// <summary>
        /// Comprueba que los campos obligatorios no esten vacios (RN02).
        /// Si algo falta, avisa y deja el cursor en el campo que hay que corregir.
        /// </summary>
        private bool DatosValidos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del autor es obligatorio.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido del autor es obligatorio.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Nuevo: deja el formulario listo para capturar un autor distinto.
        /// </summary>
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        /// <summary>
        /// Cancelar: descarta lo que se estaba escribiendo. Hace lo mismo que Nuevo.
        /// </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        /// <summary>
        /// Vacia los cuadros de texto, olvida el autor seleccionado y devuelve el
        /// cursor al primer campo.
        /// </summary>
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            idAutorSeleccionado = 0;
            QuitarResaltadoDeLista();
            txtNombre.Focus();
        }

        /// <summary>
        /// Cuando el formulario termina de mostrarse, se vuelve a limpiar el resaltado.
        ///
        /// Hace falta porque el DataGridView reasigna su celda actual en el momento en
        /// que se hace visible, y eso ocurre DESPUES del evento Load: si solo se
        /// limpiara en Load, la primera fila volveria a aparecer marcada.
        /// </summary>
        private void FrmAutores_Shown(object sender, EventArgs e)
        {
            QuitarResaltadoDeLista();
        }

        /// <summary>
        /// Quita el resaltado de la lista.
        ///
        /// Al recibir datos, el DataGridView marca sola la primera fila. Si se dejara
        /// asi, el usuario veria una fila resaltada que el formulario no considera
        /// seleccionada (idAutorSeleccionado seguiria en 0) y al pulsar Editar o
        /// Eliminar recibiria un aviso que no entenderia. CurrentCell es lo que produce
        /// ese resaltado, de modo que se pone en null.
        /// </summary>
        private void QuitarResaltadoDeLista()
        {
            if (dgvAutores.Rows.Count > 0)
            {
                dgvAutores.CurrentCell = null;
            }

            dgvAutores.ClearSelection();
        }

        /// <summary>
        /// Doble clic sobre una fila: pasa los datos de esa fila a los cuadros de texto
        /// y guarda su IdAutor, para que Editar y Eliminar sepan sobre que registro
        /// deben trabajar.
        /// </summary>
        private void dgvAutores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // e.RowIndex es -1 cuando el doble clic cae sobre el encabezado de columna.
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvAutores.Rows[e.RowIndex];

            idAutorSeleccionado = Convert.ToInt32(fila.Cells["IdAutor"].Value);
            txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
            txtApellido.Text = fila.Cells["Apellido"].Value.ToString();
        }

        /// <summary>
        /// Da formato visual a la lista. Se ejecuta una sola vez, al cargar el formulario.
        /// </summary>
        private void AplicarEstiloLista()
        {
            dgvAutores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAutores.GridColor = Color.FromArgb(222, 228, 232);

            // EnableHeadersVisualStyles ya esta en false desde el disenador; sin eso,
            // Windows pinta los encabezados con su tema y estos colores se ignoran.
            dgvAutores.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 76, 92);
            dgvAutores.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAutores.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dgvAutores.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgvAutores.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvAutores.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvAutores.DefaultCellStyle.SelectionBackColor = Color.FromArgb(19, 111, 99);
            dgvAutores.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvAutores.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 245);
        }

        /// <summary>
        /// Cambia los encabezados tecnicos de las columnas por titulos legibles y reparte
        /// el ancho. Hay que llamarlo despues de cada carga, porque al asignar un
        /// DataSource nuevo el DataGridView regenera sus columnas.
        /// </summary>
        private void DarNombreAColumnas()
        {
            if (dgvAutores.Columns.Count == 0)
            {
                return;
            }

            dgvAutores.Columns["IdAutor"].HeaderText = "ID";
            dgvAutores.Columns["Nombre"].HeaderText = "Nombre";
            dgvAutores.Columns["Apellido"].HeaderText = "Apellido";

            // FillWeight reparte proporcionalmente el ancho disponible entre columnas.
            dgvAutores.Columns["IdAutor"].FillWeight = 15;
            dgvAutores.Columns["Nombre"].FillWeight = 42;
            dgvAutores.Columns["Apellido"].FillWeight = 43;
        }
    }
}
