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

        public CloudQueueMessage ReceiveDateDePrelucratIsReady()
        {
            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(StorageAccountName, StorageAccountKey), true);
            var client = storageAccount.CreateCloudQueueClient();
            var queue = client.GetQueueReference("date-de-prelucrat-is-ready");
            queue.CreateIfNotExists();

            var messsageFromDataGenerator = String.Empty;
            while (true)
            {
                var message = queue.GetMessage();
                if (message != null)
                {
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
            var queue = client.GetQueueReference("date-de-prelucrat-is-empty");
            queue.CreateIfNotExists();

            queue.AddMessage(new CloudQueueMessage(message));
        }
    }
}
