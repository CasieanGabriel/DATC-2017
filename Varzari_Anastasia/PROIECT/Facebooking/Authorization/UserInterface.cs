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
using System.Net.Http;


namespace FaceSharp.Samples.WinForm
{
    public partial class UserInterface : Form
    {
        public UserInterface()
        {
            sendNameAndID();
            InitializeComponent();
        }

        Authorize authorize = new Authorize();
              
        private void getPostsLikesBtn_Click(object sender, EventArgs e)
        {
            
            var fb = new FacebookClient(Authorize.AccessToken);
            dynamic result = fb.Get("me/posts?fields=likes.summary(true)&limit=10000");
            int counter = 0;

            foreach (var entry in result.data)
            {
                counter += entry.likes.summary.total_count;
            }
            getPostLikesTB.Text = counter.ToString();

        }

        private void getPhotosLikesBtn_Click(object sender, EventArgs e)
        {
            var fb = new FacebookClient(Authorize.AccessToken);
            dynamic result = fb.Get("me/photos?fields=likes.summary(true)&limit=10000");
            int counter = 0;
            foreach (var entry in result.data)
            {
                counter += entry.likes.summary.total_count;
            }
            getPhotosLikesTB.Text = counter.ToString();
        }

        private void updatePostsBtn_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            StringContent stringContent = new StringContent("");
            var userId = authorize.getUserID().ToString();
            var postsLikes = int.Parse(getPostLikesTB.Text);
            var response = client.PostAsync("http://localhost:55088/api/posts/" + userId + "?postsLikes=" + postsLikes, stringContent).Result;
        }

        private void totalSumBtn_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
           
            var userId = authorize.getUserID().ToString();

            var response = client.GetAsync("http://localhost:55088/api/posts/" + userId).Result;
            var postsLikes = response.Content.ReadAsStringAsync().Result;

            var response1 = client.GetAsync("http://localhost:55088/api/values/" + userId).Result;
            var photosLikes = response1.Content.ReadAsStringAsync().Result;
           
            var totalSum = Int32.Parse(postsLikes) + Int32.Parse(photosLikes);

            totalSumTB.Text = totalSum.ToString();
        }

        private void sendNameAndID()
        {
            HttpClient client = new HttpClient();
            
            var userId = authorize.getUserID().ToString();
            var userName = authorize.getUserName().ToString();

            StringContent stringContent = new StringContent("");
            var response = client.PostAsync("http://localhost:55088/api/username/" + userId + "?name=" + userName, stringContent).Result;

        }

        private void updatePhotosBtn_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            StringContent stringContent = new StringContent("");
            var userId = authorize.getUserID().ToString();
            var photosLikes = int.Parse(getPhotosLikesTB.Text);
            var response = client.PostAsync("http://localhost:55088/api/values/" + userId + "?photosLikes=" + photosLikes, stringContent).Result;

        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            var userID = authorize.getUserID();
            var response = client.DeleteAsync("http://localhost:55088/api/values/"+userID);
        }
    }
}
