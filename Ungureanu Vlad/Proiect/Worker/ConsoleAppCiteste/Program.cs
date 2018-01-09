using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppCiteste
{
    class Program
    {
        static bool needInsert = false;
        public static List<Senzor> listaSenzori = new List<Senzor>();
        static SqlDataReader reader;
        static SqlConnection conn, connWorker;
        static String query = "SELECT COUNT(IDSenzor) FROM[dbo].[Temperatura]";
        static SqlCommand cmd, cmdWorker;

        static void Init()
        {
            String connString = "Server=tcp:silviu.database.windows.net,1433;Initial Catalog=proiect;Persist Security Info=False;User ID= silviumilu; Password = !Silviu1;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;";
            conn = new SqlConnection(connString);
            connWorker = new SqlConnection(connString);
            conn.Open();
            connWorker.Open();
            cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            reader = cmd.ExecuteReader();
            reader.Read();
            if(int.Parse(reader[0].ToString()) != 594 )
            {
                needInsert = true;
            }
        }
        public static void Main()
        {
            Inregistrare[,] inregistrareActuala = new Inregistrare[12,7];            
            int rand = 1,coloana=1;
            incarcaLngLat();
            string url = "";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            //ConnectionFactory factory = new ConnectionFactory();
            //factory.Uri = new Uri(url.Replace("amqp://", "amqps://"));
            Init();
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "queue1", durable: true, exclusive: false, autoDelete: false, arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    int dots = message.Split('.').Length - 1;
                    Thread.Sleep(dots * 100);

                    Console.WriteLine(" [x] Done");

                    inregistrareActuala[rand, coloana] =(JsonConvert.DeserializeObject<Inregistrare>(message));
                   

                    if (rand > 1 && coloana > 1)
                    {
                        determinaTemperatura(inregistrareActuala, rand, coloana);
                        determinaUmiditate(inregistrareActuala, rand, coloana);
                        determinaPresiune(inregistrareActuala, rand, coloana);
                    }

                    coloana++;
                    if (coloana > 6)
                    {
                        rand++;
                        coloana = 1;
                        if (rand == 12)
                        {
                            rand = 1;
                            coloana = 1;
                            //construct and send rest of calculation
                            determinaTemperatura(inregistrareActuala);
                            determinaUmiditate(inregistrareActuala);
                            determinaPresiune(inregistrareActuala);

                            determinaCoordonate(inregistrareActuala);

                            determinaTemperaturaCol6(inregistrareActuala);
                            determinaUmiditateCol6(inregistrareActuala);
                            determinaPresiuneCol6(inregistrareActuala);

                            SendDB(inregistrareActuala);
                            //SendQueue(inregistrareActuala);
                        }
                    }
                    
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: "queue1", autoAck: false, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }

        private static void determinaPresiuneCol6(Inregistrare[,] listaInregistrare)
        {
            int calcul;
            int coloanaDeCalculat = 6;
            int randDeCalculat;

            for (randDeCalculat = 1; randDeCalculat < 11; randDeCalculat++)
            {
                verifyValue(listaInregistrare, randDeCalculat, coloanaDeCalculat, 3);
                Inregistrare inregistrare = new Inregistrare();
                inregistrare = listaInregistrare[randDeCalculat, coloanaDeCalculat];
                int culPresiuneDominanta = ComputeCuloareRGBPresiune(listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune);
                int culoareCentru = inregistrare.presiune;
                //getSud
                Inregistrare inregistrareSud = new Inregistrare();
                inregistrareSud = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat];
                int culoareSud = ((int)inregistrareSud.presiune / 10) * 10;
                //get West
                Inregistrare inregistrareWest = new Inregistrare();
                inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                int culoareWest = ((int)inregistrareWest.presiune / 10) * 10;
                //get SudWest
                Inregistrare inregistrareSudWest = new Inregistrare();
                inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                int culoareSudWest = ((int)inregistrareSudWest.presiune / 10) * 10;

                if (randDeCalculat != 1)
                {
                    //getNord
                    Inregistrare inregistrareNord = new Inregistrare();
                    inregistrareNord = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat];
                    int culoareNord = ((int)inregistrareNord.presiune / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)(inregistrareNordWest.presiune / 10)) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
                else
                {
                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
                calcul = (culoareCentru * 7 + culoareWest * 3) / 10;  /*Grade Celsius*/
                inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = culPresiuneDominanta;
                calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                listaInregistrare[randDeCalculat, coloanaDeCalculat].inregistrarePresiuneActuala = inregistrare.inregistrarePresiuneActuala;
            }
        }

        private static void determinaUmiditateCol6(Inregistrare[,] listaInregistrare)
        {
            int calcul;
            int coloanaDeCalculat = 6;
            int randDeCalculat;

            for (randDeCalculat = 1; randDeCalculat < 11; randDeCalculat++)
            {
                verifyValue(listaInregistrare, randDeCalculat, coloanaDeCalculat, 2);
                Inregistrare inregistrare = new Inregistrare();
                inregistrare = listaInregistrare[randDeCalculat, coloanaDeCalculat];
                int culUmiditateDominanta = ComputeCuloareRGBUmiditate(listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate);
                int culoareCentru = inregistrare.umiditate;
                //getSud
                Inregistrare inregistrareSud = new Inregistrare();
                inregistrareSud = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat];
                int culoareSud = ((int)inregistrareSud.umiditate / 10) * 10;
                //get West
                Inregistrare inregistrareWest = new Inregistrare();
                inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                int culoareWest = ((int)inregistrareWest.umiditate / 10) * 10;
                //get SudWest
                Inregistrare inregistrareSudWest = new Inregistrare();
                inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                int culoareSudWest = ((int)inregistrareSudWest.umiditate / 10) * 10;

                if (randDeCalculat != 1)
                {
                    //getNord
                    Inregistrare inregistrareNord = new Inregistrare();
                    inregistrareNord = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat];
                    int culoareNord = ((int)inregistrareNord.umiditate / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)(inregistrareNordWest.umiditate / 10)) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
                else
                {
                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
                calcul = (culoareCentru * 7 + culoareWest * 3) / 10;  /*Grade Celsius*/
                inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = culUmiditateDominanta;
                calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                listaInregistrare[randDeCalculat, coloanaDeCalculat].inregistrareUmiditateActuala = inregistrare.inregistrareUmiditateActuala;
            }
        }

        private static void determinaUmiditate(Inregistrare[,] listaInregistrare)
        {
            int calcul;
            int randDeCalculat = 10;
            int coloanaDeCalculat = 6;
            while (true)
            {
                verifyValue(listaInregistrare, randDeCalculat, coloanaDeCalculat, 2);
                Inregistrare inregistrare = new Inregistrare();
                inregistrare = listaInregistrare[randDeCalculat, coloanaDeCalculat];
                int culUmiditateDominanta = ComputeCuloareRGBUmiditate(listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate);
                int culoareCentru = inregistrare.umiditate;

                //getNord
                Inregistrare inregistrareNord = new Inregistrare();
                inregistrareNord = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat];
                int culoareNord = ((int)inregistrareNord.umiditate / 10) * 10;

                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.umiditate / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                    int culoareNordEst = ((int)inregistrareNordEst.umiditate / 10) * 10;

                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.umiditate / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)inregistrareNordWest.umiditate / 10) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = culUmiditateDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
                else if (coloanaDeCalculat == 1)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.umiditate / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                    int culoareNordEst = ((int)inregistrareNordEst.umiditate / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = culUmiditateDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
                else if (coloanaDeCalculat == 6)
                {
                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.umiditate / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)inregistrareNordWest.umiditate / 10) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = culUmiditateDominanta;
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }

                listaInregistrare[randDeCalculat, coloanaDeCalculat].inregistrareUmiditateActuala = inregistrare.inregistrareUmiditateActuala;

                coloanaDeCalculat++;
                if (coloanaDeCalculat > 6)
                {
                    randDeCalculat++;
                    coloanaDeCalculat = 1;
                    if (randDeCalculat == 12)
                    {
                        break;
                    }
                }

            }
        }

        private static void determinaPresiune(Inregistrare[,] listaInregistrare)
        {
            int calcul;
            int randDeCalculat = 10;
            int coloanaDeCalculat = 6;
            while (true)
            {
                verifyValue(listaInregistrare, randDeCalculat, coloanaDeCalculat, 3);
                Inregistrare inregistrare = new Inregistrare();
                inregistrare = listaInregistrare[randDeCalculat, coloanaDeCalculat];
                int culPresiuneDominanta = ComputeCuloareRGBPresiune(listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune);
                int culoareCentru = inregistrare.presiune;

                //getNord
                Inregistrare inregistrareNord = new Inregistrare();
                inregistrareNord = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat];
                int culoareNord = ((int)inregistrareNord.presiune / 10) * 10;

                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.presiune / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                    int culoareNordEst = ((int)inregistrareNordEst.presiune / 10) * 10;

                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.presiune / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)inregistrareNordWest.presiune / 10) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = culPresiuneDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
                else if (coloanaDeCalculat == 1)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.presiune / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                    int culoareNordEst = ((int)inregistrareNordEst.presiune / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = culPresiuneDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
                else if (coloanaDeCalculat == 6)
                {
                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.presiune / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)inregistrareNordWest.presiune / 10) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = culPresiuneDominanta;
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }

                listaInregistrare[randDeCalculat, coloanaDeCalculat].inregistrarePresiuneActuala = inregistrare.inregistrarePresiuneActuala;

                coloanaDeCalculat++;
                if (coloanaDeCalculat > 6)
                {
                    randDeCalculat++;
                    coloanaDeCalculat = 1;
                    if (randDeCalculat == 12)
                    {
                        break;
                    }
                }

            }
        }

        private static void determinaPresiune(Inregistrare[,] listaInregistrare, int rand, int coloana)
        {
            int calcul;
            int randDeCalculat = rand - 1;
            int coloanaDeCalculat = coloana - 1;
            if (coloana == 0)
            {
                coloanaDeCalculat = 6;
            }
            verifyValue(listaInregistrare, randDeCalculat, coloanaDeCalculat, 3);
            Inregistrare inregistrare = new Inregistrare();
            inregistrare = listaInregistrare[randDeCalculat, coloanaDeCalculat];
            int culPresiuneDominanta = ComputeCuloareRGBPresiune(listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune);
            int culoareCentru = inregistrare.presiune;
            //getSud
            Inregistrare inregistrareSud = new Inregistrare();
            inregistrareSud = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat];
            int culoareSud = ((int)inregistrareSud.presiune / 10) * 10;

            if (randDeCalculat > 1 && randDeCalculat < 11)
            {
                //getNord
                Inregistrare inregistrareNord = new Inregistrare();
                inregistrareNord = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat];
                int culoareNord = ((int)inregistrareNord.presiune / 10) * 10;

                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.presiune / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.presiune / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                    int culoareNordEst = ((int)inregistrareNordEst.presiune / 10) * 10;

                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.presiune / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.presiune / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)(inregistrareNordWest.presiune / 10)) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = culPresiuneDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
                else if (coloanaDeCalculat == 1)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.presiune / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.presiune / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                    int culoareNordEst = ((int)inregistrareNordEst.presiune / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = culPresiuneDominanta;
                    inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
                else if (coloanaDeCalculat == 6)
                {
                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.presiune / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.presiune / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)inregistrareNordWest.presiune / 10) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = culPresiuneDominanta;
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
            }
            else if (randDeCalculat == 1)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.presiune / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.presiune / 10) * 10;

                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.presiune / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.presiune / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = culPresiuneDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
                else if (coloanaDeCalculat == 1)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.presiune / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.presiune / 10) * 10;

                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = culPresiuneDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
                else if (coloanaDeCalculat == 6)
                {
                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.presiune / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.presiune / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[1, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    inregistrare.inregistrarePresiuneActuala[2, 2].Culoare = culPresiuneDominanta;
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[2, 3].Culoare = ComputeCuloareRGBPresiune(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 1].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 2].Culoare = ComputeCuloareRGBPresiune(calcul);
                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrarePresiuneActuala[3, 3].Culoare = ComputeCuloareRGBPresiune(calcul);
                }
            }

            listaInregistrare[randDeCalculat, coloanaDeCalculat].inregistrarePresiuneActuala = inregistrare.inregistrarePresiuneActuala;
        }

        private static void determinaUmiditate(Inregistrare[,] listaInregistrare, int rand, int coloana)
        {
            int calcul;
            int randDeCalculat = rand - 1;
            int coloanaDeCalculat = coloana - 1;
            if (coloana == 0)
            {
                coloanaDeCalculat = 6;
            }
            verifyValue(listaInregistrare, randDeCalculat, coloanaDeCalculat, 2);
            Inregistrare inregistrare = new Inregistrare();
            inregistrare = listaInregistrare[randDeCalculat, coloanaDeCalculat];
            int culumiditateDominanta = ComputeCuloareRGBUmiditate(listaInregistrare[randDeCalculat,coloanaDeCalculat].umiditate);//(Color.FromName(((CuloareUmiditate)(((int)inregistrare.umiditate / 10) * 10)).ToString())).ToArgb();
            int culoareCentru = inregistrare.umiditate;
            //getSud
            Inregistrare inregistrareSud = new Inregistrare();
            inregistrareSud = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat];
            int culoareSud = ((int)inregistrareSud.umiditate / 10) * 10;

            if (randDeCalculat > 1 && randDeCalculat < 11)
            {
                //getNord
                Inregistrare inregistrareNord = new Inregistrare();
                inregistrareNord = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat];
                int culoareNord = ((int)inregistrareNord.umiditate / 10) * 10;

                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.umiditate / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.umiditate / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                    int culoareNordEst = ((int)inregistrareNordEst.umiditate / 10) * 10;

                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.umiditate / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.umiditate / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)(inregistrareNordWest.umiditate / 10)) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = culumiditateDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
                else if (coloanaDeCalculat == 1)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.umiditate / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.umiditate / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                    int culoareNordEst = ((int)inregistrareNordEst.umiditate / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = culumiditateDominanta;
                    inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
                else if (coloanaDeCalculat == 6)
                {
                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.umiditate / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.umiditate / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)inregistrareNordWest.umiditate / 10) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = culumiditateDominanta;
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
            }
            else if (randDeCalculat == 1)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.umiditate / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.umiditate / 10) * 10;

                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.umiditate / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.umiditate / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = culumiditateDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
                else if (coloanaDeCalculat == 1)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.umiditate / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.umiditate / 10) * 10;

                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = culumiditateDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
                else if (coloanaDeCalculat == 6)
                {
                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.umiditate / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.umiditate / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[1, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    inregistrare.inregistrareUmiditateActuala[2, 2].Culoare = culumiditateDominanta;
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[2, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 1].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 2].Culoare = ComputeCuloareRGBUmiditate(calcul);
                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareUmiditateActuala[3, 3].Culoare = ComputeCuloareRGBUmiditate(calcul);
                }
            }

            listaInregistrare[randDeCalculat, coloanaDeCalculat].inregistrareUmiditateActuala = inregistrare.inregistrareUmiditateActuala;
        }

        private static void verifyValue(Inregistrare[,] listaInregistrare, int rand, int coloana, int tip)
        {
            int valGresita = 0;
            int valCorectata = 0;
            ///*-20->50*//*Umd: 0->100*//*Pres 720-790*/
            switch (tip)
            {
                case 1:
                    if (listaInregistrare[rand, coloana].temperatura < -20 || listaInregistrare[rand, coloana].temperatura > 50)
                    {
                        /*Temperatura gresita*/
                        valGresita = listaInregistrare[rand, coloana].temperatura;
                        temperaturaGresita(listaInregistrare, rand, coloana);
                        valCorectata = listaInregistrare[rand, coloana].temperatura;
                        updateValue(listaInregistrare,rand,coloana,tip);
                        genErrorLog(tip,valGresita, valCorectata,listaInregistrare[rand,coloana]);
                    }
                    break;
                case 2:
                    if (listaInregistrare[rand, coloana].umiditate < 0 || listaInregistrare[rand, coloana].umiditate > 100)
                    {
                        /*Umiditate gresita*/
                        valGresita = listaInregistrare[rand, coloana].umiditate;
                        umiditateGresita(listaInregistrare, rand, coloana);
                        valCorectata = listaInregistrare[rand, coloana].umiditate;
                        updateValue(listaInregistrare, rand, coloana, tip);
                    }
                    break;
                case 3:
                    if (listaInregistrare[rand, coloana].presiune < 720 || listaInregistrare[rand, coloana].presiune > 790)
                    {
                        /*Presiune gresita*/
                        valGresita = listaInregistrare[rand, coloana].presiune;
                        presiuneGresita(listaInregistrare, rand, coloana);
                        valCorectata = listaInregistrare[rand, coloana].presiune;
                        updateValue(listaInregistrare, rand, coloana, tip);
                    }
                    break;
            }
        }

        private static void genErrorLog(int tip, int valGresita, int valCorectata, Inregistrare inregistrare)
        {
            string connectionString = "Server=tcp:silviu.database.windows.net,1433;Initial Catalog=proiect;Persist Security Info=False;User ID= silviumilu; Password = !Silviu1;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = "";
            switch (tip)
            {
                case 1:
                    query = "Insert into [dbo].[LogErori] ([IDSenzor],[ValoareEronata],[ValoareCorectata],[TipData]) values( " + 
                        inregistrare.idsenzor+","+valGresita+","+valCorectata+",'Temperatura')";
                    break;
                case 2:
                    query = "Insert into [dbo].[LogErori] ([IDSenzor],[ValoareEronata],[ValoareCorectata],[TipData]) values( " +
                        inregistrare.idsenzor + "," + valGresita + "," + valCorectata + ",'Umiditate')";
                    break;
                case 3:
                    query = "Insert into [dbo].[LogErori] ([IDSenzor],[ValoareEronata],[ValoareCorectata],[TipData]) values( " +
                        inregistrare.idsenzor + "," + valGresita + "," + valCorectata + ",'Presiune')";
                    break;
                default:
                    return;
            }
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }

        private static void updateValue(Inregistrare[,] listaInregistrare, int rand, int coloana, int tip)
        {
            string connectionString = "Server=tcp:silviu.database.windows.net,1433;Initial Catalog=proiect;Persist Security Info=False;User ID= silviumilu; Password = !Silviu1;MultipleActiveResultSets=False;Encrypt=False;Connection Timeout=30;";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query= "";
            switch (tip)
            {
                case 1:
                    query = "Update [dbo].[TabelaInregistrari] set temperatura = " + listaInregistrare[rand, coloana].temperatura + " where idsenzor=" + listaInregistrare[rand, coloana].idsenzor;
                    break;
                case 2:
                    query = "Update [dbo].[TabelaInregistrari] set umiditate = " + listaInregistrare[rand, coloana].umiditate + " where idsenzor=" + listaInregistrare[rand, coloana].idsenzor;
                    break;
                case 3:
                    query = "Update [dbo].[TabelaInregistrari] set presiune = " + listaInregistrare[rand, coloana].presiune + " where idsenzor=" + listaInregistrare[rand, coloana].idsenzor;
                    break;
                default:
                    return;
            }
            SqlCommand cmd = new SqlCommand(query);
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
        }

        private static void presiuneGresita(Inregistrare[,] listaInregistrare, int randDeCalculat, int coloanaDeCalculat)
        {
            if (randDeCalculat > 1 && randDeCalculat < 11)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].presiune + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].presiune +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].presiune + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].presiune) / 4;
                }
                else if (coloanaDeCalculat == 1)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].presiune + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].presiune +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].presiune) / 3;
                }
                else if (coloanaDeCalculat == 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].presiune +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].presiune + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].presiune) / 3;
                }
            }
            else if (randDeCalculat == 1)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune =
                        (listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].presiune +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].presiune + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].presiune) / 3;
                }
                else if (coloanaDeCalculat == 1)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune =
                        (listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].presiune +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].presiune) / 2;
                }
                else if (coloanaDeCalculat == 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].presiune +
                        listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].presiune) / 2;
                }
            }
            else if (randDeCalculat == 11)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].presiune + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].presiune +
                         listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].presiune) / 3;
                }
                else if (coloanaDeCalculat == 1)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].presiune + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].presiune) / 2;
                }
                else if (coloanaDeCalculat == 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].presiune =
                         (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].presiune + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].presiune) / 2;
                }
            }
        }

        private static void umiditateGresita(Inregistrare[,] listaInregistrare, int randDeCalculat, int coloanaDeCalculat)
        {
            if (randDeCalculat > 1 && randDeCalculat < 11)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].umiditate + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].umiditate +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].umiditate + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].umiditate) / 4;
                }
                else if (coloanaDeCalculat == 1)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].umiditate + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].umiditate +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].umiditate) / 3;
                }
                else if (coloanaDeCalculat == 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].umiditate +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].umiditate + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].umiditate) / 3;
                }
            }
            else if (randDeCalculat == 1)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate =
                        (listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].umiditate +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].umiditate + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].umiditate) / 3;
                }
                else if (coloanaDeCalculat == 1)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate =
                        (listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].umiditate +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].umiditate) / 2;
                }
                else if (coloanaDeCalculat == 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].umiditate +
                        listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].umiditate) / 2;
                }
            }
            else if (randDeCalculat == 11)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].umiditate + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].umiditate +
                         listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].umiditate) / 3;
                }
                else if (coloanaDeCalculat == 1)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].umiditate + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].umiditate) / 2;
                }
                else if (coloanaDeCalculat == 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].umiditate =
                         (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].umiditate + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].umiditate) / 2;
                }
            }
        }

        private static void temperaturaGresita(Inregistrare[,] listaInregistrare, int randDeCalculat, int coloanaDeCalculat)
        {
            if (randDeCalculat > 1 && randDeCalculat < 11)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].temperatura =
                        (listaInregistrare[randDeCalculat-1, coloanaDeCalculat].temperatura + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].temperatura +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].temperatura + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].temperatura) / 4;
                }
                else if (coloanaDeCalculat == 1)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].temperatura =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].temperatura + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].temperatura +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].temperatura ) / 3;
                }
                else if (coloanaDeCalculat == 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].temperatura =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].temperatura +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].temperatura + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].temperatura) / 3;
                }
            } else if (randDeCalculat == 1)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].temperatura =
                        (listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].temperatura +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].temperatura + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].temperatura) / 3;
                }
                else if (coloanaDeCalculat == 1)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].temperatura =
                        (listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].temperatura +
                        listaInregistrare[randDeCalculat + 1, coloanaDeCalculat].temperatura) / 2;
                }
                else if (coloanaDeCalculat == 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].temperatura =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].temperatura +
                        listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].temperatura) / 2;
                }
            }else if(randDeCalculat == 11)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].temperatura =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].temperatura + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].temperatura +
                         listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].temperatura) / 3;
                }
                else if (coloanaDeCalculat == 1)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].temperatura =
                        (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].temperatura + listaInregistrare[randDeCalculat, coloanaDeCalculat + 1].temperatura) / 2;
                }
                else if (coloanaDeCalculat == 6)
                {
                    listaInregistrare[randDeCalculat, coloanaDeCalculat].temperatura =
                         (listaInregistrare[randDeCalculat - 1, coloanaDeCalculat].temperatura + listaInregistrare[randDeCalculat, coloanaDeCalculat - 1].temperatura) / 2;
                }
            }
        }

        private static void determinaTemperaturaCol6(Inregistrare[,] listaInregistrare)
        {
            int calcul;
            int coloanaDeCalculat = 6;
            int randDeCalculat;

            for(randDeCalculat = 1; randDeCalculat < 11; randDeCalculat++)
            {
                verifyValue(listaInregistrare, randDeCalculat, coloanaDeCalculat, 1);
                Inregistrare inregistrare = new Inregistrare();
                inregistrare = listaInregistrare[randDeCalculat, coloanaDeCalculat];
                int culTemperaturaDominanta = (Color.FromName(((CuloareTemperatura)(((int)inregistrare.temperatura / 10) * 10)).ToString())).ToArgb();
                int culoareCentru = inregistrare.temperatura;
                //getSud
                Inregistrare inregistrareSud = new Inregistrare();
                inregistrareSud = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat];
                int culoareSud = ((int)inregistrareSud.temperatura / 10) * 10;
                //get West
                Inregistrare inregistrareWest = new Inregistrare();
                inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                int culoareWest = ((int)inregistrareWest.temperatura / 10) * 10;
                //get SudWest
                Inregistrare inregistrareSudWest = new Inregistrare();
                inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                int culoareSudWest = ((int)inregistrareSudWest.temperatura / 10) * 10;

                if (randDeCalculat != 1)
                {
                    //getNord
                    Inregistrare inregistrareNord = new Inregistrare();
                    inregistrareNord = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat];
                    int culoareNord = ((int)inregistrareNord.temperatura / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)(inregistrareNordWest.temperatura / 10)) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);
                }else
                {
                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);                    
                }
                calcul = (culoareCentru * 7 + culoareWest * 3) / 10;  /*Grade Celsius*/
                inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = ComputeCuloareRGB(calcul);
                inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = culTemperaturaDominanta;
                calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);

                listaInregistrare[randDeCalculat, coloanaDeCalculat].inregistrareTemperaturaActuala = inregistrare.inregistrareTemperaturaActuala;
            }
        }
         
        public static void incarcaLngLat()
        {
            listaSenzori.Clear();
            StreamReader sr = new StreamReader("senzori.txt");

            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] word = line.Split(' ');
                Senzor s = new Senzor(Convert.ToInt32(word[0]), word[1], word[2]);
                listaSenzori.Add(s);
            }
        }
        private static void determinaTemperatura(Inregistrare[,] listaInregistrare)
        {
            int calcul;
            int randDeCalculat = 10;
            int coloanaDeCalculat = 6;
            while (true)
            {
                verifyValue(listaInregistrare, randDeCalculat, coloanaDeCalculat, 1);
                Inregistrare inregistrare = new Inregistrare();
                inregistrare = listaInregistrare[randDeCalculat,coloanaDeCalculat];
                int culTemperaturaDominanta = (Color.FromName(((CuloareTemperatura)(((int)inregistrare.temperatura / 10) * 10)).ToString())).ToArgb();
                int culoareCentru = inregistrare.temperatura;

                //getNord
                Inregistrare inregistrareNord = new Inregistrare();
                inregistrareNord =  listaInregistrare[randDeCalculat - 1, coloanaDeCalculat];
                int culoareNord = ((int)inregistrareNord.temperatura / 10) * 10;

                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                    {
                        //get Est
                        Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                        int culoareEst = ((int)inregistrareEst.temperatura / 10) * 10;
                        //get NordEst
                        Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                        int culoareNordEst = ((int)inregistrareNordEst.temperatura / 10) * 10;

                        //get West
                        Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                        int culoareWest = ((int)inregistrareWest.temperatura / 10) * 10;
                        //get SudWest
                        Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                        int culoareNordWest = ((int)inregistrareNordWest.temperatura / 10) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = ComputeCuloareRGB(calcul);
                    inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = culTemperaturaDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);
                    }
                    else if (coloanaDeCalculat == 1)
                    {
                        //get Est
                        Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                        int culoareEst = ((int)inregistrareEst.temperatura / 10) * 10;
                        //get NordEst
                        Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                        int culoareNordEst = ((int)inregistrareNordEst.temperatura / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = ComputeCuloareRGB(calcul);
                    inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = culTemperaturaDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);
                    }
                    else if (coloanaDeCalculat == 6)
                    {
                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                        int culoareWest = ((int)inregistrareWest.temperatura / 10) * 10;
                        //get NordWest
                        Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                        int culoareNordWest = ((int)inregistrareNordWest.temperatura / 10) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = ComputeCuloareRGB(calcul);
                    inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = culTemperaturaDominanta;
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);
                    }

                listaInregistrare[randDeCalculat, coloanaDeCalculat].inregistrareTemperaturaActuala = inregistrare.inregistrareTemperaturaActuala;

                coloanaDeCalculat++;
                if (coloanaDeCalculat > 6)
                {
                    randDeCalculat++;
                    coloanaDeCalculat = 1;
                    if (randDeCalculat == 12)
                    {
                        break;
                    }
                }
                
            }
        }

        private static void determinaCoordonate(Inregistrare[,] listaInregistrare)
        {
            int idSenzor = 0;
            double nrLat, nrLng;
            for(int i=1;i<=11;i++)
            {
                for(int j=1;j<=6;j++)
                {
                    //A                  
                    nrLat = double.Parse(listaSenzori[idSenzor].latitudine) + 0.00003;
                    nrLng = double.Parse(listaSenzori[idSenzor].longitudine) - 0.000101;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 1].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 1].LngA = nrLng.ToString();

                    //B
                    nrLat = nrLat + 0.00001;
                   nrLng = nrLng + 0.000057;
                   listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatB = nrLat.ToString();
                   listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 1].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 1].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 1].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 1].LngC = nrLng.ToString();

                    //D
                    nrLat = nrLat - 0.00001;
                    nrLng = nrLng - 0.000057;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 1].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 1].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatB);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngB);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 2].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 2].LngA = nrLng.ToString();

                    //B
                    nrLat = nrLat + 0.00001;
                    nrLng = nrLng + 0.000057;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 2].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 2].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 2].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 2].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 2].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 2].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatB);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngB);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 3].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 3].LngA = nrLng.ToString();

                    //B
                    nrLat = nrLat + 0.00001;
                    nrLng = nrLng + 0.000056;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 3].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 3].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 3].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 3].LngC = nrLng.ToString();

                    //D0
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[1, 3].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[1, 3].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 1].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 1].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 1].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 1].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 1].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 1].LngC = nrLng.ToString();

                    //D
                    nrLat = nrLat - 0.00001;
                    nrLng = nrLng - 0.000057;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 1].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 1].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 2].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 2].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 2].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 2].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 2].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 2].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 2].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 2].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 3].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 3].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 3].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 3].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 3].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 3].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[2, 3].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[2, 3].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 1].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 1].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 1].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 1].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 1].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 1].LngC = nrLng.ToString();

                    //D
                    nrLat = nrLat - 0.00001;
                    nrLng = nrLng - 0.000057;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 1].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 1].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 2].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 2].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 2].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 2].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 2].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 2].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 2].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 2].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 3].LngA = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 3].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 3].LngB = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 3].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 3].LngC = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 3].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareUmiditateActuala[3, 3].LngD = nrLng.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrarePresiuneActuala[3, 3].LngD = nrLng.ToString();

                    idSenzor++;
                }
            }

        }

        public static void determinaTemperatura(Inregistrare[,] listaInregistrare,  int rand, int coloana)
        {
            int calcul;
            int randDeCalculat = rand-1;
            int coloanaDeCalculat = coloana-1;
            if(coloana == 0)
            {
                coloanaDeCalculat = 6;
            }
            verifyValue(listaInregistrare, randDeCalculat, coloanaDeCalculat, 1);
            Inregistrare inregistrare = new Inregistrare();
            inregistrare = listaInregistrare[randDeCalculat, coloanaDeCalculat];
            int culTemperaturaDominanta = (Color.FromName(((CuloareTemperatura)(((int)inregistrare.temperatura / 10) * 10)).ToString())).ToArgb();
            int culoareCentru = inregistrare.temperatura;
            //getSud
            Inregistrare inregistrareSud = new Inregistrare();
            inregistrareSud = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat];
            int culoareSud = ((int)inregistrareSud.temperatura / 10) * 10;

            if (randDeCalculat > 1 && randDeCalculat < 11)
            {
                //getNord
                Inregistrare inregistrareNord = new Inregistrare();
                inregistrareNord = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat];
                int culoareNord =((int)inregistrareNord.temperatura / 10) * 10;

                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.temperatura / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.temperatura / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat+1];
                    int culoareNordEst = ((int)inregistrareNordEst.temperatura / 10) * 10;

                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.temperatura / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.temperatura / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)(inregistrareNordWest.temperatura / 10)) * 10;

                    calcul = (culoareCentru*4 + culoareNord*2 + culoareNordWest*2 + culoareWest*2)/10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = ComputeCuloareRGB(calcul);
                    inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = culTemperaturaDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);
                }
                else if(coloanaDeCalculat == 1)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.temperatura / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst =((int)inregistrareSudEst.temperatura / 10) * 10;
                    //get NordEst
                    Inregistrare inregistrareNordEst = new Inregistrare();
                    inregistrareNordEst = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat + 1];
                    int culoareNordEst = ((int)inregistrareNordEst.temperatura / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = culTemperaturaDominanta;
                    inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);
                }
                else if(coloanaDeCalculat == 6)
                {
                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.temperatura / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.temperatura / 10) * 10;
                    //get NordWest
                    Inregistrare inregistrareNordWest = new Inregistrare();
                    inregistrareNordWest = listaInregistrare[randDeCalculat - 1, coloanaDeCalculat - 1];
                    int culoareNordWest = ((int)inregistrareNordWest.temperatura / 10) * 10;

                    calcul = (culoareCentru * 4 + culoareNord * 2 + culoareNordWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareNord * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 6 + culoareNord * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3)/10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = ComputeCuloareRGB(calcul);
                    inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = culTemperaturaDominanta;
                    calcul = (culoareCentru * 7 + (culoareCentru-5) * 3)/10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);
                }
            }else if(randDeCalculat == 1)
            {
                if (coloanaDeCalculat > 1 && coloanaDeCalculat < 6)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.temperatura / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.temperatura / 10) * 10;

                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.temperatura / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.temperatura / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = ComputeCuloareRGB(calcul);
                    inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = culTemperaturaDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);
                }
                else if (coloanaDeCalculat == 1)
                {
                    //get Est
                    Inregistrare inregistrareEst = new Inregistrare();
                    inregistrareEst = listaInregistrare[randDeCalculat, coloanaDeCalculat + 1];
                    int culoareEst = ((int)inregistrareEst.temperatura / 10) * 10;
                    //get SudEst
                    Inregistrare inregistrareSudEst = new Inregistrare();
                    inregistrareSudEst = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat + 1];
                    int culoareSudEst = ((int)inregistrareSudEst.temperatura / 10) * 10;

                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 6 + culoareEst * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = ComputeCuloareRGB(calcul);
                    inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = culTemperaturaDominanta;
                    calcul = (culoareCentru * 7 + culoareEst * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudEst * 2 + culoareEst * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);
                }
                else if (coloanaDeCalculat == 6)
                {
                    //get West
                    Inregistrare inregistrareWest = new Inregistrare();
                    inregistrareWest = listaInregistrare[randDeCalculat, coloanaDeCalculat - 1];
                    int culoareWest = ((int)inregistrareWest.temperatura / 10) * 10;
                    //get SudWest
                    Inregistrare inregistrareSudWest = new Inregistrare();
                    inregistrareSudWest = listaInregistrare[randDeCalculat + 1, coloanaDeCalculat - 1];
                    int culoareSudWest = ((int)inregistrareSudWest.temperatura / 10) * 10;

                    calcul = (culoareCentru * 6 + culoareWest * 2 + (culoareCentru - 5) * 2) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + (culoareCentru - 8) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[1, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 7 + culoareWest * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 1].Culoare = ComputeCuloareRGB(calcul);
                    inregistrare.inregistrareTemperaturaActuala[2, 2].Culoare = culTemperaturaDominanta;
                    calcul = (culoareCentru * 7 + (culoareCentru - 5) * 3) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[2, 3].Culoare = ComputeCuloareRGB(calcul);

                    calcul = (culoareCentru * 4 + culoareSud * 2 + culoareSudWest * 2 + culoareWest * 2) / 10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 1].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 7 + culoareSud * 3) / 10;  /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 2].Culoare = ComputeCuloareRGB(calcul);
                    calcul = (culoareCentru * 6 + culoareSud * 2 + (culoareCentru - 5) * 2)/10; /*Grade Celsius*/
                    inregistrare.inregistrareTemperaturaActuala[3, 3].Culoare = ComputeCuloareRGB(calcul);
                }
            }

            listaInregistrare[randDeCalculat, coloanaDeCalculat]= inregistrare;
        }

        public static void SendQueue(Inregistrare[,] listaInregistrare)
        {
            List<InregistrareTemperatura> inregTemp = new List<InregistrareTemperatura>();

            for(int i=1;i<=11;i++)
            {
                for(int j=1;j<=6;j++)
                {
                    for(int m=1;m<=3;m++)
                        for(int n=1;n<=3;n++)
                        inregTemp.Add(listaInregistrare[i, j].inregistrareTemperaturaActuala[m,n]);
                }
            }
            inregTemp.RemoveAll(item=>item.LatA == null);
            // CloudAMQP URL in format amqp://user:pass@hostName:port/vhost
            string url = "";

            // Create a ConnectionFactory and set the Uri to the CloudAMQP url
            // the connectionfactory is stateless and can safetly be a static resource in your app
            var connFactory = new ConnectionFactory() { HostName = "localhost" };
            //ConnectionFactory connFactory = new ConnectionFactory();
            //connFactory.Uri = new Uri(url.Replace("amqp://", "amqps://"));
            using (var conn = connFactory.CreateConnection())

            using (var channel = conn.CreateModel())
            {
                // The message we want to put on the queue
                var message = JsonConvert.SerializeObject(inregTemp);
                //var message = item.Id + "," + item.idsenzor + "," + item.temperatura + "," + item.umiditate + "," + item.presiune + "," + item.data;
                // the data put on the queue must be a byte array
                var data = Encoding.UTF8.GetBytes(message);
                // ensure that the queue exists before we publish to it
                var queueName = "queueTemperatura";
                bool durable = true;
                bool exclusive = false;
                bool autoDelete = false;
                channel.QueueDeclare(queueName, durable, exclusive, autoDelete, null);
                // publish to the "default exchange", with the queue name as the routing key
                var exchangeName = "";
                var routingKey = "queueTemperatura";
                channel.BasicPublish(exchangeName, routingKey, null, data);
            }
        }

        public static void SendDB(Inregistrare[,] listaInregistrare)
        {
            List<InregistrareTemperatura> inregTemp = new List<InregistrareTemperatura>();
            List<InregistrareUmiditate> inregUmd = new List<InregistrareUmiditate>();
            List<InregistrarePresiune> inregPres = new List<InregistrarePresiune>();
            int nr = 0;
            for (int i = 1; i <= 11; i++)
            {
                for (int j = 1; j <= 6; j++)
                {
                    for (int m = 1; m <= 3; m++)
                        for (int n = 1; n <= 3; n++)
                        {
                            listaInregistrare[i, j].inregistrareTemperaturaActuala[m, n].Idsenzor = listaInregistrare[i, j].idsenzor;
                            listaInregistrare[i, j].inregistrareUmiditateActuala[m, n].Idsenzor = listaInregistrare[i, j].idsenzor;
                            listaInregistrare[i, j].inregistrarePresiuneActuala[m, n].Idsenzor = listaInregistrare[i, j].idsenzor;
                            inregTemp.Add(listaInregistrare[i, j].inregistrareTemperaturaActuala[m, n]);
                            inregUmd.Add(listaInregistrare[i, j].inregistrareUmiditateActuala[m, n]);
                            inregPres.Add(listaInregistrare[i, j].inregistrarePresiuneActuala[m, n]);
                        }
                }
            }
            inregTemp.RemoveAll(item => item.LatA == null);
            inregUmd.RemoveAll(item => item.LatA == null);
            inregPres.RemoveAll(item => item.LatA == null);
            if (needInsert == true)
            {
                //insert them
                for (int i = 0; i < 594; i++)
                {
                    string queryWorker = "Insert into [dbo].[Temperatura]  ([IDSenzor] ,[Culoare] ,[LatA],[LngA],[LatB],[LngB],[LatC],[LngC],[LatD],[LngD]) values ('"
                       + inregTemp.ElementAt(i).Idsenzor + "' , '" + inregTemp.ElementAt(i).Culoare + "' , '" + inregTemp.ElementAt(i).LatA + "' , '"
          + inregTemp.ElementAt(i).LngA + "' , '" + inregTemp.ElementAt(i).LatB + "' ,  '"
           + inregTemp.ElementAt(i).LngB + "' , '" + inregTemp.ElementAt(i).LatC + "' , '"
            + inregTemp.ElementAt(i).LngC + "' ,  '" + inregTemp.ElementAt(i).LatD + "' ,  '"
            + inregTemp.ElementAt(i).LngD + "' )";
                    cmdWorker = new SqlCommand(queryWorker);
                    cmdWorker.CommandType = System.Data.CommandType.Text;
                    cmdWorker.Connection = connWorker;
                    cmdWorker.ExecuteNonQuery();                    
                }

                for (int i = 0; i < 594; i++)
                {
                    string queryWorker = "Insert into [dbo].[Umiditate]  ([IDSenzor] ,[Culoare] ,[LatA],[LngA],[LatB],[LngB],[LatC],[LngC],[LatD],[LngD]) values ('"
                       + inregUmd.ElementAt(i).Idsenzor + "' , '" + inregUmd.ElementAt(i).Culoare + "' , '" + inregUmd.ElementAt(i).LatA + "' , '"
          + inregUmd.ElementAt(i).LngA + "' , '" + inregUmd.ElementAt(i).LatB + "' ,  '"
           + inregUmd.ElementAt(i).LngB + "' , '" + inregUmd.ElementAt(i).LatC + "' , '"
            + inregUmd.ElementAt(i).LngC + "' ,  '" + inregUmd.ElementAt(i).LatD + "' ,  '"
            + inregUmd.ElementAt(i).LngD + "' )";
                    cmdWorker = new SqlCommand(queryWorker);
                    cmdWorker.CommandType = System.Data.CommandType.Text;
                    cmdWorker.Connection = connWorker;
                    cmdWorker.ExecuteNonQuery();
                }

                for (int i = 0; i < 594; i++)
                {
                    string queryWorker = "Insert into [dbo].[Presiune]  ([IDSenzor] ,[Culoare] ,[LatA],[LngA],[LatB],[LngB],[LatC],[LngC],[LatD],[LngD]) values ('"
                       + inregPres.ElementAt(i).Idsenzor + "' , '" + inregPres.ElementAt(i).Culoare + "' , '" + inregPres.ElementAt(i).LatA + "' , '"
          + inregPres.ElementAt(i).LngA + "' , '" + inregPres.ElementAt(i).LatB + "' ,  '"
           + inregPres.ElementAt(i).LngB + "' , '" + inregPres.ElementAt(i).LatC + "' , '"
            + inregPres.ElementAt(i).LngC + "' ,  '" + inregPres.ElementAt(i).LatD + "' ,  '"
            + inregPres.ElementAt(i).LngD + "' )";
                    cmdWorker = new SqlCommand(queryWorker);
                    cmdWorker.CommandType = System.Data.CommandType.Text;
                    cmdWorker.Connection = connWorker;
                    cmdWorker.ExecuteNonQuery();
                }
                needInsert = false;
            }
            else
            {
                for (int i = 0; i < 594; i++)
                {
                    string queryWorker = "Update [dbo].[Temperatura] set [Culoare]='" + inregTemp.ElementAt(i).Culoare +
         "' where [LatA] = '" + inregTemp.ElementAt(i).LatA + "' and [LngA] = '"
          + inregTemp.ElementAt(i).LngA + "' and [LatB] = '" + inregTemp.ElementAt(i).LatB + "' and [LngB] = '"
           + inregTemp.ElementAt(i).LngB + "' and [LatC] = '" + inregTemp.ElementAt(i).LatC + "' and [LngC] = '"
            + inregTemp.ElementAt(i).LngC + "' and [LatD] = '" + inregTemp.ElementAt(i).LatD + "' and [LngD] = '"
            + inregTemp.ElementAt(i).LngD + "' ";
                    cmdWorker = new SqlCommand(queryWorker);
                    cmdWorker.CommandType = System.Data.CommandType.Text;
                    cmdWorker.Connection = connWorker;
                    nr += cmdWorker.ExecuteNonQuery();
                }

                for (int i = 0; i < 594; i++)
                {
                    string queryWorker = "Update [dbo].[Umiditate] set [Culoare]='" + inregUmd.ElementAt(i).Culoare +
         "' where [LatA] = '" + inregUmd.ElementAt(i).LatA + "' and [LngA] = '"
          + inregUmd.ElementAt(i).LngA + "' and [LatB] = '" + inregUmd.ElementAt(i).LatB + "' and [LngB] = '"
           + inregUmd.ElementAt(i).LngB + "' and [LatC] = '" + inregUmd.ElementAt(i).LatC + "' and [LngC] = '"
            + inregUmd.ElementAt(i).LngC + "' and [LatD] = '" + inregUmd.ElementAt(i).LatD + "' and [LngD] = '"
            + inregUmd.ElementAt(i).LngD + "' ";
                    cmdWorker = new SqlCommand(queryWorker);
                    cmdWorker.CommandType = System.Data.CommandType.Text;
                    cmdWorker.Connection = connWorker;
                    nr += cmdWorker.ExecuteNonQuery();
                }

                for (int i = 0; i < 594; i++)
                {
                    string queryWorker = "Update [dbo].[Presiune] set [Culoare]='" + inregPres.ElementAt(i).Culoare +
         "' where [LatA] = '" + inregPres.ElementAt(i).LatA + "' and [LngA] = '"
          + inregPres.ElementAt(i).LngA + "' and [LatB] = '" + inregPres.ElementAt(i).LatB + "' and [LngB] = '"
           + inregPres.ElementAt(i).LngB + "' and [LatC] = '" + inregPres.ElementAt(i).LatC + "' and [LngC] = '"
            + inregPres.ElementAt(i).LngC + "' and [LatD] = '" + inregPres.ElementAt(i).LatD + "' and [LngD] = '"
            + inregPres.ElementAt(i).LngD + "' ";
                    cmdWorker = new SqlCommand(queryWorker);
                    cmdWorker.CommandType = System.Data.CommandType.Text;
                    cmdWorker.Connection = connWorker;
                    nr += cmdWorker.ExecuteNonQuery();
                }
            }
        }
        public static int ComputeCuloareRGB(int numarGrade)
        {
            Color color;
            int zecimal = numarGrade / 10;
            int unitate = numarGrade % 10;
            int rgb = 0;

            switch (zecimal)
            {
                case 5:
                    color = Color.FromArgb(128, 0, 128);
                    rgb = color.ToArgb();
                    //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                    break;
                case 4:
                    switch (unitate)
                        {
                            case 9:
                            color = Color.FromArgb(128, 0, 128);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                            case 8:
                            case 7:
                            color = Color.FromArgb(153, 0, 102);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                            case 6:
                            case 5:
                            color = Color.FromArgb(179, 0, 77);
                            rgb = color.ToArgb();
                          //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                                break;
                            case 4:
                            case 3:
                            color = Color.FromArgb(204, 0, 51);
                            rgb = color.ToArgb();
                           // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                                break;
                            case 2:
                            case 1:
                            color = Color.FromArgb(230, 0, 26);
                            rgb = color.ToArgb();
                           // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                                break;
                            case 0:
                            color = Color.FromArgb(255, 0, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                        }
                    break;
                case 3:
                    switch (unitate)
                    {
                        case 9:
                        case 8:
                            color = Color.FromArgb(255, 55, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FF3700", System.Globalization.NumberStyles.HexNumber);
                            break;                        
                        case 7:
                        case 6:
                            color = Color.FromArgb(255, 110, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FF6E00", System.Globalization.NumberStyles.HexNumber);
                            break;
                        
                        case 5:
                            color = Color.FromArgb(255, 165, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FFA500", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(255, 195, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FFC300", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(255, 225, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FFE100", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(255, 255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FFFF00", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 2:
                    switch (unitate)
                    {
                        case 9:
                        case 8:
                            color = Color.FromArgb(170, 255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("AAFF00", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 7:
                        case 6:
                            color = Color.FromArgb(85,255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("55FF00", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case 5:
                            color = Color.FromArgb(0, 255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("00FF00", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(0, 213, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("00D500", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(0, 170, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("00AA00", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(0, 128, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("008000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 1:
                    switch (unitate)
                    {
                        case 9:
                        case 8:
                            color = Color.FromArgb(21, 160, 69);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("15A045", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 7:
                        case 6:
                            color = Color.FromArgb(43, 192, 139);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("2BC08B", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case 5:
                            color = Color.FromArgb(64, 224, 208);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("40E0D0", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(88, 218, 217);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("58DAD9", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(111, 212, 226);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("6FD4E2", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(135, 206, 235);
                            rgb = color.ToArgb();
                           // rgb = int.Parse("87CEEB", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 0:
                    switch (unitate)
                    {
                        case 9:
                        case 8:
                            color = Color.FromArgb(123, 187, 236);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("7BBBEC", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 7:
                        case 6:
                            color = Color.FromArgb(112,168, 236);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("70A8EC", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case 5:
                            color = Color.FromArgb(100, 149, 237);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("6495ED", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(77, 147, 243);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("4D93F3", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(53, 146, 249);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("3592F9", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(30, 144, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("1E90FF", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -9:
                        case -8:
                            color = Color.FromArgb(22, 35, 160);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("1623A0", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -7:
                        case -6:
                            color = Color.FromArgb(43, 70, 163);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("2B46C1", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case -5:
                            color = Color.FromArgb(65, 105, 225);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("4169E1", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -4:
                        case -3:
                            color = Color.FromArgb(53, 118, 235);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("3576EB", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -2:
                        case -1:
                            color = Color.FromArgb(42, 131, 245);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("2A83F5", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case -1:
                    switch (unitate)
                    {
                        case -9:
                        case -8:
                            color = Color.FromArgb(0, 0, 21);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("000015", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -7:
                        case -6:
                            color = Color.FromArgb(0, 0, 43);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("00002B", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case -5:
                            color = Color.FromArgb(0, 0, 64);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("000040", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -4:
                        case -3:
                            color = Color.FromArgb(0, 0, 85);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("000055", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -2:
                        case -1:
                            color = Color.FromArgb(0, 0, 107);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("00006B", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(0, 0, 128);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("000080", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case -2:
                    color = Color.FromArgb(0, 0, 0);
                    rgb = color.ToArgb();
                    //rgb = int.Parse("000000", System.Globalization.NumberStyles.HexNumber);
                    break;
            }
            
            return rgb;
        }
        public static int ComputeCuloareRGBUmiditate(int numarGrade)
        {
            Color color;
            int zecimal = numarGrade / 10;
            int unitate = numarGrade % 10;
            int rgb = 0;

            switch (zecimal)
            {
                case 10:
                    switch (unitate)
                    {
                        case 0:
                            color = Color.FromArgb(102, 0, 204);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                        default:
                            rgb = 0;
                            break;
                    }
                    break;
                case 9:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(102, 0, 204);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(82, 0, 214);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(61, 0, 224);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(41, 0, 235);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(20, 0, 245);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(0, 0, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 8:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(0, 0, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(0, 31, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(0, 61, 255);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(0, 92, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(0, 122, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(0, 153, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 7:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(0, 153, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(0, 163, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(0, 173, 255);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(0, 184, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(0, 194, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(0, 204, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 6:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(0, 204, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(0, 214, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(0, 224, 255);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(0, 235, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(0, 245, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(0, 255, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 5:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(0, 255, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(10, 255, 204);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(20, 255, 153);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(31, 255, 102);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(41, 255, 51);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(51, 255, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 4:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(51, 255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(92, 245, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(133, 235, 0);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(173, 224, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(214, 214, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(255, 204, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 3:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(255, 204, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(255, 194, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(255, 184, 0);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(255, 173, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(255, 163, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(255, 153, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 2:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(255, 153, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(255, 143, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(255, 133, 0);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(255, 122, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(255, 112, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(255, 102, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 1:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(255, 102, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(255, 92, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(255, 82, 0);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(255, 71, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(255, 61, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(255, 51, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 0:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(255, 51, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(255, 41, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(255, 31, 0);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(255, 20, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(255, 10, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(255, 0, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                default:
                    rgb = 0;
                    break;

            }

            return rgb;
        }
        public static int ComputeCuloareRGBPresiune(int numarGrade)
        {
            Color color;
            int zecimal = numarGrade / 10;
            zecimal = zecimal % 10;
            int unitate = numarGrade % 10;
            int rgb = 0;

            switch (zecimal)
            {
                case 2:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(0, 102, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(0, 82, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(0, 61, 255);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(0, 41, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(0, 20, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(0, 0, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 3:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(0, 255, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(0, 224, 255);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(0, 194, 255);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(0, 163, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(0, 133, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(0, 102, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 4:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(51, 255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(41, 255, 51);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(31, 255, 102);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(20, 255, 153);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(10, 255, 204);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(0, 255, 255);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 5:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(153, 255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(133, 255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(112, 255, 0);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(92, 255, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(71, 255, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(51, 255, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 6:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(255, 255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(235, 255, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(214, 255, 0);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(194, 255, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(173, 255, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(153, 255, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 7:
                    switch (unitate)
                    {
                        case 9:
                            color = Color.FromArgb(255, 102, 51);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 8:
                        case 7:
                            color = Color.FromArgb(255, 133, 41);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 6:
                        case 5:
                            color = Color.FromArgb(255, 163, 31);
                            rgb = color.ToArgb();
                            //  rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(255, 194, 20);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(255, 224, 10);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(255, 255, 0);
                            rgb = color.ToArgb();
                            // rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 8:
                    switch (unitate)
                    {
                        case 9:
                        case 8:
                            color = Color.FromArgb(255, 0, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FF3700", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 7:
                        case 6:
                            color = Color.FromArgb(255, 20, 10);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FF6E00", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case 5:
                            color = Color.FromArgb(255, 41, 20);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FFA500", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            color = Color.FromArgb(255, 61, 31);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FFC300", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            color = Color.FromArgb(255, 82, 41);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FFE100", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            color = Color.FromArgb(255, 102, 51);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("FFFF00", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 9:
                    switch (unitate)
                    {                       
                        case 0:
                            color = Color.FromArgb(255, 0, 0);
                            rgb = color.ToArgb();
                            //rgb = int.Parse("008000", System.Globalization.NumberStyles.HexNumber);
                            break;
                        default:
                            rgb = 0;
                            break;
                    }
                    break;
                default:
                    rgb = 0;
                    break;
            }

            return rgb;
        }
    }
}
