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
            ExecuteInBackground();
        }

        static void ExecuteInBackground()
        {
            task = new BackgroundTask();
            //task.GetInfoDate();
            //task.DeleteInfoDate();
            //task.GetInfoDateDePrelucrat();
            //task.DeleteInfoIntervaleDeDate();
            //task.GetInfoIntervaleDeDate();

            ////BackgroundTask task = new BackgroundTask();
            //task.IrigationLogic();

            AsyncronousMessaging asyncMsg = new AsyncronousMessaging();
            //asyncMsg.SendDateDePrelucratIsEmpty("date-de-prelucrat-is-empty");
            CloudQueueMessage messsageFromDataGenerator = asyncMsg.ReceiveDateDePrelucratIsReady();
            //if (messsageFromDataGenerator.AsString == "date-de-prelucrat-is-ready") // DateDePrelucratIsReady
            if (messsageFromDataGenerator.AsString == "mesajul meu")
            {
                //    task.IrigationLogic();
                task.GetInfoDate();
                task.InsertDate();
            }
            asyncMsg.SendDateDePrelucratIsEmpty("date-de-prelucrat-is-empty");
        }
    }
}
