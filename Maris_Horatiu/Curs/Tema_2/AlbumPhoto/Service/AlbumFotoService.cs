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

        private BlobContainerPermissions containerPermissions;
        private static string SAS;

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

            containerPermissions = new BlobContainerPermissions();
            containerPermissions.SharedAccessPolicies.Add("twohourspolicy", new SharedAccessBlobPolicy() { SharedAccessStartTime = DateTime.UtcNow.AddSeconds(-10), SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2), Permissions = SharedAccessBlobPermissions.Read });
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Off;
            _photoContainer.SetPermissions(containerPermissions);
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
                    Url = file.Url
                });
            }

            return poze;
        }

        public List<Comment> GetCom(string poza)
        {
            var com = new List<Comment>();
            var query = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name) select file).AsTableServiceQuery<CommentEntity>(_ctx);

            foreach (var file in query)
            {
                if (file.RowKey.Contains(poza))
                {
                    com.Add(new Comment()
                    {
                        Text = file.Text,
                        MadeBy = file.MadeBy,
                    });
                }
            }

            return com;
        }

        public Link GetLink(string poza)
        {
            Link link = new Link();
            List<Poza> poze = new List<Poza>();
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name) select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var file in query)
            {
                if (file.RowKey.Contains(poza))
                {
                    link.LinkPoza = GetSasBlobUrl(file.Url);
                    link.NumePoza = poza;
                }
            }

            return link;
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

        public void IncarcaCom(string author, string text, string poza, Stream continut)
        {
            Random rnd = new Random();
            string description = poza + rnd.Next(1, 999999).ToString();
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(author, description)
            {
                Text = text,
                MadeBy = author,
            });

            _ctx.SaveChangesWithRetries();
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