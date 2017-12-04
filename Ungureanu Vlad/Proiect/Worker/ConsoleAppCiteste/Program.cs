using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
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
        public static List<Senzor> listaSenzori = new List<Senzor>();
        public static void Main()
        {
            Inregistrare[,] inregistrareActuala = new Inregistrare[12,7];            
            int rand = 1,coloana=1;
            incarcaLngLat();
            string url = "";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            //ConnectionFactory factory = new ConnectionFactory();
            //factory.Uri = new Uri(url.Replace("amqp://", "amqps://"));

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
                            determinaCoordonate(inregistrareActuala);
                            SendQueue(inregistrareActuala);
                        }
                    }
                    
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };
                channel.BasicConsume(queue: "queue1", autoAck: false, consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
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

                listaInregistrare[randDeCalculat, coloanaDeCalculat] = inregistrare;

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
                    
                    //B
                   nrLat = nrLat + 0.00001;
                   nrLng = nrLng + 0.000057;
                   listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatB = nrLat.ToString();
                   listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngC = nrLng.ToString();

                    //D
                    nrLat = nrLat - 0.00001;
                    nrLng = nrLng - 0.000057;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatB);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngB);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngA = nrLng.ToString();

                    //B
                    nrLat = nrLat + 0.00001;
                    nrLng = nrLng + 0.000057;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatB);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngB);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngA = nrLng.ToString();

                    //B
                    nrLat = nrLat + 0.00001;
                    nrLng = nrLng + 0.000056;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngC = nrLng.ToString();

                    //D0
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngC = nrLng.ToString();

                    //D
                    nrLat = nrLat - 0.00001;
                    nrLng = nrLng - 0.000057;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[1, 3].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngC = nrLng.ToString();

                    //D
                    nrLat = nrLat - 0.00001;
                    nrLng = nrLng - 0.000057;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 1].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngD = nrLng.ToString();

                    //A                  
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatD);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngD);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LatA = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LngA = nrLng.ToString();

                    //B
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[2, 3].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LatB = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LngB = nrLng.ToString();

                    //C
                    nrLat = nrLat - 0.00003;
                    nrLng = nrLng + 0.000011;
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LatC = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LngC = nrLng.ToString();

                    //D
                    nrLat = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LatC);
                    nrLng = double.Parse(listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 2].LngC);
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LatD = nrLat.ToString();
                    listaInregistrare[i, j].inregistrareTemperaturaActuala[3, 3].LngD = nrLng.ToString();

                    idSenzor++;
                }
            }

        }

        public static void determinaTemperatura(Inregistrare[,] listaInregistrare,  int rand, int coloana)
        {
            int calcul;
            int randDeCalculat = rand-1;
            int coloanaDeCalculat = coloana-1;
            if(coloana == 1)
            {
                coloanaDeCalculat = 6;
            }
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
        public static int ComputeCuloareRGB(int numarGrade)
        {
            int zecimal = numarGrade / 10;
            int unitate = numarGrade % 10;
            int rgb = 0;

            switch (zecimal)
            {
                case 5:
                    rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                    break;
                case 4:
                    switch (unitate)
                        {
                            case 9:
                                rgb = int.Parse("800080", System.Globalization.NumberStyles.HexNumber);
                                break;
                            case 8:
                            case 7:
                                rgb = int.Parse("990066", System.Globalization.NumberStyles.HexNumber);
                                break;
                            case 6:
                            case 5:
                                rgb = int.Parse("B3004D", System.Globalization.NumberStyles.HexNumber);
                                break;
                            case 4:
                            case 3:
                                rgb = int.Parse("CC0033", System.Globalization.NumberStyles.HexNumber);
                                break;
                            case 2:
                            case 1:
                                rgb = int.Parse("E6001A", System.Globalization.NumberStyles.HexNumber);
                                break;
                            case 0:
                                rgb = int.Parse("FF0000", System.Globalization.NumberStyles.HexNumber);
                                break;
                        }
                    break;
                case 3:
                    switch (unitate)
                    {
                        case 9:
                        case 8:
                            rgb = int.Parse("FF3700", System.Globalization.NumberStyles.HexNumber);
                            break;                        
                        case 7:
                        case 6:
                            rgb = int.Parse("FF6E00", System.Globalization.NumberStyles.HexNumber);
                            break;
                        
                        case 5:
                            rgb = int.Parse("FFA500", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            rgb = int.Parse("FFC300", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            rgb = int.Parse("FFE100", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            rgb = int.Parse("FFFF00", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 2:
                    switch (unitate)
                    {
                        case 9:
                        case 8:
                            rgb = int.Parse("AAFF00", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 7:
                        case 6:
                            rgb = int.Parse("55FF00", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case 5:
                            rgb = int.Parse("00FF00", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            rgb = int.Parse("00D500", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            rgb = int.Parse("00AA00", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            rgb = int.Parse("008000", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 1:
                    switch (unitate)
                    {
                        case 9:
                        case 8:
                            rgb = int.Parse("15A045", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 7:
                        case 6:
                            rgb = int.Parse("2BC08B", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case 5:
                            rgb = int.Parse("40E0D0", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            rgb = int.Parse("58DAD9", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            rgb = int.Parse("6FD4E2", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            rgb = int.Parse("87CEEB", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case 0:
                    switch (unitate)
                    {
                        case 9:
                        case 8:
                            rgb = int.Parse("7BBBEC", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 7:
                        case 6:
                            rgb = int.Parse("70A8EC", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case 5:
                            rgb = int.Parse("6495ED", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 4:
                        case 3:
                            rgb = int.Parse("4D93F3", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 2:
                        case 1:
                            rgb = int.Parse("3592F9", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            rgb = int.Parse("1E90FF", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -9:
                        case -8:
                            rgb = int.Parse("1623A0", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -7:
                        case -6:
                            rgb = int.Parse("2B46C1", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case -5:
                            rgb = int.Parse("4169E1", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -4:
                        case -3:
                            rgb = int.Parse("3576EB", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -2:
                        case -1:
                            rgb = int.Parse("2A83F5", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case -1:
                    switch (unitate)
                    {
                        case -9:
                        case -8:
                            rgb = int.Parse("000015", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -7:
                        case -6:
                            rgb = int.Parse("00002B", System.Globalization.NumberStyles.HexNumber);
                            break;

                        case -5:
                            rgb = int.Parse("000040", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -4:
                        case -3:
                            rgb = int.Parse("000055", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case -2:
                        case -1:
                            rgb = int.Parse("00006B", System.Globalization.NumberStyles.HexNumber);
                            break;
                        case 0:
                            rgb = int.Parse("000080", System.Globalization.NumberStyles.HexNumber);
                            break;
                    }
                    break;
                case -2:
                    rgb = int.Parse("000000", System.Globalization.NumberStyles.HexNumber);
                    break;
            }

            return rgb;
        }
    }
}
