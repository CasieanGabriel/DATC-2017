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
using System.Threading;
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

            while ((line = sr.ReadLine()) != null)
            {
                string[] word = line.Split(' ');
                Senzor s = new Senzor(Convert.ToInt32(word[0]), word[1], word[2]);
                l.Add(s);
            }
            foreach (Senzor s in l)
            {
                ListViewItem lv = new ListViewItem(Convert.ToString(s.idsenzor));
                lv.SubItems.Add(s.latitudine);
                lv.SubItems.Add(s.longitudine);
                listView1.Items.Add(lv);
            }


            Thread tx = new Thread(ThreadInserareSenzoriFunctie);          // Kick off a new thread
            tx.Start();

        }

        void ThreadInserareSenzoriFunctie()
        {
            foreach (Senzor senz in l)
            {
                try
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(senz);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var result = client.PostAsync("https://xdoit.azurewebsites.net/api/todo", content).Result;
                    //  MessageBox.Show(Convert.ToString(result));
                }
                catch { MessageBox.Show("Nu se poate face Post, error problem"); }
            }
        }


        private void btnInregistrari_Click(object sender, EventArgs e)
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 90000; // every 90 seconds

            Thread tfirst = new Thread(firstinrestrigation);          // Kick off a new thread
            tfirst.Start();

            timer1.Start();

            timer2 = new System.Windows.Forms.Timer();
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Interval = 179500;
            timer2.Start();
        }

        List<Inregistrare> li = new List<Inregistrare>();

        private void firstinrestrigation()
        {
            foreach (Senzor s in l)
            {
                int idsenzor = s.idsenzor;
                int temperatura = rnd.Next(0, 40);
                int umiditate = rnd.Next(0, 100);
                int presiune = rnd.Next(730, 780);
                DateTime data = DateTime.Now;

                Inregistrare i = new Inregistrare(idsenzor, temperatura, umiditate, presiune, data);
                li.Add(i);
            }

            foreach (Inregistrare i in li)
            {
                ListViewItem lvi = new ListViewItem(Convert.ToString(i.idsenzor));
                lvi.SubItems.Add(Convert.ToString(i.temperatura));
                lvi.SubItems.Add(Convert.ToString(i.umiditate));
                lvi.SubItems.Add(Convert.ToString(i.presiune));
                lvi.SubItems.Add(Convert.ToString(i.data));

                if (listView2.InvokeRequired)
                {
                    listView2.Invoke(new MethodInvoker(delegate { listView2.Items.Add(lvi); }));
                }

            }


            foreach (Inregistrare i in li)
            {
                try
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(i);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var result = client.PostAsync("https://xdoit.azurewebsites.net/api/Inregistrare", content).Result;
                    //  MessageBox.Show(Convert.ToString(result));
                }
                catch
                {
                    MessageBox.Show("Nu se poate face Post, error problem");
                }
            }

        }
        int bianca = 0;
        void ThreadFunctieTimer1()
        {

            foreach (Inregistrare i in li)
            {

                if (i.umiditate <= 25)
                {
                    i.umiditate = i.umiditate + rnd.Next(5, 50);
                    i.temperatura = i.temperatura - rnd.Next(1, 5);
                    if (i.temperatura < -20)
                        i.temperatura = -20;
                    i.presiune = i.presiune - rnd.Next(3, 7);
                    if (i.presiune < 720)
                        i.presiune = 720;
                }
                if (i.umiditate > 25)
                {
                    i.umiditate = i.umiditate - rnd.Next(5, 50);
                    if (i.umiditate < 0)
                    {
                        i.umiditate = 0;
                    }
                    i.temperatura = i.temperatura + rnd.Next(1, 5);
                    if (i.temperatura > 50)
                        i.temperatura = 50;
                    i.presiune = i.presiune + rnd.Next(3, 7);
                    if (i.presiune > 790)
                        i.presiune = 790;
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

                if (listView2.InvokeRequired)
                {
                    listView2.Invoke(new MethodInvoker(delegate { listView2.Items.Add(lvi); }));
                }

            }

            foreach (Inregistrare i in li)
            {

                if (i.idsenzor != bianca)
                {
                    try
                    {
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(i);
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        var result = client.PostAsync("https://xdoit.azurewebsites.net/api/Inregistrare", content).Result;
                        Thread.Sleep(200);
                        //  MessageBox.Show(Convert.ToString(result));
                    }
                    catch
                    {
                        MessageBox.Show("Nu se poate face Post, error problem");
                    }
                }
                else
                {
                    Inregistrare bi = new Inregistrare(bianca, 9000, 5000, 4000, DateTime.Now);

                    ListViewItem lvi = new ListViewItem(Convert.ToString(bi.idsenzor));
                    lvi.SubItems.Add(Convert.ToString(bi.temperatura));
                    lvi.SubItems.Add(Convert.ToString(bi.umiditate));
                    lvi.SubItems.Add(Convert.ToString(bi.presiune));
                    lvi.SubItems.Add(Convert.ToString(bi.data));

                    if (listView2.InvokeRequired)
                    {
                        listView2.Invoke(new MethodInvoker(delegate { listView2.Items.Add(lvi); }));
                    }


                    try
                    {
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(bi);
                        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        var result = client.PostAsync("https://xdoit.azurewebsites.net/api/Inregistrare", content).Result;
                        Thread.Sleep(200);
                    }
                    catch
                    {
                        MessageBox.Show("Nu se poate face Post, error problem");
                    }
                    bianca = 0;
                }
            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Thread t = new Thread(ThreadFunctieTimer1);          // Kick off a new thread
            t.Start();

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           /* listView1.Items.Clear();

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

                var queryString1 = $"delete from TabelaInregistrari";
                using (SqlCommand command = new SqlCommand(queryString1, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            */
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            Thread t1 = new Thread(ThreadFunctieTimer2);          // Kick off a new thread
            t1.Start();

        }



        void ThreadFunctieTimer2()
        {
            int idrandom = rnd.Next(1, 60);
            bianca = idrandom;
        }


        private void GetSensorsNoInsert_Click(object sender, EventArgs e)
        {

            StreamReader sr = new StreamReader("senzori.txt");

            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] word = line.Split(' ');
                Senzor s = new Senzor(Convert.ToInt32(word[0]), word[1], word[2]);
                l.Add(s);
            }
            foreach (Senzor s in l)
            {
                ListViewItem lv = new ListViewItem(Convert.ToString(s.idsenzor));
                lv.SubItems.Add(s.latitudine);
                lv.SubItems.Add(s.longitudine);
                listView1.Items.Add(lv);
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
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

                var queryString1 = $"delete from TabelaInregistrari";
                using (SqlCommand command = new SqlCommand(queryString1, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
