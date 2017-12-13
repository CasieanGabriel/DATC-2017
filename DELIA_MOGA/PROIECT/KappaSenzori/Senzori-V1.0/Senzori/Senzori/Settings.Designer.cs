namespace Senzori
{
    partial class Settings
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
            this.Temperature_min_bar = new System.Windows.Forms.TrackBar();
            this.Temperature_max_bar = new System.Windows.Forms.TrackBar();
            this.Humidity_min_bar = new System.Windows.Forms.TrackBar();
            this.Humidity_max_bar = new System.Windows.Forms.TrackBar();
            this.Temperature_min_txt = new System.Windows.Forms.TextBox();
            this.Temperature_max_txt = new System.Windows.Forms.TextBox();
            this.Humidity_min_txt = new System.Windows.Forms.TextBox();
            this.Humidity_max_txt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Save_Settings_btn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Temperature_min_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Temperature_max_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Humidity_min_bar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Humidity_max_bar)).BeginInit();
            this.SuspendLayout();
            // 
            // Temperature_min_bar
            // 
            this.Temperature_min_bar.Location = new System.Drawing.Point(72, 12);
            this.Temperature_min_bar.Name = "Temperature_min_bar";
            this.Temperature_min_bar.Size = new System.Drawing.Size(146, 45);
            this.Temperature_min_bar.TabIndex = 0;
            this.Temperature_min_bar.Scroll += new System.EventHandler(this.Temperature_min_bar_Scroll);
            // 
            // Temperature_max_bar
            // 
            this.Temperature_max_bar.Location = new System.Drawing.Point(72, 63);
            this.Temperature_max_bar.Name = "Temperature_max_bar";
            this.Temperature_max_bar.Size = new System.Drawing.Size(146, 45);
            this.Temperature_max_bar.TabIndex = 1;
            this.Temperature_max_bar.Scroll += new System.EventHandler(this.Temperature_max_bar_Scroll);
            // 
            // Humidity_min_bar
            // 
            this.Humidity_min_bar.Location = new System.Drawing.Point(72, 120);
            this.Humidity_min_bar.Name = "Humidity_min_bar";
            this.Humidity_min_bar.Size = new System.Drawing.Size(146, 45);
            this.Humidity_min_bar.TabIndex = 2;
            this.Humidity_min_bar.Scroll += new System.EventHandler(this.Humidity_min_bar_Scroll);
            // 
            // Humidity_max_bar
            // 
            this.Humidity_max_bar.Location = new System.Drawing.Point(72, 171);
            this.Humidity_max_bar.Name = "Humidity_max_bar";
            this.Humidity_max_bar.Size = new System.Drawing.Size(146, 45);
            this.Humidity_max_bar.TabIndex = 3;
            this.Humidity_max_bar.Scroll += new System.EventHandler(this.Humidity_max_bar_Scroll);
            // 
            // Temperature_min_txt
            // 
            this.Temperature_min_txt.BackColor = System.Drawing.Color.LightBlue;
            this.Temperature_min_txt.Location = new System.Drawing.Point(225, 13);
            this.Temperature_min_txt.Name = "Temperature_min_txt";
            this.Temperature_min_txt.Size = new System.Drawing.Size(40, 20);
            this.Temperature_min_txt.TabIndex = 4;
            this.Temperature_min_txt.TextChanged += new System.EventHandler(this.Temperature_min_txt_TextChanged);
            // 
            // Temperature_max_txt
            // 
            this.Temperature_max_txt.BackColor = System.Drawing.Color.PowderBlue;
            this.Temperature_max_txt.Location = new System.Drawing.Point(225, 63);
            this.Temperature_max_txt.Name = "Temperature_max_txt";
            this.Temperature_max_txt.Size = new System.Drawing.Size(40, 20);
            this.Temperature_max_txt.TabIndex = 5;
            this.Temperature_max_txt.TextChanged += new System.EventHandler(this.Temperature_max_txt_TextChanged);
            // 
            // Humidity_min_txt
            // 
            this.Humidity_min_txt.BackColor = System.Drawing.Color.PowderBlue;
            this.Humidity_min_txt.Location = new System.Drawing.Point(225, 120);
            this.Humidity_min_txt.Name = "Humidity_min_txt";
            this.Humidity_min_txt.Size = new System.Drawing.Size(40, 20);
            this.Humidity_min_txt.TabIndex = 6;
            this.Humidity_min_txt.TextChanged += new System.EventHandler(this.Humidity_min_txt_TextChanged);
            // 
            // Humidity_max_txt
            // 
            this.Humidity_max_txt.BackColor = System.Drawing.Color.PowderBlue;
            this.Humidity_max_txt.Location = new System.Drawing.Point(224, 171);
            this.Humidity_max_txt.Name = "Humidity_max_txt";
            this.Humidity_max_txt.Size = new System.Drawing.Size(41, 20);
            this.Humidity_max_txt.TabIndex = 7;
            this.Humidity_max_txt.TextChanged += new System.EventHandler(this.Humidity_max_txt_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Temp Min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Temp Max";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.label3.Location = new System.Drawing.Point(12, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Hum Min";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.label4.Location = new System.Drawing.Point(12, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Hum max";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.label5.Location = new System.Drawing.Point(271, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "ºC";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.label6.Location = new System.Drawing.Point(271, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(18, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "ºC";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.label7.Location = new System.Drawing.Point(271, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "%";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.LightSeaGreen;
            this.label8.Location = new System.Drawing.Point(271, 174);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "%";
            // 
            // Save_Settings_btn
            // 
            this.Save_Settings_btn.BackColor = System.Drawing.Color.LightSeaGreen;
            this.Save_Settings_btn.Location = new System.Drawing.Point(72, 214);
            this.Save_Settings_btn.Name = "Save_Settings_btn";
            this.Save_Settings_btn.Size = new System.Drawing.Size(146, 23);
            this.Save_Settings_btn.TabIndex = 16;
            this.Save_Settings_btn.Text = "Save Settings";
            this.Save_Settings_btn.UseVisualStyleBackColor = false;
            this.Save_Settings_btn.Click += new System.EventHandler(this.Save_Settings_btn_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(304, 249);
            this.Controls.Add(this.Save_Settings_btn);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Humidity_max_txt);
            this.Controls.Add(this.Humidity_min_txt);
            this.Controls.Add(this.Temperature_max_txt);
            this.Controls.Add(this.Temperature_min_txt);
            this.Controls.Add(this.Humidity_max_bar);
            this.Controls.Add(this.Humidity_min_bar);
            this.Controls.Add(this.Temperature_max_bar);
            this.Controls.Add(this.Temperature_min_bar);
            this.Name = "Settings";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.Temperature_min_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Temperature_max_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Humidity_min_bar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Humidity_max_bar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar Temperature_min_bar;
        private System.Windows.Forms.TrackBar Temperature_max_bar;
        private System.Windows.Forms.TrackBar Humidity_min_bar;
        private System.Windows.Forms.TrackBar Humidity_max_bar;
        private System.Windows.Forms.TextBox Temperature_min_txt;
        private System.Windows.Forms.TextBox Temperature_max_txt;
        private System.Windows.Forms.TextBox Humidity_min_txt;
        private System.Windows.Forms.TextBox Humidity_max_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button Save_Settings_btn;
    }
}