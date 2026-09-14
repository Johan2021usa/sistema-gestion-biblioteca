using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SistemaBiblioteca.Datos;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Modulo de Prestamos: es donde vive la logica de negocio del sistema.
    ///
    /// A diferencia de los CRUD anteriores, aqui cada operacion toca DOS tablas:
    ///   - Registrar : INSERT en Prestamos  +  Existencias - 1 en Libros   (RN06)
    ///   - Devolver  : Estado = 'Devuelto'  +  Existencias + 1 en Libros   (RN07)
    ///   - Eliminar  : DELETE en Prestamos  +  Existencias + 1 si estaba activo (RN07)
    ///
    /// Por eso las tres van dentro de una TRANSACCION: o se aplican los dos cambios, o
    /// no se aplica ninguno. Si se hicieran sueltos y fallara el segundo, la biblioteca
    /// quedaria con un prestamo registrado y las existencias sin descontar, es decir,
    /// con datos mintiendo sobre la realidad.
    ///
    /// No hay boton Editar: la seccion 5.5 del enunciado no define que significaria
    /// modificar un prestamo, y cambiar el libro obligaria a ajustar las existencias de
    /// dos libros distintos con una regla que nadie especifico.
    /// </summary>
    public partial class FrmPrestamos : Form
    {
        /// <summary>Estado que se asigna a todo prestamo recien registrado (RN06).</summary>
        private const string EstadoActivo = "Activo";

        /// <summary>Estado de un prestamo ya devuelto.</summary>
        private const string EstadoDevuelto = "Devuelto";

        /// <summary>Id del prestamo seleccionado en la lista. Vale 0 si no hay ninguno.</summary>
        private int idPrestamoSeleccionado = 0;

        /// <summary>ISBN del libro de ese prestamo, para poder devolver su existencia.</summary>
        private string isbnDelPrestamo = string.Empty;

        /// <summary>Estado de ese prestamo: de el depende si hay que devolver existencia.</summary>
        private string estadoDelPrestamo = string.Empty;

        public FrmPrestamos()
        {
            InitializeComponent();
        }

        private void FrmPrestamos_Load(object sender, EventArgs e)
        {
            AplicarEstiloLista();
            CargarUsuarios();
            CargarLibros();
            CargarPrestamos();
            LimpiarCampos();
        }

        /// <summary>
        /// El DataGridView reasigna su celda actual al hacerse visible, despues de Load.
        /// </summary>
        private void FrmPrestamos_Shown(object sender, EventArgs e)
        {
            QuitarResaltadoDeLista();
        }

        // ====================================================================
        // CARGA DE COMBOBOX Y LISTA
        // ====================================================================

        /// <summary>
        /// Llena el ComboBox de usuarios. Muestra el nombre completo y guarda el
        /// IdUsuario, que es lo que exige la llave foranea FK_Prestamos_Usuarios.
        /// </summary>
        private void CargarUsuarios()
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT IdUsuario, Nombre + ' ' + Apellido + ' - ' + Documento AS Descripcion " +
                                 "FROM dbo.Usuarios " +
                                 "ORDER BY Apellido, Nombre;";

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);

                        cboUsuario.DataSource = tabla;
                        cboUsuario.DisplayMember = "Descripcion";
                        cboUsuario.ValueMember = "IdUsuario";
                        cboUsuario.SelectedIndex = -1;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible cargar los usuarios." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Llena el ComboBox de libros mostrando, junto al titulo, cuantos ejemplares
        /// quedan disponibles. Asi el usuario ve antes de elegir que un libro esta
        /// agotado, en lugar de descubrirlo al pulsar Guardar.
        ///
        /// Se vuelve a llamar despues de cada operacion porque las existencias cambian.
        /// </summary>
        private void CargarLibros()
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT ISBN, " +
                                 "       Titulo + ' (' + CAST(Existencias AS VARCHAR(10)) + ')' AS Descripcion " +
                                 "FROM dbo.Libros " +
                                 "ORDER BY Titulo;";

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);

                        cboLibro.DataSource = tabla;
                        cboLibro.DisplayMember = "Descripcion";
                        cboLibro.ValueMember = "ISBN";
                        cboLibro.SelectedIndex = -1;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible cargar los libros." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Consulta los prestamos con el nombre del usuario y el titulo del libro.
        /// Se ordena dejando primero los activos, que son los que requieren accion.
        /// </summary>
        private void CargarPrestamos()
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql =
                        "SELECT P.IdPrestamo, " +
                        "       U.Nombre + ' ' + U.Apellido AS Usuario, " +
                        "       P.ISBN, L.Titulo, " +
                        "       P.FechaPrestamo, P.FechaDevolucion, P.Estado " +
                        "FROM dbo.Prestamos P " +
                        "INNER JOIN dbo.Usuarios U ON U.IdUsuario = P.IdUsuario " +
                        "INNER JOIN dbo.Libros L ON L.ISBN = P.ISBN " +
                        "ORDER BY P.Estado, P.FechaPrestamo DESC;";

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);
                        dgvPrestamos.DataSource = tabla;
                    }
                }

                DarNombreAColumnas();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible cargar la lista de prestamos." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // REGISTRAR PRESTAMO  (RN03, RN04, RN05, RN06)
        // ====================================================================

        /// <summary>
        /// Registra un prestamo nuevo.
        ///
        /// Secuencia completa, toda dentro de una transaccion:
        ///   1. Comprobar que el libro existe             -> RN04
        ///   2. Comprobar que le quedan existencias       -> RN05
        ///   3. INSERT en Prestamos con Estado 'Activo'   -> RN06
        ///   4. UPDATE Libros: Existencias - 1            -> RN06
        ///
        /// La comprobacion de existencias se hace DENTRO de la transaccion y no antes,
        /// para que el valor leido siga siendo valido en el momento de descontarlo.
        /// </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // RN03: el prestamo necesita un usuario. El ComboBox solo ofrece usuarios
            // que existen en la tabla, de modo que basta con exigir que haya seleccion.
            if (cboUsuario.SelectedValue == null)
            {
                MessageBox.Show("Seleccione el usuario que solicita el préstamo.",
                                "Dato faltante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboUsuario.Focus();
                return;
            }

            // RN04: lo mismo para el libro.
            if (cboLibro.SelectedValue == null)
            {
                MessageBox.Show("Seleccione el libro que se va a prestar.",
                                "Dato faltante", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLibro.Focus();
                return;
            }

            int idUsuario = Convert.ToInt32(cboUsuario.SelectedValue);
            string isbn = Convert.ToString(cboLibro.SelectedValue);

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    conexion.Open();

                    using (SqlTransaction transaccion = conexion.BeginTransaction())
                    {
                        // --- Paso 1 y 2: el libro existe y tiene ejemplares -----------
                        int existencias;

                        using (SqlCommand consulta = new SqlCommand(
                            "SELECT Existencias FROM dbo.Libros WHERE ISBN = @ISBN;",
                            conexion, transaccion))
                        {
                            consulta.Parameters.AddWithValue("@ISBN", isbn);
                            object resultado = consulta.ExecuteScalar();

                            if (resultado == null)
                            {
                                // RN04: el libro ya no esta en la base (lo borro otro modulo).
                                transaccion.Rollback();
                                MessageBox.Show("El libro seleccionado ya no existe en la base de datos.",
                                                "Libro no encontrado",
                                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                CargarLibros();
                                return;
                            }

                            existencias = Convert.ToInt32(resultado);
                        }

                        if (existencias <= 0)
                        {
                            // RN05: no se presta lo que no hay.
                            transaccion.Rollback();
                            MessageBox.Show("Este libro no tiene ejemplares disponibles." +
                                            Environment.NewLine +
                                            "No es posible registrar el préstamo.",
                                            "Sin existencias",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // --- Paso 3: registrar el prestamo como Activo ---------------
                        using (SqlCommand insercion = new SqlCommand(
                            "INSERT INTO dbo.Prestamos (IdUsuario, ISBN, FechaPrestamo, FechaDevolucion, Estado) " +
                            "VALUES (@IdUsuario, @ISBN, @FechaPrestamo, NULL, @Estado);",
                            conexion, transaccion))
                        {
                            insercion.Parameters.AddWithValue("@IdUsuario", idUsuario);
                            insercion.Parameters.AddWithValue("@ISBN", isbn);
                            insercion.Parameters.AddWithValue("@FechaPrestamo", dtpFechaPrestamo.Value.Date);
                            insercion.Parameters.AddWithValue("@Estado", EstadoActivo);
                            insercion.ExecuteNonQuery();
                        }

                        // --- Paso 4: descontar la existencia ------------------------
                        using (SqlCommand descuento = new SqlCommand(
                            "UPDATE dbo.Libros SET Existencias = Existencias - 1 WHERE ISBN = @ISBN;",
                            conexion, transaccion))
                        {
                            descuento.Parameters.AddWithValue("@ISBN", isbn);
                            descuento.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                    }
                }

                MessageBox.Show("Préstamo registrado correctamente." + Environment.NewLine +
                                "Las existencias del libro se redujeron en 1.",
                                "Registro exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RecargarTodo();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible registrar el préstamo." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // DEVOLVER PRESTAMO  (RN07)
        // ====================================================================

        /// <summary>
        /// Marca como devuelto el prestamo seleccionado y reintegra el ejemplar.
        ///
        /// El UPDATE lleva "AND Estado = 'Activo'" en el WHERE: si el prestamo ya
        /// estuviera devuelto, no se modificaria ninguna fila y la transaccion se
        /// deshace. Asi la existencia nunca se suma dos veces al mismo prestamo.
        /// </summary>
        private void btnDevolver_Click(object sender, EventArgs e)
        {
            if (idPrestamoSeleccionado == 0)
            {
                MessageBox.Show("Seleccione primero un préstamo de la lista " +
                                "(doble clic sobre la fila).", "Seleccion requerida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (estadoDelPrestamo != EstadoActivo)
            {
                MessageBox.Show("Este préstamo ya figura como devuelto.",
                                "Operacion no aplicable",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Registrar la devolución de este préstamo?" + Environment.NewLine +
                "El ejemplar volverá a quedar disponible.",
                "Confirmar devolución", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    conexion.Open();

                    using (SqlTransaction transaccion = conexion.BeginTransaction())
                    {
                        int filasAfectadas;

                        using (SqlCommand cierre = new SqlCommand(
                            "UPDATE dbo.Prestamos " +
                            "SET Estado = @EstadoDevuelto, FechaDevolucion = @Fecha " +
                            "WHERE IdPrestamo = @IdPrestamo AND Estado = @EstadoActivo;",
                            conexion, transaccion))
                        {
                            cierre.Parameters.AddWithValue("@EstadoDevuelto", EstadoDevuelto);
                            cierre.Parameters.AddWithValue("@Fecha", DateTime.Today);
                            cierre.Parameters.AddWithValue("@IdPrestamo", idPrestamoSeleccionado);
                            cierre.Parameters.AddWithValue("@EstadoActivo", EstadoActivo);

                            filasAfectadas = cierre.ExecuteNonQuery();
                        }

                        if (filasAfectadas == 0)
                        {
                            transaccion.Rollback();
                            MessageBox.Show("El préstamo ya no estaba activo. No se modificó nada.",
                                            "Operacion no aplicable",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RecargarTodo();
                            return;
                        }

                        using (SqlCommand reintegro = new SqlCommand(
                            "UPDATE dbo.Libros SET Existencias = Existencias + 1 WHERE ISBN = @ISBN;",
                            conexion, transaccion))
                        {
                            reintegro.Parameters.AddWithValue("@ISBN", isbnDelPrestamo);
                            reintegro.ExecuteNonQuery();
                        }

                        transaccion.Commit();
                    }
                }

                MessageBox.Show("Devolución registrada correctamente." + Environment.NewLine +
                                "Las existencias del libro aumentaron en 1.",
                                "Devolución exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RecargarTodo();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible registrar la devolución." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // ELIMINAR PRESTAMO  (RN07)
        // ====================================================================

        /// <summary>
        /// Elimina el prestamo seleccionado.
        ///
        /// Si el prestamo estaba activo, el ejemplar seguia contado como prestado, asi
        /// que hay que reintegrarlo (RN07). Si ya estaba devuelto, la existencia se
        /// reintegro en su momento y no se toca: sumarla otra vez inventaria un libro.
        /// </summary>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idPrestamoSeleccionado == 0)
            {
                MessageBox.Show("Seleccione primero un préstamo de la lista " +
                                "(doble clic sobre la fila).", "Seleccion requerida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool estabaActivo = (estadoDelPrestamo == EstadoActivo);

            string aviso = "¿Desea eliminar el préstamo número " + idPrestamoSeleccionado + "?";
            if (estabaActivo)
            {
                aviso += Environment.NewLine +
                         "Como está activo, el ejemplar volverá a quedar disponible.";
            }

            DialogResult respuesta = MessageBox.Show(aviso, "Confirmar eliminación",
                                                     MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    conexion.Open();

                    using (SqlTransaction transaccion = conexion.BeginTransaction())
                    {
                        using (SqlCommand borrado = new SqlCommand(
                            "DELETE FROM dbo.Prestamos WHERE IdPrestamo = @IdPrestamo;",
                            conexion, transaccion))
                        {
                            borrado.Parameters.AddWithValue("@IdPrestamo", idPrestamoSeleccionado);
                            borrado.ExecuteNonQuery();
                        }

                        if (estabaActivo)
                        {
                            using (SqlCommand reintegro = new SqlCommand(
                                "UPDATE dbo.Libros SET Existencias = Existencias + 1 WHERE ISBN = @ISBN;",
                                conexion, transaccion))
                            {
                                reintegro.Parameters.AddWithValue("@ISBN", isbnDelPrestamo);
                                reintegro.ExecuteNonQuery();
                            }
                        }

                        transaccion.Commit();
                    }
                }

                string mensaje = "Préstamo eliminado correctamente.";
                if (estabaActivo)
                {
                    mensaje += Environment.NewLine + "Las existencias del libro aumentaron en 1.";
                }

                MessageBox.Show(mensaje, "Eliminación exitosa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                RecargarTodo();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible eliminar el préstamo." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // APOYO A LA INTERFAZ
        // ====================================================================

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        /// <summary>
        /// Recarga lista y ComboBox tras una operacion. Los libros se recargan siempre
        /// porque su numero de ejemplares disponibles acaba de cambiar.
        /// </summary>
        private void RecargarTodo()
        {
            CargarLibros();
            CargarPrestamos();
            LimpiarCampos();
        }

        /// <summary>
        /// Deja el formulario listo para registrar otro prestamo: sin selecciones, con
        /// la fecha de hoy y sin ningun prestamo marcado.
        /// </summary>
        private void LimpiarCampos()
        {
            cboUsuario.SelectedIndex = -1;
            cboLibro.SelectedIndex = -1;

            // La fecha del prestamo es hoy por defecto, que es el caso normal.
            dtpFechaPrestamo.Value = DateTime.Today;

            idPrestamoSeleccionado = 0;
            isbnDelPrestamo = string.Empty;
            estadoDelPrestamo = string.Empty;

            QuitarResaltadoDeLista();
            cboUsuario.Focus();
        }

        /// <summary>
        /// Quita el resaltado que el DataGridView pone sobre la primera fila al recibir
        /// datos, para que lo que se ve coincida con lo que el formulario considera
        /// seleccionado.
        /// </summary>
        private void QuitarResaltadoDeLista()
        {
            if (dgvPrestamos.Rows.Count > 0)
            {
                dgvPrestamos.CurrentCell = null;
            }

            dgvPrestamos.ClearSelection();
        }

        /// <summary>
        /// Doble clic sobre una fila: guarda el prestamo seleccionado.
        ///
        /// Se retienen tres datos: el Id (para el UPDATE o el DELETE), el ISBN (para
        /// saber a que libro reintegrar el ejemplar) y el Estado (para saber si hay que
        /// reintegrarlo). Los ComboBox tambien se posicionan, a modo informativo.
        /// </summary>
        private void dgvPrestamos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvPrestamos.Rows[e.RowIndex];

            idPrestamoSeleccionado = Convert.ToInt32(fila.Cells["IdPrestamo"].Value);
            isbnDelPrestamo = Convert.ToString(fila.Cells["ISBN"].Value);
            estadoDelPrestamo = Convert.ToString(fila.Cells["Estado"].Value);

            cboLibro.SelectedValue = isbnDelPrestamo;
            dtpFechaPrestamo.Value = Convert.ToDateTime(fila.Cells["FechaPrestamo"].Value);
        }

        /// <summary>
        /// Da formato visual a la lista. Se ejecuta una sola vez, al cargar el formulario.
        /// </summary>
        private void AplicarEstiloLista()
        {
            dgvPrestamos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPrestamos.GridColor = Color.FromArgb(222, 228, 232);

            dgvPrestamos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 76, 92);
            dgvPrestamos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPrestamos.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dgvPrestamos.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgvPrestamos.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvPrestamos.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvPrestamos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(19, 111, 99);
            dgvPrestamos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvPrestamos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 245);
        }

        /// <summary>
        /// Pone titulos legibles a las columnas, formatea las fechas como dd/MM/yyyy y
        /// reparte el ancho disponible.
        /// </summary>
        private void DarNombreAColumnas()
        {
            if (dgvPrestamos.Columns.Count == 0)
            {
                return;
            }

            dgvPrestamos.Columns["IdPrestamo"].HeaderText = "N°";
            dgvPrestamos.Columns["Usuario"].HeaderText = "Usuario";
            dgvPrestamos.Columns["ISBN"].HeaderText = "ISBN";
            dgvPrestamos.Columns["Titulo"].HeaderText = "Libro";
            dgvPrestamos.Columns["FechaPrestamo"].HeaderText = "Préstamo";
            dgvPrestamos.Columns["FechaDevolucion"].HeaderText = "Devolución";
            dgvPrestamos.Columns["Estado"].HeaderText = "Estado";

            dgvPrestamos.Columns["IdPrestamo"].FillWeight = 7;
            dgvPrestamos.Columns["Usuario"].FillWeight = 19;
            dgvPrestamos.Columns["ISBN"].FillWeight = 18;
            dgvPrestamos.Columns["Titulo"].FillWeight = 24;
            dgvPrestamos.Columns["FechaPrestamo"].FillWeight = 12;
            dgvPrestamos.Columns["FechaDevolucion"].FillWeight = 12;
            dgvPrestamos.Columns["Estado"].FillWeight = 12;

            // Las columnas DATE llegan como DateTime; sin formato se mostraria tambien
            // la hora 00:00:00, que aqui no aporta nada.
            dgvPrestamos.Columns["FechaPrestamo"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvPrestamos.Columns["FechaDevolucion"].DefaultCellStyle.Format = "dd/MM/yyyy";

            dgvPrestamos.Columns["IdPrestamo"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvPrestamos.Columns["FechaPrestamo"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvPrestamos.Columns["FechaDevolucion"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
        }
    }
}
