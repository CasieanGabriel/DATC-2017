using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Net;

namespace IrrigationSystemWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            /*SqlConnection conn = new SqlConnection("Server=adminserverdatc.database.windows.net;Database=AdminDataBase;Trusted_Connection=False;Encrypt=True;Integrated Security=False;User ID=adminDatc;Password=Mov12345;");
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Nume FROM [Utilizatori]",conn);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            Console.WriteLine("{0}", reader.GetString(0));
            reader.Close();

            SqlCommand cmd2 = new SqlCommand("SELECT ObjectJSON FROM [ValoriParametri]", conn);
            reader = cmd2.ExecuteReader();
            reader.Read();

            obsvar json = JObject.Parse(reader.GetString(0));
            var firstNode = json["current_ervation"];
            var temp = firstNode["temp_c"];
            var um = firstNode["relative_humidity"];

            JToken tokenT = temp;
            JToken tokenU = um;
            int temperatura = tokenT.ToObject<int>();
            string um2 = tokenU.ToObject<string>();

            string um3 = um2.Remove(um2.Length - 1);
            

            int umiditate = Int32.Parse(um3);


            Console.WriteLine(temperatura);
            Console.WriteLine(umiditate);
            reader.Close();

            if ((temperatura < 20 && temperatura > 24) || umiditate < 100)
            {
                SqlCommand cmd3 = new SqlCommand("UPDATE [ValoriParametri] SET IrigatSauNu=1",conn);
                cmd3.ExecuteReader();
            }
            reader.Close();
            Console.ReadLine();*/



            WebClient wc = new WebClient();

            var json = wc.DownloadString("https://0c81bb1d.ngrok.io/get-resource");
            string culoareHexaT = "";
            string culoareHexaU = "";
            var json_test = JObject.Parse(json);
            var parcurgereJson = json_test["resources"].First;
            while(parcurgereJson.Next != null){

                var idJson = parcurgereJson.First.First;
                var temperaturaJson = parcurgereJson.First.Next.First;
                var umiditateJson = parcurgereJson.First.Next.Next.First;

                int id = Convert.ToInt32(idJson);
                int temperatura = Convert.ToInt32(temperaturaJson);
                int umiditate = Convert.ToInt32(umiditateJson);

                if (temperatura >= 25 && temperatura <= 35)
                {
                    culoareHexaT = "#7f0000";
                }
                else
                {
                    if (temperatura >= 20 && temperatura <= 30)
                    {
                        culoareHexaT = "#990000";
                    }
                    else
                    {
                        if (temperatura >= 10 && temperatura <= 20)
                        {
                            culoareHexaT = "#b20000";
                        }
                        else
                        {
                            if (temperatura >= 0 && temperatura <= 10)
                            {
                                culoareHexaT = "#cc0000";
                            }
                            else
                            {
                                if (temperatura >= -10 && temperatura <= 0)
                                {
                                    culoareHexaT = "#e50000";
                                }
                                else
                                {
                                    if (temperatura >= -20 && temperatura <= -10)
                                    {
                                        culoareHexaT = "#ff0000";
                                    }
                                }
                            }
                        }
                    }

                }

                if (umiditate >= 0 && umiditate <= 20)
                {
                    culoareHexaU = "#0000ff";
                }
                else
                {
                    if (umiditate >= 20 && umiditate <= 40)
                    {
                        culoareHexaU = "#0000cc";
                    }
                    else
                    {
                        if (umiditate >= 40 && umiditate <= 60)
                        {
                            culoareHexaU = "#000099";
                        }
                        else
                        {
                            if (umiditate >= 60 && umiditate <= 80)
                            {
                                culoareHexaU = "#00007f";
                            }
                            else
                            {
                                if (umiditate >= 80 && umiditate <= 100)
                                {
                                    culoareHexaU = "#000066";
                                }
                            }
                        }
                    }
                }



                using (WebClient client = new WebClient())
                {

                    byte[] response =
                    client.UploadValues("https://0c81bb1d.ngrok.io/add_response", new NameValueCollection()
                    {
                         { "Id_Resources", idJson.ToString() },
                         { "Color_temperature", culoareHexaT.ToString()},
                         { "Color_humidity", culoareHexaU.ToString() }
                     });

                    string result = System.Text.Encoding.UTF8.GetString(response);
                }
              
                parcurgereJson = parcurgereJson.Next;


            };
            

           

            Console.WriteLine(json);
          
           
            Console.ReadLine();

            



        }
    }
}
