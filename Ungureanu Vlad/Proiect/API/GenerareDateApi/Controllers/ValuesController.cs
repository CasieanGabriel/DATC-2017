using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using GenerareDateApi.Models;

namespace GenerareDateApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        // GET api/values
        [HttpGet]
        public IEnumerable<Senzor> Get()
        {
            List<Senzor> listaSenzori = new List<Senzor>();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "silviu.database.windows.net";
            builder.UserID = "silviumilu";
            builder.Password = "!Silviu1";
            builder.InitialCatalog = "proiect";
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
            string query = "SELECT * FROM [dbo].[TabelaSenzori]";
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = connection;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string lat = reader[2].ToString().Substring(0, 9);
                string lng = reader[3].ToString().Substring(0, 9);
                listaSenzori.Add(new Senzor(int.Parse(reader[0].ToString()), reader[1].ToString(), lat, lng));
            }

            return listaSenzori.ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IEnumerable<DateSenzor> Get(int id)
        {
            List<DateSenzor> listaDate = new List<DateSenzor>();
            string connectionString= "Server=tcp:silviu.database.windows.net,1433;Initial Catalog=proiect;Persist Security Info=False;User ID= silviumilu; Password = !Silviu1;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "SELECT * FROM [dbo].[TabelaInregistrari] where idsenzor=" + id.ToString();
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                listaDate.Add(new DateSenzor(int.Parse(reader[0].ToString()),int.Parse(reader[1].ToString()),reader[2].ToString(),reader[3].ToString(),reader[4].ToString(),reader[5].ToString()));
            }
            return listaDate.ToArray();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
