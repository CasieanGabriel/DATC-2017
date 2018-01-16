using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookingTest;
using Facebook;
//using System.Net.Http;

namespace FaceSharp.Samples.WinForm
{
    public partial class Get_Likes_Form : Form
    {
        public int counter_likes = 0;
        public int counter_photo_likes = 0;
        int one_click_posts = 0;
        int one_click_photos = 0;
        public Get_Likes_Form()
        {
   
            InitializeComponent();
        }
        Authorize authorize = new Authorize();
        private void posts_likes_Click(object sender, EventArgs e)
        {
           
            var fb = new FacebookClient(Authorize.AccessToken);
            dynamic result = fb.Get("me/posts?fields=likes.summary(true)&limit=10");

          if (one_click_posts==0)
          {
            foreach (var entry in result.data)
            {
                counter_likes += entry.likes.summary.total_count;
            }
            likes_txt.Text = counter_likes.ToString();

          }
            one_click_posts = 1;
           
        }



        private void photo_likes_Click(object sender, EventArgs e)
        {
            var fb = new FacebookClient(Authorize.AccessToken);
            dynamic result = fb.Get("me/photos?fields=likes.summary(true)&limit=10000");
            if (one_click_photos == 0)
            {
                foreach (var entry in result.data)
                {
                    counter_photo_likes += entry.likes.summary.total_count;
                }
                photos_text.Text = counter_photo_likes.ToString();
            }
            one_click_photos = 1;
        }

        private void Sum_Click(object sender, EventArgs e)
        {
            
                Sum_box.Text = (counter_likes + counter_photo_likes).ToString();
        }

        private void photos_text_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }

