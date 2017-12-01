using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumPhoto.Service.Entities
{
	public class CommentEntity : TableEntity
	{
		public CommentEntity() { }
		public CommentEntity(string userName, string fileName)
		{
			this.PartitionKey = userName;
			this.RowKey = fileName;
		}
        public string File { get; set; }
		public string Text { get; set; }
		public string MadeBy { get; set; }
	}
}