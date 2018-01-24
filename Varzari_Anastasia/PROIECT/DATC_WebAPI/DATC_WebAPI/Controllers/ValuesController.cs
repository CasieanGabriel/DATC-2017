using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DATC_WebAPI.Models;

namespace DATC_WebAPI.Controllers
{
    //PhotosController
    public class ValuesController : ApiController
    {
        CRUD_Table myTable = new CRUD_Table();
        
        // GET api/values
        public  List<MyTable> Get()
        {
            return myTable.GetAllUsers();
        }

        // GET api/values/5
        public int Get(string  id)
        {
            return Int32.Parse(myTable.GetUser(id).PhotosLikes);
        }

        // POST api/values
        public void Post(string id, string photosLikes)
        {
            myTable.updatePhotosLikes(id, photosLikes);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(string id)
        {
            myTable.deleteData(id);
        }
    }
}
