using AlbumPhoto.Service.Entities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using AlbumPhoto.Models;

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
					Url = file.Url
				});
			}

			return poze;
		}
        public List<Comentariu> GetComentariu(string pictureName)
        {
            var comentarii = new List<Comentariu>();
            var query = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                         select file).AsTableServiceQuery<CommentEntity>(_ctx);

            foreach(var comment in query)
            {

                {
                    if (!string.IsNullOrEmpty(comment.Text))
                        comentarii.Add(new Comentariu()
                        {
                            Text = comment.Text.Substring(pictureName.Length+1),
                            MadeBy = comment.MadeBy,

                        });
                }
            }
            return comentarii;
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

        public void IncarcaComentariu(string userName, string continut)
        {
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, continut)
                {
                    Text = continut,
                    MadeBy = userName,

                });
            _ctx.SaveChangesWithRetries();
            
        }

        public string GenerateLink(string pictureName)
        {
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         where file.RowKey == pictureName
                         select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var item in query)
            {
                return GetSasForBlob(item.Url);
            }
            return "link-ul nu poate fi generat";
        }
        public static string SAS = null;
        private string GetSasForBlob(string fileName)
        {
            var uri = new Uri(fileName);           
            CloudStorageAccount storageAc = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient sasBlobClient = new CloudBlobClient(storageAc.BlobEndpoint, new StorageCredentials(SAS)); CloudBlockBlob blob = (CloudBlockBlob)sasBlobClient.GetBlobReferenceFromServer(uri);
            return blob.Uri.AbsoluteUri + SAS;

        }
    }
}