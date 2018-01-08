using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DATC_WebAPI.Models;

namespace DATC_WebAPI.Controllers
{
    public class UserNameController : ApiController
    {
        CRUD_Table myTable = new CRUD_Table();

        // GET api/values
        public List<MyTable> Get()
        {
            return myTable.GetAllUsers();
        }

        // GET: api/Posts/5
        public string Get(string id)
        {
            return myTable.GetUser(id).RowKey;
        }

        // POST: api/UserName
        public void Post(string id, string name)
        {
            myTable.PostUserNameAndID(id, name);
        }

        // PUT: api/UserName/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/UserName/5
        public void Delete(int id)
        {
        }
    }
}
