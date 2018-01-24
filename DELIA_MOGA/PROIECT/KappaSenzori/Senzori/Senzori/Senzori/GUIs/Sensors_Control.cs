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
using kappa_webjob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Senzori
{
    public partial class Sensors_Control : Form
    {


        public Sensors_Control()
        {
            InitializeComponent();
            Sensors_Settings initial_settings = new Sensors_Settings(5,20,5,60);
            Variables.My_Sensors_Settings.Add(initial_settings);
        }

        private void Generate_btn_Click(object sender, EventArgs e)
        {
            Sensors_Data_dgv.Rows.Clear();
            Variables.Data_List.Clear();
            if(!string.IsNullOrEmpty(Sensors_no_txt.Text)&&!string.IsNullOrEmpty(Sensors_Values_txt.Text))
            {
                Variables.sensors_nr = Convert.ToInt32(Sensors_no_txt.Text);
                Variables.values_nr = Convert.ToInt32(Sensors_Values_txt.Text);
                if (Error_data_cb.Checked)
                {
                    GetData.GetRandomBrokenSensorDate();
                }
                else
                {
                    GetData.GetRandomSensorDate();
                }
                foreach (CustomJSON current_json in Variables.Data_List)
                {
                    Sensors_Data_dgv.Rows.Add();
                    Sensors_Data_dgv.Rows[Sensors_Data_dgv.Rows.Count - 1].Cells[0].Value = current_json.Field.ToString();
                    Sensors_Data_dgv.Rows[Sensors_Data_dgv.Rows.Count - 1].Cells[1].Value = current_json.Temperature.ToString();
                    Sensors_Data_dgv.Rows[Sensors_Data_dgv.Rows.Count - 1].Cells[2].Value = current_json.Humidity.ToString();
                }
                StringContent content = new StringContent(JsonConvert.SerializeObject(Variables.Data_List));

                AsyncronousMessaging asyncMsg = new AsyncronousMessaging();
                CloudQueueMessage messsageFromDataGenerator = asyncMsg.ReceiveDateDePrelucratIsEmpty();
                if (messsageFromDataGenerator.AsString == "date-de-prelucrat-is-empty")
                {
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                    client.PostAsync("http://kappaapi.azurewebsites.net/api/values", content);


                    
                    asyncMsg.SendDateDePrelucratIsReady("date-de-prelucrat-is-ready");
                } 
                //var clientResponse = client.PostAsync("http://kappaapi.azurewebsites.net/api/values", content).Result;
                // MessageBox.Show(clientResponse.ToString());

            }
            else
            {
                MessageBox.Show("Please complete all the required fields!","Data missing",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }

        private void Settings_btn_Click(object sender, EventArgs e)
        {
            Settings New_settings = new Settings();
            New_settings.ShowDialog();

            StringContent content = new StringContent(JsonConvert.SerializeObject(Variables.My_Sensors_Settings));

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.PostAsync("http://kappaapi.azurewebsites.net/api/values", content);
        }

        private void Upload_Data_btn_Click(object sender, EventArgs e)
        {
            if(Error_data_cb.Checked)
            {
                // Generate data from outside the range
            }
            else
            {
                
            }
        }
    }
}
