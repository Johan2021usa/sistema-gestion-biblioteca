using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Ventana contenedora del sistema (capa de presentacion).
    ///
    /// Muestra el menu de 8 opciones y aloja los formularios de cada modulo dentro del
    /// panel pnlContenedor, de manera que la aplicacion trabaja siempre en una sola
    /// ventana. FrmPrincipal no toca la base de datos: su unica responsabilidad es la
    /// navegacion.
    /// </summary>
    public partial class FrmPrincipal : Form
    {
        // --- Paleta del sistema -------------------------------------------------
        // Se declaran aqui como constantes para que los colores del menu no queden
        // dispersos por el codigo y sea facil cambiarlos en un solo punto.
        private static readonly Color ColorMenu = Color.FromArgb(10, 54, 66);
        private static readonly Color ColorMenuActivo = Color.FromArgb(19, 111, 99);

        /// <summary>Formulario que se esta mostrando dentro del panel contenedor.</summary>
        private Form formularioActivo = null;

        /// <summary>Boton del menu que aparece resaltado como opcion seleccionada.</summary>
        private IconButton botonActivo = null;

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Al arrancar la aplicacion se muestra el modulo de Inicio.
        /// </summary>
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmInicio(), btnInicio, "Inicio");
        }

        // ====================================================================
        // NAVEGACION
        // ====================================================================

        /// <summary>
        /// Coloca un formulario de modulo dentro de pnlContenedor.
        ///
        /// El truco consiste en quitarle al formulario su condicion de ventana propia:
        ///   - TopLevel = false        -> deja de ser una ventana independiente y puede
        ///                                comportarse como un control hijo.
        ///   - FormBorderStyle = None  -> se le quita la barra de titulo y el borde, que
        ///                                dentro del panel sobrarian.
        ///   - Dock = Fill             -> ocupa todo el espacio disponible del panel.
        ///
        /// Antes de abrir el nuevo formulario se cierra el anterior, para no ir
        /// acumulando formularios invisibles (y conexiones a la base) en memoria.
        /// </summary>
        /// <param name="formulario">Instancia del formulario del modulo a mostrar.</param>
        /// <param name="boton">Boton del menu que debe quedar resaltado.</param>
        /// <param name="nombreSeccion">Texto que se muestra en la cabecera.</param>
        private void AbrirFormulario(Form formulario, IconButton boton, string nombreSeccion)
        {
            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }

            formularioActivo = formulario;

            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;

            pnlContenedor.Controls.Add(formulario);
            formulario.BringToFront();
            formulario.Show();

            ResaltarBoton(boton);
            lblSeccion.Text = nombreSeccion;
        }

        /// <summary>
        /// Deja resaltada la opcion del menu en la que se hizo clic y devuelve la
        /// anterior a su color normal, para que el usuario sepa siempre donde esta.
        /// </summary>
        private void ResaltarBoton(IconButton boton)
        {
            if (botonActivo != null)
            {
                botonActivo.BackColor = ColorMenu;
            }

            botonActivo = boton;

            if (botonActivo != null)
            {
                botonActivo.BackColor = ColorMenuActivo;
            }
        }

        // ====================================================================
        // EVENTOS DE LOS 8 BOTONES DEL MENU
        // ====================================================================

        private void btnInicio_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmInicio(), btnInicio, "Inicio");
        }

        private void btnLibros_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmLibros(), btnLibros, "Libros");
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmUsuarios(), btnUsuarios, "Usuarios");
        }

        private void btnAutores_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmAutores(), btnAutores, "Autores");
        }

        private void btnEditoriales_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmEditoriales(), btnEditoriales, "Editoriales");
        }

        private void btnPrestamos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmPrestamos(), btnPrestamos, "Préstamos");
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmReportes(), btnReportes, "Reportes");
        }

        /// <summary>
        /// Cierra la aplicacion. Se pide confirmacion porque es una accion que el
        /// usuario no puede deshacer.
        /// </summary>
        private void btnSalida_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
                "¿Desea cerrar el sistema?",
                "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
