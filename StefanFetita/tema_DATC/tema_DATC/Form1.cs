using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace tema_DATC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string GET(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                    return errorText;
                }
                
            }
        }

        // POST a JSON string
        void POST(string url, string jsonContent)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                }
            }
            catch (WebException ex)
            {
                // Log exception and throw as for GET example above
            }
        }

        public string getString(string p)
        {
            WebClient proxy = new WebClient();
            string url = string.Format("http://datc-rest.azurewebsites.net/breweries/{0}", p);
            byte[] data = proxy.DownloadData(url);
            Stream mem = new MemoryStream(data);
            var reader = new StreamReader(mem);
            var result = reader.ReadToEnd();
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Json_content = textBox1.Text;
            if (comboBox1.Text == "POST")
            {
                POST("http://datc-rest.azurewebsites.net/breweries", Json_content);
            }
            if (comboBox1.Text == "GET") 
            {
                listBox1.Text = getString(Json_content);
            }

        }
            
    }
}
