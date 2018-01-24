using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Facebook;
namespace WebApplication1
{
    public partial class facebook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            checkAuthorization();
        }
        private void checkAuthorization()
        {
            string app_id = "398515607244535";
            string app_secret = "957274bda10384329f6c644a052d7910";
            string scope = "publish_actions";
            string access_token1 = "";

            if (Request["code"] == null)
            {
                Response.Redirect(string.Format(
                    "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}&scope={2}",
                    app_id, Request.Url.AbsoluteUri, scope));

            }
            else
            {
                Dictionary<string, string> tokens = new Dictionary<string, string>();
                string url = string.Format(
                    "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&scope={2}&code={3}&client_secret={4}",
                    app_id, Request.Url.AbsoluteUri, scope, Request["code"].ToString(), app_secret);

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string vals = reader.ReadToEnd();
                    int length = vals.Length - 62; 
                     access_token1 = vals.Substring(17, length);

                    foreach (string token in vals.Split('&'))
                    {
                        //tokens.Add(token.Substring(0, token.IndexOf('=')),
                        // token.Substring(token.IndexOf('=') + 1, token.Length - token.IndexOf('=') - 1));
                        //"{\"access_token\":\"EAAFqcqU1DvcBAGYknNDZCeic3oE0YZBvN9ajHMCvPExpnOvG7PlcBZAUlwHsup6EV2ZCQ2I91c60HZCgzZBapJIDBZC1mJEZATLmXaGlhlTj897GHYMEV1cfPa7DuPYpCXC7ZCNGl11ozwadxLenHXqfTIqld4DdZBlQL2xnQ8tyZAaigZDZD\",\"token_type\":\"bearer\",\"expires_in\":5182680}"
                        //""EAAFqcqU1DvcBAEqTriC5U9PZBd4EFjP32gOrsuzhNZAVMt1yr67NPlfTZCPg8dop3wjnLWEsttBIRQk1urqDDwE4wzkwVEyjf1lFsbbm9aEKZCJ6pu0X7x044JZANgCs7jZCk5w8tolDMCOqaEJL67eua2zZCvICYEnm8S23Vg0EQZDZD"
                    }
                }
                string access_token = ""; 
                var client = new FacebookClient(access_token1);
                client.Post("/me/feed", new { message = "posted from webb app" });
            }

        }
    }
}