using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace AlbumPhoto.Service.Entities
{
	public class CommentEntity : TableServiceEntity
	{
		public CommentEntity() { }
		public CommentEntity(string userName, string fileName)
		{
			this.PartitionKey = fileName;
			this.RowKey = userName + fileName;
		}

		public string Text { get; set; }
		public string MadeBy { get; set; }
	}
}