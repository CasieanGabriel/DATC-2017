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
        private static List<Poza> poze = new List<Poza>();
        bool arataComentariile = false;
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
            poze.Clear();
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

                if (arataComentariile)
                {
                    ArataCometarii(file.RowKey);
                }
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

        public void IncarcaComentariu(string fileName, string text, string userName)
        {
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, fileName)
            {
                MadeBy = userName,
                Text = text,
            });

            _ctx.SaveChangesWithRetries();
        }

        public List<Comentarii> ArataCometarii(string fileName)
        {
            var Comments = new List<Comentarii>();
            var query = (from comment in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                         select comment).AsTableServiceQuery<CommentEntity>(_ctx).ToList().Where(ce => ce.RowKey == fileName);

            foreach (var comm in query)
            {
                Comments.Add(new Comentarii()
                {
                    UserName = comm.MadeBy,
                    Comment = comm.Text,
                    PublishDate = comm.Timestamp
                });

                foreach (var poza in poze)
                {
                    if (poza.Description == comm.RowKey)
                        poza.Comentariu = Comments;
                }
            }
            return Comments;
        }

    }
}
