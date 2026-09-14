using System;
using System.Windows.Forms;

namespace SistemaBiblioteca
{
    /// <summary>
    /// Punto de entrada de la aplicacion.
    /// </summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Configura estilos visuales, DPI y fuente predeterminada de WinForms.
            ApplicationConfiguration.Initialize();

            // La aplicacion arranca en la ventana contenedora, que a su vez carga FrmInicio.
            Application.Run(new FrmPrincipal());
        }
    }
}
