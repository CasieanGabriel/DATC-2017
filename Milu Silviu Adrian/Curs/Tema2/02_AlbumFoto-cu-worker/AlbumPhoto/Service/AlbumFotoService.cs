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

		public List<PozaCuComentarii> GetPoze()
		{
            var poze = new List<PozaCuComentarii>();
            var query = (from file in _ctx.CreateQuery<FileEntity>(_filesTable.Name)
                         select file).AsTableServiceQuery<FileEntity>(_ctx);

            foreach (var file in query)
            {
                var listaComentarii = GetComentariiPePoza(file.RowKey);

                var a = new Poza()
                {

                    Description = file.RowKey,
                    ThumbnailUrl = file.ThumbnailUrl,
                    Url = file.Url
                };

                var b = new PozaCuComentarii { Poza = a,Comentarii = listaComentarii };

                poze.Add(b);
                          
            }

            return poze;
        }

        private List<CommentEntity> GetComentariiPePoza(string rowKey)
        {
            //todo:implement logic 
            //    _commentsTable.GetComentarii();

            //exemplu
            //use this
            //      _commentsTable;

            // Create a retrieve operation that takes a customer entity.
            //TableOperation retrieveOperation = TableOperation.Retrieve<CommentEntity>("comments",rowKey);
            TableQuery<CommentEntity> query = new TableQuery<CommentEntity>().Where(TableQuery.GenerateFilterCondition("File", QueryComparisons.Equal, rowKey));

            // Execute the retrieve operation.
            //TableResult retrievedResult = _commentsTable.Execute(retrieveOperation);

            //    Print the phone number of the result.

            var lista = new List<CommentEntity>();

            foreach (var entity in _commentsTable.ExecuteQuery(query))
            {
                lista.Add(entity);
            }
            // if (retrievedResult.Result != null)
            //  {

            //    var com = (CommentEntity)retrievedResult.Result;
            //lista.Add(com);
            //   }

            return lista;
        }


        public void AddComment(string fileName, string comment)
        {
            //todo:add logic
            // Create the TableOperation object that inserts the customer entity.
                TableOperation insertOperation = TableOperation.Insert(new CommentEntity("comments", $"{DateTime.UtcNow.Ticks.ToString()}")

                {
                    Text = comment, 
                    File = fileName
                });

            // Execute the insert operation.

        //    TableBatchOperation batchOperation = new TableBatchOperation();

      //      batchOperation.Insert(new CommentEntity());

            _commentsTable.Execute(insertOperation);

          /*  _commentsTable.Add(new CommentEntity(("comments", fileName)

                {
                Text = comment;
        });*/
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