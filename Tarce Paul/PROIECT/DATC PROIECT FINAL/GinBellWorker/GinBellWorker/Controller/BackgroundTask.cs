using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GinBellWorker.Model;
using Newtonsoft.Json;

namespace GinBellWorker.Controller
{
    class BackgroundTask
    {
        public static string _connectionString = "Server=tcp:ginbell-server.database.windows.net,1433;Initial Catalog=GinBell_DB;Persist Security Info=False;User ID=ginbell-server;Password=Parola12!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                                      

        public List<Date> GetInfoDate()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<Date> dataFromTableDate = new List<Date>();
            List<DateDePrelucrat> Date = new List<DateDePrelucrat>();
            try
            {
                DBConn.Open();
                string getDataFromDateTable = "SELECT * FROM DateDePrelucrat";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataFromTableDate.Add(new Date
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Valoare = Convert.ToInt32(reader["Valoare"]),
                            //Data = Convert.ToDateTime(reader["Data"])
                            //Data = DateTime.Now

                        });
                    }
                    
                }
                //InsertDataIntoDateTable(Date);
            }
            catch (Exception exp)
            {
            }

            return dataFromTableDate;
        }


        public List<Date> InsertDate()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<Date> dataFromTableDate = new List<Date>();
            List<DateDePrelucrat> Date = new List<DateDePrelucrat>();
            try
            {
                DBConn.Open();
                string getDataFromDateTable = "SELECT * FROM DateDePrelucrat";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataFromTableDate.Add(new Date
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Valoare = Convert.ToInt32(reader["Valoare"]),
                            //Data = Convert.ToDateTime(reader["Data"])

                        });
                        Date.Add(new DateDePrelucrat()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Valoare = Convert.ToInt32(reader["Valoare"])
                        });
                    }

                }
                InsertDataIntoDateTable(Date);
            }
            catch (Exception exp)
            {
            }
            //var serializedJson = JsonConvert.SerializeObject(dataFromTableDateDePrelucrat);

            return dataFromTableDate;
        }

     

        public void InsertDataIntoDateTable(List<DateDePrelucrat> ListForInsertInDateTable)
        {
            SqlConnection DBConn = null;
            SqlCommand insertCommand = null;
            try
            {
                DBConn = new SqlConnection(_connectionString);
                DBConn.Open();

                foreach (var item in ListForInsertInDateTable)
                {
                    string insertCmd = string.Format
                    (
                      "INSERT INTO DateFinal VALUES({0},{1})",
                      item.Id, item.Valoare
                    );
                    insertCommand = new SqlCommand(insertCmd, DBConn);
                    insertCommand.ExecuteNonQuery();
                }
            }
            catch (Exception exp) { }
            finally
            {
                if (DBConn != null)
                    DBConn.Dispose();
            }
        }

        
        public void DeleteInfoDate()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
           // List<Date> dataFromTable = new List<Date>();
            try
            {
                DBConn.Open();
                string getDataFromDateTable = "DELETE FROM DateDePrelucrat";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                getCommand.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
            }
        }
        


    }
}
