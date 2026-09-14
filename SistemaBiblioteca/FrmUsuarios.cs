using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using SistemaBiblioteca.Datos;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Modulo de Usuarios: gestiona las personas que pueden solicitar prestamos.
    ///
    /// Aporta dos validaciones que los modulos anteriores no tenian:
    ///   - RN09: el documento no se puede repetir entre usuarios.
    ///   - El correo, si se escribe, debe tener formato valido.
    ///
    /// Telefono y Correo son opcionales porque en la tabla admiten NULL; Nombre,
    /// Apellido y Documento son obligatorios (RN02).
    /// </summary>
    public partial class FrmUsuarios : Form
    {
        /// <summary>
        /// Id del usuario cargado tras hacer doble clic en la lista. Vale 0 cuando no
        /// hay nada seleccionado.
        /// </summary>
        private int idUsuarioSeleccionado = 0;

        /// <summary>
        /// Patron para validar el correo: uno o mas caracteres que no sean arroba ni
        /// espacio, una arroba, otro tanto para el dominio, un punto y una extension de
        /// al menos dos letras. Cubre los casos normales sin volverse ilegible.
        /// </summary>
        private const string PatronCorreo = @"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,}$";

        public FrmUsuarios()
        {
            InitializeComponent();
        }

        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            AplicarEstiloLista();
            CargarUsuarios();
            LimpiarCampos();
        }

        /// <summary>
        /// El DataGridView reasigna su celda actual al hacerse visible, despues de Load.
        /// </summary>
        private void FrmUsuarios_Shown(object sender, EventArgs e)
        {
            QuitarResaltadoDeLista();
        }

        // ====================================================================
        // ACCESO A DATOS
        // ====================================================================

        /// <summary>
        /// Consulta todos los usuarios y los muestra en el DataGridView.
        /// </summary>
        private void CargarUsuarios()
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT IdUsuario, Nombre, Apellido, Documento, Telefono, Correo " +
                                 "FROM dbo.Usuarios " +
                                 "ORDER BY Apellido, Nombre;";

                    using (SqlDataAdapter adaptador = new SqlDataAdapter(sql, conexion))
                    {
                        DataTable tabla = new DataTable();
                        adaptador.Fill(tabla);
                        dgvUsuarios.DataSource = tabla;
                    }
                }

                DarNombreAColumnas();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible cargar la lista de usuarios." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Registra un usuario nuevo.
        ///
        /// Antes de insertar se comprueba que el documento no exista ya (RN09). Se pasa
        /// 0 como "id a excluir" porque todavia no hay registro propio que ignorar.
        /// </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!DatosValidos())
            {
                return;
            }

            if (DocumentoYaRegistrado(txtDocumento.Text.Trim(), 0))
            {
                MessageBox.Show("Ya existe un usuario con el documento " +
                                txtDocumento.Text.Trim() + "." + Environment.NewLine +
                                "El documento no se puede repetir.",
                                "Documento duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumento.Focus();
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "INSERT INTO dbo.Usuarios (Nombre, Apellido, Documento, Telefono, Correo) " +
                                 "VALUES (@Nombre, @Apellido, @Documento, @Telefono, @Correo);";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        AgregarParametrosDelUsuario(comando);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuario registrado correctamente.", "Registro exitoso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MostrarErrorDeGuardado(ex, "guardar");
            }
        }

        /// <summary>
        /// Actualiza el usuario seleccionado.
        ///
        /// La comprobacion de documento repetido excluye al propio usuario: de lo
        /// contrario, guardar sin cambiar el documento se rechazaria a si mismo.
        /// </summary>
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Seleccione primero un usuario de la lista " +
                                "(doble clic sobre la fila).", "Seleccion requerida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DatosValidos())
            {
                return;
            }

            if (DocumentoYaRegistrado(txtDocumento.Text.Trim(), idUsuarioSeleccionado))
            {
                MessageBox.Show("Otro usuario ya tiene registrado el documento " +
                                txtDocumento.Text.Trim() + ".",
                                "Documento duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumento.Focus();
                return;
            }

            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "UPDATE dbo.Usuarios " +
                                 "SET Nombre = @Nombre, Apellido = @Apellido, Documento = @Documento, " +
                                 "    Telefono = @Telefono, Correo = @Correo " +
                                 "WHERE IdUsuario = @IdUsuario;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        AgregarParametrosDelUsuario(comando);
                        comando.Parameters.AddWithValue("@IdUsuario", idUsuarioSeleccionado);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuario actualizado correctamente.", "Actualizacion exitosa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MostrarErrorDeGuardado(ex, "actualizar");
            }
        }

        /// <summary>
        /// Elimina el usuario seleccionado, previa confirmacion.
        ///
        /// Si el usuario tiene prestamos registrados, la llave foranea
        /// FK_Prestamos_Usuarios impide el borrado (error 547).
        /// </summary>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Seleccione primero un usuario de la lista " +
                                "(doble clic sobre la fila).", "Seleccion requerida",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Desea eliminar al usuario " + txtNombre.Text.Trim() + " " +
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
                    string sql = "DELETE FROM dbo.Usuarios WHERE IdUsuario = @IdUsuario;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@IdUsuario", idUsuarioSeleccionado);

                        conexion.Open();
                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuario eliminado correctamente.", "Eliminacion exitosa",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    MessageBox.Show("No se puede eliminar este usuario porque tiene prestamos " +
                                    "registrados." + Environment.NewLine +
                                    "Elimine primero esos prestamos en el modulo de Prestamos.",
                                    "Eliminacion no permitida",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("No fue posible eliminar el usuario." + Environment.NewLine +
                                    ex.Message, "Error de base de datos",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Consulta si el documento ya pertenece a algun usuario (RN09).
        ///
        /// El parametro idUsuarioExcluido sirve para el caso de la edicion: al
        /// actualizar un usuario hay que ignorar su propia fila, porque de lo contrario
        /// su documento actual se detectaria como repetido. Al insertar se envia 0, que
        /// nunca coincide con un IdUsuario real porque IDENTITY empieza en 1.
        /// </summary>
        private bool DocumentoYaRegistrado(string documento, int idUsuarioExcluido)
        {
            try
            {
                using (SqlConnection conexion = Conexion.ObtenerConexion())
                {
                    string sql = "SELECT COUNT(*) FROM dbo.Usuarios " +
                                 "WHERE Documento = @Documento AND IdUsuario <> @IdExcluido;";

                    using (SqlCommand comando = new SqlCommand(sql, conexion))
                    {
                        comando.Parameters.AddWithValue("@Documento", documento);
                        comando.Parameters.AddWithValue("@IdExcluido", idUsuarioExcluido);

                        conexion.Open();

                        // ExecuteScalar devuelve el primer valor de la primera fila,
                        // que es justo lo que produce un COUNT(*).
                        return Convert.ToInt32(comando.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No fue posible verificar el documento." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Ante un fallo de consulta se responde "si existe" para frenar el
                // guardado: es preferible no registrar a registrar un duplicado.
                return true;
            }
        }

        /// <summary>
        /// Carga en el comando los cinco parametros comunes al INSERT y al UPDATE.
        ///
        /// Telefono y Correo se envian como DBNull cuando el usuario los deja vacios.
        /// Si se enviara la cadena vacia, la columna guardaria '' en lugar de NULL y la
        /// base dejaria de reflejar que el dato simplemente no se conoce.
        /// </summary>
        private void AgregarParametrosDelUsuario(SqlCommand comando)
        {
            comando.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
            comando.Parameters.AddWithValue("@Apellido", txtApellido.Text.Trim());
            comando.Parameters.AddWithValue("@Documento", txtDocumento.Text.Trim());

            if (string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                comando.Parameters.AddWithValue("@Telefono", DBNull.Value);
            }
            else
            {
                comando.Parameters.AddWithValue("@Telefono", txtTelefono.Text.Trim());
            }

            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                comando.Parameters.AddWithValue("@Correo", DBNull.Value);
            }
            else
            {
                comando.Parameters.AddWithValue("@Correo", txtCorreo.Text.Trim());
            }
        }

        /// <summary>
        /// Traduce los errores de SQL Server al guardar o actualizar un usuario.
        ///
        /// Los numeros 2627 y 2601 corresponden a la violacion de la restriccion UNIQUE
        /// del documento. La aplicacion ya lo comprueba antes, pero la base tambien lo
        /// protege: esta rama cubre el caso de que alguien registre el mismo documento
        /// justo entre la comprobacion y el guardado.
        /// </summary>
        private void MostrarErrorDeGuardado(SqlException ex, string accion)
        {
            if (ex.Number == 2627 || ex.Number == 2601)
            {
                MessageBox.Show("El documento ya esta registrado por otro usuario.",
                                "Documento duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumento.Focus();
            }
            else
            {
                MessageBox.Show("No fue posible " + accion + " el usuario." + Environment.NewLine +
                                ex.Message, "Error de base de datos",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ====================================================================
        // VALIDACIONES Y APOYO A LA INTERFAZ
        // ====================================================================

        /// <summary>
        /// Comprueba los obligatorios (RN02) y el formato del correo.
        /// El correo solo se valida si el usuario escribio algo, porque es opcional.
        /// </summary>
        private bool DatosValidos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre del usuario es obligatorio.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido del usuario es obligatorio.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                MessageBox.Show("El documento del usuario es obligatorio.", "Dato faltante",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDocumento.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtCorreo.Text) &&
                !Regex.IsMatch(txtCorreo.Text.Trim(), PatronCorreo))
            {
                MessageBox.Show("El correo no tiene un formato valido." + Environment.NewLine +
                                "Ejemplo correcto: nombre@dominio.com",
                                "Correo invalido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCorreo.Focus();
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
        /// Vacia los cinco campos, olvida el usuario seleccionado y devuelve el cursor
        /// al primer campo.
        /// </summary>
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDocumento.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            idUsuarioSeleccionado = 0;
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
            if (dgvUsuarios.Rows.Count > 0)
            {
                dgvUsuarios.CurrentCell = null;
            }

            dgvUsuarios.ClearSelection();
        }

        /// <summary>
        /// Doble clic sobre una fila: carga ese usuario en el formulario.
        ///
        /// Telefono y Correo pueden venir como NULL desde la base. Convert.ToString
        /// traduce DBNull a cadena vacia, de modo que el cuadro de texto queda en blanco
        /// en lugar de provocar un error.
        /// </summary>
        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow fila = dgvUsuarios.Rows[e.RowIndex];

            idUsuarioSeleccionado = Convert.ToInt32(fila.Cells["IdUsuario"].Value);
            txtNombre.Text = Convert.ToString(fila.Cells["Nombre"].Value);
            txtApellido.Text = Convert.ToString(fila.Cells["Apellido"].Value);
            txtDocumento.Text = Convert.ToString(fila.Cells["Documento"].Value);
            txtTelefono.Text = Convert.ToString(fila.Cells["Telefono"].Value);
            txtCorreo.Text = Convert.ToString(fila.Cells["Correo"].Value);
        }

        /// <summary>
        /// Da formato visual a la lista. Se ejecuta una sola vez, al cargar el formulario.
        /// </summary>
        private void AplicarEstiloLista()
        {
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.GridColor = Color.FromArgb(222, 228, 232);

            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(15, 76, 92);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvUsuarios.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(19, 111, 99);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(239, 243, 245);
        }

        /// <summary>
        /// Pone titulos legibles a las columnas y reparte el ancho. Se llama tras cada
        /// carga porque al asignar un DataSource nuevo el grid regenera sus columnas.
        /// </summary>
        private void DarNombreAColumnas()
        {
            if (dgvUsuarios.Columns.Count == 0)
            {
                return;
            }

            dgvUsuarios.Columns["IdUsuario"].HeaderText = "ID";
            dgvUsuarios.Columns["Nombre"].HeaderText = "Nombre";
            dgvUsuarios.Columns["Apellido"].HeaderText = "Apellido";
            dgvUsuarios.Columns["Documento"].HeaderText = "Documento";
            dgvUsuarios.Columns["Telefono"].HeaderText = "Teléfono";
            dgvUsuarios.Columns["Correo"].HeaderText = "Correo";

            dgvUsuarios.Columns["IdUsuario"].FillWeight = 8;
            dgvUsuarios.Columns["Nombre"].FillWeight = 17;
            dgvUsuarios.Columns["Apellido"].FillWeight = 17;
            dgvUsuarios.Columns["Documento"].FillWeight = 14;
            dgvUsuarios.Columns["Telefono"].FillWeight = 14;
            dgvUsuarios.Columns["Correo"].FillWeight = 30;
        }
    }
}
