namespace Senzori
{
    partial class Sensors_Control
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Sensor_no_lbl = new System.Windows.Forms.Label();
            this.Sensors_no_txt = new System.Windows.Forms.TextBox();
            this.Generate_btn = new System.Windows.Forms.Button();
            this.Sensors_Data_dgv = new System.Windows.Forms.DataGridView();
            this.Field = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Temperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Humidity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Upload_Data_btn = new System.Windows.Forms.Button();
            this.Sensors_Values_lbl = new System.Windows.Forms.Label();
            this.Sensors_Values_txt = new System.Windows.Forms.TextBox();
            this.Settings_btn = new System.Windows.Forms.Button();
            this.Error_data_cb = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.Sensors_Data_dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // Sensor_no_lbl
            // 
            this.Sensor_no_lbl.AutoSize = true;
            this.Sensor_no_lbl.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.Sensor_no_lbl.Location = new System.Drawing.Point(13, 14);
            this.Sensor_no_lbl.Name = "Sensor_no_lbl";
            this.Sensor_no_lbl.Size = new System.Drawing.Size(45, 13);
            this.Sensor_no_lbl.TabIndex = 0;
            this.Sensor_no_lbl.Text = "Sensors";
            // 
            // Sensors_no_txt
            // 
            this.Sensors_no_txt.BackColor = System.Drawing.Color.PowderBlue;
            this.Sensors_no_txt.Location = new System.Drawing.Point(64, 11);
            this.Sensors_no_txt.Name = "Sensors_no_txt";
            this.Sensors_no_txt.Size = new System.Drawing.Size(89, 20);
            this.Sensors_no_txt.TabIndex = 1;
            // 
            // Generate_btn
            // 
            this.Generate_btn.BackColor = System.Drawing.Color.LightSeaGreen;
            this.Generate_btn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Generate_btn.Location = new System.Drawing.Point(12, 77);
            this.Generate_btn.Name = "Generate_btn";
            this.Generate_btn.Size = new System.Drawing.Size(141, 23);
            this.Generate_btn.TabIndex = 2;
            this.Generate_btn.Text = "Generate";
            this.Generate_btn.UseVisualStyleBackColor = false;
            this.Generate_btn.Click += new System.EventHandler(this.Generate_btn_Click);
            // 
            // Sensors_Data_dgv
            // 
            this.Sensors_Data_dgv.AllowUserToAddRows = false;
            this.Sensors_Data_dgv.AllowUserToResizeColumns = false;
            this.Sensors_Data_dgv.AllowUserToResizeRows = false;
            this.Sensors_Data_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Sensors_Data_dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.Sensors_Data_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Sensors_Data_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Field,
            this.Temperature,
            this.Humidity});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightSeaGreen;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Sensors_Data_dgv.DefaultCellStyle = dataGridViewCellStyle2;
            this.Sensors_Data_dgv.Location = new System.Drawing.Point(168, 11);
            this.Sensors_Data_dgv.Name = "Sensors_Data_dgv";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Teal;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Sensors_Data_dgv.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Teal;
            this.Sensors_Data_dgv.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.Sensors_Data_dgv.Size = new System.Drawing.Size(353, 275);
            this.Sensors_Data_dgv.TabIndex = 3;
            // 
            // Field
            // 
            this.Field.FillWeight = 45.68528F;
            this.Field.HeaderText = "Field";
            this.Field.Name = "Field";
            this.Field.ReadOnly = true;
            this.Field.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Field.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Temperature
            // 
            this.Temperature.FillWeight = 127.1574F;
            this.Temperature.HeaderText = "Temperature";
            this.Temperature.Name = "Temperature";
            this.Temperature.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Temperature.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Humidity
            // 
            this.Humidity.FillWeight = 127.1574F;
            this.Humidity.HeaderText = "Humidity";
            this.Humidity.Name = "Humidity";
            this.Humidity.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Humidity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Upload_Data_btn
            // 
            this.Upload_Data_btn.BackColor = System.Drawing.Color.LightSeaGreen;
            this.Upload_Data_btn.Location = new System.Drawing.Point(12, 106);
            this.Upload_Data_btn.Name = "Upload_Data_btn";
            this.Upload_Data_btn.Size = new System.Drawing.Size(141, 23);
            this.Upload_Data_btn.TabIndex = 4;
            this.Upload_Data_btn.Text = "Upload";
            this.Upload_Data_btn.UseVisualStyleBackColor = false;
            this.Upload_Data_btn.Click += new System.EventHandler(this.Upload_Data_btn_Click);
            // 
            // Sensors_Values_lbl
            // 
            this.Sensors_Values_lbl.AutoSize = true;
            this.Sensors_Values_lbl.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.Sensors_Values_lbl.Location = new System.Drawing.Point(19, 42);
            this.Sensors_Values_lbl.Name = "Sensors_Values_lbl";
            this.Sensors_Values_lbl.Size = new System.Drawing.Size(39, 13);
            this.Sensors_Values_lbl.TabIndex = 5;
            this.Sensors_Values_lbl.Text = "Values";
            // 
            // Sensors_Values_txt
            // 
            this.Sensors_Values_txt.BackColor = System.Drawing.Color.PowderBlue;
            this.Sensors_Values_txt.Location = new System.Drawing.Point(64, 42);
            this.Sensors_Values_txt.Name = "Sensors_Values_txt";
            this.Sensors_Values_txt.Size = new System.Drawing.Size(89, 20);
            this.Sensors_Values_txt.TabIndex = 6;
            // 
            // Settings_btn
            // 
            this.Settings_btn.BackColor = System.Drawing.Color.IndianRed;
            this.Settings_btn.Location = new System.Drawing.Point(12, 263);
            this.Settings_btn.Name = "Settings_btn";
            this.Settings_btn.Size = new System.Drawing.Size(141, 23);
            this.Settings_btn.TabIndex = 7;
            this.Settings_btn.Text = "Settings";
            this.Settings_btn.UseVisualStyleBackColor = false;
            this.Settings_btn.Click += new System.EventHandler(this.Settings_btn_Click);
            // 
            // Error_data_cb
            // 
            this.Error_data_cb.AutoSize = true;
            this.Error_data_cb.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.Error_data_cb.Location = new System.Drawing.Point(12, 149);
            this.Error_data_cb.Name = "Error_data_cb";
            this.Error_data_cb.Size = new System.Drawing.Size(76, 17);
            this.Error_data_cb.TabIndex = 8;
            this.Error_data_cb.Text = "Send Error";
            this.Error_data_cb.UseVisualStyleBackColor = true;
            // 
            // Sensors_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(533, 299);
            this.Controls.Add(this.Error_data_cb);
            this.Controls.Add(this.Settings_btn);
            this.Controls.Add(this.Sensors_Values_txt);
            this.Controls.Add(this.Sensors_Values_lbl);
            this.Controls.Add(this.Upload_Data_btn);
            this.Controls.Add(this.Sensors_Data_dgv);
            this.Controls.Add(this.Generate_btn);
            this.Controls.Add(this.Sensors_no_txt);
            this.Controls.Add(this.Sensor_no_lbl);
            this.Name = "Sensors_Control";
            this.Text = "Sensors Control";
            ((System.ComponentModel.ISupportInitialize)(this.Sensors_Data_dgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Sensor_no_lbl;
        private System.Windows.Forms.TextBox Sensors_no_txt;
        private System.Windows.Forms.Button Generate_btn;
        private System.Windows.Forms.Button Upload_Data_btn;
        private System.Windows.Forms.Label Sensors_Values_lbl;
        private System.Windows.Forms.TextBox Sensors_Values_txt;
        private System.Windows.Forms.Button Settings_btn;
        private System.Windows.Forms.CheckBox Error_data_cb;
        private System.Windows.Forms.DataGridViewTextBoxColumn Field;
        private System.Windows.Forms.DataGridViewTextBoxColumn Temperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn Humidity;
        private System.Windows.Forms.DataGridView Sensors_Data_dgv;
    }
}

