namespace SistemaBiblioteca
{
    partial class FrmPruebaConexion
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
            this.pnlTitulo = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.btnProbar = new System.Windows.Forms.Button();
            this.txtResultado = new System.Windows.Forms.TextBox();
            this.pnlTitulo.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTitulo
            //
            this.pnlTitulo.BackColor = System.Drawing.Color.FromArgb(15, 76, 92);
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(584, 64);
            this.pnlTitulo.TabIndex = 0;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 17);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(268, 28);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Prueba de conexion a SQL Server";
            //
            // lblDescripcion
            //
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblDescripcion.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblDescripcion.Location = new System.Drawing.Point(20, 84);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(420, 34);
            this.lblDescripcion.TabIndex = 1;
            this.lblDescripcion.Text = "Comprueba que la clase Conexion puede abrir la base de datos Biblioteca\r\ny leer las tablas creadas por el script SQL.";
            //
            // btnProbar
            //
            this.btnProbar.BackColor = System.Drawing.Color.FromArgb(19, 111, 99);
            this.btnProbar.FlatAppearance.BorderSize = 0;
            this.btnProbar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProbar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnProbar.ForeColor = System.Drawing.Color.White;
            this.btnProbar.Location = new System.Drawing.Point(20, 132);
            this.btnProbar.Name = "btnProbar";
            this.btnProbar.Size = new System.Drawing.Size(180, 40);
            this.btnProbar.TabIndex = 2;
            this.btnProbar.Text = "Probar conexion";
            this.btnProbar.UseVisualStyleBackColor = false;
            this.btnProbar.Click += new System.EventHandler(this.btnProbar_Click);
            //
            // txtResultado
            //
            this.txtResultado.BackColor = System.Drawing.Color.White;
            this.txtResultado.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.txtResultado.Location = new System.Drawing.Point(20, 188);
            this.txtResultado.Multiline = true;
            this.txtResultado.Name = "txtResultado";
            this.txtResultado.ReadOnly = true;
            this.txtResultado.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResultado.Size = new System.Drawing.Size(544, 150);
            this.txtResultado.TabIndex = 3;
            //
            // FrmPruebaConexion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(244, 246, 248);
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.txtResultado);
            this.Controls.Add(this.btnProbar);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.pnlTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmPruebaConexion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema de Gestion de Biblioteca - Prueba de conexion";
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.Button btnProbar;
        private System.Windows.Forms.TextBox txtResultado;
    }
}
