using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace WebJob1
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration
            {
                StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=datc3;AccountKey=4i4/+waN7+Om5cJ3jzcpCryF5dQjG9xc22yv6I7ezTz5/8WUU1Gi0knnX63TO7g3Uay2M4omTXvOY/W+Jz2nOg==;EndpointSuffix=core.windows.net",
                DashboardConnectionString = "DefaultEndpointsProtocol=https;AccountName=datc3;AccountKey=4i4/+waN7+Om5cJ3jzcpCryF5dQjG9xc22yv6I7ezTz5/8WUU1Gi0knnX63TO7g3Uay2M4omTXvOY/W+Jz2nOg==;EndpointSuffix=core.windows.net"
            };
            // The following code ensures that the WebJob will be running continuously
            //    
            
            var host = new JobHost();
            var worker = new Worker();
            Worker.conexiune();
     //       Worker.proces();
            host.RunAndBlock();
        }
    }
}
