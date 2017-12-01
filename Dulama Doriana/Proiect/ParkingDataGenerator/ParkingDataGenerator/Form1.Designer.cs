namespace ParkingDataGenerator
{
    partial class Form1
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
            this.buttonGenerateData = new System.Windows.Forms.Button();
            this.listBoxParkingData = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonGenerateData
            // 
            this.buttonGenerateData.Location = new System.Drawing.Point(267, 46);
            this.buttonGenerateData.Name = "buttonGenerateData";
            this.buttonGenerateData.Size = new System.Drawing.Size(209, 80);
            this.buttonGenerateData.TabIndex = 0;
            this.buttonGenerateData.Text = "Generate data";
            this.buttonGenerateData.UseVisualStyleBackColor = true;
            this.buttonGenerateData.Click += new System.EventHandler(this.buttonGenerateData_Click);
            // 
            // listBoxParkingData
            // 
            this.listBoxParkingData.FormattingEnabled = true;
            this.listBoxParkingData.Location = new System.Drawing.Point(225, 132);
            this.listBoxParkingData.Name = "listBoxParkingData";
            this.listBoxParkingData.Size = new System.Drawing.Size(276, 277);
            this.listBoxParkingData.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 431);
            this.Controls.Add(this.listBoxParkingData);
            this.Controls.Add(this.buttonGenerateData);
            this.Name = "Form1";
            this.Text = "Parking data generator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGenerateData;
        private System.Windows.Forms.ListBox listBoxParkingData;
    }
}

