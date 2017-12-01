using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Laborator_2.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        public static List<string> MyList = new List<string>();

        // GET api/values
        public IEnumerable<string> Get()
        {
            return MyList;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return MyList.ElementAt(id);
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
            MyList.Add(value);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
            if (MyList.ElementAt(id) != null)
                MyList[id] = value;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
