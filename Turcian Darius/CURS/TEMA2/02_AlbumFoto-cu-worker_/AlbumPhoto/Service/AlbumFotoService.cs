using AlbumPhoto.Models;
using AlbumPhoto.Service.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
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
            if (!_photoContainer.GetPermissions().PublicAccess.Equals(BlobContainerPublicAccessType.Off))
            {
                _photoContainer.SetPermissions(new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Off
                });
            }

            var poze = new List<Poza>();
            var comentarii = new List<Comentariu>();

            var commentQuery = (from comm in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                                select comm).AsTableServiceQuery<CommentEntity>(_ctx);

            comentarii = commentQuery.Select(commentEntity => new Comentariu()
            {
                MadeBy = commentEntity.MadeBy,
                Text = commentEntity.Text,
                Description = commentEntity.PartitionKey
            }).ToList();

            var fileQuery = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                             select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var file in fileQuery)
            {
                poze.Add(new Poza()
                {
                    Description = file.RowKey,
                    ThumbnailUrl = file.ThumbnailUrl,
                    Url = file.Url
                });
            }

            foreach (var poza in poze)
            {
                var comments = comentarii.FindAll(c => c.Description == poza.Description);
                if (comments.Count > 0)
                    poza.Comments = comments;
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

        public void IncarcaComentariu(string userName, string fileName, string continut)
        {
            var blob = _photoContainer.GetBlockBlobReference(fileName);
            blob.UploadText(continut);

            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, fileName)
            {
                Text = continut,
                Timestamp = DateTime.UtcNow,
                MadeBy = userName
            });

            _ctx.SaveChangesWithRetries();
        }

        public string GetAcceessLink(string fileName)
        {
            var blob = _photoContainer.GetBlockBlobReference(fileName);

            //var sasConstraints =
            //    new SharedAccessBlobPolicy
            //    {
            //        SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
            //        SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddMinutes(2), // 2 minutes expired
            //        Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write  //Read & Write
            //    };

            //var sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //return blob.Uri + sasBlobToken;

            var builder = new UriBuilder(blob.Uri)
            {
                Query = blob.GetSharedAccessSignature(
                new SharedAccessBlobPolicy
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessStartTime = new DateTimeOffset(DateTime.UtcNow.AddMinutes(-5)),
                    SharedAccessExpiryTime = new DateTimeOffset(DateTime.Now.AddHours(2))

                }).TrimStart('?')
            };

            return builder.Uri.ToString();
        }

    }
}