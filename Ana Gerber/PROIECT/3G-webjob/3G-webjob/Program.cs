using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _3G_webjob.Controller;
//using Microsoft.WindowsAzure.Storage.Queue;

namespace _3G_webjob
{
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            while (true)
            {
                ExecuteInBackground();
            }
        }

        static void ExecuteInBackground()
        {
            Tasks task = new Tasks();

           

            RabbitMQMsg Msg = new RabbitMQMsg();
            Msg.Publish("am_primit");

            var messsageFromDataGenerator = Msg.Get();

            if (messsageFromDataGenerator == "ti-am_trimis")
            {
                task.Irigation();
            }

            Msg.Publish("am_primit");

            
            
        }
    }
}
