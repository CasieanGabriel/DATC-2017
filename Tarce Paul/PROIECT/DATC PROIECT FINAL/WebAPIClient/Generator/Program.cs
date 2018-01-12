using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            int flag = 0;
            string a = null;


            while (true)
            {
                //AsyncronousMessaging asyncMsg = new AsyncronousMessaging();
                // CloudQueueMessage messsageFromDataGenerator = asyncMsg.ReceiveDateDePrelucratIsReady();
                //if (messsageFromDataGenerator.AsString == "datele trebuie generate") // DateDePrelucratIsReady
                //while(true)
                // {
                if (flag == 1)
                {
                    AsyncronousMessaging asyncMsg = new AsyncronousMessaging();
                    CloudQueueMessage messsageFromDataGenerator = asyncMsg.ReceiveDateDePrelucratIsReady();
                    
                    if (messsageFromDataGenerator.AsString == "date pot fi generate") // DateDePrelucratIsReady
                    {
                        asyncMsg.SendDateDePrelucratIsEmpty("");
                        Generare();
                    }
                }
                if (flag == 0)
                {
                    Generare();
                    flag = 1;
                }

                // One event per device


                //}
            }

        }

        static void Generare()
        {

            List<Events> eventList = new List<Events>();
            int numberofdevices = 10;
            Random random = new Random();
            int idParcare = 1;

            for (int devices = 0; devices < numberofdevices; devices++)
            {
                Events events = new Events();

                events.Timestamp = DateTime.Now;
                events.ParkingId = idParcare;
                events.Status = random.Next(3);

                eventList.Add(events);
                idParcare++;
            }

            //////////////////
            HttpClient client = new HttpClient();
            List<string> eventListApi = new List<string>();

            var jsonString = "";
            int eventId = 0;
            foreach (Events ev in eventList)
            {
                eventId++;
                jsonString = JsonConvert.SerializeObject(ev);
                eventListApi.Add(jsonString);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                // var result = client.PostAsync("http://localhost:54287/api/values", content).Result;
                var result = client.PostAsync("https://datcproiectginbell.azurewebsites.net/api/values", content).Result;
                if (eventId == 10)
                {
                    AsyncronousMessaging asyncMsg2 = new AsyncronousMessaging();
                    asyncMsg2.SendDateDePrelucratIsEmpty("datele sunt trimise la api");
                    //Thread.Sleep(20000);
                }

            }
        }
    }
}
