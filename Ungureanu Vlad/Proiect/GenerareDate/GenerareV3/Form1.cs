using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenerareV3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Senzor> l = new List<Senzor>();
        Random rnd = new Random();
        static HttpClient client = new HttpClient();

        private void getSensors_Click(object sender, EventArgs e)
        {
                StreamReader sr = new StreamReader("senzori.txt");
           
            string line = "";

            while((line = sr.ReadLine())!=null)
            {
                string[] word = line.Split(' ');
                Senzor s = new Senzor(Convert.ToInt32(word[1]), word[2], word[3]);
                l.Add(s);
            }
            foreach(Senzor s in l)
            {
                ListViewItem lv = new ListViewItem(Convert.ToString(s.idsenzor));
                lv.SubItems.Add(s.latitudine);
                lv.SubItems.Add(s.longitudine);
                listView1.Items.Add(lv);
            }

            foreach (Senzor senz in l)
            {
                try
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(senz);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var result = client.PostAsync("http://localhost:50922/api/todo", content).Result;
                    //  MessageBox.Show(Convert.ToString(result));
                }
                catch { MessageBox.Show("Nu se poate face Post, error problem"); }
            }


        }

        private void btnInregistrari_Click(object sender, EventArgs e)
        {
            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 20000; // every 20 seconds
            firstinrestrigation();
            timer1.Start();

            timer2 = new Timer();
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 30000; // every 20 seconds
            timer2.Start();
        }

        List<Inregistrare> li = new List<Inregistrare>();

        private void firstinrestrigation()
        {
            foreach(Senzor s in l)
            {
                int idsenzor = s.idsenzor;
                int temperatura = rnd.Next(0, 45);
                int umiditate = rnd.Next(0, 100);
                int presiune = rnd.Next(900,1100);
                DateTime data = DateTime.Now;

                Inregistrare i = new Inregistrare(idsenzor,temperatura,umiditate,presiune,data);
                li.Add(i);
            }

            foreach(Inregistrare i in li)
            {
                ListViewItem lvi = new ListViewItem(Convert.ToString(i.idsenzor));
                lvi.SubItems.Add(Convert.ToString(i.temperatura));
                lvi.SubItems.Add(Convert.ToString(i.umiditate));
                lvi.SubItems.Add(Convert.ToString(i.presiune));
                lvi.SubItems.Add(Convert.ToString(i.data));
                listView2.Items.Add(lvi);
            }

            foreach (Inregistrare i in li)
            {
                try
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(i);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var result = client.PostAsync("http://localhost:50922/api/Inregistrare", content).Result;
                    //  MessageBox.Show(Convert.ToString(result));
                }
                catch
                {
                    MessageBox.Show("Nu se poate face Post, error problem");
                }
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
           foreach(Inregistrare i in li)
            {

                if (i.umiditate <= 25)
                {
                    i.umiditate = i.umiditate + rnd.Next(5, 50);
                    i.temperatura = i.temperatura - rnd.Next(1, 5);
                    i.presiune = i.presiune - rnd.Next(10, 100);
                }
                if (i.umiditate > 25)
                {
                    i.umiditate = i.umiditate - rnd.Next(5, 50);
                    if (i.umiditate < 0)
                    {
                        i.umiditate = 0;
                    }
                    i.temperatura = i.temperatura + rnd.Next(1, 5);
                    i.presiune = i.presiune + rnd.Next(10, 100);
                }
                i.data = DateTime.Now;
            }
            foreach (Inregistrare i in li)
            {
                ListViewItem lvi = new ListViewItem(Convert.ToString(i.idsenzor));
                lvi.SubItems.Add(Convert.ToString(i.temperatura));
                lvi.SubItems.Add(Convert.ToString(i.umiditate));
                lvi.SubItems.Add(Convert.ToString(i.presiune));
                lvi.SubItems.Add(Convert.ToString(i.data));
                listView2.Items.Add(lvi);
            }

            foreach (Inregistrare i in li)
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(i);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var result = client.PostAsync("http://localhost:50922/api/Inregistrare", content).Result;
                //  MessageBox.Show(Convert.ToString(result));
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
           
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            listView1.Items.Clear();

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "silviu.database.windows.net";
            builder.UserID = "silviumilu";
            builder.Password = "!Silviu1";
            builder.InitialCatalog = "proiect";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("\nQuery data example:");
                Console.WriteLine("=========================================\n");

                connection.Open();

                var queryString = $"delete from TabelaSenzori";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.ExecuteNonQuery();
                }
                var queryString1 = $"delete from TabelaInregistrari";
                using (SqlCommand command = new SqlCommand(queryString1, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Inregistrare i = new Inregistrare(1,9000,5000,1000, DateTime.Now);


            ListViewItem lvi = new ListViewItem(Convert.ToString(i.idsenzor));
            lvi.SubItems.Add(Convert.ToString(i.temperatura));
            lvi.SubItems.Add(Convert.ToString(i.umiditate));
            lvi.SubItems.Add(Convert.ToString(i.presiune));
            lvi.SubItems.Add(Convert.ToString(i.data));
            listView2.Items.Add(lvi);

            try
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(i);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var result = client.PostAsync("http://localhost:50922/api/Inregistrare", content).Result;
            }
            catch
            {
                MessageBox.Show("Nu se poate face Post, error problem");
            }
        }
    }
}
