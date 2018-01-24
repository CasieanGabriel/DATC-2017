using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Facebook;

namespace TableStorage
{
    public partial class Facebook_Likes_Counter : Form
    {
        private string ApplicationId = "141803926501574";
        private  string _ExtendedPermissions = "user_about_me, publish_actions, user_likes, user_location, user_photos, user_posts, user_hometown, user_about_me, email";
        FacebookClient fbClient = new FacebookClient();
        private string logOutURL = null;

        public Facebook_Likes_Counter(string logOutURL)
        {
            InitializeComponent();
            this.logOutURL = logOutURL;
        }

        public string AccessToken { get; set; }

        private void Login(object sender, EventArgs e)
        {
            if(this.logOutURL == "")
            {
                var destinationURL = String.Format(
               @"https://www.facebook.com/dialog/oauth?client_id={0}&scope={1}&redirect_uri=http://www.facebook.com/connect/login_success.html&response_type=token",
               this.ApplicationId, this._ExtendedPermissions);

                webBrowserLogin.Navigated += WebBrowserNavigated;

                webBrowserLogin.Navigate(destinationURL);
            } else
            {
                webBrowserLogin.Navigate(this.logOutURL);
            }
            
        }

        private void WebBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // get token
            var url = e.Url.Fragment;
            if (url.Contains("access_token") && url.Contains("#"))
            {
                this.Hide();
                url = (new Regex("#")).Replace(url, "?", 1);
                this.AccessToken = System.Web.HttpUtility.ParseQueryString(url).Get("access_token");
                Likes_Counter lk = new Likes_Counter(AccessToken);
                lk.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please Login");
            }
        }


        private void webBrowserLogin_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }
    }
}
