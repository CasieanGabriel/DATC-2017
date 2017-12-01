using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerApp
{
    public class Worker
    {
       
        SqlConnection conn, connWorker;
        String query = "SELECT * FROM[dbo].[TableValori]";
        String queryWorker;
        SqlCommand cmd,cmdWorker;

        internal void Init()
        {
            String connString = "Server=tcp:xdoit.database.windows.net,1433;Initial Catalog=DATC;Persist Security Info=False;User ID= x; Password = x;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; ";
            conn = new SqlConnection(connString);
            connWorker = new SqlConnection(connString);
            conn.Open();
            connWorker.Open();
            cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            AfiseazaValori();
        }

        private void AfiseazaValori()
        {
            SqlDataReader reader = cmd.ExecuteReader();
                       
            while(reader.Read())
            {
                Console.WriteLine(reader[0].ToString()+"," +reader[1].ToString()+ "," + reader[2].ToString()+ "," + reader[3].ToString()+ "," + reader[4].ToString()+ "," + reader[5].ToString());
            }
            reader.Close();
        }

        internal void Process()
        {            
            SqlDataReader reader;
            query = "SELECT * FROM[dbo].[TableValori] where valProcesat = '0'";
            cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            while (true)
            {
                reader = cmd.ExecuteReader();                
                while(reader.Read())
                {
                    queryWorker = "Update [dbo].[TableValori] set [valProcesat]='2' where id = "+ reader[0];
                    cmdWorker = new SqlCommand(queryWorker);
                    cmdWorker.CommandType = System.Data.CommandType.Text;
                    cmdWorker.Connection = connWorker;
                    cmdWorker.ExecuteNonQuery();                    
                }
                reader.Close();
            }
        }
    }
}
