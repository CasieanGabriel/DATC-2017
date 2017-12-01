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
            var comments = new List<String>();
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var file in query)
            {
                var commQuery = _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                        .AsTableServiceQuery<CommentEntity>(_ctx).ToList().Where(fe => fe.PartitionKey == file.RowKey).ToList();

                List<Comentarii> Comments = new List<Comentarii>();
                foreach (var entry in commQuery)
                {
                    Comments.Add(new Comentarii()
                    {
                        Text = entry.Text,
                        MadeBy = entry.MadeBy,
                        PublishDate = entry.Timestamp
                    });
                }

                poze.Add(new Poza()
                {
                    Description = file.RowKey,
                    ThumbnailUrl = file.ThumbnailUrl,
                    Url = file.Url,
                    Comments = Comments
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

        public void AdaugaComentariu(string user, string poza, string comentariu)
        {
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(poza, Guid.NewGuid().ToString())
            {
                Text = comentariu,
                MadeBy = user,
            });

            _ctx.SaveChangesWithRetries();
        }

        /*public string GetSASBlobUrl(string fileName)
        {

            CloudBlobClient sasBlobClient = new CloudBlobClient(_account.BlobEndpoint, new StorageCredentials(sas));

            CloudBlob blob = (CloudBlob)sasBlobClient.GetBlobReferenceFromServer(new Uri(fileName));

            return blob.Uri.AbsoluteUri + sas;

        }

        public static List<Poza> GetPozeRef()
        {
            return poze;
        }*/
    }
}