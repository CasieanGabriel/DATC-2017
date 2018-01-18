namespace TableStorage
{
    partial class Likes_Counter
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
            this.label1 = new System.Windows.Forms.Label();
            this.postsList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.wantedPosts = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.deleteTables = new System.Windows.Forms.Button();
            this.message = new System.Windows.Forms.TextBox();
            this.postMessage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.totalLikes = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.wantedPosts)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(467, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Facebook Like Counter";
            // 
            // postsList
            // 
            this.postsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.postsList.Location = new System.Drawing.Point(12, 185);
            this.postsList.Name = "postsList";
            this.postsList.Size = new System.Drawing.Size(1188, 410);
            this.postsList.TabIndex = 3;
            this.postsList.UseCompatibleStateImageBehavior = false;
            this.postsList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Message or Photo";
            this.columnHeader1.Width = 487;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Time when posted";
            this.columnHeader2.Width = 226;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Total Likes";
            this.columnHeader3.Width = 113;
            // 
            // wantedPosts
            // 
            this.wantedPosts.Location = new System.Drawing.Point(267, 130);
            this.wantedPosts.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.wantedPosts.Name = "wantedPosts";
            this.wantedPosts.Size = new System.Drawing.Size(120, 20);
            this.wantedPosts.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(414, 130);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 20);
            this.button1.TabIndex = 5;
            this.button1.Text = "Load Posts";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(17, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(248, 17);
            this.label5.TabIndex = 6;
            this.label5.Text = "How many likes would you like to load:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1081, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "LogOut";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // deleteTables
            // 
            this.deleteTables.Location = new System.Drawing.Point(544, 130);
            this.deleteTables.Name = "deleteTables";
            this.deleteTables.Size = new System.Drawing.Size(136, 23);
            this.deleteTables.TabIndex = 8;
            this.deleteTables.Text = "Delete the stored data";
            this.deleteTables.UseVisualStyleBackColor = true;
            // 
            // message
            // 
            this.message.Location = new System.Drawing.Point(789, 64);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(367, 20);
            this.message.TabIndex = 9;
            // 
            // postMessage
            // 
            this.postMessage.Location = new System.Drawing.Point(1080, 130);
            this.postMessage.Name = "postMessage";
            this.postMessage.Size = new System.Drawing.Size(75, 23);
            this.postMessage.TabIndex = 10;
            this.postMessage.Text = "Post";
            this.postMessage.UseVisualStyleBackColor = true;
            this.postMessage.Click += new System.EventHandler(this.postMessage_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(13, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(174, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Total Likes Counted:";
            // 
            // totalLikes
            // 
            this.totalLikes.AutoSize = true;
            this.totalLikes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalLikes.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.totalLikes.Location = new System.Drawing.Point(193, 64);
            this.totalLikes.Name = "totalLikes";
            this.totalLikes.Size = new System.Drawing.Size(19, 20);
            this.totalLikes.TabIndex = 12;
            this.totalLikes.Text = "0";
            // 
            // Likes_Counter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1227, 639);
            this.Controls.Add(this.totalLikes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.postMessage);
            this.Controls.Add(this.message);
            this.Controls.Add(this.deleteTables);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.wantedPosts);
            this.Controls.Add(this.postsList);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Likes_Counter";
            this.Text = "Facebook Likes Counter";
            ((System.ComponentModel.ISupportInitialize)(this.wantedPosts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView postsList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.NumericUpDown wantedPosts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button deleteTables;
        private System.Windows.Forms.TextBox message;
        private System.Windows.Forms.Button postMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label totalLikes;
    }
}