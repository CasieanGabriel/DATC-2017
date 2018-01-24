using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GoogleMapsMVC.Controllers
{
    public class LocPController : ApiController
    {
        private DB1Entities db = new DB1Entities();

        // GET api/LocP
        public IEnumerable<parking> Getparkings()
        {
            return db.parking.AsEnumerable();
        }

        // GET api/LocP/5
        public parking Getparking(int id)
        {
            parking parking = db.parking.Find(id);
            if (parking == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return parking;
        }

        // PUT api/LocP/5
        public HttpResponseMessage Putparking(int id, parking parking)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != parking.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(parking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/LocP
        public HttpResponseMessage Postparking(parking parking)
        {
            if (ModelState.IsValid)
            {
                db.parking.Add(parking);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, parking);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = parking.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/LocP/5
        public HttpResponseMessage Deleteparking(int id)
        {
            parking parking = db.parking.Find(id);
            if (parking == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.parking.Remove(parking);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, parking);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}