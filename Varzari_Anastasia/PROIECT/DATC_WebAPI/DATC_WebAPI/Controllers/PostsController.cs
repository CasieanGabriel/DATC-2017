using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DATC_WebAPI.Models;

namespace DATC_WebAPI.Controllers
{
    public class PostsController : ApiController
    {
        CRUD_Table myTable = new CRUD_Table();

        // GET api/values
        public List<MyTable> Get()
        {
            return myTable.GetAllUsers();
        }

        // GET: api/Posts/5
        public int Get(string id)
        {
            return Int32.Parse(myTable.GetUser(id).PostsLikes);
        }

        // POST: api/Posts
        public void Post(string id, string postsLikes)
        {
            myTable.updatePostsLikes(id, postsLikes);
        }

        // PUT: api/Posts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Posts/5
        public void Delete(string id)
        {
            myTable.deleteData(id);
        }
    }
}
