using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ApiParcare.Models;

namespace ApiParcare.Controllers
{
    public class ParcareController : ApiController
    {
        private MyDatabaseEntities db = new MyDatabaseEntities();
        //GET: api/Parcare    merge
        public IQueryable<Parcare> GetParcare()
        {
            return db.Parcares;
        }

        //PUT :api/Parcare/i   merge...
        public HttpResponseMessage Put(int id,Parcare parcare)
        {
            try
            {
                using (MyDatabaseEntities entities = new MyDatabaseEntities())
                {
                    var entity = entities.Parcares.FirstOrDefault(e => e.LocID == id);
                    if (entity != null && (parcare.StareLoc == "liber"|| parcare.StareLoc=="ocupat"))
                    {

                            entity.StareLoc = parcare.StareLoc;
                            entities.SaveChanges();
                            return Request.CreateResponse(HttpStatusCode.OK, entity);

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound,"Parking not found");
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        public HttpResponseMessage Put( Parcare parcare)
        {
            try
            {
                using (MyDatabaseEntities entities = new MyDatabaseEntities())
                {
                    var entity = entities.Parcares.FirstOrDefault(e => e.LocID == parcare.LocID);
                    if (entity != null && (parcare.StareLoc == "liber" || parcare.StareLoc == "ocupat"))
                    {

                        entity.StareLoc_Veche = parcare.StareLoc_Veche;
                        entity.DataModificare = parcare.DataModificare;
                        entity.StareLoc = parcare.StareLoc;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, "Parking not found");
                    }
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //POST : api/Parcare
        public HttpResponseMessage Post([FromBody]Parcare parcare)
        {
            if (ModelState.IsValid)
            {
                db.Parcares.Add(parcare);
                db.SaveChanges();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, parcare);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = parcare.LocID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }
    }
}
