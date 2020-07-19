using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XsollaSummerSchoolTest;
using XsollaSummerSchoolTest.Controllers;
using XsollaSummerSchoolTest.Models;

namespace NewsApiTests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void GetOneNewsItem_OK()
        {
            // Arrange
            var controller = new NewsItemsController();

            // Act
            var response = controller.GetNewsItem(1);
            var result = response as OkNegotiatedContentResult<NewsItemSend>;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public void GetOneNewsItem_NotFound()
        {
            // Arrange
            var controller = new NewsItemsController();

            // Act
            var response = controller.GetNewsItem(-1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetAllNewsItems_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetNewsItemSet();
            IEnumerable<NewsItemSend> newsItems;

            // Assert
            Assert.IsTrue(response.TryGetContentValue<IEnumerable<NewsItemSend>>(out newsItems));
        }

        [TestMethod]
        public void GetNewsItemsByCategory_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.GetNewsItemSet("Искусство");
            IEnumerable<NewsItemSend> newsItems;

            // Assert
            Assert.IsTrue(response.TryGetContentValue<IEnumerable<NewsItemSend>>(out newsItems));
            Assert.AreEqual("Искусство", newsItems.First().Category);
        }

        [TestMethod]
        public void GetTopRatedNewsItems_OK()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            double leastAverage = 7;

            // Act
            var response = controller.GetNewsItemSet(leastAverage);
            IEnumerable<NewsItemSend> newsItems;

            // Assert
            Assert.IsTrue(response.TryGetContentValue<IEnumerable<NewsItemSend>>(out newsItems));
            Assert.IsTrue(newsItems.First().TotalRate / newsItems.First().RateCount >= leastAverage);
        }

        [TestMethod]
        public void PutNewsItem_NoContent()
        {
            // Arrange
            var controller = new NewsItemsController();

            // Act
            var response = (StatusCodeResult)controller.PutNewsItem(1, new XsollaSummerSchoolTest.NewsItem { Headline = "Test put", Body = "Test put", Category = "Test" });

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        [TestMethod]
        public void PutNewsItem_NotFound()
        {
            // Arrange
            var controller = new NewsItemsController();

            // Act
            var response = controller.PutNewsItem(-4, new XsollaSummerSchoolTest.NewsItem { Headline = "Test put", Body = "Test put", Category = "Test" });
            var result = response as NotFoundResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PutNewsItem_BadRequest()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.ModelState.AddModelError("Body", "The Body field is required.");

            // Act
            var response = controller.PutNewsItem(1, new NewsItem { Headline = "Test put", Category = "Test" });
            var result = response as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);
            
        }

        [TestMethod]
        public void PostNewsItem_BadRequest()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.ModelState.AddModelError("Body", "The Body field is required.");

            // Act
            var response = controller.PostNewsItem(new XsollaSummerSchoolTest.NewsItem { Headline = "Test post", Category = "Test" });
            var result = response as InvalidModelStateResult;

            // Assert
            Assert.IsNotNull(result);           
        }

        [TestMethod]
        public void PostNewsItem_CreatedAtRoute()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.PostNewsItem(new XsollaSummerSchoolTest.NewsItem { Headline = "Test post", Body = "Test post", Category = "Test" });
            var result = response as CreatedAtRouteNegotiatedContentResult<NewsItemSend>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("DefaultApi", result.RouteName);
            Assert.IsNotNull(result.RouteValues["id"]);
        }

        [TestMethod]
        public void DeleteNewsItem_NotFound()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.DeleteNewsItem(-1);
            var result = response as NotFoundResult;

            // Assert            
            Assert.IsNotNull(result);          
        }

        [TestMethod]
        public void DeleteNewsItem_Ok()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.DeleteNewsItem(1);
            var result = response as OkNegotiatedContentResult<NewsItemSend>;

            // Assert            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PostRate_NotFound()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.PostRate(-3, 8);

            // Assert            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void PostRate_BadRequest()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // Act
            var response = controller.PostRate(2, 123);

            // Assert            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public void PostRate_Ok()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("sessionString", "i71S2pE8tJ5LV0F658HCXK2zF57M3L");
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/");

            // Act
            var response = controller.PostRate(2, 8);

            // Assert            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void PostRate_Forbidden()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("Cookie", "sessionString=i81S2p54tJ5LV0F658HCXK2zF57M3L");
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/");
            controller.PostRate(2, 8);

            // Act
            var response = controller.PostRate(2, 8);

            // Assert            
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }


        [TestMethod]
        public void DeleteRate_Ok()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("Cookie", "sessionString=i71S2pE4tJ5LV0F658HCXK2zF57M3L");
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/");
            controller.PostRate(2, 8);

            // Act
            var response = controller.DeleteRate(2);

            // Assert            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void DeleteRate_Forbidden()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("Cookie", "sessionString=i81S2pE4tJ5LV0F658HCXK2zF57M3L");
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/");

            // Act
            var response = controller.DeleteRate(2);

            // Assert            
            Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void DeleteRate_NotFound()
        {
            // Arrange
            var controller = new NewsItemsController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            controller.Request.Headers.Add("Cookie", "sessionString=i81S2pE4tJ5LV0F658HCXK2zF57M3L");
            controller.Request.RequestUri = new Uri("https://localhost:44341/api/");

            // Act
            var response = controller.DeleteRate(-1);

            // Assert            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    
}
