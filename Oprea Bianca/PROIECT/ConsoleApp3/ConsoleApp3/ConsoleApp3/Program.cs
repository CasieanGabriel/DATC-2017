using System;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using Configuration;

namespace ConsoleApp3
{
    class Program
    {
        private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(); //create and manage the contents of connection strings 
        private static int locuri_parcare = 26;

        public void connectToDatabase() //conexiunea la baza de date
        {
            try
            {
                builder.DataSource = "serverdbdata.database.windows.net";
                builder.UserID = "Bianca"; 
                builder.Password = "VERDE2018!"; 
                builder.InitialCatalog = "DBData"; 
                Console.WriteLine("Conexiune realizata cu succes");
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void SendMessage() // trimite mesaj prin coada
        {
            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            
            CloudQueue queue = queueClient.GetQueueReference("myqueue");
            
            queue.CreateIfNotExists();
            
            CloudQueueMessage message = new CloudQueueMessage("Start");
            queue.AddMessage(message);
            Console.WriteLine("Mesaj trimis");
            CloudQueueMessage rcvmessage = queue.GetMessage();

            Console.WriteLine("Mesaj primit: {0}", rcvmessage.AsString);
            //CloudQueueMessage peekedMessage = queue.PeekMessage();
            
            //Console.WriteLine(peekedMessage.AsString);
            Console.WriteLine();
        }

       
        public void UpdateTable(Parking park)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                string sqlselect = "select count(*) from DateGenerate where id_loc=@id"; // se verifica daca sunt inserate deja valori pentru un anumit id
   
                using (SqlCommand commandSelect = new SqlCommand(sqlselect, connection)) // se executa comanda
                {
                    commandSelect.Parameters.AddWithValue("@id", park.Id); //variabilei @id ii este atribuita valoarea variabilei park.Id
                    connection.Open(); //conexiunea se deschide
                    int result = (int)commandSelect.ExecuteScalar(); 
                    if (result == 0) // daca nu exista id-ul in baza de date, se face insert
                    {
                        Console.WriteLine("Insert");  
                        String sql = "insert into DateGenerate values(@id, @data0, @data1, @data2, @data3, @data4, @data5, @data6, @data7, @data8, @data9)"; 
                        using (SqlCommand command = new SqlCommand(sql, connection)) // comanda de inserare
                        {
                            command.Parameters.AddWithValue("@id", park.Id); 
                            command.Parameters.AddWithValue("@data0", park.Data[0]);
                            command.Parameters.AddWithValue("@data1", park.Data[1]);
                            command.Parameters.AddWithValue("@data2", park.Data[2]);
                            command.Parameters.AddWithValue("@data3", park.Data[3]);
                            command.Parameters.AddWithValue("@data4", park.Data[4]);
                            command.Parameters.AddWithValue("@data5", park.Data[5]);
                            command.Parameters.AddWithValue("@data6", park.Data[6]);
                            command.Parameters.AddWithValue("@data7", park.Data[7]);
                            command.Parameters.AddWithValue("@data8", park.Data[8]);
                            command.Parameters.AddWithValue("@data9", park.Data[9]);


                            int result1 = command.ExecuteNonQuery(); //returneaza numarul de linii afectate

                            // Check Error
                            if (result1 < 0)
                                Console.WriteLine("Eroare la inserare");
                            else
                            {
                                Console.WriteLine("Inserare cu Succes");
                                Console.WriteLine("ID:" + park.Id + " Data1:" + park.Data[0] + " Data2:" + park.Data[1] + " Data3:" + park.Data[2] + " Data4:" + park.Data[3] + " Data5:" + park.Data[4] + " Data6:" + park.Data[5] + " Data7:" + park.Data[6] + " Data8:" + park.Data[7] + " Data9:" + park.Data[8] + " Data10:" + park.Data[9]);
                            }

                        }

                    }
                    else //daca exista id-ul in baza de date, se face update
                    {
                        Console.WriteLine("Update");

                        String sqlUpdate = "update DateGenerate set  val1=@data0, val2=@data1, val3=@data2, val4=@data3, val5=@data4, val6=@data5, val7=@data6, val8=@data7, val9=@data8, val10=@data9 where id_loc=@id";
                        using (SqlCommand command2 = new SqlCommand(sqlUpdate, connection)) // comanda de update 
                        {
                            command2.Parameters.AddWithValue("@id", park.Id);
                            command2.Parameters.AddWithValue("@data0", park.Data[0]);
                            command2.Parameters.AddWithValue("@data1", park.Data[1]);
                            command2.Parameters.AddWithValue("@data2", park.Data[2]);
                            command2.Parameters.AddWithValue("@data3", park.Data[3]);
                            command2.Parameters.AddWithValue("@data4", park.Data[4]);
                            command2.Parameters.AddWithValue("@data5", park.Data[5]);
                            command2.Parameters.AddWithValue("@data6", park.Data[6]);
                            command2.Parameters.AddWithValue("@data7", park.Data[7]);
                            command2.Parameters.AddWithValue("@data8", park.Data[8]);
                            command2.Parameters.AddWithValue("@data9", park.Data[9]);

                            int result2 = command2.ExecuteNonQuery();

                            // Check Error
                            if (result2 < 0)
                                Console.WriteLine("Eroare la actualizare");
                            else
                            {
                                Console.WriteLine("Actualizare cu Succes");
                                Console.WriteLine("ID:" + park.Id + " Data1:" + park.Data[0] + " Data2:" + park.Data[1] + " Data3:" + park.Data[2] + " Data4:" + park.Data[3] + " Data5:" + park.Data[4] + " Data6:" + park.Data[5] + " Data7:" + park.Data[6] + " Data8:" + park.Data[7] + " Data9:" + park.Data[8] + " Data10:" + park.Data[9]);
                            }
                        }
                    }
                }
            }
        }
         public void genData() // generarea datelor
        {
            Parking park = new Parking(); 
            int j = 0;
            for (int i = 1; i <= locuri_parcare; i++)
            {
                while (j < 10)
                {
                    park.Data[j] = park.RandomGen(255); // metoda de generare a numerelor de la -10 la 255
                    System.Threading.Thread.Sleep(50);
                    j++;
                }
                park.Id = i; 
                UpdateTable(park); //inserarea in tabela
                j = 0;
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.connectToDatabase();
            while (true)
            {
                p.genData();
                p.SendMessage();
                System.Threading.Thread.Sleep(14000);
            }
        }
    }
}
