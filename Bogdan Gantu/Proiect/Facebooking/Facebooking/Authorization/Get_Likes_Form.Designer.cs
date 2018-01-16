namespace FaceSharp.Samples.WinForm
{
    partial class Get_Likes_Form
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
            this.posts_likes = new System.Windows.Forms.Button();
            this.photo_likes = new System.Windows.Forms.Button();
            this.likes_txt = new System.Windows.Forms.TextBox();
            this.photos_text = new System.Windows.Forms.TextBox();
            this.Sum = new System.Windows.Forms.Button();
            this.Sum_box = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // posts_likes
            // 
            this.posts_likes.Location = new System.Drawing.Point(41, 45);
            this.posts_likes.Name = "posts_likes";
            this.posts_likes.Size = new System.Drawing.Size(114, 29);
            this.posts_likes.TabIndex = 0;
            this.posts_likes.Text = "Posts_Likes";
            this.posts_likes.UseVisualStyleBackColor = true;
            this.posts_likes.Click += new System.EventHandler(this.posts_likes_Click);
            // 
            // photo_likes
            // 
            this.photo_likes.Location = new System.Drawing.Point(41, 149);
            this.photo_likes.Name = "photo_likes";
            this.photo_likes.Size = new System.Drawing.Size(114, 32);
            this.photo_likes.TabIndex = 1;
            this.photo_likes.Text = "Photos_Likes";
            this.photo_likes.UseVisualStyleBackColor = true;
            this.photo_likes.Click += new System.EventHandler(this.photo_likes_Click);
            // 
            // likes_txt
            // 
            this.likes_txt.Location = new System.Drawing.Point(255, 45);
            this.likes_txt.Name = "likes_txt";
            this.likes_txt.Size = new System.Drawing.Size(100, 20);
            this.likes_txt.TabIndex = 2;
            // 
            // photos_text
            // 
            this.photos_text.Location = new System.Drawing.Point(255, 161);
            this.photos_text.Name = "photos_text";
            this.photos_text.Size = new System.Drawing.Size(100, 20);
            this.photos_text.TabIndex = 3;
            this.photos_text.TextChanged += new System.EventHandler(this.photos_text_TextChanged);
            // 
            // Sum
            // 
            this.Sum.Location = new System.Drawing.Point(41, 251);
            this.Sum.Name = "Sum";
            this.Sum.Size = new System.Drawing.Size(114, 32);
            this.Sum.TabIndex = 4;
            this.Sum.Text = "Likes_Sum";
            this.Sum.UseVisualStyleBackColor = true;
            this.Sum.Click += new System.EventHandler(this.Sum_Click);
            // 
            // Sum_box
            // 
            this.Sum_box.Location = new System.Drawing.Point(255, 258);
            this.Sum_box.Name = "Sum_box";
            this.Sum_box.Size = new System.Drawing.Size(100, 20);
            this.Sum_box.TabIndex = 5;
            // 
            // Get_Likes_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 353);
            this.Controls.Add(this.Sum_box);
            this.Controls.Add(this.Sum);
            this.Controls.Add(this.photos_text);
            this.Controls.Add(this.likes_txt);
            this.Controls.Add(this.photo_likes);
            this.Controls.Add(this.posts_likes);
            this.Name = "Get_Likes_Form";
            this.Text = "Get_Likes_Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button posts_likes;
        private System.Windows.Forms.Button photo_likes;
        private System.Windows.Forms.TextBox likes_txt;
        private System.Windows.Forms.TextBox photos_text;
        private System.Windows.Forms.Button Sum;
        private System.Windows.Forms.TextBox Sum_box;
    }
}