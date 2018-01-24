using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Facebook;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TableStorage.Model;

namespace TableStorage
{
    public partial class Likes_Counter : Form
    {
        FacebookClient client = new FacebookClient();
        int totalLikesValue = 0;
        private string access_token;
        string tableName = "Likes_Counter" + Guid.NewGuid().ToString().Substring(0, 5);

        public  Likes_Counter(string access_token)
        {
            InitializeComponent();
            this.access_token = access_token;
        }

   
        private async void LikesAsync(string number_of_loaded_likes)
        {

            string tableName = "FacebookLikesData" + Guid.NewGuid().ToString().Substring(0, 5);

            // Create or reference an existing table
            CloudTable table = await Common.CreateTableAsync(tableName);
          
            var client = new FacebookClient();
            client.AccessToken = this.access_token;

          

            //show user's profile picture
            dynamic me = client.Get("me/posts?fields=likes,message,created_time,full_picture&limit=" + number_of_loaded_likes);
            var facebookData = me.data;

            foreach (var element in facebookData)
            {
                if (element.message != null)
                {

                    FacebookLikesEntity post = new FacebookLikesEntity(element.id, element.created_time)
                    {
                        Message_or_photo = element.message,
                        Total_likes = 0
                    };

                    post = await InsertOrMergeEntityAsync(table, post);

                    ListViewItem item = new ListViewItem(element.message);
                    item.SubItems.Add(element.created_time);

                    if (element.likes != null)
                    {
                        string x = Convert.ToString(element.likes.data.Count);
                        item.SubItems.Add(x);
                        TotalLikesForPosts(element.likes.data.Count, 0);
                        post.Total_likes = element.likes.data.Count;
                        await InsertOrMergeEntityAsync(table, post);

                    }
                    else
                    {
                        item.SubItems.Add("No likes for this post");
                    }
                    postsList.Items.Add(item);

                }
                else if (element.full_picture != null)
                {

                    FacebookLikesEntity post = new FacebookLikesEntity(element.id, element.created_time)
                    {
                        Message_or_photo = element.full_picture,
                        Total_likes = 0
                    };

                    post = await InsertOrMergeEntityAsync(table, post);

                    ListViewItem item = new ListViewItem(element.full_picture);
                    item.SubItems.Add(element.created_time);
                    if (element.likes != null)
                    {
                        string x = Convert.ToString(element.likes.data.Count);
                        item.SubItems.Add(x);
                        post.Total_likes = element.likes.data.Count;
                        TotalLikesForPosts(element.likes.data.Count, 0);
                        await InsertOrMergeEntityAsync(table, post);
                    }
                    else
                    {
                        item.SubItems.Add("No likes for this photo");
                      
                    }
                    postsList.Items.Add(item);
                }
            }
                this.deleteTables.Click += (sender, EventArgs) => { deleteTables_ClickAsync(sender, EventArgs, table); };
        }

  

        public void TotalLikesForPosts(int likes, int reset)
        {
            if(reset == 1)
            {
                totalLikesValue = 0;
            }

            totalLikesValue = totalLikesValue + likes;
            string x = Convert.ToString(totalLikesValue);
            totalLikes.Text = x;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.TotalLikesForPosts(0, 1);
            ListViewItem item = new ListViewItem();
            postsList.Items.Clear();
            decimal Posts = wantedPosts.Value;
            string number_of_loaded_likes = Convert.ToString(Posts);
            this.LikesAsync(number_of_loaded_likes);
        }

        public static string GetLogoutURL(string AccessToken)
        {
            var fb = new FacebookClient();
            var logoutUrl = fb.GetLogoutUrl(new { access_token = AccessToken, next = "https://www.facebook.com/connect/login_success.html" });
            return logoutUrl.ToString();
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            var logOutURL = GetLogoutURL(this.access_token);
            this.Hide();
            Facebook_Likes_Counter flk = new Facebook_Likes_Counter(logOutURL);
            flk.ShowDialog();
            
        }

        private static async Task<FacebookLikesEntity> InsertOrMergeEntityAsync(CloudTable table, FacebookLikesEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {
                // Create the InsertOrReplace table operation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);
                FacebookLikesEntity insertedCustomer = result.Result as FacebookLikesEntity;

                return insertedCustomer;
            }
            catch (StorageException e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
        private void deleteTables_ClickAsync(object sender, EventArgs e, CloudTable table)
        {
            table.DeleteIfExistsAsync();
            MessageBox.Show("All the stored data are deleted! \n" + "Table " + table + " is deleted!" );
        }

        private void postMessage_Click(object sender, EventArgs e)
        {
            string postMessage = message.Text;
            var fb = new FacebookClient(this.access_token);
            dynamic result = fb.Post("me/feed", new { message = postMessage });
            var newPostId = result.id;
            MessageBox.Show("Successfully posted");
        }
    }
}
