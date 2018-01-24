using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using DATC_API.Models;

namespace DATC_API.Controllers
{
    public class MobileController : ApiController
    {

        // GET: api/Mobile
        public IEnumerable<DateFinale> Get()
        {

            //  citesc din BD ..din cea cu 2 campuri .... si returnez o lista pentru aplicatia mobila
            // return new string[] { "value1", "value2" };
            string _connectionString = "Server=tcp:ginbell-server.database.windows.net,1433;Initial Catalog=GinBell_DB;Persist Security Info=False;User ID=ginbell-server;Password=Parola12!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader = null;
            List<DateFinale> dataFromTableDate = new List<DateFinale>();
            //List<DateDePrelucrat> Date = new List<DateDePrelucrat>();
            try
            {
                DBConn.Open();
                string getDataFromDateTable = "SELECT * FROM DateFinal";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataFromTableDate.Add(new DateFinale
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



        // GET: api/Mobile/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Mobile
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Mobile/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Mobile/5
        public void Delete(int id)
        {
        }
    }
}
