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

        private BlobContainerPermissions _blobContainterPermissions;
        public static string sharedaccess = "";
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

            //link acces

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

        public void AdaugaComentarii(string comment, string userName, string description)
        {
            var blob = _photoContainer.GetBlockBlobReference(description);
            try
            {
                _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, description)
                {
                    Text = comment,
                    MadeBy = userName,
                });

                _ctx.SaveChangesWithRetries();
            }
            catch(Exception e)
            {
                e.ToString();
            }
            
        }

        public List<Poza.Comentarii> GetCommentarii(string description)
        {
            var comentarii = new List<Poza.Comentarii>();
            var query = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                         select file).AsTableServiceQuery<CommentEntity>(_ctx);

            foreach (var file in query)
            {
                if (file.Text != null)
                {
                    if (file.Text.StartsWith(description))
                    {
                        comentarii.Add(new Poza.Comentarii()
                        {
                            Text = file.Text.Substring(description.Length),
                            MadeBy = file.MadeBy

                        });
                    }
                }
            }
            return comentarii;
        }

        public void GetLink(string pictureName)
       {
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         where file.RowKey == pictureName
                         select file).AsTableServiceQuery<FileEntity>(_ctx); 
           
        } 
    }
}