using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Generator
{
    class AsyncronousMessaging
    {
        public static string StorageAccountName = "ginbellstorage";          
        public static string StorageAccountKey = "iKP5XlGsT1aTWzegm4GxF0S64zg6TvfomlYPrsZ0JJbpIc/pL9TCcE5d5zo0QDxV+n/I17xWxfOOcyHG96X7Jg==";

        public CloudQueueMessage ReceiveDateDePrelucratIsReady()
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(StorageAccountName, StorageAccountKey), true);
            var client = storageAccount.CreateCloudQueueClient();
            var queue = client.GetQueueReference("coada2");
            queue.CreateIfNotExists();

            var messsageFromDataGenerator = String.Empty;
            while (true)
            {
                var message = queue.GetMessage();
                if (message != null)
                {
                    //prelucrare
                    queue.Clear();
                    return message;
                }
            }
        }

        public void SendDateDePrelucratIsEmpty(String message)
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(StorageAccountName, StorageAccountKey), true);
            var client = storageAccount.CreateCloudQueueClient();
            var queue = client.GetQueueReference("coada1");
            queue.CreateIfNotExists();

            queue.AddMessage(new CloudQueueMessage(message));
        }
    }
}
