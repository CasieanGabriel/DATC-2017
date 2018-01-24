using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using GenerareDateApi.Models;
using System.Data.SqlClient;

namespace GenerareDateApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Umiditate")]
    public class UmiditateController : Controller
    {
        // GET: api/Umiditate
        [HttpGet]
        public IEnumerable<HeatMap> GetUm()
        {
            List<HeatMap> listaHeatMap = new List<HeatMap>();
            string connectionString = "Server=tcp:silviu.database.windows.net,1433;Initial Catalog=proiect;Persist Security Info=False;User ID= silviumilu; Password = !Silviu1;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "SELECT * FROM [dbo].[Umiditate]";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listaHeatMap.Add(new HeatMap(int.Parse(reader[1].ToString()), int.Parse(reader[2].ToString()), reader[3].ToString().Replace(".", ","), reader[4].ToString().Replace(".", ","), reader[5].ToString().Replace(".", ","), reader[6].ToString().Replace(".", ","), reader[7].ToString().Replace(".", ","), reader[8].ToString().Replace(".", ","), reader[9].ToString().Replace(".", ","), reader[10].ToString().Replace(".", ",")));
            }
            return listaHeatMap.ToArray();
        }        
    }
}
