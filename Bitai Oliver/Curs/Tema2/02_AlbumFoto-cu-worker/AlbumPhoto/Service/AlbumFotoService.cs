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
        private BlobContainerPermissions containerPermissions;
        private static string SAS;

        public AlbumFotoService()
		{
			//_account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("PhotoStorage"));
			_account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
			_blobClient = _account.CreateCloudBlobClient();
			_photoContainer = _blobClient.GetContainerReference("poze");
            if (_photoContainer.CreateIfNotExists())
            {
                containerPermissions = _photoContainer.GetPermissions();
                
                SharedAccessBlobPolicy sharedPolicy = new SharedAccessBlobPolicy();
                sharedPolicy.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5);
                sharedPolicy.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(2);
                sharedPolicy.Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write;

                containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                containerPermissions.SharedAccessPolicies.Add("twohours", sharedPolicy);

                _photoContainer.SetPermissions(containerPermissions);

            }

            SAS = _photoContainer.GetSharedAccessSignature(null, "twohours");

            _tableClient = _account.CreateCloudTableClient();
			_filesTable = _tableClient.GetTableReference("files");
			_filesTable.CreateIfNotExists();
			_commentsTable = _tableClient.GetTableReference("comments");
			_commentsTable.CreateIfNotExists();
			_ctx = _tableClient.GetTableServiceContext();
		}

        
        public string GetSASUrl(string fileName)
        {
            var blob = _photoContainer.GetBlockBlobReference(fileName);
            return blob.Uri.ToString()+SAS;
        }

        public Link GetLink(string poza)
        {
            Link link = new Link();
            var poze = new List<Poza>();
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_ctx);
            foreach (var file in query)
            {
                if (file.RowKey.Equals(poza))
                    link.LinkPoza = GetSASUrl(file.Url);
                link.Poza = poza;
            }
            return link;
        }

        public List<Comentariu> GetComment(string poza)
        {
            var comentarii = new List<Comentariu>();
            var query = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                         select file).AsTableServiceQuery<CommentEntity>(_ctx);

            foreach (var file in query)
            {
                if (file.Text != null)
                {
                    string[] split = file.Text.Split(new string[] { "#%#" }, StringSplitOptions.None);
                    if (split.Length > 1)
                    {
                        string pozaComentata = split[0];
                        string comentariu = split[1];
                        if (poza.Equals(pozaComentata))
                        {
                            comentarii.Add(new Comentariu()
                            {
                                Text = comentariu,
                                MadeBy = file.MadeBy,
                            });
                        }
                    }
                }
            }

            return comentarii;
        }

        public void AddComment(string userName, string textComm, string by, Stream continut)
        {
            Random r = new Random();
            int rnd = r.Next(0, 99999999);
            string reff = by + rnd.ToString();
            var blob = _photoContainer.GetBlockBlobReference(reff);
            blob.UploadFromStream(continut);
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, reff)
            {
                Text = textComm,
                MadeBy = by,
            });

            _ctx.SaveChangesWithRetries();
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


	}
}