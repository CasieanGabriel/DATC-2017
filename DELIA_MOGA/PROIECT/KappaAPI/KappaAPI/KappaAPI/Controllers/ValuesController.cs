using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using KappaAPI.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Linq;

namespace KappaAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        public static string _connectionString = "Server=tcp:iogrserver.database.windows.net,1433;Initial Catalog=IoGrDatabase;Persist Security Info=False;User ID=IonutGrad;Password=GradIonut1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";


        [HttpGet]
        public string Get() // get data from Date
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<Date> dataFromTable = new List<Date>();
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
                        dataFromTable.Add(new Date
                        {
                            Data = Convert.ToDateTime(reader["Data"]),
                            Longitude = Convert.ToDouble(reader["Longitude"]),
                            Temperature = Convert.ToDouble(reader["Temperature"]),
                            Latitude = Convert.ToDouble(reader["Latitude"]),
                            Humidity = Convert.ToDouble(reader["Humidity"]),
                            NeedIrigation = reader["NeedIrigation"].ToString()
                        });
                    }
                }
            }
            catch (Exception exp)
            {
            }
            var serializedJson = JsonConvert.SerializeObject(dataFromTable);

            return serializedJson;
        }

        // GET api/values/5
        [HttpGet("{zona}")]
        public string Get(string zona) // get data from Zona
        {
            SqlConnection DBConn = new SqlConnection(_connectionString);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<Zone> dataFromZoneTable = new List<Zone>();
            try
            {
                DBConn.Open();
                string getDataFromDateTable = "SELECT * FROM Zone";
                getCommand = new SqlCommand(getDataFromDateTable, DBConn);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dataFromZoneTable.Add(new Zone
                        {
                            Latitude = Convert.ToDouble(reader["Latitude"]),
                            Longitude = Convert.ToDouble(reader["Longitude"]),
                            Zona = Convert.ToInt32(reader["Zona"])
                        });
                    }
                }
            }
            catch (Exception exp)
            {
            }
            var serializedJson = JsonConvert.SerializeObject(dataFromZoneTable);

            return serializedJson;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] dynamic fromBody)
        {
            IEnumerable<DateDePrelucrat> IdateDePrelucrat;
            SqlConnection DBConn = null;
            SqlCommand insertCommand = null;
            var value = Convert.ToString(fromBody);
            try
            {
                //value = request.Content.ReadAsStringAsync().Result;
                IdateDePrelucrat = JsonConvert.DeserializeObject<IEnumerable<DateDePrelucrat>>(value);
                DBConn = new SqlConnection(_connectionString);
                DBConn.Open();
                if (IdateDePrelucrat.First().Temperature == 0 && IdateDePrelucrat.First().Humidity == 0 && IdateDePrelucrat.First().Zona == 0)
                { }
                else
                {
                    foreach (var item in IdateDePrelucrat)
                    {
                        string insertCmd = string.Format
                            (
                                "INSERT INTO DateDePrelucrat VALUES({0},{1},{2},{3})",
                                item.Zona, item.Data, item.Temperature, item.Humidity
                            );
                        insertCommand = new SqlCommand(insertCmd, DBConn);
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
            catch { }
            finally
            {
                if (DBConn != null)
                    DBConn.Dispose();
            }

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
