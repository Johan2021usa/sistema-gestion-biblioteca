using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SistemaBiblioteca.Datos;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Modulo de Libros: el CRUD mas completo del sistema.
    ///
    /// Aporta tres cosas que los modulos anteriores no tenian:
    ///   - Dos ComboBox enlazados a las tablas Autores y Editoriales, para respetar las
    ///     llaves foraneas sin obligar al usuario a conocer los Id.
    ///   - Busqueda por ISBN.
    ///   - Validaciones numericas de Anio y Existencias (RN10).
    ///
    /// El ISBN es la llave primaria natural: lo escribe el usuario al registrar el libro,
    /// pero una vez guardado no se puede cambiar (RN08).
    /// </summary>
    public partial class FrmLibros : Form
    {
        /// <summary>
        /// ISBN del libro que esta cargado en el formulario. Cadena vacia cuando no hay
        /// ningun libro seleccionado; es la senal que usan Editar y Eliminar para exigir
        /// una seleccion previa.
        /// </summary>
        private string isbnSeleccionado = string.Empty;

        public FrmLibros()
        {
            InitializeComponent();
        }

        private void FrmLibros_Load(object sender, EventArgs e)
        {
            AplicarEstiloLista();
            CargarAutores();
            CargarEditoriales();
            CargarLibros();
            LimpiarCampos();
        }

        /// <summary>
        /// El DataGridView reasigna su celda actual al hacerse visible, despues de Load.
        /// </summary>
        private void FrmLibros_Shown(object sender, EventArgs e)
        {
            QuitarResaltadoDeLista();
        }

        // ====================================================================
        // CARGA DE LOS COMBOBOX (llaves foraneas)
        // ====================================================================

        /// <summary>
        /// Llena el ComboBox de autores.
        ///
        /// DisplayMember indica que texto ve el usuario (el nombre completo) y
        /// ValueMember que dato guarda por debajo (el IdAutor). Asi el formulario
        /// muestra "Gabriel Garcia Marquez" pero envia el 1 a la columna IdAutor, que es
        /// lo que exige la llave foranea FK_Libros_Autores.
        ///
        /// El nombre completo se arma en la consulta con una concatenacion de columnas
        /// de la propia base; no interviene ningun dato escrito por el usuario.
        /// </summary>
        private void CargarAutores()
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT IdAutor, Nombre + ' ' + Apellido AS NombreCompleto " +
                                 "FROM dbo.Autores " +
                                 "ORDER BY Apellido, Nombre;";

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);

                        cboAutor.DataSource = tabla;
                        cboAutor.DisplayMember = "NombreCompleto";
                        cboAutor.ValueMember = "IdAutor";
                        cboAutor.SelectedIndex = -1;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible cargar los autores." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Llena el ComboBox de editoriales con la misma tecnica que el de autores.
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

                        cboEditorial.DataSource = tabla;
                        cboEditorial.DisplayMember = "Nombre";
                        cboEditorial.ValueMember = "IdEditorial";
                        cboEditorial.SelectedIndex = -1;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible cargar las editoriales." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // ACCESO A DATOS
        // ====================================================================

        /// <summary>
        /// Consulta los libros junto con el nombre de su autor y su editorial.
        ///
        /// Se usa INNER JOIN porque IdAutor e IdEditorial son obligatorios: todo libro
        /// tiene siempre autor y editorial, de modo que ninguna fila se pierde. Los Id
        /// tambien se traen, aunque luego se ocultan, porque el doble clic los necesita
        /// para posicionar los ComboBox.
        /// </summary>
        private void CargarLibros()
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql =
                        "SELECT L.ISBN, L.Titulo, " +
                        "       L.IdAutor, A.Nombre + ' ' + A.Apellido AS Autor, " +
                        "       L.IdEditorial, E.Nombre AS Editorial, " +
                        "       L.Categoria, L.Anio, L.Existencias " +
                        "FROM dbo.Libros L " +
                        "INNER JOIN dbo.Autores A ON A.IdAutor = L.IdAutor " +
                        "INNER JOIN dbo.Editoriales E ON E.IdEditorial = L.IdEditorial " +
                        "ORDER BY L.Titulo;";

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);
                        dgvLibros.DataSource = tabla;
                    }
                }

                DarNombreAColumnas();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible cargar la lista de libros." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Registra un libro nuevo, despues de comprobar que el ISBN no exista (RN01).
        /// </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            int anio;
            int existencias;

            if (!DatosValidos(out anio, out existencias))
            {
                return;
            }

            if (IsbnYaRegistrado(txtISBN.Text.Trim()))
            {
                MessageBox.Show("Ya existe un libro registrado con el ISBN " +
                                txtISBN.Text.Trim() + "." + Environment.NewLine +
                                "El ISBN es la llave del libro y no se puede repetir.",
                                "ISBN duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtISBN.Focus();
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql =
                        "INSERT INTO dbo.Libros (ISBN, Titulo, IdAutor, IdEditorial, Categoria, Anio, Existencias) " +
                        "VALUES (@ISBN, @Titulo, @IdAutor, @IdEditorial, @Categoria, @Anio, @Existencias);";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@ISBN", txtISBN.Text.Trim());
                        AgregarParametrosDelLibro(comando, anio, existencias);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Libro registrado correctamente.", "Registro exitoso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarLibros();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MostrarErrorDeGuardado(ex, "guardar");
            }
        }

        /// <summary>
        /// Actualiza el libro cargado en el formulario.
        ///
        /// El ISBN aparece unicamente en el WHERE: identifica la fila, pero nunca se
        /// modifica (RN08). Por eso el cuadro de texto queda bloqueado mientras hay un
        /// libro cargado.
        /// </summary>
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(isbnSeleccionado))
            {
                MessageBox.Show("Seleccione primero un libro de la lista (doble clic sobre " +
                                "la fila) o busquelo por su ISBN.", "Seleccion requerida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int anio;
            int existencias;

            if (!DatosValidos(out anio, out existencias))
            {
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql =
                        "UPDATE dbo.Libros " +
                        "SET Titulo = @Titulo, IdAutor = @IdAutor, IdEditorial = @IdEditorial, " +
                        "    Categoria = @Categoria, Anio = @Anio, Existencias = @Existencias " +
                        "WHERE ISBN = @ISBN;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@ISBN", isbnSeleccionado);
                        AgregarParametrosDelLibro(comando, anio, existencias);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Libro actualizado correctamente.", "Actualizacion exitosa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarLibros();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MostrarErrorDeGuardado(ex, "actualizar");
            }
        }

        /// <summary>
        /// Elimina el libro cargado, previa confirmacion.
        ///
        /// Si el libro tiene prestamos registrados, la llave foranea FK_Prestamos_Libros
        /// impide el borrado y SQL Server devuelve el error 547.
        /// </summary>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(isbnSeleccionado))
            {
                MessageBox.Show("Seleccione primero un libro de la lista (doble clic sobre " +
                                "la fila) o busquelo por su ISBN.", "Seleccion requerida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Desea eliminar el libro \"" + txtTitulo.Text.Trim() + "\"?",
                "Confirmar eliminacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "DELETE FROM dbo.Libros WHERE ISBN = @ISBN;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@ISBN", isbnSeleccionado);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Libro eliminado correctamente.", "Eliminacion exitosa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarLibros();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show("No se puede eliminar este libro porque tiene prestamos " +
                                    "registrados." + Environment.NewLine +
                                    "Elimine primero esos prestamos en el modulo de Prestamos.",
                                    "Eliminacion no permitida",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("No fue posible eliminar el libro." + Environment.NewLine +
                                    ex.Message, "Error de base de datos",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Busca un libro por su ISBN y, si existe, lo carga en el formulario.
        /// Si no existe, lo informa y deja el ISBN escrito para poder registrarlo.
        /// </summary>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show("Escriba el ISBN que desea buscar.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtISBN.Focus();
                return;
            }

            string isbnBuscado = txtISBN.Text.Trim();

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT ISBN, Titulo, IdAutor, IdEditorial, Categoria, Anio, Existencias " +
                                 "FROM dbo.Libros WHERE ISBN = @ISBN;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@ISBN", isbnBuscado);

                        conexion.Open();

                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (!lector.Read())
                            {
                                MessageBox.Show("No existe ningun libro con el ISBN " +
                                                isbnBuscado + ".", "Libro no encontrado",
                                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Se conserva el ISBN escrito por si el usuario quiere
                                // registrar ese libro a continuacion.
                                LimpiarCamposSalvoIsbn();
                                return;
                            }

                            txtTitulo.Text = Convert.ToString(lector["Titulo"]);
                            cboAutor.SelectedValue = lector["IdAutor"];
                            cboEditorial.SelectedValue = lector["IdEditorial"];
                            txtCategoria.Text = Convert.ToString(lector["Categoria"]);
                            txtAnio.Text = Convert.ToString(lector["Anio"]);
                            txtExistencias.Text = Convert.ToString(lector["Existencias"]);

                            MarcarLibroCargado(isbnBuscado);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible realizar la busqueda." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Consulta si el ISBN ya pertenece a algun libro (RN01).
        /// </summary>
        private bool IsbnYaRegistrado(string isbn)
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT COUNT(*) FROM dbo.Libros WHERE ISBN = @ISBN;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@ISBN", isbn);

                        conexion.Open();
                        return Convert.ToInt32(comando.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible verificar el ISBN." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Ante un fallo de consulta se frena el guardado: es preferible no
                // registrar a registrar un ISBN repetido.
                return true;
            }
        }

        /// <summary>
        /// Carga los parametros comunes al INSERT y al UPDATE.
        ///
        /// Categoria y Anio admiten NULL en la tabla, asi que cuando el usuario los deja
        /// vacios se envia DBNull en lugar de una cadena vacia o un cero, que serian
        /// datos falsos. El anio llega ya convertido a entero desde DatosValidos, donde
        /// se usa el valor -1 para indicar "el usuario no escribio nada".
        /// </summary>
        private void AgregarParametrosDelLibro(SqlCommand comando, int anio, int existencias)
        {
            comando.Parameters.AddWithValue("@Titulo", txtTitulo.Text.Trim());
            comando.Parameters.AddWithValue("@IdAutor", cboAutor.SelectedValue);
            comando.Parameters.AddWithValue("@IdEditorial", cboEditorial.SelectedValue);
            comando.Parameters.AddWithValue("@Existencias", existencias);

            if (string.IsNullOrWhiteSpace(txtCategoria.Text))
            {
                comando.Parameters.AddWithValue("@Categoria", DBNull.Value);
            }
            else
            {
                comando.Parameters.AddWithValue("@Categoria", txtCategoria.Text.Trim());
            }

            if (anio < 0)
            {
                comando.Parameters.AddWithValue("@Anio", DBNull.Value);
            }
            else
            {
                comando.Parameters.AddWithValue("@Anio", anio);
            }
        }

        /// <summary>
        /// Traduce los errores de SQL Server al guardar o actualizar un libro.
        /// El numero 2627 es la violacion de la llave primaria PK_Libros: cubre el caso
        /// de que el ISBN se registre justo entre la comprobacion y el guardado.
        /// </summary>
        private void MostrarErrorDeGuardado(SqlException ex, string accion)
        {
            if (ex.Number == 2627 || ex.Number == 2601)
            {
                MessageBox.Show("Ese ISBN ya esta registrado.", "ISBN duplicado",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtISBN.Focus();
            }
            else
            {
                MessageBox.Show("No fue posible " + accion + " el libro." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // VALIDACIONES
        // ====================================================================

        /// <summary>
        /// Comprueba los campos obligatorios (RN02) y los numericos (RN10).
        /// </summary>
        /// <param name="anio">
        /// Devuelve el anio convertido a entero, o -1 si el usuario dejo el campo vacio
        /// (es opcional porque la columna admite NULL).
        /// </param>
        /// <param name="existencias">Devuelve las existencias convertidas a entero.</param>
        private bool DatosValidos(out int anio, out int existencias)
        {
            anio = -1;
            existencias = 0;

            if (string.IsNullOrWhiteSpace(txtISBN.Text))
            {
                MessageBox.Show("El ISBN es obligatorio.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtISBN.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("El título del libro es obligatorio.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitulo.Focus();
                return false;
            }

            if (cboAutor.SelectedValue == null)
            {
                MessageBox.Show("Seleccione el autor del libro.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboAutor.Focus();
                return false;
            }

            if (cboEditorial.SelectedValue == null)
            {
                MessageBox.Show("Seleccione la editorial del libro.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboEditorial.Focus();
                return false;
            }

            // Anio: opcional, pero si se escribe algo tiene que ser un numero entero.
            // int.TryParse devuelve false en lugar de lanzar excepcion cuando el texto
            // no es convertible, que es justo lo que se necesita para avisar al usuario.
            if (!string.IsNullOrWhiteSpace(txtAnio.Text))
            {
                if (!int.TryParse(txtAnio.Text.Trim(), out anio))
                {
                    MessageBox.Show("El año debe ser un número entero." + Environment.NewLine +
                                    "Ejemplo: 1967", "Año inválido",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAnio.Focus();
                    anio = -1;
                    return false;
                }

                if (anio < 0)
                {
                    MessageBox.Show("El año no puede ser negativo.", "Año inválido",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtAnio.Focus();
                    anio = -1;
                    return false;
                }
            }

            // Existencias: obligatorio, entero y nunca negativo (RN10).
            if (string.IsNullOrWhiteSpace(txtExistencias.Text))
            {
                MessageBox.Show("Las existencias son obligatorias. Escriba 0 si no hay " +
                                "ejemplares disponibles.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtExistencias.Focus();
                return false;
            }

            if (!int.TryParse(txtExistencias.Text.Trim(), out existencias))
            {
                MessageBox.Show("Las existencias deben ser un número entero.",
                                "Existencias inválidas",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtExistencias.Focus();
                return false;
            }

            if (existencias < 0)
            {
                MessageBox.Show("Las existencias no pueden ser un número negativo.",
                                "Existencias inválidas",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtExistencias.Focus();
                return false;
            }

            return true;
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
        /// Deja el formulario listo para registrar un libro distinto: vacia todo,
        /// desbloquea el ISBN y olvida el libro que estuviera cargado.
        /// </summary>
        private void LimpiarCampos()
        {
            txtISBN.Clear();
            LimpiarCamposSalvoIsbn();
            txtISBN.Focus();
        }

        /// <summary>
        /// Vacia todos los campos menos el ISBN.
        ///
        /// Se separa de LimpiarCampos porque la busqueda sin resultados necesita dejar
        /// el ISBN escrito: asi el usuario puede registrar ese libro sin volver a
        /// teclearlo.
        /// </summary>
        private void LimpiarCamposSalvoIsbn()
        {
            txtTitulo.Clear();
            cboAutor.SelectedIndex = -1;
            cboEditorial.SelectedIndex = -1;
            txtCategoria.Clear();
            txtAnio.Clear();
            txtExistencias.Clear();

            isbnSeleccionado = string.Empty;
            txtISBN.ReadOnly = false;
            txtISBN.BackColor = Color.White;

            QuitarResaltadoDeLista();
        }

        /// <summary>
        /// Marca que hay un libro cargado en el formulario y bloquea el cuadro del ISBN.
        ///
        /// Bloquearlo es la forma visible de cumplir RN08: mientras se trabaja sobre un
        /// libro existente, su llave primaria no se puede alterar. Para registrar otro
        /// libro hay que pulsar Nuevo, que vuelve a liberar el campo.
        /// </summary>
        private void MarcarLibroCargado(string isbn)
        {
            isbnSeleccionado = isbn;
            txtISBN.Text = isbn;
            txtISBN.ReadOnly = true;
            txtISBN.BackColor = Color.FromArgb(238, 241, 243);
        }

        /// <summary>
        /// Quita el resaltado que el DataGridView pone sobre la primera fila al recibir
        /// datos, para que lo que se ve coincida con lo que el formulario considera
        /// seleccionado.
        /// </summary>
        private void QuitarResaltadoDeLista()
        {
            if (dgvLibros.Rows.Count > 0)
            {
                dgvLibros.CurrentCell = null;
            }

            dgvLibros.ClearSelection();
        }

        /// <summary>
        /// Doble clic sobre una fila: carga ese libro en el formulario.
        ///
        /// Los ComboBox se posicionan asignando SelectedValue con el Id que trae la fila;
        /// como ValueMember es ese mismo Id, el control se coloca solo en la opcion
        /// correcta y muestra el nombre.
        /// </summary>
        private void dgvLibros_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvLibros.Rows[e.RowIndex];

            txtTitulo.Text = Convert.ToString(fila.Cells["Titulo"].Value);
            cboAutor.SelectedValue = fila.Cells["IdAutor"].Value;
            cboEditorial.SelectedValue = fila.Cells["IdEditorial"].Value;
            txtCategoria.Text = Convert.ToString(fila.Cells["Categoria"].Value);
            txtAnio.Text = Convert.ToString(fila.Cells["Anio"].Value);
            txtExistencias.Text = Convert.ToString(fila.Cells["Existencias"].Value);

            MarcarLibroCargado(Convert.ToString(fila.Cells["ISBN"].Value));
        }

        /// <summary>
        /// Da formato visual a la lista. Se ejecuta una sola vez, al cargar el formulario.
        /// </summary>
        private void AplicarEstiloLista()
        {
            dgvLibros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLibros.GridColor = Color.FromArgb(222, 228, 232);

            dgvLibros.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 76, 92);
            dgvLibros.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLibros.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dgvLibros.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgvLibros.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvLibros.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvLibros.DefaultCellStyle.SelectionBackColor = Color.FromArgb(19, 111, 99);
            dgvLibros.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvLibros.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 245);
        }

        /// <summary>
        /// Pone titulos legibles a las columnas, oculta los Id que solo sirven
        /// internamente y reparte el ancho disponible.
        /// </summary>
        private void DarNombreAColumnas()
        {
            if (dgvLibros.Columns.Count == 0)
            {
                return;
            }

            // Los Id viajan en la consulta para poder posicionar los ComboBox al hacer
            // doble clic, pero al usuario no le dicen nada: se ocultan.
            dgvLibros.Columns["IdAutor"].Visible = false;
            dgvLibros.Columns["IdEditorial"].Visible = false;

            dgvLibros.Columns["ISBN"].HeaderText = "ISBN";
            dgvLibros.Columns["Titulo"].HeaderText = "Título";
            dgvLibros.Columns["Autor"].HeaderText = "Autor";
            dgvLibros.Columns["Editorial"].HeaderText = "Editorial";
            dgvLibros.Columns["Categoria"].HeaderText = "Categoría";
            dgvLibros.Columns["Anio"].HeaderText = "Año";
            dgvLibros.Columns["Existencias"].HeaderText = "Existencias";

            // Los pesos suman 100 y se reparten sobre el ancho util, que es menor cuando
            // aparece la barra de desplazamiento vertical. Por eso Existencias lleva un
            // peso holgado: con uno mas ajustado, su encabezado se recortaba.
            dgvLibros.Columns["ISBN"].FillWeight = 18;
            dgvLibros.Columns["Titulo"].FillWeight = 24;
            dgvLibros.Columns["Autor"].FillWeight = 17;
            dgvLibros.Columns["Editorial"].FillWeight = 15;
            dgvLibros.Columns["Categoria"].FillWeight = 11;
            dgvLibros.Columns["Anio"].FillWeight = 6;
            dgvLibros.Columns["Existencias"].FillWeight = 14;

            dgvLibros.Columns["Anio"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
            dgvLibros.Columns["Existencias"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleCenter;
        }
    }
}
