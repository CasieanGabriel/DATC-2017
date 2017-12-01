namespace GenerareDate
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
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxNr = new System.Windows.Forms.TextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Temperatura = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Umiditate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnGenerateChanges = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(56, 119);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(219, 29);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Genereaza date noi";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Numar senzori :";
            // 
            // textBoxNr
            // 
            this.textBoxNr.Location = new System.Drawing.Point(167, 69);
            this.textBoxNr.Name = "textBoxNr";
            this.textBoxNr.Size = new System.Drawing.Size(138, 22);
            this.textBoxNr.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.Temperatura,
            this.Umiditate});
            this.listView1.Location = new System.Drawing.Point(406, 69);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(305, 329);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 62;
            // 
            // Temperatura
            // 
            this.Temperatura.Text = "Temperatura";
            this.Temperatura.Width = 95;
            // 
            // Umiditate
            // 
            this.Umiditate.Text = "Umiditate";
            this.Umiditate.Width = 131;
            // 
            // btnGenerateChanges
            // 
            this.btnGenerateChanges.Location = new System.Drawing.Point(56, 174);
            this.btnGenerateChanges.Name = "btnGenerateChanges";
            this.btnGenerateChanges.Size = new System.Drawing.Size(234, 34);
            this.btnGenerateChanges.TabIndex = 4;
            this.btnGenerateChanges.Text = "Genereaza schimbari";
            this.btnGenerateChanges.UseVisualStyleBackColor = true;
            this.btnGenerateChanges.Click += new System.EventHandler(this.btnGenerateChanges_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(56, 327);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(93, 32);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(193, 326);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(112, 33);
            this.btnSend.TabIndex = 6;
            this.btnSend.Text = "Send to API";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 545);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGenerateChanges);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBoxNr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGenerate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxNr;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnGenerateChanges;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader Temperatura;
        private System.Windows.Forms.ColumnHeader Umiditate;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSend;
    }
}

