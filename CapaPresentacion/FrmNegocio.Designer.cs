namespace CapaPresentacion
{
    partial class FrmNegocio
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
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGuardarCambios = new FontAwesome.Sharp.IconButton();
            this.txtDireccion = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRUC = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCargar = new FontAwesome.Sharp.IconButton();
            this.label2 = new System.Windows.Forms.Label();
            this.piclogo = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.piclogo)).BeginInit();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.DarkCyan;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(14, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(192, 29);
            this.label9.TabIndex = 21;
            this.label9.Text = "Detalle Negocio";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DarkCyan;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(779, 723);
            this.label1.TabIndex = 20;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightSeaGreen;
            this.groupBox1.Controls.Add(this.btnGuardarCambios);
            this.groupBox1.Controls.Add(this.txtDireccion);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtRUC);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtNombre);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnCargar);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.piclogo);
            this.groupBox1.Location = new System.Drawing.Point(19, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 550);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            // 
            // btnGuardarCambios
            // 
            this.btnGuardarCambios.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            this.btnGuardarCambios.IconColor = System.Drawing.Color.Black;
            this.btnGuardarCambios.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnGuardarCambios.IconSize = 30;
            this.btnGuardarCambios.Location = new System.Drawing.Point(343, 295);
            this.btnGuardarCambios.Name = "btnGuardarCambios";
            this.btnGuardarCambios.Size = new System.Drawing.Size(307, 40);
            this.btnGuardarCambios.TabIndex = 9;
            this.btnGuardarCambios.Text = "Guardar Cambios";
            this.btnGuardarCambios.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGuardarCambios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardarCambios.UseVisualStyleBackColor = true;
            this.btnGuardarCambios.Click += new System.EventHandler(this.btnGuardarCambios_Click);
            // 
            // txtDireccion
            // 
            this.txtDireccion.Location = new System.Drawing.Point(343, 226);
            this.txtDireccion.Name = "txtDireccion";
            this.txtDireccion.Size = new System.Drawing.Size(307, 22);
            this.txtDireccion.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(340, 191);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 16);
            this.label5.TabIndex = 7;
            this.label5.Text = "Dirección:";
            // 
            // txtRUC
            // 
            this.txtRUC.Location = new System.Drawing.Point(343, 145);
            this.txtRUC.Name = "txtRUC";
            this.txtRUC.Size = new System.Drawing.Size(307, 22);
            this.txtRUC.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(340, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "R.U.C:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(343, 63);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(307, 22);
            this.txtNombre.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(340, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Nombre del Negocio:";
            // 
            // btnCargar
            // 
            this.btnCargar.IconChar = FontAwesome.Sharp.IconChar.Upload;
            this.btnCargar.IconColor = System.Drawing.Color.Black;
            this.btnCargar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCargar.IconSize = 30;
            this.btnCargar.Location = new System.Drawing.Point(38, 295);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(253, 40);
            this.btnCargar.TabIndex = 2;
            this.btnCargar.Text = "Cargar";
            this.btnCargar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCargar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCargar.UseVisualStyleBackColor = true;
            this.btnCargar.Click += new System.EventHandler(this.btnCargar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(35, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Logo";
            // 
            // piclogo
            // 
            this.piclogo.BackColor = System.Drawing.Color.White;
            this.piclogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.piclogo.Location = new System.Drawing.Point(38, 63);
            this.piclogo.Name = "piclogo";
            this.piclogo.Size = new System.Drawing.Size(253, 216);
            this.piclogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.piclogo.TabIndex = 0;
            this.piclogo.TabStop = false;
            // 
            // FrmNegocio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1393, 723);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Name = "FrmNegocio";
            this.Text = "FrmNegocio";
            this.Load += new System.EventHandler(this.FrmNegocio_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.piclogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private FontAwesome.Sharp.IconButton btnCargar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox piclogo;
        private FontAwesome.Sharp.IconButton btnGuardarCambios;
        private System.Windows.Forms.TextBox txtDireccion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRUC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label3;
    }
}