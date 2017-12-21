using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kappa_webjob.Model;
using Newtonsoft.Json;

namespace kappa_webjob.Controller
{
    class BackgroundTask
    {
        //public static string _connectionString = "Server=tcp:kappaserver.database.windows.net;Database=kappa_database;User ID =IonutGrad;Password=GradIonut1;Trusted_Connection=False;Encrypt=True;";
        public static string _connectionString = "Server=tcp:iogrserver.database.windows.net,1433;Initial Catalog=IoGrDatabase;Persist Security Info=False;User ID=IonutGrad;Password=GradIonut1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public List<DateDePrelucrat> GetInfoDateDePrelucrat()
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
                            Zona = Convert.ToInt32(reader["Zona"]),
                            Data = Convert.ToDateTime(reader["Data"]),
                            Temperature = Convert.ToDouble(reader["Temperature"]),
                            Humidity = Convert.ToDouble(reader["Humidity"])
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
        
        public void DeleteInfoDateDePrelucrat()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            List<DateDePrelucrat> dataFromTable = new List<DateDePrelucrat>();
            try
            {
                DBConn.Open();
                string getDataFromDateTable = "DELETE * FROM DateDePrelucrat";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                getCommand.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
            }
        }
        
        public List<IntervalDeDate> GetInfoIntervalDeDate()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<IntervalDeDate> dataFromTable = new List<IntervalDeDate>();
            try
            {
                DBConn.Open();
                string getDataFromDateTable = "SELECT * FROM IntervalDeDate";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataFromTable.Add(new IntervalDeDate
                        {
                            TemperatureMin = Convert.ToDouble(reader["TemperatureMin"]),
                            TemperatureMax = Convert.ToDouble(reader["TemperatureMax"]),
                            HumidityMin = Convert.ToDouble(reader["HumidityMin"]),
                            HumidityMax = Convert.ToDouble(reader["HumidityMax"])
                        });
                    }
                }
            }
            catch (Exception exp)
            {
            }
           // var serializedJson = JsonConvert.SerializeObject(dataFromTable);

            return dataFromTable;        
        }

        public void DeleteInfoIntevalDeDate()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            List<IntervalDeDate> dataFromTable = new List<IntervalDeDate>();
            try
            {
                DBConn.Open();
                string getDataFromDateTable = "DELETE * FROM IntervalDeDate";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                getCommand.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
            }
        }

        public void IrigationLogic()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<Date> colectedDataForInsertion = new List<Date>();

            var ListaIntervale = GetInfoIntervalDeDate();

            try
            {
                for (int i = 0; i <= 20; i++)
                {
                    DBConn.Open();
                    double avg_temp = -30;
                    double avg_umidit = 0;
                    DateTime data_masuratoare = DateTime.Now;
                    double longit = 0;
                    double latit = 0;
                    string NeedWater = String.Empty;

                    string getDataFromDateDePrelucratTable = "SELECT AVG(Temperature) AS avg_temp, AVG(Humidity) AS avg_umidit, max(Data) as DataMasuratoare FROM DateDePrelucrat WHERE Zona = " + i;
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

                    string getDataFromZoneTable = "SELECT * FROM Zone WHERE Zona = " + i;
                    getCommand = new SqlCommand(getDataFromZoneTable, DBConn);
                    reader = getCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            longit = Convert.ToDouble(reader["Latitude"]);
                            latit = Convert.ToDouble(reader["Longitude"]);
                        }
                    }

                    if (avg_temp > ListaIntervale[1].TemperatureMax || avg_temp < ListaIntervale[1].TemperatureMin) // Normal: [-20, 30] gradeC
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
                            NeedWater = "Yes";
                        }
                        else
                        {
                            NeedWater = "No";
                        }
                    }
                    colectedDataForInsertion.Add(new Date
                    {
                        Data = data_masuratoare,
                        Lng = longit,
                        Temperature = avg_temp,
                        Lat = latit,
                        Humidity = avg_umidit,
                        NeedIrigation = NeedWater
                    });                    
                }

                InsertDataIntoDateTable(colectedDataForInsertion);
                DeleteInfoDateDePrelucrat();
                DeleteInfoIntevalDeDate();
            }
            catch (Exception exp)
            {
            }
            //var serializedJson = JsonConvert.SerializeObject(dataFromTable);

            //return serializedJson;
        }

        public void InsertDataIntoDateTable(List<Date> ListForInsertInDateTable)
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
                      "INSERT INTO Date VALUES({0},{1},{2},{3},{4},{5}})",
                      item.Lat, item.Lng, item.Temperature, item.Humidity, item.Data, item.NeedIrigation
                    );
                    insertCommand = new SqlCommand(insertCmd, DBConn);
                    insertCommand.ExecuteNonQuery();
                }
            }
            catch { }
            finally
            {
                if (DBConn != null)
                    DBConn.Dispose();
            }
        }

        public void HistoryConstructor()
        {
            // ???
        }
    }
}
