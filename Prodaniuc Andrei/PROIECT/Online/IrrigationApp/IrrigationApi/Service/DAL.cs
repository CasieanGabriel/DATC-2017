using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IrrigationApi.Service
{
    public class DAL
    {
        private readonly string connString="Server=tcp:irrigationtempserver.database.windows.net,1433;Initial Catalog=IrrigationTempDB;Persist Security Info=False;User Id=aprodaniuc;Password=P@ssw0rd123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public List<string> GetWeatherInfo()
        {
            List<string> infos = new List<string>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = string.Format("Select * from WeatherInfo");
                    cmd.Connection = conn;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string info = reader["info"].ToString();
                        infos.Add(info);
                    }
                    conn.Close();
                    return infos;
                }
            }
        }
    }
}