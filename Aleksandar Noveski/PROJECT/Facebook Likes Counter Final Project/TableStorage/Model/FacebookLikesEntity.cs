using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorage.Model
{
    using Microsoft.WindowsAzure.Storage.Table;

    class FacebookLikesEntity : TableEntity
    {
        public FacebookLikesEntity(string id, string message)
        {
            PartitionKey = id;
            RowKey = message;
        }
        public string Message_or_photo { get; set; }
        public int Total_likes { get; set; }
    }
}
