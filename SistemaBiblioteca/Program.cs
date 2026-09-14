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

            // Formulario de arranque. En el bloque 3 se cambiara por FrmPrincipal.
            Application.Run(new FrmPruebaConexion());
        }
    }
}
