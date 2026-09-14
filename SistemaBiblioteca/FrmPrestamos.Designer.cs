namespace SistemaBiblioteca
{
    partial class FrmPrestamos
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
            this.pnlDatos = new System.Windows.Forms.Panel();
            this.btnCancelar = new FontAwesome.Sharp.IconButton();
            this.btnEliminar = new FontAwesome.Sharp.IconButton();
            this.btnDevolver = new FontAwesome.Sharp.IconButton();
            this.btnGuardar = new FontAwesome.Sharp.IconButton();
            this.btnNuevo = new FontAwesome.Sharp.IconButton();
            this.dtpFechaPrestamo = new System.Windows.Forms.DateTimePicker();
            this.lblFechaPrestamo = new System.Windows.Forms.Label();
            this.cboLibro = new System.Windows.Forms.ComboBox();
            this.lblLibro = new System.Windows.Forms.Label();
            this.cboUsuario = new System.Windows.Forms.ComboBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.pnlLista = new System.Windows.Forms.Panel();
            this.dgvPrestamos = new System.Windows.Forms.DataGridView();
            this.lblLista = new System.Windows.Forms.Label();
            this.pnlTitulo.SuspendLayout();
            this.pnlDatos.SuspendLayout();
            this.pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrestamos)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTitulo
            //
            this.pnlTitulo.BackColor = System.Drawing.Color.FromArgb(15, 76, 92);
            this.pnlTitulo.Controls.Add(this.lblTitulo);
            this.pnlTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitulo.Location = new System.Drawing.Point(0, 0);
            this.pnlTitulo.Name = "pnlTitulo";
            this.pnlTitulo.Size = new System.Drawing.Size(880, 56);
            this.pnlTitulo.TabIndex = 0;
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(28, 14);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(230, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Registro de Préstamos";
            //
            // pnlDatos
            //
            this.pnlDatos.BackColor = System.Drawing.Color.White;
            this.pnlDatos.Controls.Add(this.btnCancelar);
            this.pnlDatos.Controls.Add(this.btnEliminar);
            this.pnlDatos.Controls.Add(this.btnDevolver);
            this.pnlDatos.Controls.Add(this.btnGuardar);
            this.pnlDatos.Controls.Add(this.btnNuevo);
            this.pnlDatos.Controls.Add(this.dtpFechaPrestamo);
            this.pnlDatos.Controls.Add(this.lblFechaPrestamo);
            this.pnlDatos.Controls.Add(this.cboLibro);
            this.pnlDatos.Controls.Add(this.lblLibro);
            this.pnlDatos.Controls.Add(this.cboUsuario);
            this.pnlDatos.Controls.Add(this.lblUsuario);
            this.pnlDatos.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDatos.Location = new System.Drawing.Point(0, 56);
            this.pnlDatos.Name = "pnlDatos";
            this.pnlDatos.Size = new System.Drawing.Size(880, 190);
            this.pnlDatos.TabIndex = 1;
            //
            // lblUsuario
            //
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblUsuario.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblUsuario.Location = new System.Drawing.Point(30, 18);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(54, 17);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuario";
            //
            // cboUsuario
            //
            this.cboUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsuario.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cboUsuario.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboUsuario.Location = new System.Drawing.Point(30, 40);
            this.cboUsuario.Name = "cboUsuario";
            this.cboUsuario.Size = new System.Drawing.Size(330, 25);
            this.cboUsuario.TabIndex = 1;
            //
            // lblLibro
            //
            this.lblLibro.AutoSize = true;
            this.lblLibro.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblLibro.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblLibro.Location = new System.Drawing.Point(380, 18);
            this.lblLibro.Name = "lblLibro";
            this.lblLibro.Size = new System.Drawing.Size(210, 17);
            this.lblLibro.TabIndex = 2;
            this.lblLibro.Text = "Libro  (entre paréntesis, disponibles)";
            //
            // cboLibro
            //
            this.cboLibro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLibro.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cboLibro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboLibro.Location = new System.Drawing.Point(380, 40);
            this.cboLibro.Name = "cboLibro";
            this.cboLibro.Size = new System.Drawing.Size(440, 25);
            this.cboLibro.TabIndex = 3;
            //
            // lblFechaPrestamo
            //
            this.lblFechaPrestamo.AutoSize = true;
            this.lblFechaPrestamo.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblFechaPrestamo.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblFechaPrestamo.Location = new System.Drawing.Point(30, 78);
            this.lblFechaPrestamo.Name = "lblFechaPrestamo";
            this.lblFechaPrestamo.Size = new System.Drawing.Size(126, 17);
            this.lblFechaPrestamo.TabIndex = 4;
            this.lblFechaPrestamo.Text = "Fecha del préstamo";
            //
            // dtpFechaPrestamo
            //
            this.dtpFechaPrestamo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpFechaPrestamo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaPrestamo.Location = new System.Drawing.Point(30, 100);
            this.dtpFechaPrestamo.Name = "dtpFechaPrestamo";
            this.dtpFechaPrestamo.Size = new System.Drawing.Size(200, 25);
            this.dtpFechaPrestamo.TabIndex = 5;
            //
            // btnNuevo
            //
            this.btnNuevo.BackColor = System.Drawing.Color.FromArgb(19, 111, 99);
            this.btnNuevo.FlatAppearance.BorderSize = 0;
            this.btnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.IconChar = FontAwesome.Sharp.IconChar.Plus;
            this.btnNuevo.IconColor = System.Drawing.Color.White;
            this.btnNuevo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnNuevo.IconSize = 18;
            this.btnNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNuevo.Location = new System.Drawing.Point(30, 136);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(120, 40);
            this.btnNuevo.TabIndex = 6;
            this.btnNuevo.Text = "  Nuevo";
            this.btnNuevo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            //
            // btnGuardar
            //
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(19, 111, 99);
            this.btnGuardar.FlatAppearance.BorderSize = 0;
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnGuardar.IconColor = System.Drawing.Color.White;
            this.btnGuardar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnGuardar.IconSize = 18;
            this.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardar.Location = new System.Drawing.Point(160, 136);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(120, 40);
            this.btnGuardar.TabIndex = 7;
            this.btnGuardar.Text = "  Guardar";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnDevolver
            //
            this.btnDevolver.BackColor = System.Drawing.Color.FromArgb(15, 76, 92);
            this.btnDevolver.FlatAppearance.BorderSize = 0;
            this.btnDevolver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDevolver.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnDevolver.ForeColor = System.Drawing.Color.White;
            this.btnDevolver.IconChar = FontAwesome.Sharp.IconChar.RotateLeft;
            this.btnDevolver.IconColor = System.Drawing.Color.White;
            this.btnDevolver.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnDevolver.IconSize = 18;
            this.btnDevolver.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDevolver.Location = new System.Drawing.Point(290, 136);
            this.btnDevolver.Name = "btnDevolver";
            this.btnDevolver.Size = new System.Drawing.Size(120, 40);
            this.btnDevolver.TabIndex = 8;
            this.btnDevolver.Text = "  Devolver";
            this.btnDevolver.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDevolver.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDevolver.UseVisualStyleBackColor = false;
            this.btnDevolver.Click += new System.EventHandler(this.btnDevolver_Click);
            //
            // btnEliminar
            //
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(178, 58, 72);
            this.btnEliminar.FlatAppearance.BorderSize = 0;
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.IconChar = FontAwesome.Sharp.IconChar.TrashCan;
            this.btnEliminar.IconColor = System.Drawing.Color.White;
            this.btnEliminar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnEliminar.IconSize = 18;
            this.btnEliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEliminar.Location = new System.Drawing.Point(420, 136);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(120, 40);
            this.btnEliminar.TabIndex = 9;
            this.btnEliminar.Text = "  Eliminar";
            this.btnEliminar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(107, 114, 128);
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnCancelar.ForeColor = System.Drawing.Color.White;
            this.btnCancelar.IconChar = FontAwesome.Sharp.IconChar.Ban;
            this.btnCancelar.IconColor = System.Drawing.Color.White;
            this.btnCancelar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCancelar.IconSize = 18;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(550, 136);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 40);
            this.btnCancelar.TabIndex = 10;
            this.btnCancelar.Text = "  Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // pnlLista
            //
            this.pnlLista.BackColor = System.Drawing.Color.FromArgb(244, 246, 248);
            this.pnlLista.Controls.Add(this.dgvPrestamos);
            this.pnlLista.Controls.Add(this.lblLista);
            this.pnlLista.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLista.Location = new System.Drawing.Point(0, 246);
            this.pnlLista.Name = "pnlLista";
            this.pnlLista.Padding = new System.Windows.Forms.Padding(30, 10, 30, 20);
            this.pnlLista.Size = new System.Drawing.Size(880, 334);
            this.pnlLista.TabIndex = 2;
            //
            // lblLista
            //
            this.lblLista.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLista.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblLista.ForeColor = System.Drawing.Color.FromArgb(15, 76, 92);
            this.lblLista.Location = new System.Drawing.Point(30, 10);
            this.lblLista.Name = "lblLista";
            this.lblLista.Size = new System.Drawing.Size(820, 24);
            this.lblLista.TabIndex = 0;
            this.lblLista.Text = "Préstamos registrados  (doble clic en una fila para seleccionarla)";
            //
            // dgvPrestamos
            //
            this.dgvPrestamos.AllowUserToAddRows = false;
            this.dgvPrestamos.AllowUserToDeleteRows = false;
            this.dgvPrestamos.AllowUserToResizeRows = false;
            this.dgvPrestamos.BackgroundColor = System.Drawing.Color.White;
            this.dgvPrestamos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPrestamos.ColumnHeadersHeight = 34;
            this.dgvPrestamos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPrestamos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPrestamos.EnableHeadersVisualStyles = false;
            this.dgvPrestamos.Location = new System.Drawing.Point(30, 34);
            this.dgvPrestamos.MultiSelect = false;
            this.dgvPrestamos.Name = "dgvPrestamos";
            this.dgvPrestamos.ReadOnly = true;
            this.dgvPrestamos.RowHeadersVisible = false;
            this.dgvPrestamos.RowTemplate.Height = 28;
            this.dgvPrestamos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPrestamos.Size = new System.Drawing.Size(820, 280);
            this.dgvPrestamos.TabIndex = 1;
            this.dgvPrestamos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPrestamos_CellDoubleClick);
            //
            // FrmPrestamos
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(244, 246, 248);
            this.ClientSize = new System.Drawing.Size(880, 580);
            this.Controls.Add(this.pnlLista);
            this.Controls.Add(this.pnlDatos);
            this.Controls.Add(this.pnlTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmPrestamos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Préstamos";
            this.Load += new System.EventHandler(this.FrmPrestamos_Load);
            this.Shown += new System.EventHandler(this.FrmPrestamos_Shown);
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.pnlDatos.ResumeLayout(false);
            this.pnlDatos.PerformLayout();
            this.pnlLista.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrestamos)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlDatos;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.ComboBox cboUsuario;
        private System.Windows.Forms.Label lblLibro;
        private System.Windows.Forms.ComboBox cboLibro;
        private System.Windows.Forms.Label lblFechaPrestamo;
        private System.Windows.Forms.DateTimePicker dtpFechaPrestamo;
        private FontAwesome.Sharp.IconButton btnNuevo;
        private FontAwesome.Sharp.IconButton btnGuardar;
        private FontAwesome.Sharp.IconButton btnDevolver;
        private FontAwesome.Sharp.IconButton btnEliminar;
        private FontAwesome.Sharp.IconButton btnCancelar;
        private System.Windows.Forms.Panel pnlLista;
        private System.Windows.Forms.Label lblLista;
        private System.Windows.Forms.DataGridView dgvPrestamos;
    }
}
