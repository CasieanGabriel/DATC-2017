using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parcare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            builder = new SqlConnectionStringBuilder();
            builder.DataSource = "serverdbdata.database.windows.net";
            builder.UserID = "Bianca";
            builder.Password = "VERDE2018!";
            builder.InitialCatalog = "DBData";
        }

       // DateLocParcare obj_parcare;

   //    public DateLocParcare date = new DateLocParcare();

        public SqlConnectionStringBuilder builder;

        public static List<DateLocParcare> listadate = new List<DateLocParcare>();

        public void fct()
        {
                builder = new SqlConnectionStringBuilder();
                builder.DataSource = "serverdbdata.database.windows.net";
                builder.UserID = "Bianca";
                builder.Password = "VERDE2018!";
                builder.InitialCatalog = "DBData";
        }

        public void fct2()
        {
            /*  int suma = 0;
              float medie = 0;*/
            //try
            //{
            //          if(queueRcvMessage.AsString.Equals("Start"))
            //          {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                Console.WriteLine("\nConexiune realizata");
                Console.WriteLine("=========================================\n");
                Console.WriteLine("\nDate din tabela Date_generate:");
                Console.WriteLine("=========================================\n");
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("Select * from DatePrelucrate");
                string sql = sb.ToString();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            DateLocParcare date = new DateLocParcare();
                            date.SetNrLoc = reader.GetInt32(0);
                            date.SetLocL = reader.GetInt32(1);
                            date.SetLocO = reader.GetInt32(2);
                            listadate.Add(date);
                            Console.WriteLine("{0}", listadate.FirstOrDefault());
                        }
                    }
                }
            }
            //}
            /*catch
            { }*/

        }

        private void button1_Click(object sender, EventArgs e)
        {
   //         fct();
            fct2();

            Label[] v = new Label[] { label1, label2, label3, label4, label5, label6, label7, label8, label9, label10, label10, label11, label12, label13, label14, label15, label16, label17, label18, label19, label20, label21, label22, label23, label24, label25, label26 };
            int i=0;
            foreach (DateLocParcare d in listadate)
                {
                
                if (i == (d.GetNrLoc -1) && d.GetLocO == 0)
                        v[i].BackColor = Color.Blue;

                    if (i == (d.GetNrLoc - 1) && d.GetLocO == 1)
                        v[i].BackColor = Color.Red;

                    if (i == (d.GetNrLoc - 1) && d.GetLocL == 1)
                        v[i].BackColor = Color.Blue;
                    if (i == (d.GetNrLoc - 1) && d.GetLocL == 0)
                        v[i].BackColor = Color.Red;
                i++;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}