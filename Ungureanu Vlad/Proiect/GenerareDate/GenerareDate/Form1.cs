using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace GenerareDate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Senzor> lista = new List<Senzor>();
        Random rnd = new Random();
        static HttpClient client = new HttpClient();

        private void btnGenerate_Click(object sender, EventArgs e)
        {


            int nr = Convert.ToInt32(textBoxNr.Text);        

            for(int i =0;i<nr;i++)
            {
                int idsenzor = i;
                int temperatura = rnd.Next(0, 45);
                int umiditate = rnd.Next(0,100);
                Senzor x = new Senzor(idsenzor, temperatura,umiditate);

                lista.Add(x);
            }

        //    ListViewItem lv = new ListViewItem();

            foreach(Senzor s in lista)
            {
                ListViewItem lv = new ListViewItem(Convert.ToString(s.idsenzor));
                lv.SubItems.Add(Convert.ToString(s.temperatura));
                lv.SubItems.Add(Convert.ToString(s.umiditate));
                listView1.Items.Add(lv);
            }
        }


        private void btnGenerateChanges_Click(object sender, EventArgs e)
        {
            foreach(Senzor s in lista)
            {
                if(s.umiditate <25)
                {
                    s.umiditate = s.umiditate + rnd.Next(5,50);
                    s.temperatura = s.temperatura - rnd.Next(1,5);
                }
                if(s.umiditate > 25)
                {
                    s.umiditate = s.umiditate - rnd.Next(5,50);
                    if(s.umiditate <0)
                    {
                        s.umiditate = 0;
                    }
                    s.temperatura = s.temperatura + rnd.Next(1, 5);
                }

            }

            foreach (Senzor s in lista)
            {
                ListViewItem lv = new ListViewItem(Convert.ToString(s.idsenzor));
                lv.SubItems.Add(Convert.ToString(s.temperatura));
                lv.SubItems.Add(Convert.ToString(s.umiditate));
                listView1.Items.Add(lv);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            lista.Clear();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            foreach (Senzor senz in lista)
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(senz);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var result = client.PostAsync("http://localhost:50922/api/todo", content).Result;
                MessageBox.Show(Convert.ToString(result));
            }
        }
    }
}
