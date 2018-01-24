using AlbumPhoto.Models;
using AlbumPhoto.Service.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AlbumPhoto.Service
{
    public class AlbumFotoService
    {
        private CloudStorageAccount _account;
        private CloudBlobClient _blobClient;
        private CloudBlobContainer _photoContainer;
        private CloudTableClient _tableClient;
        private CloudBlobContainer _thumbContainer;
        private CloudTable _filesTable;
        private CloudTable _commentsTable;
        private TableServiceContext _ctx;
        private static List<Poza> poze = new List<Poza>();

        public AlbumFotoService()
        {
            //_account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("PhotoStorage"));
            _account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            _blobClient = _account.CreateCloudBlobClient();
            _photoContainer = _blobClient.GetContainerReference("poze");
            _thumbContainer = _blobClient.GetContainerReference("thumbnail");
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
            poze.Clear();

            var comments = new List<String>();
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var file in query)
            {
                var commQuery = _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                        .AsTableServiceQuery<CommentEntity>(_ctx).ToList().Where(fe => fe.PartitionKey == file.RowKey).ToList();

                List<Comment> Comments = new List<Comment>();
                foreach (var entry in commQuery)
                {
                    Comments.Add(new Comment()
                    {
                        Text = entry.Text,
                        Author = entry.MadeBy,
                        PublishDate = entry.Timestamp
                    });
                }
                poze.Add(new Poza()
                {
                    Description = file.RowKey,
                    ThumbnailUrl = file.ThumbnailUrl,
                    Url = file.Url,//GetTemporaryDownloadUrl(file.Url, "poze", 2.0) // file.Url
                    Comments = Comments
                });
            }

            return poze;
        }
        private string GetContainerSharedAccessUri(CloudBlockBlob blob, double timeToLive)
        {
            SharedAccessBlobPolicy sharedAccessBlobPolicy = new SharedAccessBlobPolicy();
            sharedAccessBlobPolicy.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1);
            sharedAccessBlobPolicy.SharedAccessExpiryTime = DateTime.UtcNow.AddHours(timeToLive);
            sharedAccessBlobPolicy.Permissions = SharedAccessBlobPermissions.Read;
            var sharedAccessSignature = blob.GetSharedAccessSignature(sharedAccessBlobPolicy);
            return blob.Uri + sharedAccessSignature;
        }
        public string GetTemporaryDownloadUrl(string fileName, string containerName, double linkDuration)
        {
            //var container = await GetOrCreateContainerReferenceAsync(containerName);
            var blob = _photoContainer.GetBlockBlobReference(fileName);

            return GetContainerSharedAccessUri(blob, linkDuration);
        }
        public void IncarcaPoza(string userName, string description, Stream continut)
        {
            var blob = _photoContainer.GetBlockBlobReference(description);
            blob.UploadFromStream(continut);
            continut.Position = 0;
            var thumbBlob = _thumbContainer.GetBlockBlobReference(description);
            thumbBlob.UploadFromStream(continut);

            _ctx.AddObject(_filesTable.Name, new FileEntity(userName, description)
            {
                PublishDate = DateTime.UtcNow,
                Size = continut.Length,
                Url = GetTemporaryDownloadUrl(description, "poze", 2.0),//blob.Uri.ToString(),
                ThumbnailUrl = thumbBlob.Uri.ToString()
            });

            _ctx.SaveChangesWithRetries();
        }

        public void AdaugaComentariu(string user, string poza, string comentariu)
        {
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(poza, Guid.NewGuid().ToString())
            {
                Text = comentariu,
                MadeBy = user,
            });

            _ctx.SaveChangesWithRetries();
        }

        public static List<Poza> GetPozeRef()
        {
            return poze;
        }
    }
}