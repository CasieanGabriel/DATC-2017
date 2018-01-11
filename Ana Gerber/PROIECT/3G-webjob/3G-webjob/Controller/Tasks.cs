using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _3G_webjob.Model;

namespace _3G_webjob.Controller
{
    public class Tasks
    {
        public static string _connectionString = "Server=tcp:3g-server.database.windows.net,1433;Initial Catalog=3G-DB;Persist Security Info=False;User ID=AnaGerber;Password=Angerb48!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public List<DataToBeProcessed> GetDataToBeProcessed()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<DataToBeProcessed> dataFromTableDataToBeProcessed = new List<DataToBeProcessed>();
            try
            {
                DBConn.Open();
                string getDataFromDataTable = "SELECT * FROM DataToBeProcessed";
                getCommand = new SqlCommand(getDataFromDataTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataFromTableDataToBeProcessed.Add(new DataToBeProcessed
                        {
                            Zona = Convert.ToInt32(reader["Zona"]),
                            Data = Convert.ToDateTime(reader["Data"]),
                            Temperatura = Convert.ToDouble(reader["Temperatura"]),
                            Umiditate = Convert.ToDouble(reader["Umiditate"])
                        });
                    }
                }
            }
            catch (Exception exp)
            {
            }
            return dataFromTableDataToBeProcessed;
        }
        public void DeleteDataToBeProcessed()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand deleteCommand = null;
            List<DataToBeProcessed> dataFromTable = new List<DataToBeProcessed>();
            try
            {
                DBConn.Open();
                string deleteDataFromDataToBeProcessedTable = "DELETE FROM DataToBeProcessed";
                deleteCommand = new SqlCommand(deleteDataFromDataToBeProcessedTable, DBConn);
                deleteCommand.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
            }
        }

        public List<DataRange> GetDataRange()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<DataRange> dataFromTable = new List<DataRange>();
            try
            {
                DBConn.Open();
                string getDataFromDataRangeTable = "SELECT * FROM DataRange";
                getCommand = new SqlCommand(getDataFromDataRangeTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataFromTable.Add(new DataRange
                        {
                            Temperatura_Min = Convert.ToDouble(reader["Temperatura_Min"]),
                            Temperatura_Max = Convert.ToDouble(reader["Temperatura_Max"]),
                            Umididate_Min = Convert.ToDouble(reader["Umiditate_Min"]),
                            Umididate_Max = Convert.ToDouble(reader["Umiditate_Max"])
                        });
                    }
                }
            }
            catch (Exception exp)
            {
            }

            return dataFromTable;
        }

        public void DeleteDataRange()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand deleteCommand = null;
            List<DataRange> dataFromTable = new List<DataRange>();
            try
            {
                DBConn.Open();
                string deleteDataFromDataRangeTable = "DELETE FROM DataRange";
                deleteCommand = new SqlCommand(deleteDataFromDataRangeTable, DBConn);
                deleteCommand.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
            }
        }

        public List<Areas> GetAreas(int nrZona)
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<Areas> dataFromTable = new List<Areas>();
            try
            {
                DBConn.Open();
                string getDataFromAreasTable = "SELECT * FROM Areas WHERE Zona = " + nrZona;
                getCommand = new SqlCommand(getDataFromAreasTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataFromTable.Add(new Areas
                        {
                            Zona = Convert.ToInt32(reader["Zona"]),
                            Latitudine = Convert.ToDouble(reader["Latitudine"]),
                            Longitudine = Convert.ToDouble(reader["Longitudine"])
                        });
                    }
                }
            }
            catch (Exception exp)
            {
            }

            return dataFromTable;
        }
        public string GetSenzorStricat(int i, List<DataRange> Intervale)
        {
            bool senzorTemperaturaStricat = false;
            bool senzorUmiditateStricat = false;

            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<DataToBeProcessed> dataFromTableDataToBeProcessed = new List<DataToBeProcessed>();
            try
            {
                DBConn.Open();
                string getDataFromDataTable = "SELECT * FROM DataToBeProcessed WHERE Zona =" + i;
                getCommand = new SqlCommand(getDataFromDataTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataFromTableDataToBeProcessed.Add(new DataToBeProcessed
                        {
                            Zona = Convert.ToInt32(reader["Zona"]),
                            Data = Convert.ToDateTime(reader["Data"]),
                            Temperatura = Convert.ToDouble(reader["Temperatura"]),
                            Umiditate = Convert.ToDouble(reader["Umiditate"])
                        });
                    }
                }
            }
            catch (Exception exp)
            {
            }

            foreach (var item in dataFromTableDataToBeProcessed)
            {
                if (item.Umiditate > Intervale[0].Umididate_Max || item.Umiditate < Intervale[0].Umididate_Min)
                {
                    senzorUmiditateStricat = true;
                    break;
                }
            }
            foreach (var item in dataFromTableDataToBeProcessed)
            {
                if (item.Temperatura > Intervale[0].Temperatura_Max || item.Temperatura < Intervale[0].Temperatura_Min)
                {
                    senzorTemperaturaStricat = true;
                    break;
                }
            }
            if (senzorUmiditateStricat == true && senzorTemperaturaStricat == true)
                return "AmbiiSenzoriStricati";
            else if (senzorUmiditateStricat == true)
                return "SenzorUmiditateStricat";
            else if (senzorTemperaturaStricat == true)
                return "SenzorTemperaturaStricat";
            else
                return "OK";
        }
        public void Irigation()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<Data> colectedInsertionData = new List<Data>();

            var Intervale = GetDataRange();

            try
            {
                for (int i = 1; i <= 20; i++)
                {
                    DBConn.Open();
                    double avg_temperatura = -30;
                    double avg_umiditate = 0;
                    DateTime data_masurarii = DateTime.Now;
                    string NevoieApa = String.Empty;

                    string getDataFromDataToBeProcessedTable = "SELECT AVG(Temperatura) AS avg_temperatura, AVG(Umiditate) AS avg_umiditate, max(Data) AS DataMasurarii FROM DataToBeProcessed WHERE Zona = " + i;
                    getCommand = new SqlCommand(getDataFromDataToBeProcessedTable, DBConn);
                    reader = getCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            avg_temperatura = Convert.ToDouble(reader["avg_temperatura"]);
                            avg_umiditate = Convert.ToDouble(reader["avg_umiditate"]);
                            data_masurarii = Convert.ToDateTime(reader["DataMasurarii"]);
                        }
                    }

                    NevoieApa = GetSenzorStricat(i, Intervale);

                    var Zone = GetAreas(i);
                    if (NevoieApa == "OK")
                    {
                        if (avg_umiditate < 40)
                        {
                            NevoieApa = "YES";
                        }
                        else
                        {
                            NevoieApa = "NO";
                        }
                    }
                    colectedInsertionData.Add(new Data
                    {
                        DataInregistrare = data_masurarii,
                        Longitudine = Zone[0].Longitudine,
                        Temperatura = avg_temperatura,
                        Latitudine = Zone[0].Latitudine,
                        Umiditate = avg_umiditate,
                        NevoieIrigare = NevoieApa
                    });


                    DBConn.Close();
                }

                foreach (var item in colectedInsertionData)
                {
                    InsertDataIntoDataTable(item);

                }

                DeleteDataToBeProcessed();
                
            }
            catch (Exception exp)
            {
            }
        }

        public void InsertDataIntoDataTable(Data item)
        {
            SqlConnection DBConn = null;
            SqlCommand insertCommand = null;
            try
            {
                DBConn = new SqlConnection(_connectionString);
                DBConn.Open();

                
               

                string insertCmd = string.Format
                        (
                          "INSERT INTO Data VALUES({0},{1},{2},{3},'{4}','{5}')",
                          item.Latitudine,
                          item.Longitudine,
                          item.Temperatura,
                          item.Umiditate,
                          item.DataInregistrare.ToString("yyyy-dd-MM hh:mm:ss"),
                          item.NevoieIrigare.ToString()
                        );

                insertCommand = new SqlCommand(insertCmd, DBConn);
                insertCommand.ExecuteNonQuery();
            }
            catch (Exception exp) { }
            finally
            {
                if (DBConn != null)
                    DBConn.Dispose();
            }
        }
    }
}
