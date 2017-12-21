namespace GenerareV3
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
            this.components = new System.ComponentModel.Container();
            this.listView1 = new System.Windows.Forms.ListView();
            this.IdSenzor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Latitudine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Longitudine = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.getSensors = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.listView2 = new System.Windows.Forms.ListView();
            this.senzor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.temperatura = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.umiditate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.presiune = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.data = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnInregistrari = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.GetSensorsNoInsert = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IdSenzor,
            this.Latitudine,
            this.Longitudine});
            this.listView1.Location = new System.Drawing.Point(209, 67);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(334, 159);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // IdSenzor
            // 
            this.IdSenzor.Text = "IdSenzor";
            this.IdSenzor.Width = 113;
            // 
            // Latitudine
            // 
            this.Latitudine.Text = "Latitudine";
            this.Latitudine.Width = 108;
            // 
            // Longitudine
            // 
            this.Longitudine.Text = "Longitudine";
            this.Longitudine.Width = 105;
            // 
            // getSensors
            // 
            this.getSensors.Location = new System.Drawing.Point(48, 67);
            this.getSensors.Name = "getSensors";
            this.getSensors.Size = new System.Drawing.Size(121, 31);
            this.getSensors.TabIndex = 1;
            this.getSensors.Text = "GenSensors";
            this.getSensors.UseVisualStyleBackColor = true;
            this.getSensors.Click += new System.EventHandler(this.getSensors_Click);
            // 
            // listView2
            // 
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.senzor,
            this.temperatura,
            this.umiditate,
            this.presiune,
            this.data});
            this.listView2.Location = new System.Drawing.Point(209, 232);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(535, 178);
            this.listView2.TabIndex = 3;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // senzor
            // 
            this.senzor.Text = "senzor";
            // 
            // temperatura
            // 
            this.temperatura.Text = "temperatura";
            this.temperatura.Width = 112;
            // 
            // umiditate
            // 
            this.umiditate.Text = "umiditate";
            this.umiditate.Width = 101;
            // 
            // presiune
            // 
            this.presiune.Text = "presiune";
            this.presiune.Width = 113;
            // 
            // data
            // 
            this.data.Text = "data";
            // 
            // btnInregistrari
            // 
            this.btnInregistrari.Location = new System.Drawing.Point(23, 232);
            this.btnInregistrari.Name = "btnInregistrari";
            this.btnInregistrari.Size = new System.Drawing.Size(167, 35);
            this.btnInregistrari.TabIndex = 4;
            this.btnInregistrari.Text = "GenereazaInregistrari";
            this.btnInregistrari.UseVisualStyleBackColor = true;
            this.btnInregistrari.Click += new System.EventHandler(this.btnInregistrari_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // GetSensorsNoInsert
            // 
            this.GetSensorsNoInsert.Location = new System.Drawing.Point(48, 123);
            this.GetSensorsNoInsert.Name = "GetSensorsNoInsert";
            this.GetSensorsNoInsert.Size = new System.Drawing.Size(142, 30);
            this.GetSensorsNoInsert.TabIndex = 5;
            this.GetSensorsNoInsert.Text = "SensorsNoInsert";
            this.GetSensorsNoInsert.UseVisualStyleBackColor = true;
            this.GetSensorsNoInsert.Click += new System.EventHandler(this.GetSensorsNoInsert_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Location = new System.Drawing.Point(771, 104);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteAll.TabIndex = 6;
            this.btnDeleteAll.Text = "DeleteAll";
            this.btnDeleteAll.UseVisualStyleBackColor = true;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 522);
            this.Controls.Add(this.btnDeleteAll);
            this.Controls.Add(this.GetSensorsNoInsert);
            this.Controls.Add(this.btnInregistrari);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.getSensors);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button getSensors;
        private System.Windows.Forms.ColumnHeader IdSenzor;
        private System.Windows.Forms.ColumnHeader Latitudine;
        private System.Windows.Forms.ColumnHeader Longitudine;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.Button btnInregistrari;
        private System.Windows.Forms.ColumnHeader senzor;
        private System.Windows.Forms.ColumnHeader temperatura;
        private System.Windows.Forms.ColumnHeader umiditate;
        private System.Windows.Forms.ColumnHeader presiune;
        private System.Windows.Forms.ColumnHeader data;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button GetSensorsNoInsert;
        private System.Windows.Forms.Button btnDeleteAll;
    }
}

