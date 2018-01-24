using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Diagnostics;

namespace RandomDataGenerator
{
    public partial class generateOutOfRange : Form
    {
        List<Range> rangeList = new List<Range>();
        List<dataForProcess> dataList = new List<dataForProcess>();
        Random r = new Random();
        int a;
        public generateOutOfRange()
        {
            InitializeComponent();
            Range range = new Range(0, 30, 40, 80);
            rangeList.Add(range);
            a = 0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void setRangeBtn_Click(object sender, EventArgs e)
        {
            rangeList.Clear();
            int temperatura_minima = Convert.ToInt16(textBoxtempMin.Text);
            int temperatura_maxima = Convert.ToInt16(textBoxTempMax.Text);
            int umiditate_minima = Convert.ToInt16(textBoxHumMin.Text);
            int umiditate_maxima = Convert.ToInt16(textBoxHumMax.Text);
            if (temperatura_maxima >= temperatura_minima && umiditate_maxima >= umiditate_minima)
            {
                Range range = new Range(temperatura_minima, temperatura_maxima, umiditate_minima, umiditate_maxima);
                rangeList.Add(range);
            }
            else
            {
                Range range = new Range(temperatura_maxima, temperatura_minima, umiditate_maxima, umiditate_minima);
                rangeList.Add(range);
            }
            StringContent content = new StringContent(JsonConvert.SerializeObject(rangeList));

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.PostAsync("http://api20180109081124.azurewebsites.net/api/values", content);
           // client.PostAsync("http://localhost:55006/api/values", content);

        }

        private void ganarateInRangebtn_Click(object sender, EventArgs e)
        {
            dataList.Clear();
            for (int i = 1; i <=20; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int temperatura = r.Next(rangeList.ElementAt(0).Temperatura_Min, rangeList.ElementAt(0).Temperatura_Max);
                    int umiditate = r.Next(rangeList.ElementAt(0).Umididate_Min, rangeList.ElementAt(0).Umididate_Max);
                    DateTime date = DateTime.Now;
                    dataForProcess data = new dataForProcess(i, temperatura, umiditate, date);
                    dataList.Add(data);
                }
            }
            StringContent content = new StringContent(JsonConvert.SerializeObject(dataList));

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            Rabbitcs rabitMQ = new Rabbitcs();
            while (true)
            {
                if (rabitMQ.Get() == ("am_primit"))
                {
                    client.PostAsync("http://api20180109081124.azurewebsites.net/api/values", content);
                    rabitMQ.Publish("ti-am_trimis");
                }
                break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int randomcase = r.Next(0, 3);
            switch(randomcase)
            {
                case 0:
                {
                        for (int i = 1; i < 20; i++)
                        {
                            for (int j = 0; i < 3; i++)
                            {
                                int temperatura = r.Next(rangeList.ElementAt(0).Temperatura_Min, rangeList.ElementAt(0).Temperatura_Max);
                                int umiditate = r.Next(rangeList.ElementAt(0).Umididate_Min, rangeList.ElementAt(0).Umididate_Max);
                                DateTime date = DateTime.Now;
                                dataForProcess data = new dataForProcess(i, temperatura, umiditate, date);
                                dataList.Add(data);
                            }
                        }
                        break;
                }
                case 1:
                 {
                        for (int i = 1; i < 20; i++)
                        {
                            for (int j = 0; i < 3; i++)
                            {
                                int temperatura = r.Next(rangeList.ElementAt(0).Temperatura_Min, rangeList.ElementAt(0).Temperatura_Max);
                                int umiditate = r.Next(rangeList.ElementAt(0).Umididate_Min, rangeList.ElementAt(0).Umididate_Max);
                                DateTime date = DateTime.Now;
                                dataForProcess data = new dataForProcess(i, temperatura, umiditate, date);
                                dataList.Add(data);
                            }
                        }
                        break;
                 }
                case 2:
                {
                        for (int i = 1; i < 20; i++)
                        {
                            for (int j = 0; i < 3; i++)
                            {
                                int temperatura = r.Next(rangeList.ElementAt(0).Temperatura_Min, rangeList.ElementAt(0).Temperatura_Max);
                                int umiditate = r.Next(rangeList.ElementAt(0).Umididate_Min, rangeList.ElementAt(0).Umididate_Max);
                                DateTime date = DateTime.Now;
                                dataForProcess data = new dataForProcess(i, temperatura, umiditate, date);
                                dataList.Add(data);
                            }
                        }
                        break;
                }
                case 3:
                {
                        for (int i = 1; i < 20; i++)
                        {
                            for (int j = 0; i < 3; i++)
                            {
                                int temperatura = r.Next(rangeList.ElementAt(0).Temperatura_Min, rangeList.ElementAt(0).Temperatura_Max);
                                int umiditate = r.Next(rangeList.ElementAt(0).Umididate_Min, rangeList.ElementAt(0).Umididate_Max);
                                DateTime date = DateTime.Now;
                                dataForProcess data = new dataForProcess(i, temperatura, umiditate, date);
                                dataList.Add(data);
                            }
                        }
                        break;
                }
            }
            StringContent content = new StringContent(JsonConvert.SerializeObject(dataList));

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            Rabbitcs rabbit = new Rabbitcs();
            while (true)
            {
                if (rabbit.Get() == ("am_primit"))
                { 
                    client.PostAsync("http://api20180109081124.azurewebsites.net/api/values", content);
                rabbit.Publish("ti-am_trimis");
                }
                break;
            }
        }
    }
}
