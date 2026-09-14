namespace SistemaBiblioteca
{
    partial class FrmLibros
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
            this.btnEditar = new FontAwesome.Sharp.IconButton();
            this.btnGuardar = new FontAwesome.Sharp.IconButton();
            this.btnNuevo = new FontAwesome.Sharp.IconButton();
            this.btnBuscar = new FontAwesome.Sharp.IconButton();
            this.txtExistencias = new System.Windows.Forms.TextBox();
            this.lblExistencias = new System.Windows.Forms.Label();
            this.txtAnio = new System.Windows.Forms.TextBox();
            this.lblAnio = new System.Windows.Forms.Label();
            this.txtCategoria = new System.Windows.Forms.TextBox();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.cboEditorial = new System.Windows.Forms.ComboBox();
            this.lblEditorial = new System.Windows.Forms.Label();
            this.cboAutor = new System.Windows.Forms.ComboBox();
            this.lblAutor = new System.Windows.Forms.Label();
            this.txtTitulo = new System.Windows.Forms.TextBox();
            this.lblTituloLibro = new System.Windows.Forms.Label();
            this.txtISBN = new System.Windows.Forms.TextBox();
            this.lblISBN = new System.Windows.Forms.Label();
            this.pnlLista = new System.Windows.Forms.Panel();
            this.dgvLibros = new System.Windows.Forms.DataGridView();
            this.lblLista = new System.Windows.Forms.Label();
            this.pnlTitulo.SuspendLayout();
            this.pnlDatos.SuspendLayout();
            this.pnlLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLibros)).BeginInit();
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
            this.lblTitulo.Size = new System.Drawing.Size(180, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestión de Libros";
            //
            // pnlDatos
            //
            this.pnlDatos.BackColor = System.Drawing.Color.White;
            this.pnlDatos.Controls.Add(this.btnCancelar);
            this.pnlDatos.Controls.Add(this.btnEliminar);
            this.pnlDatos.Controls.Add(this.btnEditar);
            this.pnlDatos.Controls.Add(this.btnGuardar);
            this.pnlDatos.Controls.Add(this.btnNuevo);
            this.pnlDatos.Controls.Add(this.btnBuscar);
            this.pnlDatos.Controls.Add(this.txtExistencias);
            this.pnlDatos.Controls.Add(this.lblExistencias);
            this.pnlDatos.Controls.Add(this.txtAnio);
            this.pnlDatos.Controls.Add(this.lblAnio);
            this.pnlDatos.Controls.Add(this.txtCategoria);
            this.pnlDatos.Controls.Add(this.lblCategoria);
            this.pnlDatos.Controls.Add(this.cboEditorial);
            this.pnlDatos.Controls.Add(this.lblEditorial);
            this.pnlDatos.Controls.Add(this.cboAutor);
            this.pnlDatos.Controls.Add(this.lblAutor);
            this.pnlDatos.Controls.Add(this.txtTitulo);
            this.pnlDatos.Controls.Add(this.lblTituloLibro);
            this.pnlDatos.Controls.Add(this.txtISBN);
            this.pnlDatos.Controls.Add(this.lblISBN);
            this.pnlDatos.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDatos.Location = new System.Drawing.Point(0, 56);
            this.pnlDatos.Name = "pnlDatos";
            this.pnlDatos.Size = new System.Drawing.Size(880, 236);
            this.pnlDatos.TabIndex = 1;
            //
            // lblISBN
            //
            this.lblISBN.AutoSize = true;
            this.lblISBN.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblISBN.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblISBN.Location = new System.Drawing.Point(30, 14);
            this.lblISBN.Name = "lblISBN";
            this.lblISBN.Size = new System.Drawing.Size(118, 17);
            this.lblISBN.TabIndex = 0;
            this.lblISBN.Text = "ISBN  (llave, único)";
            //
            // txtISBN
            //
            this.txtISBN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtISBN.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtISBN.Location = new System.Drawing.Point(30, 34);
            this.txtISBN.MaxLength = 20;
            this.txtISBN.Name = "txtISBN";
            this.txtISBN.Size = new System.Drawing.Size(200, 25);
            this.txtISBN.TabIndex = 1;
            //
            // btnBuscar
            //
            this.btnBuscar.BackColor = System.Drawing.Color.FromArgb(15, 76, 92);
            this.btnBuscar.FlatAppearance.BorderSize = 0;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnBuscar.ForeColor = System.Drawing.Color.White;
            this.btnBuscar.IconChar = FontAwesome.Sharp.IconChar.MagnifyingGlass;
            this.btnBuscar.IconColor = System.Drawing.Color.White;
            this.btnBuscar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBuscar.IconSize = 16;
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(238, 34);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 25);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "  Buscar";
            this.btnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBuscar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            //
            // lblTituloLibro
            //
            this.lblTituloLibro.AutoSize = true;
            this.lblTituloLibro.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblTituloLibro.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblTituloLibro.Location = new System.Drawing.Point(368, 14);
            this.lblTituloLibro.Name = "lblTituloLibro";
            this.lblTituloLibro.Size = new System.Drawing.Size(43, 17);
            this.lblTituloLibro.TabIndex = 3;
            this.lblTituloLibro.Text = "Título";
            //
            // txtTitulo
            //
            this.txtTitulo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTitulo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTitulo.Location = new System.Drawing.Point(368, 34);
            this.txtTitulo.MaxLength = 200;
            this.txtTitulo.Name = "txtTitulo";
            this.txtTitulo.Size = new System.Drawing.Size(452, 25);
            this.txtTitulo.TabIndex = 4;
            //
            // lblAutor
            //
            this.lblAutor.AutoSize = true;
            this.lblAutor.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblAutor.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblAutor.Location = new System.Drawing.Point(30, 70);
            this.lblAutor.Name = "lblAutor";
            this.lblAutor.Size = new System.Drawing.Size(40, 17);
            this.lblAutor.TabIndex = 5;
            this.lblAutor.Text = "Autor";
            //
            // cboAutor
            //
            this.cboAutor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAutor.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cboAutor.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboAutor.Location = new System.Drawing.Point(30, 90);
            this.cboAutor.Name = "cboAutor";
            this.cboAutor.Size = new System.Drawing.Size(240, 25);
            this.cboAutor.TabIndex = 6;
            //
            // lblEditorial
            //
            this.lblEditorial.AutoSize = true;
            this.lblEditorial.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblEditorial.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblEditorial.Location = new System.Drawing.Point(290, 70);
            this.lblEditorial.Name = "lblEditorial";
            this.lblEditorial.Size = new System.Drawing.Size(58, 17);
            this.lblEditorial.TabIndex = 7;
            this.lblEditorial.Text = "Editorial";
            //
            // cboEditorial
            //
            this.cboEditorial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEditorial.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cboEditorial.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboEditorial.Location = new System.Drawing.Point(290, 90);
            this.cboEditorial.Name = "cboEditorial";
            this.cboEditorial.Size = new System.Drawing.Size(240, 25);
            this.cboEditorial.TabIndex = 8;
            //
            // lblCategoria
            //
            this.lblCategoria.AutoSize = true;
            this.lblCategoria.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblCategoria.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblCategoria.Location = new System.Drawing.Point(550, 70);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(140, 17);
            this.lblCategoria.TabIndex = 9;
            this.lblCategoria.Text = "Categoría  (opcional)";
            //
            // txtCategoria
            //
            this.txtCategoria.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCategoria.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCategoria.Location = new System.Drawing.Point(550, 90);
            this.txtCategoria.MaxLength = 100;
            this.txtCategoria.Name = "txtCategoria";
            this.txtCategoria.Size = new System.Drawing.Size(270, 25);
            this.txtCategoria.TabIndex = 10;
            //
            // lblAnio
            //
            this.lblAnio.AutoSize = true;
            this.lblAnio.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblAnio.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblAnio.Location = new System.Drawing.Point(30, 126);
            this.lblAnio.Name = "lblAnio";
            this.lblAnio.Size = new System.Drawing.Size(119, 17);
            this.lblAnio.TabIndex = 11;
            this.lblAnio.Text = "Año  (opcional)";
            //
            // txtAnio
            //
            this.txtAnio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAnio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAnio.Location = new System.Drawing.Point(30, 146);
            this.txtAnio.MaxLength = 4;
            this.txtAnio.Name = "txtAnio";
            this.txtAnio.Size = new System.Drawing.Size(120, 25);
            this.txtAnio.TabIndex = 12;
            //
            // lblExistencias
            //
            this.lblExistencias.AutoSize = true;
            this.lblExistencias.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblExistencias.ForeColor = System.Drawing.Color.FromArgb(70, 70, 70);
            this.lblExistencias.Location = new System.Drawing.Point(170, 126);
            this.lblExistencias.Name = "lblExistencias";
            this.lblExistencias.Size = new System.Drawing.Size(79, 17);
            this.lblExistencias.TabIndex = 13;
            this.lblExistencias.Text = "Existencias";
            //
            // txtExistencias
            //
            this.txtExistencias.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExistencias.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtExistencias.Location = new System.Drawing.Point(170, 146);
            this.txtExistencias.MaxLength = 6;
            this.txtExistencias.Name = "txtExistencias";
            this.txtExistencias.Size = new System.Drawing.Size(120, 25);
            this.txtExistencias.TabIndex = 14;
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
            this.btnNuevo.Location = new System.Drawing.Point(30, 182);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(120, 40);
            this.btnNuevo.TabIndex = 15;
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
            this.btnGuardar.Location = new System.Drawing.Point(160, 182);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(120, 40);
            this.btnGuardar.TabIndex = 16;
            this.btnGuardar.Text = "  Guardar";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnEditar
            //
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(15, 76, 92);
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnEditar.ForeColor = System.Drawing.Color.White;
            this.btnEditar.IconChar = FontAwesome.Sharp.IconChar.PenToSquare;
            this.btnEditar.IconColor = System.Drawing.Color.White;
            this.btnEditar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnEditar.IconSize = 18;
            this.btnEditar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditar.Location = new System.Drawing.Point(290, 182);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(120, 40);
            this.btnEditar.TabIndex = 17;
            this.btnEditar.Text = "  Editar";
            this.btnEditar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnEditar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
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
            this.btnEliminar.Location = new System.Drawing.Point(420, 182);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(120, 40);
            this.btnEliminar.TabIndex = 18;
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
            this.btnCancelar.Location = new System.Drawing.Point(550, 182);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(120, 40);
            this.btnCancelar.TabIndex = 19;
            this.btnCancelar.Text = "  Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // pnlLista
            //
            this.pnlLista.BackColor = System.Drawing.Color.FromArgb(244, 246, 248);
            this.pnlLista.Controls.Add(this.dgvLibros);
            this.pnlLista.Controls.Add(this.lblLista);
            this.pnlLista.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLista.Location = new System.Drawing.Point(0, 292);
            this.pnlLista.Name = "pnlLista";
            this.pnlLista.Padding = new System.Windows.Forms.Padding(30, 10, 30, 20);
            this.pnlLista.Size = new System.Drawing.Size(880, 288);
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
            this.lblLista.Text = "Libros registrados  (doble clic en una fila para cargarla en el formulario)";
            //
            // dgvLibros
            //
            this.dgvLibros.AllowUserToAddRows = false;
            this.dgvLibros.AllowUserToDeleteRows = false;
            this.dgvLibros.AllowUserToResizeRows = false;
            this.dgvLibros.BackgroundColor = System.Drawing.Color.White;
            this.dgvLibros.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLibros.ColumnHeadersHeight = 34;
            this.dgvLibros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvLibros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLibros.EnableHeadersVisualStyles = false;
            this.dgvLibros.Location = new System.Drawing.Point(30, 34);
            this.dgvLibros.MultiSelect = false;
            this.dgvLibros.Name = "dgvLibros";
            this.dgvLibros.ReadOnly = true;
            this.dgvLibros.RowHeadersVisible = false;
            this.dgvLibros.RowTemplate.Height = 28;
            this.dgvLibros.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLibros.Size = new System.Drawing.Size(820, 234);
            this.dgvLibros.TabIndex = 1;
            this.dgvLibros.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLibros_CellDoubleClick);
            //
            // FrmLibros
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
            this.Name = "FrmLibros";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Libros";
            this.Load += new System.EventHandler(this.FrmLibros_Load);
            this.Shown += new System.EventHandler(this.FrmLibros_Shown);
            this.pnlTitulo.ResumeLayout(false);
            this.pnlTitulo.PerformLayout();
            this.pnlDatos.ResumeLayout(false);
            this.pnlDatos.PerformLayout();
            this.pnlLista.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLibros)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlDatos;
        private System.Windows.Forms.Label lblISBN;
        private System.Windows.Forms.TextBox txtISBN;
        private FontAwesome.Sharp.IconButton btnBuscar;
        private System.Windows.Forms.Label lblTituloLibro;
        private System.Windows.Forms.TextBox txtTitulo;
        private System.Windows.Forms.Label lblAutor;
        private System.Windows.Forms.ComboBox cboAutor;
        private System.Windows.Forms.Label lblEditorial;
        private System.Windows.Forms.ComboBox cboEditorial;
        private System.Windows.Forms.Label lblCategoria;
        private System.Windows.Forms.TextBox txtCategoria;
        private System.Windows.Forms.Label lblAnio;
        private System.Windows.Forms.TextBox txtAnio;
        private System.Windows.Forms.Label lblExistencias;
        private System.Windows.Forms.TextBox txtExistencias;
        private FontAwesome.Sharp.IconButton btnNuevo;
        private FontAwesome.Sharp.IconButton btnGuardar;
        private FontAwesome.Sharp.IconButton btnEditar;
        private FontAwesome.Sharp.IconButton btnEliminar;
        private FontAwesome.Sharp.IconButton btnCancelar;
        private System.Windows.Forms.Panel pnlLista;
        private System.Windows.Forms.Label lblLista;
        private System.Windows.Forms.DataGridView dgvLibros;
    }
}
