namespace SistemaBiblioteca
{
    partial class FrmInicio
    {
        /// <summary>Contenedor de componentes del disenador.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Libera los recursos usados por el formulario.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codigo generado por el Disenador de Windows Forms

        private void InitializeComponent()
        {
            this.pnlEncabezado = new System.Windows.Forms.Panel();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.grpModulos = new System.Windows.Forms.GroupBox();
            this.lblModulosNombre = new System.Windows.Forms.Label();
            this.lblModulosDetalle = new System.Windows.Forms.Label();
            this.lblPie = new System.Windows.Forms.Label();
            this.pnlEncabezado.SuspendLayout();
            this.grpModulos.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlEncabezado
            //
            this.pnlEncabezado.BackColor = System.Drawing.Color.White;
            this.pnlEncabezado.Controls.Add(this.lblSubtitulo);
            this.pnlEncabezado.Controls.Add(this.lblBienvenida);
            this.pnlEncabezado.Location = new System.Drawing.Point(40, 40);
            this.pnlEncabezado.Name = "pnlEncabezado";
            this.pnlEncabezado.Size = new System.Drawing.Size(800, 110);
            this.pnlEncabezado.TabIndex = 0;
            //
            // lblBienvenida
            //
            this.lblBienvenida.AutoSize = true;
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI Semibold", 18F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.ForeColor = System.Drawing.Color.FromArgb(15, 76, 92);
            this.lblBienvenida.Location = new System.Drawing.Point(28, 24);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(330, 32);
            this.lblBienvenida.TabIndex = 0;
            this.lblBienvenida.Text = "Bienvenido al sistema";
            //
            // lblSubtitulo
            //
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(90, 90, 90);
            this.lblSubtitulo.Location = new System.Drawing.Point(30, 64);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(520, 19);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Gestión de libros, autores, editoriales, usuarios y préstamos de la biblioteca.";
            //
            // grpModulos
            //
            this.grpModulos.BackColor = System.Drawing.Color.White;
            this.grpModulos.Controls.Add(this.lblModulosDetalle);
            this.grpModulos.Controls.Add(this.lblModulosNombre);
            this.grpModulos.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.grpModulos.ForeColor = System.Drawing.Color.FromArgb(15, 76, 92);
            this.grpModulos.Location = new System.Drawing.Point(40, 174);
            this.grpModulos.Name = "grpModulos";
            this.grpModulos.Size = new System.Drawing.Size(800, 290);
            this.grpModulos.TabIndex = 1;
            this.grpModulos.TabStop = false;
            this.grpModulos.Text = "Módulos disponibles en el menú lateral";
            //
            // lblModulosNombre
            //
            // Los nombres y las descripciones van en dos etiquetas separadas, una por
            // columna: alinearlos con espacios dentro de un solo Label no funciona
            // porque Segoe UI es una fuente proporcional y cada letra mide distinto.
            this.lblModulosNombre.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblModulosNombre.ForeColor = System.Drawing.Color.FromArgb(15, 76, 92);
            this.lblModulosNombre.Location = new System.Drawing.Point(28, 42);
            this.lblModulosNombre.Name = "lblModulosNombre";
            this.lblModulosNombre.Size = new System.Drawing.Size(130, 220);
            this.lblModulosNombre.TabIndex = 0;
            this.lblModulosNombre.Text = "Libros\r\n\r\nUsuarios\r\n\r\nAutores\r\n\r\nEditoriales\r\n\r\nPréstamos\r\n\r\nReportes";
            //
            // lblModulosDetalle
            //
            this.lblModulosDetalle.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblModulosDetalle.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblModulosDetalle.Location = new System.Drawing.Point(166, 42);
            this.lblModulosDetalle.Name = "lblModulosDetalle";
            this.lblModulosDetalle.Size = new System.Drawing.Size(600, 220);
            this.lblModulosDetalle.TabIndex = 1;
            this.lblModulosDetalle.Text = "Registro de libros con su autor, editorial, categoría, año y existencias.\r\n\r\n" +
                                          "Registro de las personas que pueden solicitar préstamos.\r\n\r\n" +
                                          "Catálogo de autores asociados a los libros.\r\n\r\n" +
                                          "Catálogo de editoriales asociadas a los libros.\r\n\r\n" +
                                          "Registro de préstamos y control de existencias disponibles.\r\n\r\n" +
                                          "Préstamos activos, préstamos devueltos e inventario de libros.";
            //
            // lblPie
            //
            this.lblPie.AutoSize = true;
            this.lblPie.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPie.ForeColor = System.Drawing.Color.FromArgb(120, 120, 120);
            this.lblPie.Location = new System.Drawing.Point(42, 490);
            this.lblPie.Name = "lblPie";
            this.lblPie.Size = new System.Drawing.Size(400, 15);
            this.lblPie.TabIndex = 2;
            this.lblPie.Text = "ACA - Programación Avanzada | Base de datos: SQL Server (Biblioteca)";
            //
            // FrmInicio
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(244, 246, 248);
            this.ClientSize = new System.Drawing.Size(880, 580);
            this.Controls.Add(this.lblPie);
            this.Controls.Add(this.grpModulos);
            this.Controls.Add(this.pnlEncabezado);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmInicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inicio";
            this.pnlEncabezado.ResumeLayout(false);
            this.pnlEncabezado.PerformLayout();
            this.grpModulos.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.GroupBox grpModulos;
        private System.Windows.Forms.Label lblModulosNombre;
        private System.Windows.Forms.Label lblModulosDetalle;
        private System.Windows.Forms.Label lblPie;
    }
}
