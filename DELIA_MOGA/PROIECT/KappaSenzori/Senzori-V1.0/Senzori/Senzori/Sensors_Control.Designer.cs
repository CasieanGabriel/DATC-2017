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
            this.Sensor_no_lbl = new System.Windows.Forms.Label();
            this.Sensors_no_txt = new System.Windows.Forms.TextBox();
            this.Generate_btn = new System.Windows.Forms.Button();
            this.Sensors_Data_dgv = new System.Windows.Forms.DataGridView();
            this.Sensor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Temperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Humidity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Upload_Data_btn = new System.Windows.Forms.Button();
            this.Sensors_Values_lbl = new System.Windows.Forms.Label();
            this.Sensors_Values_txt = new System.Windows.Forms.TextBox();
            this.Settings_btn = new System.Windows.Forms.Button();
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
            this.Sensors_Data_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Sensors_Data_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sensor,
            this.Temperature,
            this.Humidity});
            this.Sensors_Data_dgv.Location = new System.Drawing.Point(168, 11);
            this.Sensors_Data_dgv.Name = "Sensors_Data_dgv";
            this.Sensors_Data_dgv.Size = new System.Drawing.Size(353, 275);
            this.Sensors_Data_dgv.TabIndex = 3;
            // 
            // Sensor
            // 
            this.Sensor.FillWeight = 45.68528F;
            this.Sensor.HeaderText = "Sensor";
            this.Sensor.Name = "Sensor";
            this.Sensor.ReadOnly = true;
            this.Sensor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Sensor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // Sensors_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(533, 299);
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
        private System.Windows.Forms.DataGridView Sensors_Data_dgv;
        private System.Windows.Forms.Button Upload_Data_btn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sensor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Temperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn Humidity;
        private System.Windows.Forms.Label Sensors_Values_lbl;
        private System.Windows.Forms.TextBox Sensors_Values_txt;
        private System.Windows.Forms.Button Settings_btn;
    }
}

