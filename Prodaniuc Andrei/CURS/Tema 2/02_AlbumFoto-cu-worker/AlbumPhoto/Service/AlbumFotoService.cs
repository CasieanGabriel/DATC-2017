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
        private string _sharedAccessSignature;

        public AlbumFotoService()
        {
            //_account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("PhotoStorage"));
            _account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            _blobClient = _account.CreateCloudBlobClient();
            _photoContainer = _blobClient.GetContainerReference("poze");
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();

            containerPermissions.SharedAccessPolicies.Add(
                "TwoHoursAccessPolicy", new SharedAccessBlobPolicy
                {
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(1),
                    Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Read
                });
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Off;

            if (_photoContainer.CreateIfNotExists())
            {
                
            }
            _photoContainer.SetPermissions(containerPermissions);
            _sharedAccessSignature = _photoContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy(),"TwoHoursAccessPolicy");

            _tableClient = _account.CreateCloudTableClient();
            _filesTable = _tableClient.GetTableReference("files");
            _filesTable.CreateIfNotExists();
            _commentsTable = _tableClient.GetTableReference("comments");
            _commentsTable.CreateIfNotExists();
            _ctx = _tableClient.GetTableServiceContext();
        }

        public List<Comment> GetComments(string key)
        {
            var comments = new List<Comment>();
            var query = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                         select file).AsTableServiceQuery<CommentEntity>(_ctx).Where(c => c.RowKey == key);

            foreach (var comment in query)
            {
                comments.Add(new Comment()
                {
                    Text = comment.Text,
                    File = comment.RowKey,
                    User = comment.PartitionKey,
                    Timestamp = comment.Timestamp,
                    MadeBy = comment.MadeBy
                });
            }
            return comments;
        }

        public  string GetAccessUrl(string fileName)
        {
            CloudBlobClient sasBlobClient = new CloudBlobClient(_account.BlobEndpoint, new StorageCredentials(_sharedAccessSignature));

            CloudBlob blob = (CloudBlob)sasBlobClient.GetBlobReferenceFromServer(new Uri(fileName));

            return blob.Uri.AbsoluteUri + _sharedAccessSignature;
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
                    Url = GetAccessUrl(file.Url),
                    Comments = GetComments(file.RowKey)
                });
            }

            return poze;
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

        public void PosteazaComentariu(string commentId, string fileName, string text)
        {
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(commentId, fileName)
            {
                Timestamp = DateTime.UtcNow,
                MadeBy = "anonim",
                Text = text
            });

            _ctx.SaveChangesWithRetries();
        }

    }
}