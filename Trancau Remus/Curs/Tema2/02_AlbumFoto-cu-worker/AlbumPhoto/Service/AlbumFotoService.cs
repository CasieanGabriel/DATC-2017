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
        private BlobContainerPermissions _permissions;
        public static string signature = string.Empty;
        string nume_temp;
        static LinkTemp link_temp = new LinkTemp();


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

            _permissions = new BlobContainerPermissions();
            _permissions.SharedAccessPolicies.Add("2hourspolicy", new SharedAccessBlobPolicy()
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1),
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(2),
                Permissions = SharedAccessBlobPermissions.Read
            });
            _permissions.PublicAccess = BlobContainerPublicAccessType.Off;
            _photoContainer.SetPermissions(_permissions);
            signature = _photoContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy(), "2hourspolicy");
        }
        public List<Poza> GetPoze()
		{
			var poze = new List<Poza>();
            var comments= GetComments();

            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
						        select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var item in query)
            {
                var comm_poza = new List<Comments>();
                foreach (Comments comm in comments)
                {
                    if (comm.PhotoDescription == item.RowKey)
                        comm_poza.Add(comm);
                }

                if (item.RowKey == nume_temp)
                {
                    poze.Add(new Poza()
                    {
                        Description = item.RowKey,
                        ThumbnailUrl = item.ThumbnailUrl,
                        Url = item.Url,
                        Comentarii = comm_poza,
                        Linkul = link_temp
                    });
                }
                else
                {
                    poze.Add(new Poza()
                    {
                        Description = item.RowKey,
                        ThumbnailUrl = item.ThumbnailUrl,
                        Url = item.Url,
                        Comentarii = comm_poza
                    });
                }
            }
			return poze;
		}

        public void IncarcaPoza(string username, string descriere, Stream continut)
		{
			var blob = _photoContainer.GetBlockBlobReference(descriere);
			blob.UploadFromStream(continut);

			_ctx.AddObject(_filesTable.Name, new FileEntity(username, descriere)
			{
				PublishDate = DateTime.UtcNow,
				Size = continut.Length,
				Url = blob.Uri.ToString(),
			});

			_ctx.SaveChangesWithRetries();
		}

        public void AddComment( string username, string comment, string content)
        {
            //string description = returnDescription(username);
            _ctx.AddObject(_commentsTable.Name, new CommentEntity(username, comment)
            {
                MadeBy = username,
                Text =  content
                
            });

            _ctx.SaveChanges();
        }
        /*public static string returnDescription(string userName, int length = 10)
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
            var rnd = Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray();
            return userName + new string(rnd);
        }*/

        public static string GetSignatureBlobUrl(string fileName)
        {
            var uri = new Uri(fileName);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient signatureBlobClient = new CloudBlobClient(storageAccount.BlobEndpoint, new StorageCredentials(signature));
            ICloudBlob blob = signatureBlobClient.GetBlobReferenceFromServer(uri);

            return blob.Uri.AbsoluteUri + signature;
        }

		

	

        public void GetLink(string picture_name)
        {
            nume_temp = picture_name;
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name) where file.RowKey == picture_name select file).AsTableServiceQuery<FileEntity>(_ctx);
            foreach(var item in query)
            {
                 if (picture_name == item.RowKey)
                     link_temp.text = GetSignatureBlobUrl(item.Url);
            }
        }

        public List<Comments> GetComments()
        {
            var comment_list = new List<Comments>();
            var commquery = (from file in _ctx.CreateQuery<CommentEntity>(_commentsTable.Name) select file).AsTableServiceQuery<CommentEntity>(_ctx);
            foreach(var comm in commquery)
            {
                comment_list.Add(new Comments()
                {
                    MadeBy = comm.MadeBy,
                    Text = comm.Text,
                    PhotoDescription = comm.RowKey
                });
            }
            return comment_list;
        }
	}
}