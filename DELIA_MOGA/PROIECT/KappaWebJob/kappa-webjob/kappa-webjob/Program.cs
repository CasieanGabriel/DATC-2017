using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using kappa_webjob.Controller;
using Microsoft.Azure.WebJobs;

namespace kappa_webjob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var thread = new Thread(ExecuteInForeground);
            thread.Start();
            Thread.Sleep(1000);
            Console.WriteLine("Main thread ({0}) exiting...",
                              Thread.CurrentThread.ManagedThreadId);

            //////var config = new JobHostConfiguration();

            //////if (config.IsDevelopment)
            //////{
            //////    config.UseDevelopmentSettings();
            //////}

            //////var host = new JobHost();
            //////// The following code ensures that the WebJob will be running continuously
            //////host.RunAndBlock();
        }

        static void ExecuteInForeground()
        {
            var task = new BackgroundTask();

            //task.GetInfoDateDePrelucrat();
            //task.ComputeAverage();
            //task.GetInfoIntevaleDeDate();
            //task.IrigationLogic();
        }
    }
}
