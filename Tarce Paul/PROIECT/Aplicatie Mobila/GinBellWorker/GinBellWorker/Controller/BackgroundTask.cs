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
                string getDataFromDateTable = "SELECT * FROM Date";
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
                            Data = Convert.ToDateTime(reader["Data"])

                        });
                        /*
                        Date.Add(new DateDePrelucrat()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Valoare = Convert.ToInt32(reader["Valoare"])
                        });
                        */
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
                string getDataFromDateTable = "SELECT * FROM Date";
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
                            Data = Convert.ToDateTime(reader["Data"])

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

        public void DeleteInfoDate()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            List<Date> dataFromTable = new List<Date>();
            try
            {
                DBConn.Open();
                string getDataFromDateTable = "DELETE FROM Date WHERE Id=1";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                getCommand.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
            }
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
                      "INSERT INTO DateDePrelucrat VALUES({0},{1})",
                      item.Id, item.Valoare
                    //item.Lat, item.Lng, item.Temperature,
                    //item.Humidity, item.Data.ToString("yyyy-MM-dd hh:mm:ss"), item.NeedIrigation
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
        /*    public List<DateDePrelucrat> GetInfoDateDePrelucrat()
      {
          SqlConnection DBConn = new SqlConnection(_connectionString);
          SqlCommand getCommand = null;
          SqlDataReader reader;
          List<DateDePrelucrat> dataFromTableDateDePrelucrat = new List<DateDePrelucrat>();
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
                      dataFromTableDateDePrelucrat.Add(new DateDePrelucrat
                      {
                          Id = Convert.ToInt32(reader["Id"]),
                          Valoare = Convert.ToInt32(reader["Valoare"])//Convert.ToDateTime(reader["Valoare"])//,
                                                                  // Temperature = Convert.ToDouble(reader["Temperature"]),
                                                                  // Humidity = Convert.ToDouble(reader["Humidity"])

                      });
                  }
              }
          }
          catch (Exception exp)
          {
          }
          //var serializedJson = JsonConvert.SerializeObject(dataFromTableDateDePrelucrat);

          return dataFromTableDateDePrelucrat;
      }
      */

        /*
                public void DeleteInfoDateDePrelucrat()
                {
                    SqlConnection DBConn = new SqlConnection(_connectionString);
                    SqlCommand getCommand = null;
                    List<DateDePrelucrat> dataFromTable = new List<DateDePrelucrat>();
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
        */
        /*
                public void IrigationLogic()
                {
                    SqlConnection DBConn = new SqlConnection(_connectionString);
                    SqlCommand getCommand = null;
                    SqlDataReader reader;
                    List<Date> colectedDataForInsertion = new List<Date>();

                    //var ListaIntervale = GetInfoIntervaleDeDate();

                    try
                    {
                        for (int i = 0; i <= 20; i++)
                        {
                            DBConn.Open();
                            double avg_temp = -30;
                            double avg_umidit = 0;
                            DateTime data_masuratoare = DateTime.Now;
                            string NeedWater = String.Empty;

                            string getDataFromDateDePrelucratTable = "SELECT AVG(Temperature) AS avg_temp, AVG(Humidity) AS avg_umidit, max(Data) AS DataMasuratoare FROM DateDePrelucrat WHERE Zona = " + i;
                            getCommand = new SqlCommand(getDataFromDateDePrelucratTable, DBConn);
                            reader = getCommand.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    avg_temp = Convert.ToDouble(reader["avg_temp"]);
                                    avg_umidit = Convert.ToDouble(reader["avg_umidit"]);
                                    data_masuratoare = Convert.ToDateTime(reader["DataMasuratoare"]);
                                }
                            }

                            var ListaZone = GetInfoZone(i);
                            // toate valorile in interiorul intervalului
                            if (avg_temp > ListaIntervale[0].TemperatureMax || avg_temp < ListaIntervale[0].TemperatureMin) // Normal: [-20, 30] gradeC
                            {
                                //wrong
                                if (avg_umidit < ListaIntervale[1].HumidityMin || avg_umidit > ListaIntervale[1].HumidityMax) // Normal : [20, 100] %
                                {
                                    NeedWater = "SenzorStricat";
                                }
                            }
                            else
                            {
                                if (avg_umidit < 50)
                                {
                                    NeedWater = "YES";
                                }
                                else
                                {
                                    NeedWater = "NO";
                                }
                            }
                            colectedDataForInsertion.Add(new Date
                            {
                                Data = data_masuratoare,
                                Lng = ListaZone[0].Longitude,
                                Temperature = avg_temp,
                                Lat = ListaZone[0].Latitude,
                                Humidity = avg_umidit,
                                NeedIrigation = NeedWater
                            });
                            DBConn.Close();
                        }

                        InsertDataIntoDateTable(colectedDataForInsertion);

                        DeleteInfoDateDePrelucrat();
                        DeleteInfoIntervaleDeDate();
                    }
                    catch (Exception exp)
                    {
                    }
                }
        */


        /*
         * public void InsertDataIntoDateTable(List<Date> ListForInsertInDateTable)
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
                             "INSERT INTO Date VALUES({0},{1},{2},{3},{4},'{5}')",
                             item.Id, item.Valoare, item.Data.ToString("yyyy-MM-dd hh:mm:ss")
                             //item.Lat, item.Lng, item.Temperature,
                             //item.Humidity, item.Data.ToString("yyyy-MM-dd hh:mm:ss"), item.NeedIrigation
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
               */
    }
}
