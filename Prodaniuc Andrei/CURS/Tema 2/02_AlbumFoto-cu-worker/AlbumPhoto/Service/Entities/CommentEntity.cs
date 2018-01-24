using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumPhoto.Service.Entities
{
	public class CommentEntity : TableServiceEntity
	{
		public CommentEntity() { }
		public CommentEntity(string commentId, string fileName)
		{
			this.PartitionKey = commentId;
			this.RowKey = fileName;
		}

		public string Text { get; set; }
		public string MadeBy { get; set; }
	}
}