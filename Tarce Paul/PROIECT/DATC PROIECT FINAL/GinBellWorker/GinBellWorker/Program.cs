using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GinBellWorker.Controller;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;


namespace GinBellWorker
{
    class Program
    {
        public static BackgroundTask task;
        static void Main()//string[] args)
        {
            //AsyncronousMessaging asyncMsg = new AsyncronousMessaging();
            //asyncMsg.SendDateDePrelucratIsEmpty("date pot fi generate");
            //ExecuteInBackground();
            

            while (true)
            {

                ExecuteInBackground();
               // Thread.Sleep(9000);
            }
            
        }

        static void ExecuteInBackground()
        {
            task = new BackgroundTask();
            //task.GetInfoDate();
            //task.InsertDate();
            //task.DeleteInfoDate();
            //task.GetInfoDateDePrelucrat();
            //task.DeleteInfoIntervaleDeDate();
            //task.GetInfoIntervaleDeDate();

            ////BackgroundTask task = new BackgroundTask();
            //task.IrigationLogic();

            AsyncronousMessaging asyncMsg = new AsyncronousMessaging();
           
            CloudQueueMessage messsageFromDataGenerator = asyncMsg.ReceiveDateDePrelucratIsReady();
            if (messsageFromDataGenerator.AsString == "datele sunt trimise la api") // DateDePrelucratIsReady
            {
               // task.GetInfoDate();
                task.InsertDate();         // insereaza db2
                task.DeleteInfoDate();    //sterge db1
               
                 AsyncronousMessaging asyncMsg2 = new AsyncronousMessaging();
                // asyncMsg.SendDateDePrelucratIsEmpty("");
                 asyncMsg2.SendDateDePrelucratIsEmpty("date pot fi generate");
                //bag flag pe care il pun in if dar cu alta valoare
                //daca nu merge -> Thread.Sleep(1000);
            }

           // asyncMsg.SendDateDePrelucratIsEmpty("");
        }
    }
}
