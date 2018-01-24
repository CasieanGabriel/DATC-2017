namespace RandomDataGenerator
{
    partial class generateOutOfRange
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
            this.setRangeBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ganarateInRangebtn = new System.Windows.Forms.Button();
            this.textBoxtempMin = new System.Windows.Forms.TextBox();
            this.textBoxHumMax = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxHumMin = new System.Windows.Forms.TextBox();
            this.textBoxTempMax = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // setRangeBtn
            // 
            this.setRangeBtn.Location = new System.Drawing.Point(12, 170);
            this.setRangeBtn.Name = "setRangeBtn";
            this.setRangeBtn.Size = new System.Drawing.Size(75, 23);
            this.setRangeBtn.TabIndex = 1;
            this.setRangeBtn.Text = "Set range";
            this.setRangeBtn.UseVisualStyleBackColor = true;
            this.setRangeBtn.Click += new System.EventHandler(this.setRangeBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Give the range :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Minimum Temperature";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // ganarateInRangebtn
            // 
            this.ganarateInRangebtn.Location = new System.Drawing.Point(12, 215);
            this.ganarateInRangebtn.Name = "ganarateInRangebtn";
            this.ganarateInRangebtn.Size = new System.Drawing.Size(108, 23);
            this.ganarateInRangebtn.TabIndex = 4;
            this.ganarateInRangebtn.Text = "Generate in range";
            this.ganarateInRangebtn.UseVisualStyleBackColor = true;
            this.ganarateInRangebtn.Click += new System.EventHandler(this.ganarateInRangebtn_Click);
            // 
            // textBoxtempMin
            // 
            this.textBoxtempMin.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxtempMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxtempMin.Location = new System.Drawing.Point(131, 39);
            this.textBoxtempMin.Name = "textBoxtempMin";
            this.textBoxtempMin.Size = new System.Drawing.Size(43, 20);
            this.textBoxtempMin.TabIndex = 5;
            // 
            // textBoxHumMax
            // 
            this.textBoxHumMax.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxHumMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxHumMax.Location = new System.Drawing.Point(131, 136);
            this.textBoxHumMax.Name = "textBoxHumMax";
            this.textBoxHumMax.Size = new System.Drawing.Size(43, 20);
            this.textBoxHumMax.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Maximum Humidity";
            // 
            // textBoxHumMin
            // 
            this.textBoxHumMin.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxHumMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxHumMin.Location = new System.Drawing.Point(131, 103);
            this.textBoxHumMin.Name = "textBoxHumMin";
            this.textBoxHumMin.Size = new System.Drawing.Size(43, 20);
            this.textBoxHumMin.TabIndex = 8;
            // 
            // textBoxTempMax
            // 
            this.textBoxTempMax.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxTempMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTempMax.Location = new System.Drawing.Point(131, 70);
            this.textBoxTempMax.Name = "textBoxTempMax";
            this.textBoxTempMax.Size = new System.Drawing.Size(43, 20);
            this.textBoxTempMax.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Maximum temperature ";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Minimum Humidity";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(131, 215);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "Generate out of range";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // generateOutOfRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(417, 298);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxTempMax);
            this.Controls.Add(this.textBoxHumMin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxHumMax);
            this.Controls.Add(this.textBoxtempMin);
            this.Controls.Add(this.ganarateInRangebtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.setRangeBtn);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "generateOutOfRange";
            this.Text = "Data Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button setRangeBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ganarateInRangebtn;
        private System.Windows.Forms.TextBox textBoxtempMin;
        private System.Windows.Forms.TextBox textBoxHumMax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxHumMin;
        private System.Windows.Forms.TextBox textBoxTempMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button3;
    }
}

