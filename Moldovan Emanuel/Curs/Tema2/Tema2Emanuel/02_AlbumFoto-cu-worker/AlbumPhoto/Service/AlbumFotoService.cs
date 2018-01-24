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

        public static string GetBlobSasUri(CloudBlobContainer container, string fileName)
        {            
            CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(2);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);
            return blob.Uri + sasBlobToken;
        }        

        public CloudBlobContainer PhotoContainer { get { return _photoContainer; } }

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

        public Poza GetPoza(Poza poza)
        {
            Poza poze;
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).Where(w => w.RowKey == poza.Description).AsTableServiceQuery<FileEntity>(_ctx).FirstOrDefault();

                poze = new Poza
                {
                    Description = query.RowKey,
                    ThumbnailUrl = query.ThumbnailUrl,
                    Url = query.Url,
                    Comentarii = GetComentarii(query.RowKey)
                };

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

        public void AdaugaComentariu(string userName, string comentariu, string pozaDescription)
        {
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, pozaDescription)
            {
                Text = comentariu,
                MadeBy = userName,
            });

            _ctx.SaveChangesWithRetries();
        }

        public List<Comentariu> GetComentarii(string poza)
        {
            var comentarii = new List<Comentariu>();
            var query = (from comm in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                         select comm).Where(w => w.RowKey == poza).AsTableServiceQuery<CommentEntity>(_ctx);

            foreach (var comm in query)
            {
                comentarii.Add(new Comentariu()
                {
                    PozaDescription = comm.RowKey,
                    Text = comm.Text,
                    MadeBy = comm.MadeBy
                });
            }

            return comentarii;
        }


    }
}