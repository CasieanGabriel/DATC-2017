using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace kappa_webjob
{
    public class AsyncronousMessaging
    {
        public static string StorageAccountName = "kappa";
        public static string StorageAccountKey = "wlz0zxWZDiTpzJj5r5Dkvyj0rYzb2lXHRTNniVsKk0VXOOlStTqmP5/7QPGthVCK+zeuKkRRJce+tDh9j4TE6Q==";

        public CloudQueueMessage ReceiveDateDePrelucratIsEmpty()
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(StorageAccountName, StorageAccountKey), true);
            var client = storageAccount.CreateCloudQueueClient();
            var queue = client.GetQueueReference("date-de-prelucrat-is-empty");
            queue.CreateIfNotExists();

            var messsageFromDataGenerator = String.Empty;
            while (true)
            {
                var message = queue.GetMessage();
               // messsageFromDataGenerator = message.ToString();
                if (message != null)
                {
                    queue.Clear();
                    //prelucrare
                    return message;
                }
            }            
        }

        public void SendDateDePrelucratIsReady(String message)
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(StorageAccountName, StorageAccountKey), true);
            var client = storageAccount.CreateCloudQueueClient();
            var queue = client.GetQueueReference("date-de-prelucrat-is-ready");
            queue.CreateIfNotExists();

            queue.AddMessage(new CloudQueueMessage(message));
        }
    }
}
