using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using XsollaSummerSchoolTest.Models;

namespace XsollaSummerSchoolTest.Controllers
{
    public class NewsItemsController : ApiController
    {
        private NewsDataModelContainer db = new NewsDataModelContainer();

        private static int MaxMark = 10;
        private static int MinMark = 1;

        // GET: api/NewsItems
        public HttpResponseMessage GetNewsItemSet()
        {
            var rawNews = db.NewsItemSet.ToList();
            var news = rawNews.Select(x => (NewsItemSend)x);
            var response = Request.CreateResponse(HttpStatusCode.OK, news);
            response.Headers.Add("News-count", news.Count().ToString());
            return response;
        }

        // GET: api/NewsItems?category
        public HttpResponseMessage GetNewsItemSet(string category)
        {
            var news = (IQueryable<NewsItemSend>)db.NewsItemSet.Where(x => x.Category == category);
            var response = Request.CreateResponse(HttpStatusCode.OK, news);
            response.Headers.Add("News-count", news.Count().ToString());
            
            return response;
        }

        // GET: api/NewsItems?LeastAverage
        public HttpResponseMessage GetNewsItemSet(double leastAverage)
        {
            var rawNews = db.NewsItemSet.ToList().Where(x => x.Rate.Count > 0);
            var news = rawNews.Select(x => (NewsItemSend)x);
            news = news.Where(x => x.TotalRate / x.RateCount >= leastAverage);
            var response = Request.CreateResponse(HttpStatusCode.OK, news);
            response.Headers.Add("News-count", news.Count().ToString());

            return response;
        }

        // GET: api/NewsItems/5
        [ResponseType(typeof(NewsItemSend))]
        public IHttpActionResult GetNewsItem(int id)
        {
            NewsItemSend newsItem = db.NewsItemSet.Find(id);
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
        [ResponseType(typeof(NewsItemSend))]
        public IHttpActionResult PostNewsItem(NewsItem newsItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NewsItemSet.Add(newsItem);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = newsItem.Id }, (NewsItemSend)newsItem);
        }

        // POST: api/NewsItem/PostRate/5
        public HttpResponseMessage PostRate(int id, short mark)
        {
            NewsItem newsItem = db.NewsItemSet.Find(id);
            if (newsItem == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }           
            if (mark > MaxMark || mark < MinMark)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    $"Mark was out of available range. Mark should be between {MinMark} and {MaxMark}");
            }
            CookieHeaderValue cookie = Request.Headers.GetCookies("sessionstring").FirstOrDefault();
            if (cookie == null)
            {
                string sessionString = GetRandomString();
                newsItem.Rate.Add(new Rate { Mark = mark, SessionString = sessionString });
                db.SaveChanges();
                var response = Request.CreateResponse(HttpStatusCode.OK);
                cookie = new CookieHeaderValue("sessionstring", sessionString); 
                cookie.Expires = DateTimeOffset.Now.AddDays(1); 
                cookie.Domain = Request.RequestUri.Host; 
                cookie.Path = "/";
                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                return response;
            }
            else
            {
                if (newsItem.Rate.Any(x => x.SessionString == cookie["sessionstring"].Value))
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "Can not rate news item more than once");
                }
                else
                {
                    newsItem.Rate.Add(new Rate { Mark = mark, SessionString = cookie["sessionstring"].Value });
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
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

        // DELETE: api/NewsItem/DeleteRate/5
        public HttpResponseMessage DeleteRate(int id)
        {
            NewsItem newsItem = db.NewsItemSet.Find(id);
            if (newsItem == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            CookieHeaderValue cookie = Request.Headers.GetCookies("sessionstring").FirstOrDefault();
            if (cookie == null)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You have not post rate to this news item yet");
            }
            else
            {
                if (newsItem.Rate.Any(x => x.SessionString == cookie["sessionstring"].Value))
                {
                    var rateToRemove = newsItem.Rate.First(x => x.SessionString == cookie["sessionstring"].Value);
                    db.Entry(rateToRemove).State = EntityState.Deleted;
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "You have not post rate to this news item yet");
                }
            }
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

        private string GetRandomString()
        {
            string s = "";
            Random rnd = new Random();
            for (int i = 0; i < 30; i++)
            {
                char[] chars = { (char)rnd.Next(48, 58), (char)rnd.Next(65, 91), (char)rnd.Next(97, 122) };
                s += chars[rnd.Next(0, 3)];
            }
            return s;
        }
    }
}