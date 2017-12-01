using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrrigationWorker.Services
{
    public class DAL
    {
        private readonly string connString = "Server=tcp:irrigationtempserver.database.windows.net,1433;Initial Catalog=IrrigationTempDB;Persist Security Info=False;User ID=aprodaniuc;Password=P@ssw0rd123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public void SaveWeatherInfo(string info)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using(SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = string.Format("Insert into WeatherInfo(Info) values('{0}')", info);
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }
    }
}
