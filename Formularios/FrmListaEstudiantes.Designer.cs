namespace RegistroEstudiantes.Formularios
{
    partial class FrmListaEstudiantes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dgvEstudiantes = new DataGridView();
            Carnet = new DataGridViewTextBoxColumn();
            Nombrecompleto = new DataGridViewTextBoxColumn();
            Sexo = new DataGridViewTextBoxColumn();
            Carrera = new DataGridViewTextBoxColumn();
            Nivel = new DataGridViewTextBoxColumn();
            Correo = new DataGridViewTextBoxColumn();
            Promedio = new DataGridViewTextBoxColumn();
            btnVerDetalle = new Button();
            btnBuscar = new Button();
            txtBuscar = new TextBox();
            btnEditar = new Button();
            btnDesactivar = new Button();
            btnBloqueante = new Button();
            btnProcesar = new Button();
            btnCancelar = new Button();
            prgProceso = new ProgressBar();
            lblEstado = new Label();
            txtLog = new TextBox();
            btnResumenPLINQ = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvEstudiantes).BeginInit();
            SuspendLayout();
            // 
            // dgvEstudiantes
            // 
            dgvEstudiantes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEstudiantes.Columns.AddRange(new DataGridViewColumn[] { Carnet, Nombrecompleto, Sexo, Carrera, Nivel, Correo, Promedio });
            dgvEstudiantes.Location = new Point(0, 0);
            dgvEstudiantes.MultiSelect = false;
            dgvEstudiantes.Name = "dgvEstudiantes";
            dgvEstudiantes.ReadOnly = true;
            dgvEstudiantes.RowHeadersWidth = 51;
            dgvEstudiantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstudiantes.Size = new Size(800, 412);
            dgvEstudiantes.TabIndex = 0;
            // 
            // Carnet
            // 
            Carnet.HeaderText = "Carnet";
            Carnet.MinimumWidth = 6;
            Carnet.Name = "Carnet";
            Carnet.ReadOnly = true;
            Carnet.Width = 125;
            // 
            // Nombrecompleto
            // 
            Nombrecompleto.HeaderText = "Nombre completo";
            Nombrecompleto.MinimumWidth = 6;
            Nombrecompleto.Name = "Nombrecompleto";
            Nombrecompleto.ReadOnly = true;
            Nombrecompleto.Width = 125;
            // 
            // Sexo
            // 
            Sexo.HeaderText = "Sexo";
            Sexo.MinimumWidth = 6;
            Sexo.Name = "Sexo";
            Sexo.ReadOnly = true;
            Sexo.Width = 125;
            // 
            // Carrera
            // 
            Carrera.HeaderText = "Carrera";
            Carrera.MinimumWidth = 6;
            Carrera.Name = "Carrera";
            Carrera.ReadOnly = true;
            Carrera.Width = 125;
            // 
            // Nivel
            // 
            Nivel.HeaderText = "Nivel";
            Nivel.MinimumWidth = 6;
            Nivel.Name = "Nivel";
            Nivel.ReadOnly = true;
            Nivel.Width = 125;
            // 
            // Correo
            // 
            Correo.HeaderText = "Correo";
            Correo.MinimumWidth = 6;
            Correo.Name = "Correo";
            Correo.ReadOnly = true;
            Correo.Width = 125;
            // 
            // Promedio
            // 
            Promedio.HeaderText = "Promedio";
            Promedio.MinimumWidth = 6;
            Promedio.Name = "Promedio";
            Promedio.ReadOnly = true;
            Promedio.Width = 125;
            // 
            // btnVerDetalle
            // 
            btnVerDetalle.Location = new Point(0, 418);
            btnVerDetalle.Name = "btnVerDetalle";
            btnVerDetalle.Size = new Size(218, 36);
            btnVerDetalle.TabIndex = 1;
            btnVerDetalle.Text = "Detalle";
            btnVerDetalle.UseVisualStyleBackColor = true;
            btnVerDetalle.Click += btnVerDetalle_Click;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(224, 418);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(218, 36);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(470, 423);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(181, 27);
            txtBuscar.TabIndex = 3;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(0, 478);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(218, 36);
            btnEditar.TabIndex = 4;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnDesactivar
            // 
            btnDesactivar.Location = new Point(224, 478);
            btnDesactivar.Name = "btnDesactivar";
            btnDesactivar.Size = new Size(218, 36);
            btnDesactivar.TabIndex = 5;
            btnDesactivar.Text = "Desactivar";
            btnDesactivar.UseVisualStyleBackColor = true;
            btnDesactivar.Click += btnDesactivar_Click;
            // 
            // btnBloqueante
            // 
            btnBloqueante.Location = new Point(470, 477);
            btnBloqueante.Name = "btnBloqueante";
            btnBloqueante.Size = new Size(199, 37);
            btnBloqueante.TabIndex = 6;
            btnBloqueante.Text = "Bloqueo";
            btnBloqueante.UseVisualStyleBackColor = true;
            btnBloqueante.Click += btnBloqueante_Click;
            // 
            // btnProcesar
            // 
            btnProcesar.Location = new Point(688, 478);
            btnProcesar.Name = "btnProcesar";
            btnProcesar.Size = new Size(199, 37);
            btnProcesar.TabIndex = 7;
            btnProcesar.Text = "Procesar";
            btnProcesar.UseVisualStyleBackColor = true;
            btnProcesar.Click += btnProcesar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(579, 540);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(199, 37);
            btnCancelar.TabIndex = 8;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // prgProceso
            // 
            prgProceso.Location = new Point(61, 559);
            prgProceso.Name = "prgProceso";
            prgProceso.Size = new Size(414, 48);
            prgProceso.TabIndex = 9;
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(247, 621);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(54, 20);
            lblEstado.TabIndex = 10;
            lblEstado.Text = "Estado";
            // 
            // txtLog
            // 
            txtLog.Location = new Point(577, 609);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(319, 34);
            txtLog.TabIndex = 11;
            // 
            // btnResumenPLINQ
            // 
            btnResumenPLINQ.Location = new Point(820, 76);
            btnResumenPLINQ.Name = "btnResumenPLINQ";
            btnResumenPLINQ.Size = new Size(219, 68);
            btnResumenPLINQ.TabIndex = 12;
            btnResumenPLINQ.Text = "Resumen de Estudiantes";
            btnResumenPLINQ.UseVisualStyleBackColor = true;
            btnResumenPLINQ.Click += btnResumenPLINQ_Click;
            // 
            // FrmListaEstudiantes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1063, 681);
            Controls.Add(btnResumenPLINQ);
            Controls.Add(txtLog);
            Controls.Add(lblEstado);
            Controls.Add(prgProceso);
            Controls.Add(btnCancelar);
            Controls.Add(btnProcesar);
            Controls.Add(btnBloqueante);
            Controls.Add(btnDesactivar);
            Controls.Add(btnEditar);
            Controls.Add(txtBuscar);
            Controls.Add(btnBuscar);
            Controls.Add(btnVerDetalle);
            Controls.Add(dgvEstudiantes);
            Name = "FrmListaEstudiantes";
            Text = "FrmListaEstudiantes";
            Load += FrmListaEstudiantes_Load;
            ((System.ComponentModel.ISupportInitialize)dgvEstudiantes).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvEstudiantes;
        private DataGridViewTextBoxColumn Carnet;
        private DataGridViewTextBoxColumn Nombrecompleto;
        private DataGridViewTextBoxColumn Sexo;
        private DataGridViewTextBoxColumn Carrera;
        private DataGridViewTextBoxColumn Nivel;
        private DataGridViewTextBoxColumn Correo;
        private DataGridViewTextBoxColumn Promedio;
        private Button btnVerDetalle;
        private Button btnBuscar;
        private TextBox txtBuscar;
        private Button btnEditar;
        private Button btnDesactivar;
        private Button btnBloqueante;
        private Button btnProcesar;
        private Button btnCancelar;
        private ProgressBar prgProceso;
        private Label lblEstado;
        private TextBox txtLog;
        private Button btnResumenPLINQ;
    }
}