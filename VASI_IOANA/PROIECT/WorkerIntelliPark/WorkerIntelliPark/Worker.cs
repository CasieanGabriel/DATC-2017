using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WorkerIntelliPark
{
    class Worker
    {
        int number_of_parking_spots = 40;
        string free, occupied_or_reserved, malfunction;
        SqlConnection con, conUpdate;
        String query = "SELECT free, occupied_or_reserved, malfunction FROM senzori WHERE data_citirii = ( SELECT MAX(data_citirii) FROM senzori);";
        SqlCommand sqlCmd, sqlCmdUpdate;

        private string getStatus(int spot_number, string free_or_not, string occupied_or_reserved, string malfunction)
        {
            if (malfunction[spot_number] == '0')
            {
                if (free_or_not[spot_number] == '0')
                    return "F";
                else if (occupied_or_reserved[spot_number] == '0')
                    return "R";
                else
                    return "O";
            }
            else return "M";
        }

        internal void InitializeProjections()
        {
            String status;
            String conString = "Server=tcp:intelli.database.windows.net,1433;Initial Catalog=intellipark;Persist Security Info=False;User ID=intellipark;Password=#3intellius;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; ";
            con = new SqlConnection(conString);
            conUpdate = new SqlConnection(conString);
            con.Open();
            conUpdate.Open();

            sqlCmd = new SqlCommand(query);
            sqlCmd.CommandType = System.Data.CommandType.Text;
            sqlCmd.Connection = con;

            SqlDataReader reader = sqlCmd.ExecuteReader();

            while (reader.Read())
            {
                for (int j = 1; j <= number_of_parking_spots; j++)
                {
                    status = getStatus(j - 1, reader[0].ToString(), reader[1].ToString(), reader[2].ToString());
                    query = "Update [dbo].[Projections] set [status]='" + status + "'where id = " + j;

                    Console.WriteLine(query);
                    Console.WriteLine(j);
                    Console.WriteLine(status);

                    sqlCmd = new SqlCommand(query);
                    sqlCmd.CommandType = System.Data.CommandType.Text;
                    sqlCmd.Connection = conUpdate;
                    sqlCmd.ExecuteNonQuery();

                    free = reader[0].ToString();
                    occupied_or_reserved = reader[1].ToString();
                    malfunction = reader[2].ToString();
                }
            }
            reader.Close();

        }

        internal void UpdateProjectionsAsync()
        {
            string[] parking_lots = new string[] { "A", "B", "C", "D" };
            string parking_lot;
            int parking_space;
            string status;
            SqlDataReader reader;
            String query;
            int nr_loc;
            
            while (true)
            {
                query = "SELECT free, occupied_or_reserved, malfunction FROM senzori WHERE data_citirii = ( SELECT MAX(data_citirii) FROM senzori);";
                sqlCmd = new SqlCommand(query);
                sqlCmd.CommandType = System.Data.CommandType.Text;
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();

                while (reader.Read())
                {
                    if (malfunction != reader[2].ToString() | free != reader[1].ToString() | occupied_or_reserved != reader[0].ToString())
                    {
                        for (int j = 1; j <= number_of_parking_spots; j++)
                            if (malfunction[j - 1] != reader[2].ToString()[j - 1] | free[j - 1] != reader[0].ToString()[j - 1] | occupied_or_reserved[j - 1] != reader[1].ToString()[j - 1])
                            {
                                status = getStatus(j - 1, reader[0].ToString(), reader[1].ToString(), reader[2].ToString());

                                query = "Update [dbo].[Projections] set [status]='" + status + "'where id = " + j;
                                sqlCmdUpdate = new SqlCommand(query);
                                sqlCmdUpdate.CommandType = System.Data.CommandType.Text;
                                sqlCmdUpdate.Connection = conUpdate;
                                sqlCmdUpdate.ExecuteNonQuery();
                            }

                        free = reader[0].ToString();
                        occupied_or_reserved = reader[1].ToString();
                        malfunction = reader[2].ToString();
                    }
                }
                reader.Close();

                query = "SELECT * from rezervari";
                sqlCmd = new SqlCommand(query);
                sqlCmd.CommandType = System.Data.CommandType.Text;
                sqlCmd.Connection = con;
                reader = sqlCmd.ExecuteReader();

                while (reader.Read())
                {
                    if (Convert.ToInt32(reader[2]) == 0)
                    {
                        nr_loc = Convert.ToInt32(reader[0]) - 1;

                        parking_lot = parking_lots[nr_loc / 10];
                        parking_space = nr_loc % 10;

                        HttpClient client = new HttpClient();

                        Dictionary<string, string> d = new Dictionary<string, string>
                        {
                            { "PL", parking_lot },
                            { "PS", (parking_space + 1).ToString() }
                        };

                        var content = new FormUrlEncodedContent(d);

                        var response = client.PostAsync("http://localhost:60454/Home/PostReservationRequests", content);

                        query = "Update rezervari set [transmis]=1 where nr_loc = " + (nr_loc + 1);
                        sqlCmdUpdate = new SqlCommand(query);
                        sqlCmdUpdate.CommandType = System.Data.CommandType.Text;
                        sqlCmdUpdate.Connection = conUpdate;
                        sqlCmdUpdate.ExecuteNonQuery();
                    }
                }
                reader.Close();


            }
        }
    }
}

