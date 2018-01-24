using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

namespace WebAPIClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public static string StorageAccountName = "ginbellstorage";               //aa
        public static string StorageAccountKey = "iKP5XlGsT1aTWzegm4GxF0S64zg6TvfomlYPrsZ0JJbpIc/pL9TCcE5d5zo0QDxV+n/I17xWxfOOcyHG96X7Jg==";    //aa

        int numberofdevices = 10;
        public MainWindow()
        {
            InitializeComponent();
            GetData();
        }

        private void GetData()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:56851/");

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("api/Events").Result;

            if (response.IsSuccessStatusCode)
            {
                var events = response.Content.ReadAsAsync<IEnumerable<Events>>().Result;

            }
            else
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            /*
            HttpClient client = new HttpClient();
            Events obj = new Events(DateTime.Now,2,2);

            var jsonString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = client.PostAsync("http://localhost:56851/api/values",content).Result;
            */


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:56851/");
            Random random = new Random();
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            int k = 2;
            int limit_variable = 0;
            while (k == 2)
            {
                // One event per device
                for (int devices = 0; devices < numberofdevices; devices++)
                {
                    var events = new Events();

                   // events.Timestamp = DateTime.Now;
                    events.ParkingId = random.Next(10);
                    events.Status = random.Next(3);

                    var response = client.PostAsJsonAsync("api/Events", events).Result;
                    limit_variable++;
                    if (response.IsSuccessStatusCode)
                    {
                        GetData();  //nu merge momentan
                    }
                    else
                    {
                        MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    }
                    if (limit_variable == 10)   // pentru doar 10 senzori sa generez valori
                    {
                        devices = numberofdevices;
                        k = 3;
                        SendDateDePrelucratIsEmpty("mesajul meu");
                    }

                }

            }
        }

        /*
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
                     //prelucrare
                     queue.Clear();
                     return message;
                 }
             }
         }
         */

        /*private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }*/

        /*  private void Button_PreviewStylusButtonDown(object sender, StylusButtonEventArgs e)
          {

          }*/
    }
}

