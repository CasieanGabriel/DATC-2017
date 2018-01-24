using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GenerareDateApi.Models;
using System.Data.SqlClient;

namespace GenerareDateApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Logs")]
    public class LogsController : Controller
    {
        // GET: api/Logs
        [HttpGet]
        public IEnumerable<Log> Get()
        {
            List<Log> listaLogs = new List<Log>();
            string connectionString = "Server=tcp:silviu.database.windows.net,1433;Initial Catalog=proiect;Persist Security Info=False;User ID= silviumilu; Password = !Silviu1;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "SELECT * FROM [dbo].[LogErori] order by Data";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listaLogs.Add(new Log(int.Parse(reader[1].ToString()), int.Parse(reader[2].ToString()), int.Parse(reader[3].ToString()),reader[4].ToString(), reader[5].ToString()));
            }
            return listaLogs.ToArray();
        }

        // GET: api/Logs/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Logs
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Logs/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
