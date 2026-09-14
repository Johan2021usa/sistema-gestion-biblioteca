using System.Windows.Forms;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Pantalla de bienvenida del sistema.
    ///
    /// Es el formulario que FrmPrincipal carga al arrancar. No accede a la base de datos:
    /// solo presenta el sistema y describe que hace cada modulo del menu lateral.
    /// </summary>
    public partial class FrmInicio : Form
    {
        public FrmInicio()
        {
            InitializeComponent();
        }
    }
}
