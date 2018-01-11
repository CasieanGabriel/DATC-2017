using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Configuration;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure;
using Microsoft.Azure.WebJobs;

namespace WebJob1
{
    public class Worker
    {
        static SqlConnectionStringBuilder builder;
        static DateLocParcare date = new DateLocParcare();
        // aplic care gen date trimite prin coada mesaj dupa ce incarca date in tabela
        
            // Retrieve storage account from connection string.
        static CloudStorageAccount storageAccount;
                // Create the queue client.
        CloudQueueClient queueClient;
        // Retrieve a reference to a container.
        static CloudQueue queue;
                //receive msg
        static CloudQueueMessage queueRcvMessage;
                  
        public static void conexiune()
        {
            try
            {
                storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                queue = queueClient.GetQueueReference("myqueue");
                //      queue.CreateIfNotExists();
                //         queueRcvMessage = await queue.GetMessageAsync();
                //          Console.WriteLine("Recept message coada '{0}'", queueRcvMessage.AsString);
                //         queue.Clear();
        //        CloudQueueMessage message = new CloudQueueMessage("Start");
                //  queue.AddMessage(message);
   //             CloudQueueMessage queueRcvMessage = queue.GetMessage();
     //           Console.WriteLine("{0}", queueRcvMessage.AsString);
                //          queue.DeleteMessage(queueRcvMessage);
                builder = new SqlConnectionStringBuilder();
                builder.DataSource = "serverdbdata.database.windows.net";
                builder.UserID = "Bianca";
                builder.Password = "VERDE2018!";
                builder.InitialCatalog = "DBData";
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public static void QueueMessage([QueueTrigger("myqueue")] string queueMessage,
                                        [Queue("outputqueue")] out string outputQueueMessage)
        {
            outputQueueMessage = queueMessage;
            Console.WriteLine("Receptionat: {0}", queueMessage);
            proces();
        }
        public static void proces()
        {
            int suma = 0;
            float medie = 0;
            try
            {
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
                    StringBuilder sb2 = new StringBuilder();
                    StringBuilder sb3 = new StringBuilder();
                    StringBuilder sb4 = new StringBuilder();
                    sb.Append("Select * from DateGenerate");
                    sb2.Append("Update DatePrelucrate set ocupat=@p2, liber=@p3 where id_loc=@p1");
                    sb3.Append("Insert into DatePrelucrate values (@p1, @p2, @p3)");
                    
                    string sql = sb.ToString();
                    string sqlu = sb2.ToString();
                    string sqli = sb3.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    date.SetNrLoc = reader.GetInt32(0);
                                    for (int i = 1; i < 11; i++)
                                    {
                                        Console.Write("{0} ", reader.GetInt32(i));
                                        if (reader.GetInt32(i) >= 0 && reader.GetInt32(i) < 300)
                                            suma += reader.GetInt32(i);
                                    }
                                    medie = suma / 10;
                                    suma = 0;
                                    date = verifica(medie);
                                using (SqlConnection connection2 = new SqlConnection(builder.ConnectionString))
                                {
                                    using (SqlCommand command_select = connection2.CreateCommand())
                                    {
                                        command_select.CommandText = "Select count(*) from DatePrelucrate where id_loc=@p1";
                                        command_select.Parameters.AddWithValue("@p1", date.GetNrLoc);
                                        connection2.Open();
                                        int nrinreg = (int)command_select.ExecuteScalar();
                                        if (nrinreg > 0)
                                        {
                                            using (SqlCommand command_update = new SqlCommand(sqlu, connection2))
                                            {
                                                command_update.Parameters.AddWithValue("@p1", date.GetNrLoc);
                                                command_update.Parameters.AddWithValue("@p2", date.GetLocO);
                                                command_update.Parameters.AddWithValue("@p3", date.GetLocL);
                                                nrinreg = command_update.ExecuteNonQuery();
                                                if (nrinreg < 0)
                                                    Console.WriteLine("Eroare la actualizare");
                                                else
                                                {
                                                    Console.WriteLine("Actualizare cu Succes");
                                                    Console.WriteLine("\nDate din tabela DatePrelucrate:");
                                                    Console.Write("{0} {1} {2}", date.GetNrLoc, date.GetLocL, date.GetLocO);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (SqlCommand command_insert = new SqlCommand(sqli, connection2))
                                            {
                                                command_insert.Parameters.AddWithValue("@p1", date.GetNrLoc);
                                                command_insert.Parameters.AddWithValue("@p2", date.GetLocL);
                                                command_insert.Parameters.AddWithValue("@p3", date.GetLocO);
                                                nrinreg = command_insert.ExecuteNonQuery();
                                                if (nrinreg < 0)
                                                    Console.WriteLine("Eroare la inserare");
                                                else
                                                {
                                                    Console.WriteLine("Inserare cu Succes");
                                                    Console.WriteLine("\nDate din tabela DatePrelucrate:");
                                                    Console.Write("{0} {1} {2}", date.GetNrLoc, date.GetLocL, date.GetLocO);
                                                }
                                            }
                                        }
                                    }
                                    
                                }
                                Console.WriteLine("\n");
                                }
                            }
                        }                        
                    }
     //           }                
           }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
      //      Console.ReadLine();
        }

        static DateLocParcare verifica(float medie)
        {
            if (medie >= 0 && medie <= 110)
            {
                date.SetLocL = 0;
                date.SetLocO = 1;
            }
            else if (medie > 110 && medie < 300)
            {
                date.SetLocL = 1;
                date.SetLocO = 0;
            }
            Console.WriteLine("Medie: {0}", medie);
            return date;
        }
    }
} 
