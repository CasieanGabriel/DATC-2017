using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
//using KappaAPI.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Linq;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        public static string connection = "Server=tcp:3g-server.database.windows.net,1433;Initial Catalog = 3G-DB;Persist Security Info=False;User ID = AnaGerber; Password=Angerb48!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";
        
        // GET api/values
        [HttpGet]
        public string Get()
        {
            SqlConnection DBConnection = new SqlConnection(connection);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<RecievedData> recievedDataList = new List<RecievedData>();
            try
            {
                DBConnection.Open();
                string getDataFromDateTable = "SELECT * FROM Data";
                getCommand = new SqlCommand(getDataFromDateTable, DBConnection);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        recievedDataList.Add(new RecievedData
                        {
                            DataInregistrare = Convert.ToDateTime(reader["Data"].ToString()),
                            Longitudine = Convert.ToDouble(reader["Longitudine"].ToString()),
                            Temperatura = Convert.ToDouble(reader["Temperatura"].ToString()),
                            Latitudine = Convert.ToDouble(reader["Latitudine"].ToString()),
                            Umiditate = Convert.ToDouble(reader["Umiditate"].ToString()),
                            NevoieIrigare = reader["NevoieIrigare"].ToString()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            finally
            {
                if (DBConnection != null)
                    DBConnection.Dispose();
            }
            var returnJson = JsonConvert.SerializeObject(recievedDataList);

            return returnJson;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(string latitudine,string longitudine)
        {

            SqlConnection DBConnection = new SqlConnection(connection);
            SqlCommand getCommand = null;
            SqlDataReader reader;
            List<RecievedData> recievedDataList = new List<RecievedData>();
            try
            {
                DBConnection.Open();
                string getDataFromDateTable = string.Format("SELECT * FROM Data WHERE Latitudine = {0} and Longitudine = {1}", latitudine, longitudine);
                getCommand = new SqlCommand(getDataFromDateTable, DBConnection);
                reader = getCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        recievedDataList.Add(new RecievedData
                        {
                            DataInregistrare = Convert.ToDateTime(reader["DataInregistrare"]),
                            Longitudine = Convert.ToDouble(reader["Longitudine"]),
                            Temperatura = Convert.ToDouble(reader["Temperatura"]),
                            Latitudine = Convert.ToDouble(reader["Latitudine"]),
                            Umiditate = Convert.ToDouble(reader["Umiditate"]),
                            NevoieIrigare = reader["NevoieIrigare"].ToString()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            finally
            {
                if (DBConnection != null)
                    DBConnection.Dispose();
            }
            var returnJson = JsonConvert.SerializeObject(recievedDataList);

            return returnJson;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] dynamic value)
        {
            IEnumerable<DateSenzori> IdateDePrelucrat;
            IEnumerable<IntervalValide> IintervalDeDate;
            SqlConnection DBConnection = null;
            SqlCommand insertCommand = null;
            SqlCommand deleteCommand = null;
            string dataString = Convert.ToString(value);

            try
            {
               // IdateDePrelucrat = JsonConvert.DeserializeObject<IEnumerable<DateSenzori>>(value);
                IintervalDeDate = JsonConvert.DeserializeObject<IEnumerable<IntervalValide>>(dataString);
                if (IintervalDeDate == null || (IintervalDeDate.First().Temperatura_Max == 0 && IintervalDeDate.First().Temperatura_Min == 0 &&
                                                IintervalDeDate.First().Umididate_Max == 0 && IintervalDeDate.First().Umididate_Min == 0))
                {
                    try
                    {
                        IdateDePrelucrat = JsonConvert.DeserializeObject<IEnumerable<DateSenzori>>(dataString);
                        DBConnection = new SqlConnection(connection);
                        DBConnection.Open();
                        foreach (var item in IdateDePrelucrat)
                        {
                            try
                            {
                                string insertCmd = string.Format
                                    (
                                        "INSERT INTO DataToBeProcessed VALUES({0},{1},{2},{3})",
                                        item.Zona, "'" + item.Data + "'" ,item.Temperatura, item.Umiditate
                                    );
                                insertCommand = new SqlCommand(insertCmd, DBConnection);
                                insertCommand.ExecuteNonQuery();
                                deleteCommand.Dispose();
                            }
                            catch (Exception e)
                            {
                                e.Message.ToString();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        e.Message.ToString();
                    }
                    string mesaj = "ti-am_trimis";
                    RabbitMQ rabbitmq = new RabbitMQ();
                   // rabbitmq.Publish(mesaj);
                }
                else
                {
                    DBConnection = new SqlConnection(connection);
                    DBConnection.Open();
                    string deleteCmd = "DELETE FROM DataRange";
                    string insertCmd = string.Format
                            (
                                "INSERT INTO DataRange VALUES({0},{1},{2},{3})",
                                IintervalDeDate.First().Temperatura_Min, IintervalDeDate.First().Temperatura_Max, IintervalDeDate.First().Umididate_Min, IintervalDeDate.First().Umididate_Max

                            );
                    insertCommand = new SqlCommand(insertCmd, DBConnection);
                    deleteCommand = new SqlCommand(deleteCmd, DBConnection);
                    deleteCommand.ExecuteNonQuery();
                    insertCommand.ExecuteNonQuery();
                    insertCommand.Dispose();
                    deleteCommand.Dispose();
                }
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            finally
            {
                if (DBConnection != null)
                    DBConnection.Dispose();
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
