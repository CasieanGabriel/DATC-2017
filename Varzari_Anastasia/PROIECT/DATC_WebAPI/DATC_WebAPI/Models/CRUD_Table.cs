using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;


namespace DATC_WebAPI.Models
{
    public class TableOperations{}

    

    public class MyTable : TableEntity
    {
       
        public MyTable(string id, string name)
        {
            this.PartitionKey = id;
            this.RowKey = name;
        }

        public MyTable() { }

        public string PhotosLikes { get; set; }
        public string PostsLikes { get; set; }
    }

    public class CRUD_Table
    {
        private CloudStorageAccount account;
        private CloudTableClient tableClient;
        private CloudTable postsAndLikesTable;

        public CRUD_Table()
        {
            account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

            tableClient = account.CreateCloudTableClient();
            postsAndLikesTable = tableClient.GetTableReference("PostsLikes");
            postsAndLikesTable.CreateIfNotExists();
        }

        public void updatePhotosLikes(string id, string photosLikes)
        {
            TableQuery<MyTable> query = new TableQuery<MyTable>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));

            MyTable entry = postsAndLikesTable.ExecuteQuery(query).ToList<MyTable>().First();

            if(entry.PartitionKey.ToString() == id)
            {
                entry.PhotosLikes = photosLikes;
            }
            else
            {
                if (entry.PartitionKey.ToString() == null)
                {
                    throw new ArgumentNullException();
                }
            }
                    
            TableOperation updateData = TableOperation.InsertOrReplace(entry);
            postsAndLikesTable.Execute(updateData);
        }

        public void updatePostsLikes(string id, string postsLikes)
        {
            TableQuery<MyTable> query = new TableQuery<MyTable>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));

            MyTable entry = postsAndLikesTable.ExecuteQuery(query).ToList<MyTable>().First();

            if (entry.PartitionKey.ToString() == id)
            {
                entry.PostsLikes = postsLikes;
            }
            else
            {
                if (entry.PartitionKey.ToString() == null)
                {
                    throw new ArgumentNullException();
                }
            }
            
            TableOperation updateData = TableOperation.InsertOrReplace(entry);
            postsAndLikesTable.Execute(updateData);
        }

        public void deleteData(string id)
        {
            TableQuery<MyTable> query = new TableQuery<MyTable>().Where(
               TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));

            MyTable entry = postsAndLikesTable.ExecuteQuery(query).ToList<MyTable>().First();

            if (entry.PartitionKey.ToString() == id)
            {
                TableOperation deleteData = TableOperation.Delete(entry);
                postsAndLikesTable.Execute(deleteData);
            }
            else
            {
                if (entry.PartitionKey.ToString() == null)
                {
                    throw new ArgumentNullException();
                }
            }            
        }

        public List<MyTable> GetAllUsers()
        {
            TableQuery<MyTable> query = new TableQuery<MyTable>();

            return postsAndLikesTable.ExecuteQuery(query).ToList<MyTable>();
        }

        public MyTable GetUser(string id)
        {
            TableQuery<MyTable> query = new TableQuery<MyTable>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));

            return postsAndLikesTable.ExecuteQuery(query).ToList<MyTable>().FirstOrDefault();
        }

        public void  PostUserNameAndID(string id, string name)
        {
            TableQuery<MyTable> query = new TableQuery<MyTable>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));

            MyTable entry = postsAndLikesTable.ExecuteQuery(query).DefaultIfEmpty<MyTable>().First();
                                 
            if(entry == null)
            {
                entry = new MyTable();
                entry.PartitionKey = id;
                entry.RowKey = name;
            }     

            TableOperation insertData = TableOperation.InsertOrReplace(entry);
            postsAndLikesTable.Execute(insertData);
        }
    }

   
}