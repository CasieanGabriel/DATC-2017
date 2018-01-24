using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AlbumPhoto.Service
{
    public class BlobHandler
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GetBlobSasUri(string filePath, string fileName)
        {
            CloudStorageAccount storageAccount =
             CloudStorageAccount.Parse(
              ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("poze");
            container.CreateIfNotExists();

            if (!container.GetPermissions().PublicAccess.Equals(BlobContainerPublicAccessType.Off))
            {
                container.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Off
                });
            }
     
            ICloudBlob blob = blobClient.GetBlobReferenceFromServer(new Uri(filePath));

            SharedAccessBlobPolicy sasConstraints =
                new SharedAccessBlobPolicy
                {
                    SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-1),
                    SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(2), // 2 hours expired
                    Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write  
                };

     
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            return blob.Uri + sasBlobToken;
        }
    }
}