using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using XsollaSummerSchoolTest;

namespace XsollaSummerSchoolTest.Controllers
{
    public class NewsItemsController : ApiController
    {
        private NewsDataModelContainer db = new NewsDataModelContainer();

        // GET: api/NewsItems
        public HttpResponseMessage GetNewsItemSet()
        {
            var news = db.NewsItemSet;
            var response = Request.CreateResponse(HttpStatusCode.OK, news);
            response.Headers.Add("News-count", news.Count().ToString());
            return response;
        }

        // GET: api/NewsItems/category
        public HttpResponseMessage GetNewsItemSet(string category)
        {
            var news = db.NewsItemSet.Where(x => x.Category == category);
            var response = Request.CreateResponse(HttpStatusCode.OK, news);
            response.Headers.Add("News-count", news.Count().ToString());
            
            return response;
        }

        // GET: api/NewsItems/5
        [ResponseType(typeof(NewsItem))]
        public IHttpActionResult GetNewsItem(int id)
        {
            NewsItem newsItem = db.NewsItemSet.Find(id);
            if (newsItem == null)
            {
                return NotFound();
            }

            return Ok(newsItem);
        }

        // PUT: api/NewsItems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNewsItem(int id, NewsItem newsItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != newsItem.Id)
            {
                string message = "Id из параметра запроса и Id переданного объекта не совпадают, " +
                    "возможно переданы не все свойства изменяемого объекта";
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, message);
                return BadRequest(message);
            }

            db.Entry(newsItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/NewsItems
        [ResponseType(typeof(NewsItem))]
        public IHttpActionResult PostNewsItem(NewsItem newsItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NewsItemSet.Add(newsItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = newsItem.Id }, newsItem);
        }

        // POST: api/NewsItem/5
        public IHttpActionResult PostRate(int rate)
        {
            
            return Ok();
        }

        // DELETE: api/NewsItems/5
        [ResponseType(typeof(NewsItem))]
        public IHttpActionResult DeleteNewsItem(int id)
        {
            NewsItem newsItem = db.NewsItemSet.Find(id);
            if (newsItem == null)
            {
                return NotFound();
            }

            db.NewsItemSet.Remove(newsItem);
            db.SaveChanges();

            return Ok(newsItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsItemExists(int id)
        {
            return db.NewsItemSet.Count(e => e.Id == id) > 0;
        }
    }
}