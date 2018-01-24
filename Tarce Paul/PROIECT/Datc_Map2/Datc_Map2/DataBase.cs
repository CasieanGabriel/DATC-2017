using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Datc_Map2.Models;
using System.Data.SqlClient;

namespace Datc_Map2
{
    public class DataBase
    {           
        private string  _connectionString = "Server=tcp:ginbell-server.database.windows.net,1433;Initial Catalog=GinBell_DB;Persist Security Info=False;User ID=ginbell-server;Password=Parola12!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
      //  https://forums.xamarin.com/discussion/93842/xamarin-android-with-azure-sql-database
        public List<DateDePrelucrat> GetInfoDate()
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
           // List<Date> dataFromTableDate = new List<Date>();
            List<DateDePrelucrat> Date = new List<DateDePrelucrat>();
            try // doarACIS
            {
                DBConn.Open();
                string getDataFromDateTable = "SELECT * FROM Date";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                      /*  dataFromTableDate.Add(new Date
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Valoare = Convert.ToInt32(reader["Valoare"]),
                            Data = Convert.ToDateTime(reader["Data"])

                        });*/
                        
                        Date.Add(new DateDePrelucrat()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Valoare = Convert.ToInt32(reader["Valoare"])
                        });
                        
                    }

                }
                //InsertDataIntoDateTable(Date);
            }
            catch (Exception exp)
            {
            }

            return Date;
        }
    }
}