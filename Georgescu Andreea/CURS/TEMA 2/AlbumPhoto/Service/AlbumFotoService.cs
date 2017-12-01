using AlbumPhoto.Models;
using AlbumPhoto.Service.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace AlbumPhoto.Service
{
    public class AlbumFotoService
    {
        private CloudStorageAccount _account;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _photoContainer;
        private CloudTableClient _tableClient;
        private CloudTable _filesTable;
        private CloudTable _commentsTable;
        private TableServiceContext _ctx;


        private BlobContainerPermissions _BlobPermissions;
        public static string SAS = string.Empty;


        public AlbumFotoService()
        {
            //_account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("PhotoStorage"));
            _account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            _blobClient = _account.CreateCloudBlobClient();
            _photoContainer = _blobClient.GetContainerReference("poze");
            if (_photoContainer.CreateIfNotExists())
            {
                _photoContainer.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            _tableClient = _account.CreateCloudTableClient();
            _filesTable = _tableClient.GetTableReference("files");
            _filesTable.CreateIfNotExists();
            _commentsTable = _tableClient.GetTableReference("comments");
            _commentsTable.CreateIfNotExists();
            _ctx = _tableClient.GetTableServiceContext();


            _BlobPermissions = new BlobContainerPermissions();
            _BlobPermissions.SharedAccessPolicies.Add("twohourspolicy", new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1),
                Permissions = SharedAccessBlobPermissions.Read
            });
            _BlobPermissions.PublicAccess = BlobContainerPublicAccessType.Off;
            _photoContainer.SetPermissions(_BlobPermissions);
            SAS = _photoContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "twohourspolicy");

        }

        public List<Poza> GetPoze()
        {
            var poze = new List<Poza>();
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var file in query)
            {
                poze.Add(new Poza()
                {
                    Description = file.RowKey,
                    ThumbnailUrl = file.ThumbnailUrl,
                    Url = file.Url,
                    PictureName = file.RowKey

                });
            }

            return poze;
        }

        public List<Comment> GetComment(string pictureName)
        {
            var comment = new List<Comment>();
            if (!string.IsNullOrEmpty(pictureName))
            {
                var query = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                                    select file).AsTableServiceQuery<CommentEntity>(_ctx);


                foreach (var comm in query)
                {
                    if (comm.Text != null)
                    {
                        string[] stringSplit = comm.Text.Split(new string[] { "@@@" }, StringSplitOptions.None);
                        if (stringSplit.Length > 0 && stringSplit[0] == pictureName)
                        {
                            comment.Add(new Comment()
                            {

                                MadeBy = comm.MadeBy,
                                Text = stringSplit[1],
                            });
                        }
                    }
                }
            }
            return comment;
        }
        

        public void IncarcaPoza(string userName, string description, Stream continut)
        {
            var blob = _photoContainer.GetBlockBlobReference(description);
            blob.UploadFromStream(continut);

            _ctx.AddObject(_filesTable.Name, new FileEntity(userName, description)
            {
                PublishDate = DateTime.UtcNow,
                Size = continut.Length,
                Url = blob.Uri.ToString(),
            });

            _ctx.SaveChangesWithRetries();
        }

        public void AddComment(string userName, string comment, string fileName)
        {
            Random rnd = new Random();
            int randomNr = rnd.Next(0, 1000000);
            string description = userName + rnd.ToString();
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, description)
            {
                Text = fileName + "@@@" + comment,
                MadeBy = userName,
            });

            _ctx.SaveChangesWithRetries();
        }
        public string GetLink(string poza)
        {
            
                var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                             where file.RowKey == poza
                             select file).AsTableServiceQuery<FileEntity>(_ctx);

                foreach (var item in query)
                {
                    
                     return GetSasBlobUrl(item.Url);
                }

            return "404";

        }
        public static string GetSasBlobUrl(string fileName)
        {
            
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient sasBlobClient = new CloudBlobClient(storageAccount.BlobEndpoint, new StorageCredentials(SAS));
            CloudBlob blob = (CloudBlob)sasBlobClient.GetBlobReferenceFromServer(new Uri(fileName));
            return blob.Uri.AbsoluteUri + SAS;
        }
    }
}