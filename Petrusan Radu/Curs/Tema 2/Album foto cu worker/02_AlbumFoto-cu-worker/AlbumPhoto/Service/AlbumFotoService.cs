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
                    Comentarii = GetComentarii(file.RowKey)
                });
            }

            return poze;
        }

        private List<Comentariu> GetComentarii(string rowKey)
        {
            return _ctx.CreateQuery<CommentEntity>(_commentsTable.Name).Where(c => c.RowKey.Equals(rowKey)).AsTableServiceQuery<CommentEntity>(_ctx).Select(c => new Comentariu() { Autor = c.MadeBy, Text = c.Text }).ToList();
        }

        public void AdaugaComentariu(Comentariu comentariu, string rowKey)
        {
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(comentariu.Autor, rowKey)
            {
                Text = comentariu.Text,
                MadeBy = comentariu.Autor
            });

            _ctx.SaveChangesWithRetries();
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

        public string GenerateLink(string fileName)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("poze");

            if (container.CreateIfNotExists())
            {
                container.SetPermissions(new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            var containerPermissions = new BlobContainerPermissions();
            containerPermissions.SharedAccessPolicies.Add("mypolicy", new SharedAccessBlobPolicy()
              {
                  SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1),
                  SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                  Permissions = SharedAccessBlobPermissions.Read
              });

            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Off;
            container.SetPermissions(containerPermissions);

            var sas = container.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "mypolicy");

            var client = new CloudBlobClient(storageAccount.BlobEndpoint, new StorageCredentials(sas));
            var blob = (CloudBlockBlob)client.GetBlobReferenceFromServer(new Uri(fileName));

            return blob.Uri.AbsoluteUri + sas;
        }
    }
}