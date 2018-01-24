using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace ParkingDataGenerator
{
    public partial class Form1 : Form
    {
        LocParcare[] locuriParcare = new LocParcare[96];
        Random rnd = new Random();
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        public Form1()
        {
            InitializeComponent();
            connectToDatabase();
        }

        private void connectToDatabase()
        {
            try
            {
                builder.DataSource = "parkingserver-12345.database.windows.net";
                builder.UserID = "ServerAdmin";
                builder.Password = "Qwerty_12345";
                builder.InitialCatalog = "parking";

                Console.WriteLine("Conexiune realizata cu succes");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void buttonGenerateData_Click(object sender, EventArgs e)
        {
            listBoxParkingData.Items.Clear();
            for(int i = 0; i < 96; i++)
            {
                int nr=rnd.Next(0, 2);
                locuriParcare[i] = new LocParcare();
                locuriParcare[i].Stare = ((Stare)nr).ToString();
                locuriParcare[i].Id = i.ToString();
                string loc = "Locul " + locuriParcare[i].Id + " este " + locuriParcare[i].Stare;
                listBoxParkingData.Items.Add(loc);
               // Console.WriteLine(locuriParcare[i].Stare);
            }
            string jsonString = JsonConvert.SerializeObject(locuriParcare);
            Console.Write(jsonString);

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("\nInsert into database");
                Console.WriteLine("=========================================\n");

                StringBuilder sb = new StringBuilder();
                sb.Append("insert into Table values(jsonString)");

                String sql = "insert into parking values(@parcare)";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@parcare", jsonString);
                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    // Check Error
                    if (result < 0)
                        Console.WriteLine("Error inserting data into Database!");
                    else
                        Console.WriteLine("Succesfully insert into Database");
                }
            }

        }
    }
}
