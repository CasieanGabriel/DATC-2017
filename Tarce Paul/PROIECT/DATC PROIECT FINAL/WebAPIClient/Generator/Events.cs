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
    public class Events
    {
        public Events()
        {
        }

        public Events(DateTime timestamp, int parkingId, int status)
        {
            Timestamp = timestamp;
            ParkingId = parkingId;
            Status = status;
        }

        public DateTime Timestamp { get; set; }
        public int ParkingId { get; set; }
        public int Status { get; set; }
       

    }
}
