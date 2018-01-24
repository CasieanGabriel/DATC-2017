using AlbumPhoto.Models;
using AlbumPhoto.Service.Entities;
using Microsoft.WindowsAzure;
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
using System.Web;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;
using Microsoft.WindowsAzure.Storage.Auth;

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
        private TableServiceContext _tableServiceContext;
        private BlobContainerPermissions _blobContainerPermissions;
        private static string sas;

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

            _blobContainerPermissions = new BlobContainerPermissions();
            _blobContainerPermissions.SharedAccessPolicies.Add(
                "twohourspolicy", new SharedAccessBlobPolicy()
                {
                    SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-7),
                    SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                    Permissions = SharedAccessBlobPermissions.Read
                });
            _blobContainerPermissions.PublicAccess = BlobContainerPublicAccessType.Off;
            _photoContainer.SetPermissions(_blobContainerPermissions);
            sas = _photoContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "twohourspolicy");
            _tableClient = _account.CreateCloudTableClient();
            _filesTable = _tableClient.GetTableReference("files");
            _filesTable.CreateIfNotExists();
            _commentsTable = _tableClient.GetTableReference("comments");
            _commentsTable.CreateIfNotExists();
            _tableServiceContext = _tableClient.GetTableServiceContext();
        }

        public static string GetSasBlobUrl(string fileName)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient sasBlobClient = new CloudBlobClient(storageAccount.BlobEndpoint, new StorageCredentials(sas));
            CloudBlobContainer blob = sasBlobClient.GetContainerReference(fileName);
            return blob.Uri.AbsoluteUri + sas;
        }
        
        public void IncarcaPoza(string userName, string description, Stream continut)
        {
            var blob = _photoContainer.GetBlockBlobReference(description);
            blob.UploadFromStream(continut);

            _tableServiceContext.AddObject(_filesTable.Name, new FileEntity(userName, description)
            {
                PublishDate = DateTime.UtcNow,
                Size = continut.Length,
                Url = blob.Uri.ToString(),
            });

            _tableServiceContext.SaveChangesWithRetries();
        }

        public List<Poza> GetPoze()
        {
            var poze = new List<Poza>();
            var query = (from file in _tableServiceContext.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_tableServiceContext);

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

        public Link GetLink(string poza)
        {
            Link link = new Link();
            var poze = new List<Poza>();
            var query = (from file in _tableServiceContext.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_tableServiceContext);
            foreach (var file in query)
            {
                if (file.RowKey.Equals(poza))
                    link.LinkPoza = GetSasBlobUrl(file.Url);
                link.Poza = poza;
            }
            return link;
        }

        int numberImage = 0;
        public void UploadComment(string name, string comment, string by, Stream continut)
        {
            string fileName = by + numberImage.ToString();
            var blob = _photoContainer.GetBlockBlobReference(fileName);
            blob.UploadFromStream(continut);
            _tableServiceContext.AddObject(_commentsTable.Name, new CommentEntity(name, fileName)
            {
                Text = comment,
                MadeBy = by,
            });

            numberImage++;

            _tableServiceContext.SaveChangesWithRetries();
        }

        public List<Comentariu> GetComments(string fotografie)
        {
            var comments = new List<Comentariu>();
            var query = (from file in _tableServiceContext.CreateQuery<CommentEntity>(_commentsTable.Name)
                         select file).AsTableServiceQuery<CommentEntity>(_tableServiceContext);
            foreach (var file in query)
            {
                if (file.Text != null)
                {
                    string[] split = file.Text.Split(new string[] { "@@@" }, StringSplitOptions.None);
                    if (split.Length > 1)
                    {
                        string photoCom = split[0];
                        string com = split[1];
                        if (fotografie.Equals(photoCom))
                        {
                            comments.Add(new Comentariu()
                            {
                                Text = com,
                                MadeBy = file.MadeBy,
                            });
                        }
                    }
                }
            }
            return comments;
        }
    }
} 