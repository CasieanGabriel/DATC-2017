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
        private BlobContainerPermissions containerPermission;
        static private string sas;
        static LinkTemporar linkTemporar = new LinkTemporar();
        string numePozaLinkTemporar;

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
            containerPermission = new BlobContainerPermissions();
            containerPermission.SharedAccessPolicies.Add(
             "policy", new SharedAccessBlobPolicy()
             {
                 SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1),
                 SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                  Permissions =  SharedAccessBlobPermissions.Read
                });
            containerPermission.PublicAccess = BlobContainerPublicAccessType.Off;
            _photoContainer.SetPermissions(containerPermission);
            sas = _photoContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy(),"policy");

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
            var comenturi = GetComment();
			var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
						 select file).AsTableServiceQuery<FileEntity>(_ctx);

			foreach (var file in query)
			{
                var commPoza = new List<Comentariu>();
                foreach (Comentariu comm in comenturi)
                {
                    if(comm.PhotoDescription == file.RowKey)
                    {
                        commPoza.Add(comm);
                    }
                }
                if (file.RowKey == numePozaLinkTemporar)
                {
                    poze.Add(new Poza()
                    {
                        Description = file.RowKey,
                        ThumbnailUrl = file.ThumbnailUrl,
                        Url = file.Url,
                        Commenturi = commPoza,
                        LinkulTemporar = linkTemporar
                    });
                }
                else
                {
                    poze.Add(new Poza()
                    {
                        Description = file.RowKey,
                        ThumbnailUrl = file.ThumbnailUrl,
                        Url = file.Url,
                        Commenturi = commPoza
                    });
                }
			}

			return poze;
		}
        public List<Comentariu> GetComment()
        {
            var Commenturi = new List<Comentariu>();
            var query = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name)
                         select file).AsTableServiceQuery<CommentEntity>(_ctx);

            foreach (var file in query)
            {
                Commenturi.Add(new Comentariu()
                {
                    Text = file.Text,
                    MadeBy = file.MadeBy,
                    PhotoDescription = file.RowKey
                });
            }

            return Commenturi;
        }

        public void GetLinkTemporar(string pathPoza)
        {
            numePozaLinkTemporar = pathPoza;
            
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var file in query)
            {
                if(pathPoza == file.RowKey)
                {
                    linkTemporar.Text = GetSasBlobUrl(file.Url);                    
                }                
            }
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

        public void IncarcaComentariu(string userName, string description, string continut)
        {
            //var blob = _photoContainer.GetBlockBlobReference(description);
           // blob.UploadFromStream(continut);

            _ctx.AddObject(_commentsTable.Name, new CommentEntity(userName, description)
            {
                MadeBy = userName,
                Text = continut//(new StreamReader(continut)).ReadLine()                  
            });

            _ctx.SaveChangesWithRetries();
        }

        public static string GetSasBlobUrl( string url)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
             CloudBlobClient sasBlobClient = new CloudBlobClient(storageAccount.BlobEndpoint,
              new StorageCredentials(sas));
            CloudBlob blob = (CloudBlob)sasBlobClient.GetBlobReferenceFromServer(new Uri(url));
            return blob.Uri.AbsoluteUri + sas;
        }

    }
}