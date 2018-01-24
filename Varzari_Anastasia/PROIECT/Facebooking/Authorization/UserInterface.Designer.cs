namespace FaceSharp.Samples.WinForm
{
    partial class UserInterface
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
            this.getPostsLikesBtn = new System.Windows.Forms.Button();
            this.getPhotosLikesBtn = new System.Windows.Forms.Button();
            this.updatePhotosBtn = new System.Windows.Forms.Button();
            this.updatePostsBtn = new System.Windows.Forms.Button();
            this.totalSumBtn = new System.Windows.Forms.Button();
            this.clearBtn = new System.Windows.Forms.Button();
            this.getPostLikesTB = new System.Windows.Forms.TextBox();
            this.getPhotosLikesTB = new System.Windows.Forms.TextBox();
            this.totalSumTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // getPostsLikesBtn
            // 
            this.getPostsLikesBtn.Location = new System.Drawing.Point(43, 56);
            this.getPostsLikesBtn.Name = "getPostsLikesBtn";
            this.getPostsLikesBtn.Size = new System.Drawing.Size(133, 23);
            this.getPostsLikesBtn.TabIndex = 0;
            this.getPostsLikesBtn.Text = "Get likes for posts";
            this.getPostsLikesBtn.UseVisualStyleBackColor = true;
            this.getPostsLikesBtn.Click += new System.EventHandler(this.getPostsLikesBtn_Click);
            // 
            // getPhotosLikesBtn
            // 
            this.getPhotosLikesBtn.Location = new System.Drawing.Point(43, 85);
            this.getPhotosLikesBtn.Name = "getPhotosLikesBtn";
            this.getPhotosLikesBtn.Size = new System.Drawing.Size(133, 23);
            this.getPhotosLikesBtn.TabIndex = 1;
            this.getPhotosLikesBtn.Text = "Get likes for photos";
            this.getPhotosLikesBtn.UseVisualStyleBackColor = true;
            this.getPhotosLikesBtn.Click += new System.EventHandler(this.getPhotosLikesBtn_Click);
            // 
            // updatePhotosBtn
            // 
            this.updatePhotosBtn.Location = new System.Drawing.Point(43, 143);
            this.updatePhotosBtn.Name = "updatePhotosBtn";
            this.updatePhotosBtn.Size = new System.Drawing.Size(133, 23);
            this.updatePhotosBtn.TabIndex = 2;
            this.updatePhotosBtn.Text = "Update likes for photos";
            this.updatePhotosBtn.UseVisualStyleBackColor = true;
            this.updatePhotosBtn.Click += new System.EventHandler(this.updatePhotosBtn_Click);
            // 
            // updatePostsBtn
            // 
            this.updatePostsBtn.Location = new System.Drawing.Point(43, 114);
            this.updatePostsBtn.Name = "updatePostsBtn";
            this.updatePostsBtn.Size = new System.Drawing.Size(133, 23);
            this.updatePostsBtn.TabIndex = 3;
            this.updatePostsBtn.Text = "Update likes for posts";
            this.updatePostsBtn.UseVisualStyleBackColor = true;
            this.updatePostsBtn.Click += new System.EventHandler(this.updatePostsBtn_Click);
            // 
            // totalSumBtn
            // 
            this.totalSumBtn.Location = new System.Drawing.Point(43, 172);
            this.totalSumBtn.Name = "totalSumBtn";
            this.totalSumBtn.Size = new System.Drawing.Size(133, 23);
            this.totalSumBtn.TabIndex = 4;
            this.totalSumBtn.Text = "Total sum of likes";
            this.totalSumBtn.UseVisualStyleBackColor = true;
            this.totalSumBtn.Click += new System.EventHandler(this.totalSumBtn_Click);
            // 
            // clearBtn
            // 
            this.clearBtn.Location = new System.Drawing.Point(97, 230);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(133, 23);
            this.clearBtn.TabIndex = 5;
            this.clearBtn.Text = "Clear data";
            this.clearBtn.UseVisualStyleBackColor = true;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // getPostLikesTB
            // 
            this.getPostLikesTB.Location = new System.Drawing.Point(182, 58);
            this.getPostLikesTB.Name = "getPostLikesTB";
            this.getPostLikesTB.Size = new System.Drawing.Size(100, 20);
            this.getPostLikesTB.TabIndex = 6;
            // 
            // getPhotosLikesTB
            // 
            this.getPhotosLikesTB.Location = new System.Drawing.Point(182, 87);
            this.getPhotosLikesTB.Name = "getPhotosLikesTB";
            this.getPhotosLikesTB.Size = new System.Drawing.Size(100, 20);
            this.getPhotosLikesTB.TabIndex = 7;
            // 
            // totalSumTB
            // 
            this.totalSumTB.Location = new System.Drawing.Point(182, 175);
            this.totalSumTB.Name = "totalSumTB";
            this.totalSumTB.Size = new System.Drawing.Size(100, 20);
            this.totalSumTB.TabIndex = 10;
            // 
            // UserInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 306);
            this.Controls.Add(this.totalSumTB);
            this.Controls.Add(this.getPhotosLikesTB);
            this.Controls.Add(this.getPostLikesTB);
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.totalSumBtn);
            this.Controls.Add(this.updatePostsBtn);
            this.Controls.Add(this.updatePhotosBtn);
            this.Controls.Add(this.getPhotosLikesBtn);
            this.Controls.Add(this.getPostsLikesBtn);
            this.Name = "UserInterface";
            this.Text = "UserInterface";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getPostsLikesBtn;
        private System.Windows.Forms.Button getPhotosLikesBtn;
        private System.Windows.Forms.Button updatePhotosBtn;
        private System.Windows.Forms.Button updatePostsBtn;
        private System.Windows.Forms.Button totalSumBtn;
        private System.Windows.Forms.Button clearBtn;
        private System.Windows.Forms.TextBox getPostLikesTB;
        private System.Windows.Forms.TextBox getPhotosLikesTB;
        private System.Windows.Forms.TextBox totalSumTB;
    }
}